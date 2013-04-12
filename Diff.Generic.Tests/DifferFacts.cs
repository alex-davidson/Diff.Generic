using System;
using Diff.Generic.Model;
using Xunit;
using Xunit.Extensions;

namespace Diff.Generic.Tests
{
    public class DifferFacts
    {
        public class CreateCustomDiffs
        {
            [Fact]
            public void Will_throw_if_oldText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateCustomDiffs(null, "someString", false, null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("oldText", ex.ParamName);
            }

            [Fact]
            public void Will_throw_if_newText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateCustomDiffs("someString", null, false, null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("newText", ex.ParamName);
            }

            [Fact]
            public void Will_throw_if_chunker_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateCustomDiffs("someString", "otherString", false, null)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("chunker", ex.ParamName);
            }
        }

        public class CreateLineDiffs
        {
            [Fact]
            public void Will_throw_if_oldText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateLineDiffs(null, "someString", false)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("oldText", ex.ParamName);
            }

            [Fact]
            public void Will_throw_if_newText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateLineDiffs("someString", null, false)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("newText", ex.ParamName);
            }

            [Fact]
            public void Will_return_list_of_length_zero_if_there_are_no_differences()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("matt\ncatt\nhat\n", "matt\ncatt\nhat\n", false);

                Assert.NotNull(res);
                Assert.Equal(0, res.DiffBlocks.Count);
            }

            [Fact]
            public void Will_return_correct_diff_block_when_oldText_is_empty_and_newText_is_non_empty()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("", "matt\npat\nhat\n", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(0, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_diff_block_when_newText_is_empty_and_oldText_is_non_empty()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("matt\npat\nhat\n", "", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(4, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(0, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_for_strings_with_one_difference()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("matt\ncat\nhat\n", "matt\npat\nhat\n", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(1, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(1, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(1, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_when_no_deletions()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("matt\ncat\nhat\n", "matt\ncat\ntat\nhat\n", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(2, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(1, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_when_no_insertions()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("matt\r\ncat\ntat\r\nhat\n", "matt\ncat\nhat\n", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(2, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_for_strings_with_multiple_difference()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("a\nb\nc\nd", "a\nb\ne\nf\ng\nh", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(2, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(2, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_two_item_list_for_strings_with_multiple_difference_non_conesecutivly()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("z\ra\nb\rc\nd", "x\nv\na\rb\ne\nf\ng\nh", false);

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_two_item_list_for_strings_with_multiple_difference_non_conesecutivly_and_ignoring_whitespace()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("z\t\na  \n b\nc\n\t\td", "x\nv\n a\nb\n e\nf\t\ng\nh\t\t", true);

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }

            [Fact]
            public void Will_ignore_case_when_ignore_case_is_true()
            {
                var differ = new Differ();

                var res = differ.CreateLineDiffs("z\t\na  \n b\nc\n\t\td", "X\nV\n A\nB\n E\nF\t\nG\nH\t\t", true, true);

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }
        }


        public class CreateCharacterDiffs
        {
            [Fact]
            public void Will_throw_if_oldText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateCharacterDiffs(null, "someString", false)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("oldText", ex.ParamName);
            }

            [Fact]
            public void Will_throw_if_newText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateCharacterDiffs("someString", null, false)) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("newText", ex.ParamName);
            }

            [Fact]
            public void Will_return_list_of_length_zero_if_there_are_no_differences()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("abc defg", "abc defg", false);

                Assert.NotNull(res);
                Assert.Equal(0, res.DiffBlocks.Count);
            }

            [Fact]
            public void Will_return_correct_diff_block_when_oldText_is_empty_and_newText_is_non_empty()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("", "ab c", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(0, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_diff_block_when_newText_is_empty_and_oldText_is_non_empty()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("xy w", "", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(4, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(0, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_for_strings_with_one_difference()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("xjzwv", "xyzwv", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(1, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(1, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(1, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_when_no_deletions()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("abce", "ab ce", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(2, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(1, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_when_no_insertions()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("ab ce", "abce", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(2, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_for_strings_with_multiple_difference()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("abcd", "abefgh", false);

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(2, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(2, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_two_item_list_for_strings_with_multiple_difference_non_conesecutivly()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("zabcd", "xvabefgh", false);

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }

            [Fact]
            public void Will_ignore_case_when_ignore_case_is_true()
            {
                var differ = new Differ();

                var res = differ.CreateCharacterDiffs("zabcd", "XVABEFGH", false, true);

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }
        }

        public class CreateWordDiffs
        {
            [Fact]
            public void Will_throw_if_oldText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateWordDiffs(null, "someString", false, new[] { ' ' })) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("oldText", ex.ParamName);
            }

            [Fact]
            public void Will_throw_if_newText_is_null()
            {
                var differ = new Differ();

                var ex = Record.Exception(() => differ.CreateWordDiffs("someString", null, false, new[] { ' ' })) as ArgumentNullException;

                Assert.NotNull(ex);
                Assert.Equal("newText", ex.ParamName);
            }

            [Fact]
            public void Will_return_list_of_length_zero_if_there_are_no_differences()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("abc defg", "abc defg", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(0, res.DiffBlocks.Count);
            }

            [Fact]
            public void Will_return_correct_diff_block_when_oldText_is_empty_and_newText_is_non_empty()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("", "ab c", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(0, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_diff_block_when_newText_is_empty_and_oldText_is_non_empty()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("xy w", "", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(0, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_for_strings_with_one_difference()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("x j zwv", "x y zwv", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(1, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(1, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(1, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_when_no_deletions()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("ab ce", "ab d ce", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(0, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(1, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(1, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_when_no_insertions()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("ab d ce", "ab ce", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(1, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_one_item_list_for_strings_with_multiple_difference()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("a b c d", "a b e f g h", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);
                Assert.Equal(2, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(2, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[0].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_two_item_list_for_strings_with_multiple_difference_non_conesecutivly()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("z a b c d  ", "x v a b e f g h  ", false, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }


            [Fact]
            public void Will_ignore_case_when_ignore_case_is_true()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs("z a b c d  ", "X V A B E F G H  ", false, true, new[] { ' ' });

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }


            [Theory]
            [InlineData(' ')]
            [InlineData(';')]
            [InlineData(',')]
            [InlineData('-')]
            [InlineData('(')]
            public void Will_return_correct_diff_for_arbitratry_separators(char separator)
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs(string.Format("z{0}a{0}b{0}c{0}d{0}{0}", separator),
                                                 string.Format("x{0}v{0}a{0}b{0}e{0}f{0}g{0}h{0}{0}", separator),
                                                 false,
                                                 new[] { separator });

                Assert.NotNull(res);
                Assert.Equal(2, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(1, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(2, res.DiffBlocks[0].InsertCountB);

                Assert.Equal(3, res.DiffBlocks[1].DeleteStartA);
                Assert.Equal(2, res.DiffBlocks[1].DeleteCountA);
                Assert.Equal(4, res.DiffBlocks[1].InsertStartB);
                Assert.Equal(4, res.DiffBlocks[1].InsertCountB);
            }

            [Fact]
            public void Will_return_correct_diff_for_different_separators()
            {
                var differ = new Differ();

                var res = differ.CreateWordDiffs(string.Format("z{0}a{0}b{0}c{0}d{0}{0}", ' '), string.Format("x{0}v{0}a{0}b{0}e{0}f{0}g{0}h{0}{0}", ';'), false, new[] { ' ', ';' });

                Assert.NotNull(res);
                Assert.Equal(1, res.DiffBlocks.Count);

                Assert.Equal(0, res.DiffBlocks[0].DeleteStartA);
                Assert.Equal(6, res.DiffBlocks[0].DeleteCountA);
                Assert.Equal(0, res.DiffBlocks[0].InsertStartB);
                Assert.Equal(9, res.DiffBlocks[0].InsertCountB);
            }
        }


    }
}