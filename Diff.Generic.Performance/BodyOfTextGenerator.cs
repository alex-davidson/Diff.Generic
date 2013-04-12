using System;
using System.Collections.Generic;
using System.Linq;

namespace Diff.Generic.Performance
{
    public class BodyOfTextGenerator
    {
        private readonly int maxLineLength;

        public BodyOfTextGenerator(int maxLineLength = 150)
        {
            this.maxLineLength = maxLineLength;
        }

        public IList<string> MakeDifferent(IList<string> lines, double differenceAmount = 0.2)
        {
            var random = new Random();
            var newLines = new List<string>();
            foreach (var i in Enumerable.Range(0, lines.Count))
            {
                if(random.NextDouble() <= differenceAmount)
                {
                    // Either delete line or add different one
                    if(random.Next(2) % 2 == 1)
                    {
                        newLines.Add(RandomString(maxLineLength));
                    }

                }
                else
                {
                    newLines.Add(lines[i]);
                }
            }

            return newLines;

        }

        public IList<string> GenerateLines(int lines = 8000)
        {
            return Enumerable.Range(0, lines).Select(i => RandomString(maxLineLength)).ToList();
        }


        private static readonly char[] CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private static string RandomString(int maxLength)
        {
            var random = new Random();
            var chars = new char[random.Next(1, maxLength)];
            for (var i = 0; i < chars.Length; i++)
            {
                chars[i] = RandomChar(random);
            }
            var str = new String(chars);

            return random.Next(2) % 2 == 0 ? str.ToLower() : str;
        }

        private static char RandomChar(Random random)
        {
            return CharacterSet[random.Next(CharacterSet.Length)];
        }
    }
}
