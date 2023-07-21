using SlnParser.Common.Utilities;
using System;

namespace SlnParser.Contracts.Helper
{
    public class NestedProjectMapping
    {
        public NestedProjectMapping(
            string targetId,
            string destinationId)
        {
            TargetId = new Guid(targetId);
            DestinationId = new Guid(destinationId);
        }

        public Guid TargetId { get; }

        public Guid DestinationId { get; }

        public override string ToString() => $"        {TargetId.ToString().ToUpper().WithBraces()} = {DestinationId.ToString().ToUpper().WithBraces()}";
    }
}
