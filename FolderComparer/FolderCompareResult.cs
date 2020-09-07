using System;
using System.Collections.Generic;
using System.Text;

namespace FolderComparer
{
    public class FolderCompareResult
    {
        public readonly IReadOnlyList<(String, String)> Matches;
        public readonly IReadOnlyList<String> Differences;
        public readonly Boolean IsIdentical;

        public FolderCompareResult(List<(String, String)> matches, List<String> differences) : this(matches.AsReadOnly(), differences.AsReadOnly())
        { }

        public FolderCompareResult(IReadOnlyList<(String, String)> matches, IReadOnlyList<String> differences)
        {
            Matches = matches;
            Differences = differences;
            IsIdentical = differences.Count == 0;
        }

        private FolderCompareResult()
        {
            Matches = new List<(String, String)>(0);
            Differences = new List<String>(0);
            IsIdentical = true;
        }

        public static FolderCompareResult IdenticalFoldersResult => new FolderCompareResult();

        public override String ToString()
        {
            if (IsIdentical)
                return "Folders are equals";

            StringBuilder builder = new StringBuilder();
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