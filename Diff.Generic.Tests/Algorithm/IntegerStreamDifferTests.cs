using System.Linq;
using Diff.Generic.Algorithm;
using Diff.Generic.Tests.Generators;
using Xunit;
using Xunit.Extensions;

namespace Diff.Generic.Tests.Algorithm
{
    public class IntegerStreamDifferTests
    {
        private readonly IntegerStreamDiffer integerStreamDiffer = new IntegerStreamDiffer();

        [Fact]
        public void Will_return_empty_modifications_for_empty_strings()
        {
            var modifications = integerStreamDiffer.GetModifications(new int[0], new int[0]);

            Assert.Empty(modifications.Old);
            Assert.Empty(modifications.New);
        }

        [Fact]
        public void Will_return_all_modifications_for_empty_vs_non_empty_string()
        {
            var modifications = integerStreamDiffer.GetModifications(new int[0], new[] { 1, 2, 3, 4 });

            foreach (var mod in modifications.New)
            {
                Assert.True(mod);
            }
        }

        [Fact]
        public void Will_return_all_modifications_for_non_empty_vs_empty_string()
        {
            var modifications = integerStreamDiffer.GetModifications(new[] { 1, 2, 3, 4 }, new int[0]);

            foreach (var mod in modifications.Old)
            {
                Assert.True(mod);
            }
        }

        [Fact]
        public void Will_return_no_modifications_for_same_strings()
        {
            var modifications = integerStreamDiffer.GetModifications(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 });

            foreach (var mod in modifications.Old)
            {
                Assert.False(mod);
            }
            foreach (var mod in modifications.New)
            {
                Assert.False(mod);
            }
        }

        [Fact]
        public void Will_return_all_modifications_for_unique_strings()
        {
            var modifications = integerStreamDiffer.GetModifications(new[] { 1, 2, 3, 4 }, new[] { 5, 6, 7, 8 });

            foreach (var mod in modifications.Old)
            {
                Assert.True(mod);
            }
            foreach (var mod in modifications.New)
            {
                Assert.True(mod);
            }
        }

        [Fact]
        public void Will_return_correct_modifications_two_partially_similiar_strings()
        {
            var modifications = integerStreamDiffer.GetModifications(new[] { 1, 2, 3, 4 }, new[] { 1, 4, 5 });

            Assert.False(modifications.Old[0]);
            Assert.True(modifications.Old[1]);
            Assert.True(modifications.Old[2]);
            Assert.False(modifications.Old[3]);

            Assert.False(modifications.New[0]);
            Assert.False(modifications.New[1]);
            Assert.True(modifications.New[2]);
        }

        [Fact]
        public void Will_return_correct_modifications_for_strings_with_two_differences()
        {
            var modifications = integerStreamDiffer.GetModifications(new[] { 1, 2, 3 }, new[] { 1, 4, 3 });

            Assert.False(modifications.Old[0]);
            Assert.True(modifications.Old[1]);
            Assert.False(modifications.Old[2]);

            Assert.False(modifications.New[0]);
            Assert.True(modifications.New[1]);
            Assert.False(modifications.New[2]);
        }

        [Fact]
        public void Will_return_correct_modifications_for_strings_with_one_difference()
        {
            var modifications = integerStreamDiffer.GetModifications(new[] { 1, 2, 3 }, new[] { 1, 2, 4, 3 });

            Assert.False(modifications.Old[0]);
            Assert.False(modifications.Old[1]);
            Assert.False(modifications.Old[2]);

            Assert.False(modifications.New[0]);
            Assert.False(modifications.New[1]);
            Assert.True(modifications.New[2]);
            Assert.False(modifications.New[3]);
        }

        [Theory]
        [ClassData(typeof(TestingEditLengthGenerator))]
        public void Will_return_correct_modifications_count_for_random_data(int[] aLines, int[] bLines, int editLength)
        {
            var modifications = integerStreamDiffer.GetModifications(aLines, bLines);

            var modCount = modifications.Old.Count(x => x == true) + modifications.New.Count(x => x == true);

            Assert.Equal(editLength, modCount);
        }
    }
}