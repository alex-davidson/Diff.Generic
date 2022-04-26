using System;
using Diff.Generic.Algorithm;
using Diff.Generic.Tests.Generators;
using Xunit;
using Xunit.Extensions;

namespace Diff.Generic.Tests.Algorithm
{
    public class EditLengthCalculatorTests
    {
        [Fact]
        public void Will_throw_if_arrays_are_null()
        {
            var ex = Record.Exception(() =>
                {
                    new EditLengthCalculator(null, null);
                }) as ArgumentNullException;

            Assert.NotNull(ex);
        }

        [Fact]
        public void Will_return_length_0_for_arrays()
        {
            var calc = new EditLengthCalculator(new int[0], new int[0]);
            var res = calc.CalculateEditLength(0, 0, 0, 0);

            Assert.Equal(0, res.EditLength);
        }

        [Fact]
        public void Will_return_length_of_a_if_b_is_empty()
        {
            var a = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var b = new int[] { };

            var calc = new EditLengthCalculator(a, b);
            var res = calc.CalculateEditLength(0, a.Length, 0, b.Length);

            Assert.Equal(a.Length, res.EditLength);
        }

        [Fact]
        public void Will_return_length_of_b_if_a_is_empty()
        {
            var b = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var a = new int[] { };

            var calc = new EditLengthCalculator(a, b);
            var res = calc.CalculateEditLength(0, a.Length, 0, b.Length);

            Assert.Equal(b.Length, res.EditLength);
        }

        [Fact]
        public void Will_return_correct_length_when_start_and_ends_are_changed()
        {
            var b = new int[] { 1, 2, 3, 0, 5, 6, 7, 8 };
            var a = new int[] { 4, 2, 3, 4, 5, 6, 7, 9 };

            var calc = new EditLengthCalculator(a, b);
            var res = calc.CalculateEditLength(1, a.Length - 1, 1, b.Length - 1);

            Assert.Equal(2, res.EditLength);
        }

        [Fact]
        public void Will_return_snake_of_zero_length_for_unique_arrays()
        {
            var a = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var b = new int[] { 11, 12, 23, 54, 56 };

            var calc = new EditLengthCalculator(a, b);
            var res = calc.CalculateEditLength(0, a.Length, 0, b.Length);

            Assert.Equal(res.StartX, res.EndX);
            Assert.Equal(res.StartY, res.EndY);
            Assert.Equal(a.Length + b.Length, res.EditLength);
        }

        [Theory]
        [ClassData(typeof(TestingEditLengthGenerator))]
        public void Will_return_correct_edit_length_random_strings(int[] a, int[] b, int actualEditLength)
        {
            var calc = new EditLengthCalculator(a, b);
            var res = calc.CalculateEditLength(0, a.Length, 0, b.Length);

            Assert.Equal(actualEditLength, res.EditLength);
        }
    }
}