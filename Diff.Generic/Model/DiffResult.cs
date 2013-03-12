using System.Collections.Generic;

namespace Diff.Generic.Model
{
    /// <summary>
    /// The result of diffing two pieces of text
    /// </summary>
    public class DiffResult<T>
    {
        /// <summary>
        /// The chunked pieces of the old text
        /// </summary>
        public T[] PiecesOld { get; private set; }

        /// <summary>
        /// The chunked pieces of the new text
        /// </summary>
        public T[] PiecesNew { get; private set; }


        /// <summary>
        /// A collection of DiffBlocks which details deletions and insertions
        /// </summary>
        public IList<DiffBlock> DiffBlocks { get; private set; }

        public DiffResult(T[] piecesOld, T[] piecesNew, IList<DiffBlock> blocks)
        {
            PiecesOld = piecesOld;
            PiecesNew = piecesNew;
            DiffBlocks = blocks;
        }
    }
}