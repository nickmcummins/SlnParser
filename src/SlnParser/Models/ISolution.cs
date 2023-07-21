using System.Collections.Generic;
using System.IO;

namespace SlnParser.Models
{
    /// <summary>
    ///     An interface representing all the information contained in a Visual Studio Solution File (sln)
    /// </summary>
    public interface ISolution
    {
        /// <summary>
        ///     The name of the solution
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     The File of the solution
        /// </summary>
        FileInfo File { get; set; }

        /// <summary>
        ///     The file format version of the solution
        /// </summary>
        string FileFormatVersion { get; set; }

        /// <summary>
        ///     The <see cref="VisualStudioVersion" /> of the solution
        /// </summary>
        VisualStudioVersion VisualStudioVersion { get; set; }

        /// <summary>
        ///     A flat list of all <see cref="ISlnProject" />s contained in the solution
        /// </summary>
        IEnumerable<ISlnProject> Projects { get; }

        /// <summary>
        ///     A structured list of all <see cref="ISlnProject" />s contained in the solution
        /// </summary>
        IEnumerable<ISlnProject> StructuredProjects { get; }

        /// <summary>
        ///     The <see cref="ConfigurationPlatform" />s configured for this solution
        /// </summary>
        IReadOnlyCollection<ConfigurationPlatform> SolutionConfigurationPlatforms { get; }
    }
}
