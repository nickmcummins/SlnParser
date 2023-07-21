using System.Collections.Generic;

namespace SlnParser.Contracts.Helper
{
    internal interface IParseSolutionSection<TSection>
    {
        TSection Parse(
            IEnumerable<string> fileContents,
            string startSection);
    }
}
