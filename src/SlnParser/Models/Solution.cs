using SlnParser.Common.Utilities;
using SlnParser.Contracts.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SlnParser.Common.Utilities.StringExtensions;


namespace SlnParser.Models
{
    /// <inheritdoc />
    public class Solution : ISolution
    {
        private IDictionary<string, ISlnProject> _projects;
        public readonly IEnumerable<string> _allLines;
        public readonly IEnumerable<string> _allLinesTrimmed;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public FileInfo File { get; set; }

        /// <inheritdoc />
        public string FileFormatVersion { get; set; }

        /// <inheritdoc />
        public VisualStudioVersion VisualStudioVersion { get; set; }

        /// <inheritdoc />
        public IEnumerable<ISlnProject> Projects { get => _projects.Values; internal set => _projects = value.ToDictionary(project => project.Name); }

        /// <inheritdoc />
        public IEnumerable<ISlnProject> StructuredProjects { get; internal set; }

        /// <inheritdoc />
        public IReadOnlyCollection<ConfigurationPlatform> SolutionConfigurationPlatforms { get; internal set; }
        public IEnumerable<ProjectConfigurationPlatform> ProjectConfigurationPlatforms { get; set; }
        public IEnumerable<NestedProjectMapping> NestedProjectMappings { get; set; }

        /// <summary>
        ///     Creates a new instance of <see cref="Solution" />
        /// </summary>
        public Solution()
        {
        }

        public Solution(FileInfo solutionFile)
        {
            File = solutionFile;
            Name = Path.GetFileNameWithoutExtension(solutionFile.FullName);
            _allLines = System.IO.File.ReadAllLines(solutionFile.FullName);
            _allLinesTrimmed = _allLines.Select(line => line.Trim())
                .Where(line => line.Length > 0);
        }

        public override string ToString() => $"""
Microsoft Visual Studio Solution File, Format Version {FileFormatVersion}
# Visual Studio Version 17
{VisualStudioVersion}
{string.Join(NewLine, Projects.Select(project => project.ToString()))}
Global
    GlobalSection(SolutionConfigurationPlatforms) = preSolution
{string.Join(NewLine, SolutionConfigurationPlatforms.Select(configurationPlatform => configurationPlatform.ToString().Indent(2)))}        
    EndGlobalSection
    GlobalSection(ProjectConfigurationPlatforms) = postSolution
{string.Join(NewLine, ProjectConfigurationPlatforms.Select(projectConfiguration => projectConfiguration.ToString()))}
    EndGlobalSection
    GlobalSection(SolutionProperties) = preSolution
	    HideSolutionNode = FALSE
    EndGlobalSection
    GlobalSection(NestedProjects) = preSolution
{string.Join(NewLine, NestedProjectMappings.Select(nestedProjectMapping => nestedProjectMapping.ToString()))}
    EndGlobalSection
EndGlobal
""";
    }
}
