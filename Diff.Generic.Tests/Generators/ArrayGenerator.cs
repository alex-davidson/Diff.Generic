using System;
using System.Collections.Generic;

namespace Diff.Generic.Tests.Generators
{
    class ArrayGenerator
    {
        private readonly int minLength;
        private readonly int maxLength;
        private readonly Random random = new Random();

        public ArrayGenerator()
            : this(0, 1005)
        {
        }

        private ArrayGenerator(int minLength, int maxLength)
        {
            if (minLength < 0) throw new ArgumentNullException("minLength");
            if (maxLength < 0) throw new ArgumentNullException("maxLength");

            this.maxLength = maxLength;
            this.minLength = minLength;
        }

        public void Generate(out int[] aArr, out int[] bArr, out int editLength)
        {
            var a = new List<int>();
            var b = new List<int>();

            int aLength = random.Next(minLength, maxLength);
            int bLength = random.Next(minLength, maxLength);

            int commonLength = random.Next(0, Math.Min(aLength, bLength));
            editLength = (aLength + bLength) - 2 * commonLength;

            const int someInt = 1;
            for (int j = 0; j < commonLength; j++)
            {
                a.Add(someInt);
                b.Add(someInt);
            }

            while (a.Count < aLength)
                a.Add(2);
            while (b.Count < bLength)
                b.Add(3);
            Shuffle(a);
            Shuffle(b);

            aArr = a.ToArray();
            bArr = b.ToArray();
        }

        private void Shuffle(IList<int> arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                int temp = arr[i];
                int rand = random.Next(0, arr.Count - 1);
                arr[i] = arr[rand];
                arr[rand] = temp;
            }
        }
    }
}