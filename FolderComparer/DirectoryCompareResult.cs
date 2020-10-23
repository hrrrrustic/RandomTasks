using System;
using System.Collections.Generic;
using System.Text;

namespace FolderComparer.Tools
{
    public class DirectoryCompareResult
    {
        public readonly IReadOnlyList<(String, String)> Matches;
        public readonly IReadOnlyList<String> Differences;
        public readonly Boolean IsIdentical;

        public DirectoryCompareResult(IReadOnlyList<(String, String)> matches, IReadOnlyList<String> differences) 
            => (Matches, Differences, IsIdentical) = (matches, differences, differences.Count == 0);

        private DirectoryCompareResult() => (Matches, Differences, IsIdentical) = (new List<(String, String)>(0), new List<String>(0), true);

        public static DirectoryCompareResult IdenticalFoldersResult => new DirectoryCompareResult();

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