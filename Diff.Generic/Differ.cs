using System;
using System.Collections.Generic;
using System.Linq;
using Diff.Generic.Model;

namespace Diff.Generic
{
    public class Differ : IDiffer
    {
        public DiffResult<string> CreateLineDiffs(string oldText, string newText, bool ignoreWhitespace)
        {
            return CreateLineDiffs(oldText, newText, ignoreWhitespace, false);
        }

        public DiffResult<string> CreateLineDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");


            return CreateCustomDiffs(oldText, newText, ignoreWhitespace,ignoreCase, str => NormalizeNewlines(str).Split('\n'));
        }

        public DiffResult<string> CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace)
        {
            return CreateCharacterDiffs(oldText, newText, ignoreWhitespace, false);
        }

        public DiffResult<string> CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");


            return CreateCustomDiffs(
                oldText,
                newText,
                ignoreWhitespace,
                ignoreCase,
                str =>
                    {
                        var s = new string[str.Length];
                        for (int i = 0; i < str.Length; i++) s[i] = str[i].ToString();
                        return s;
                    });
        }

        public DiffResult<string> CreateWordDiffs(string oldText, string newText, bool ignoreWhitespace, char[] separators)
        {
            return CreateWordDiffs(oldText, newText, ignoreWhitespace, false, separators);
        }

        public DiffResult<string> CreateWordDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase, char[] separators)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");


            return CreateCustomDiffs(
                oldText,
                newText,
                ignoreWhitespace,
                ignoreCase,
                str => SmartSplit(str, separators));
        }

        public DiffResult<string> CreateCustomDiffs(string oldText, string newText, bool ignoreWhiteSpace, Func<string, string[]> chunker)
        {
            return CreateCustomDiffs(oldText, newText, ignoreWhiteSpace, false, chunker);
        }

        private static IEnumerable<string> ApplyStringChunking(string original, bool ignoreWhiteSpace, bool ignoreCase, Func<string, string[]> chunker)
        {
            if (String.IsNullOrEmpty(original)) return new string[0];

            var pieces = chunker(original);

            return pieces.Select(p =>
            {
                if (ignoreWhiteSpace) p = p.Trim();
                if (ignoreCase) p = p.ToUpperInvariant();
                return p;
            });
        }

        public DiffResult<string> CreateCustomDiffs(string oldText, string newText, bool ignoreWhiteSpace, bool ignoreCase, Func<string, string[]> chunker)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");
            if (chunker == null) throw new ArgumentNullException("chunker");

            var oldStream = ApplyStringChunking(oldText, ignoreWhiteSpace, ignoreCase, chunker);
            var newStream = ApplyStringChunking(newText, ignoreWhiteSpace, ignoreCase, chunker);

            return new DiffEngine<string>().CreateDiffs(oldStream, newStream);
        }

        private static string NormalizeNewlines(string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        private static string[] SmartSplit(string str, IEnumerable<char> delims)
        {
            var list = new List<string>();
            int begin = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (delims.Contains(str[i]))
                {
                    list.Add(str.Substring(begin, (i + 1 - begin)));
                    begin = i + 1;
                }
                else if (i >= str.Length - 1)
                {
                    list.Add(str.Substring(begin, (i + 1 - begin)));
                }
            }

            return list.ToArray();
        }
    }
}