using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Diff.Generic.Model;

namespace Diff.Generic
{
    /// <summary>
    /// Text-related diffing functions.
    /// </summary>
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


            return CreateCustomDiffs(oldText, newText, ignoreWhitespace, ignoreCase,
                                     str => NormalizeNewlines(str).Split('\n'));
        }

        public DiffResult<string> CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace)
        {
            return CreateCharacterDiffs(oldText, newText, ignoreWhitespace, false);
        }

        public DiffResult<string> CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace,
                                                       bool ignoreCase)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");


            return CreateCustomDiffs(
                oldText,
                newText,
                ignoreWhitespace,
                ignoreCase,
                str => str.ToCharArray().Select(Char.ToString).ToArray());
        }

        public DiffResult<string> CreateWordDiffs(string oldText, string newText, bool ignoreWhitespace, char[] separators)
        {
            return CreateWordDiffs(oldText, newText, ignoreWhitespace, false, separators);
        }

        public DiffResult<string> CreateWordDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase, char[] separators)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");

            var splitter = CreateSmartSplitter(separators);

            return CreateCustomDiffs(
                oldText,
                newText,
                ignoreWhitespace,
                ignoreCase,
                splitter);
        }

        public DiffResult<string> CreateCustomDiffs(string oldText, string newText, bool ignoreWhiteSpace, Func<string, string[]> chunker)
        {
            return CreateCustomDiffs(oldText, newText, ignoreWhiteSpace, false, chunker);
        }

        class TextChunkEqualityComparer : IEqualityComparer<string>
        {
            private readonly bool ignoreWhiteSpace;
            private readonly IEqualityComparer<string> baseComparer;
            public TextChunkEqualityComparer(bool ignoreWhiteSpace, bool ignoreCase)
            {
                this.ignoreWhiteSpace = ignoreWhiteSpace;
                baseComparer = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
            }

            private string Transform(string s)
            {
                return ignoreWhiteSpace ? s.Trim() : s;
            }

            public bool Equals(string x, string y)
            {
                return baseComparer.Equals(Transform(x), Transform(y));
            }

            public int GetHashCode(string obj)
            {
                return baseComparer.GetHashCode(Transform(obj));
            }
        }

        private static IList<string> ApplyStringChunking(string original, Func<string, string[]> chunker)
        {
            if (String.IsNullOrEmpty(original)) return new string[0];

            var pieces = chunker(original);

            return pieces.ToList();
        }

        public DiffResult<string> CreateCustomDiffs(string oldText, string newText, bool ignoreWhiteSpace,
                                                    bool ignoreCase, Func<string, string[]> chunker)
        {
            return CreateCustomDiffs(oldText, newText, chunker,
                                     new TextChunkEqualityComparer(ignoreWhiteSpace, ignoreCase));
            
        }

        public DiffResult<string> CreateCustomDiffs(string oldText, string newText, Func<string, string[]> chunker, IEqualityComparer<string> equalityComparer)
        {
            if (oldText == null) throw new ArgumentNullException("oldText");
            if (newText == null) throw new ArgumentNullException("newText");
            if (chunker == null) throw new ArgumentNullException("chunker");

            var oldStream = ApplyStringChunking(oldText, chunker);
            var newStream = ApplyStringChunking(newText, chunker);

            return new DiffEngine<string>().CreateDiffs(oldStream, newStream, equalityComparer);
        }

        private static string NormalizeNewlines(string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        private static Func<string, string[]> CreateSmartSplitter(IEnumerable<char> delimiters)
        {
            var delimiterHash = new HashSet<char>(delimiters);
            return s => SmartSplit(s, delimiterHash);
        }

        
        private static string[] SmartSplit(string str, ICollection<char> delims)
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