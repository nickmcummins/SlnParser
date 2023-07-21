using System.ComponentModel;

namespace SlnParser.Models
{
    /// <summary>
    ///     The Platform a Project or Solution Targets
    /// </summary>
    public enum BuildPlatform
    {
        /// <summary>
        ///     Any CPU-Platform is targeted
        /// </summary>
        [Description("Any CPU")]
        AnyCpu,

        /// <summary>
        ///     Only x64 (64 Bit) CPU-Platforms are targeted
        /// </summary>
        [Description("x64")]
        X64,

        /// <summary>
        ///     Only x86 (32 Bit) CPU-Platforms are targeted
        /// </summary>
        [Description("x86")]

        X86
    }
}
