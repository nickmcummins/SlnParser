using SlnParser.Contracts.Helper;
using SlnParser.Models;
using System.Collections.Generic;
using System.Linq;

namespace SlnParser.Helper
{
    internal sealed class EnrichSolutionWithSolutionConfigurationPlatforms : IEnrichSolution
    {
        private readonly SolutionFileParser _parseSolutionConfigurationPlatform;

        public EnrichSolutionWithSolutionConfigurationPlatforms()
        {
            _parseSolutionConfigurationPlatform = new SolutionFileParser();
        }

        public void Enrich(Solution solution, IEnumerable<string> fileContents)
        {
            var projectConfigurations = _parseSolutionConfigurationPlatform.Parse(
                fileContents,
                "GlobalSection(SolutionConfiguration");
            solution.SolutionConfigurationPlatforms = projectConfigurations
                .Select(projectConfiguration => projectConfiguration.ConfigurationPlatform)
                .ToList()
                .AsReadOnly();
        }
    }
}
