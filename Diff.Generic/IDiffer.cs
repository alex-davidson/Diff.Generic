using System;
using Diff.Generic.Model;

namespace Diff.Generic
{
    /// <summary>
    /// Text-related diffing functions.
    /// </summary>
    public interface IDiffer
    {
        DiffResult<string> CreateLineDiffs(string oldText, string newText, bool ignoreWhitespace);
        DiffResult<string> CreateLineDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase);
        DiffResult<string> CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace);
        DiffResult<string> CreateCharacterDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase);
        DiffResult<string> CreateWordDiffs(string oldText, string newText, bool ignoreWhitespace, char[] separators);
        DiffResult<string> CreateWordDiffs(string oldText, string newText, bool ignoreWhitespace, bool ignoreCase, char[] separators);
        DiffResult<string> CreateCustomDiffs(string oldText, string newText, bool ignoreWhiteSpace, Func<string, string[]> chunker);
        DiffResult<string> CreateCustomDiffs(string oldText, string newText, bool ignoreWhiteSpace, bool ignoreCase, Func<string, string[]> chunker);
    }
}