using SlnParser.Contracts.Exceptions;
using SlnParser.Contracts.Helper;
using SlnParser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace SlnParser.Helper
{
    internal sealed class EnrichSolutionWithProjects : IEnrichSolution
    {
        private readonly SolutionFileParser _parseProjectDefinition;

        public EnrichSolutionWithProjects()
        {
            _parseProjectDefinition = new SolutionFileParser();
        }

        public void Enrich(Solution solution, IEnumerable<string> fileContents)
        {
            if (solution == null) throw new ArgumentNullException(nameof(solution));
            if (fileContents == null) throw new ArgumentNullException(nameof(fileContents));

            solution.Projects = GetProjectsFlat(solution, fileContents);
            solution.NestedProjectMappings = GetGlobalSectionForNestedProjects(fileContents);
            solution.StructuredProjects = GetProjectsStructured(solution.Projects, solution.NestedProjectMappings);
        }

        private IEnumerable<ISlnProject> GetProjectsFlat(Solution solution, IEnumerable<string> fileContents)
        {
            var flatProjects = new DictionaryList<string, ISlnProject>(project => project.Name);
            foreach (var line in fileContents)
            {
                if (!_parseProjectDefinition.TryParseProjectDefinition(solution, line, out var project)) continue;
                flatProjects.Add(project);
            }

            return flatProjects;
        }

        private static IEnumerable<ISlnProject> GetProjectsStructured(
            IEnumerable<ISlnProject> flatProjects,
            IEnumerable<NestedProjectMapping> nestedProjectMappings)
        {
            var structuredProjects = new List<ISlnProject>();

            ApplyProjectNesting(flatProjects, structuredProjects, nestedProjectMappings);

            return structuredProjects;
        }

        private static IEnumerable<NestedProjectMapping> GetGlobalSectionForNestedProjects(
            IEnumerable<string> fileContents)
        {
            const string startNestedProjects = "GlobalSection(NestedProjects";
            const string endNestedProjects = "EndGlobalSection";

            var section = fileContents
                .SkipWhile(line => !line.StartsWith(startNestedProjects))
                .TakeWhile(line => !line.StartsWith(endNestedProjects))
                .Where(line => !line.StartsWith(startNestedProjects))
                .Where(line => !line.StartsWith(endNestedProjects))
                .Where(line => !string.IsNullOrWhiteSpace(line));

            var nestedProjectMappings = new List<NestedProjectMapping>();
            foreach (var nestedProject in section)
                if (TryGetNestedProjectMapping(nestedProject, out var nestedProjectMapping))
                    nestedProjectMappings.Add(nestedProjectMapping);

            return nestedProjectMappings;
        }

        private static bool TryGetNestedProjectMapping(string nestedProject,
            out NestedProjectMapping nestedProjectMapping)
        {
            // https://regexr.com/653pi
            const string pattern = @"{(?<targetProjectId>[A-Za-z0-9\-]+)} = {(?<destinationProjectId>[A-Za-z0-9\-]+)}";

            nestedProjectMapping = null;
            var match = Regex.Match(nestedProject, pattern);
            if (!match.Success) return false;

            var targetProjectId = match.Groups["targetProjectId"].Value;
            var destinationProject = match.Groups["destinationProjectId"].Value;

            nestedProjectMapping = new NestedProjectMapping(targetProjectId, destinationProject);
            return true;
        }

        private static void ApplyProjectNesting(
            IEnumerable<ISlnProject> flatProjects,
            ICollection<ISlnProject> structuredProjects,
            IEnumerable<NestedProjectMapping> nestedProjectMappings)
        {
            foreach (var project in flatProjects)
                ApplyNestingForProject(project, flatProjects, structuredProjects, nestedProjectMappings);
        }

        private static void ApplyNestingForProject(
            ISlnProject project,
            IEnumerable<ISlnProject> flatProjects,
            ICollection<ISlnProject> structuredProjects,
            IEnumerable<NestedProjectMapping> nestedProjectMappings)
        {
            var mappingCandidate = nestedProjectMappings.FirstOrDefault(mapping => mapping.TargetId == project.Id);
            if (mappingCandidate == null)
            {
                structuredProjects.Add(project);
                return;
            }

            var destinationCandidate = flatProjects.FirstOrDefault(proj => proj.Id == mappingCandidate.DestinationId);
            if (destinationCandidate == null)
                throw new UnexpectedSolutionStructureException($"Expected to find a project with id '{mappingCandidate.DestinationId}', but found none");

            if (destinationCandidate is not SolutionFolder solutionFolder)
                throw new UnexpectedSolutionStructureException($"Expected project with id '{destinationCandidate.Id}' to be a Solution-Folder but found '{destinationCandidate.GetType()}'");

            solutionFolder.AddProject(project);
        }
    }
}
