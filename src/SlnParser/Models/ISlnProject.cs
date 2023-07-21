using System;

namespace SlnParser.Models
{
    /// <summary>
    ///     A project that can be contained in a <see cref="Solution" />
    /// </summary>
    public interface ISlnProject
    {
        /// <summary>
        ///     The Id of the Project
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     The Name of the Project
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     The well-known <see cref="Type" />
        /// </summary>
        ProjectType Type { get; }
    }

    public class SlnProject : ISlnProject
    {
        protected readonly (Guid guid, ProjectType type) _projectType;

        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ProjectType Type => _projectType.type;

        public Solution Sln { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="SolutionProject" />
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="name">The name</param>
        /// <param name="typeGuid">The project-type id</param>
        /// <param name="type">The well-known project-type</param>
        public SlnProject(
            Guid id,
            string name,
            Guid typeGuid,
            ProjectType type,
            Solution sln = null)
        {
            Id = id;
            Name = name;
            _projectType = (typeGuid, type);
            Sln = sln;
        }
    }
}
