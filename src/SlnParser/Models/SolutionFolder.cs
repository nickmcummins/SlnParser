using SlnParser.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static SlnParser.Common.Utilities.StringExtensions;

namespace SlnParser.Models
{
    /// <summary>
    ///     A Solution Folder that can be contained in a <see cref="Solution" />
    /// </summary>
    public class SolutionFolder : SlnProject, ISlnProject
    {
        private readonly ICollection<FileInfo> _files;
        private readonly ICollection<ISlnProject> _projects;

        /// <summary>
        ///     The contained <see cref="ISlnProject" />s in the Solution Folder
        /// </summary>
        public IEnumerable<ISlnProject> Projects => _projects;

        /// <summary>
        ///     The contained <see cref="FileInfo" />s in the Solution Folder
        /// </summary>
        public IEnumerable<FileInfo> Files => _files;

        /// <summary>
        ///     Creates a new instance of <see cref="SolutionFolder" />
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="name">The name</param>
        /// <param name="typeGuid">The project-type id</param>
        /// <param name="type">The well-known project-type</param>
        public SolutionFolder(
            Guid id,
            string name,
            Guid typeGuid,
            ProjectType type,
            Solution sln = null) : base(id, name, typeGuid, type, sln)
        {
            _files = new List<FileInfo>();
            _projects = new List<ISlnProject>();
        }

        internal void AddProject(ISlnProject project)
        {
            _projects.Add(project);
        }

        internal void AddFile(FileInfo fileInfo)
        {
            _files.Add(fileInfo);
        }

        public override string ToString()
        {
            var s = new StringBuilder($"""
Project("{_projectType.guid.ToUpper().WithBraces()}") = "{Name}", "{Name}", "{Id.ToUpper().WithBraces()}"
""");
            if (Files.Any())
            {
                s.Append("\n    ProjectSection(SolutionItems) = preProject\r\n");
                foreach (var file in Files) s.Append(string.Concat(Sln.File.FullName.RelativePathTo(file.FullName), " = ", Sln.File.FullName.RelativePathTo(file.FullName), NewLine).Indent(3));
                s.Append("    EndProjectSection");
            }
            s.Append("\r\nEndProject");
            return s.ToString();
        }
    }
}
