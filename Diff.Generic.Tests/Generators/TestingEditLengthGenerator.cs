using System;
using System.Collections;
using System.Collections.Generic;

namespace Diff.Generic.Tests.Generators
{
    class TestingEditLengthGenerator : IEnumerable<object[]>
    {
        private readonly int count;
        private readonly ArrayGenerator generator = new ArrayGenerator();

        public TestingEditLengthGenerator()
            : this(50)
        {
        }

        private TestingEditLengthGenerator(int count)
        {
            if (count < 0) throw new ArgumentNullException("count");

            this.count = count;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                int[] a, b;
                int editLength;
                generator.Generate(out a, out b, out editLength);

                yield return new object[] { a, b, editLength };
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}