using SlnParser.Common.Utilities;
using SlnParser.Models;
using System;

namespace SlnParser.Contracts.Helper
{
    public sealed class ProjectConfigurationPlatform
    {
        public ProjectConfigurationPlatform(
            Guid? projectId,
            ConfigurationPlatform configurationPlatform)
        {
            ProjectId = projectId;
            ConfigurationPlatform = configurationPlatform;
        }

        public Guid? ProjectId { get; }

        public ConfigurationPlatform ConfigurationPlatform { get; }

        public override string ToString() => $"         {ProjectId.ToString().ToUpper().WithBraces()}.{ConfigurationPlatform}";

    }
}
