using SlnParser.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SlnParser.Models
{
    /// <summary>
    ///     A Solution Project that can be contained in a <see cref="Solution" />
    /// </summary>
    public class SolutionProject : SlnProject, ISlnProject
    {
        private readonly ICollection<ConfigurationPlatform> _configurationPlatforms;

        /// <summary>
        ///     The File of the Project
        /// </summary>
        public FileInfo File { get; }

        /// <summary>
        ///     The <see cref="ConfigurationPlatform" />s configured for this solution
        /// </summary>
        public IEnumerable<ConfigurationPlatform> ConfigurationPlatforms { get; }


        /// <summary>
        ///     Creates a new instance of <see cref="SolutionProject" />
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="name">The name</param>
        /// <param name="typeGuid">The project-type id</param>
        /// <param name="type">The well-known project-type</param>
        /// <param name="fileInfo">The <see cref="FileInfo" /> for the Project-File</param>
        public SolutionProject(
            Guid id,
            string name,
            Guid typeGuid,
            ProjectType type,
            FileInfo fileInfo,
            Solution sln = null) : base(id, name, typeGuid, type, sln)
        {
            File = fileInfo;
            _configurationPlatforms = new List<ConfigurationPlatform>();
        }

        internal void AddConfigurationPlatform(ConfigurationPlatform configurationPlatform)
        {
            if (configurationPlatform == null) throw new ArgumentNullException(nameof(configurationPlatform));

            _configurationPlatforms.Add(configurationPlatform);
        }

        public override string ToString() => $"""
Project("{_projectType.guid.ToUpper().WithBraces()}") = "{Name}", "{File.FullName.Replace($@"{Sln.File.DirectoryName}\", string.Empty)}", "{Id.ToUpper().WithBraces()}"
EndProject
""";
    }
}
