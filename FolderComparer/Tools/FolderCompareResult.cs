using System;
using System.Collections.Generic;
using System.Text;

namespace FolderComparer.Tools
{
    public class FolderCompareResult
    {
        public readonly IReadOnlyList<(String, String)> Matches;
        public readonly IReadOnlyList<String> Differences;
        public readonly Boolean IsIdentical;

        public FolderCompareResult(IReadOnlyList<(String, String)> matches, IReadOnlyList<String> differences) 
            => (Matches, Differences, IsIdentical) = (matches, differences, differences.Count == 0);

        private FolderCompareResult() => (Matches, Differences, IsIdentical) = (new List<(String, String)>(0), new List<String>(0), true);

        public static FolderCompareResult IdenticalFoldersResult => new FolderCompareResult();

        public override String ToString()
        {
            if (IsIdentical)
                return "Folders are equals";

            StringBuilder builder = new();
            builder
                .Append("Same files : ")
                .AppendLine();

            Matches.ForEach(k => builder
                .Append($"{k.Item1} ====== {k.Item2}")
                .AppendLine());

            builder
                .Append("Difference :")
                .AppendLine();

            Differences.ForEach(k => builder
                .Append($"{k}")
                .AppendLine());

            return builder.ToString();
        }
    }
}