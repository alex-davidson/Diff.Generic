using System;
using System.Collections.Generic;
using System.Linq;
using Diff.Generic.Algorithm;
using Diff.Generic.Model;

namespace Diff.Generic
{
    /// <summary>
    /// Generic implementation of the diffing algorithm. Operates on any type T for which equality
    /// can be defined.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DiffEngine<T> : IDiffEngine<T>
    {
        public DiffResult<T> CreateDiffs(IList<T> oldStream, IList<T> newStream, IEqualityComparer<T> equalityComparer)
        {
            return CreateDiffs(oldStream, newStream, new EqualityPreservingChunkMapper<T>(equalityComparer));
        }

        public DiffResult<T> CreateDiffs(IList<T> oldStream, IList<T> newStream)
        {
            return CreateDiffs(oldStream, newStream, new EqualityPreservingChunkMapper<T>());
        }

        private static DiffResult<T> CreateDiffs(IList<T> oldStream, IList<T> newStream, EqualityPreservingChunkMapper<T> chunkMap)
        {
            // We map our chunks to integers such that equal chunks are represented by the same integer.
            // This allows the entire algorithm to be defined over known types for which comparison is fast.
            var oldStreamAsIntegers = oldStream.Select(chunkMap.GetHash).ToArray();
            var newStreamAsIntegers = newStream.Select(chunkMap.GetHash).ToArray();

            // For each chunk in each stream, determine if it counts as a modification.
            // This is the core of the diffing engine.
            var modifications = IntegerStreamDiffer.Instance.GetModifications(oldStreamAsIntegers, newStreamAsIntegers);

            // Finally, aggregate adjacent modifications for the convenience of consuming code.
            var blocks = CondenseToBlocks(oldStream, newStream, modifications);

            return new DiffResult<T>(oldStream.ToArray(), newStream.ToArray(), blocks);
        }

        private static List<DiffBlock> CondenseToBlocks(IList<T> oldStream, IList<T> newStream, Modifications modifications)
        {
            var piecesALength = oldStream.Count;
            var piecesBLength = newStream.Count;
            var posA = 0;
            var posB = 0;
            var lineDiffs = new List<DiffBlock>();

            do
            {
                // Find next modification on either side.
                while (posA < piecesALength
                       && posB < piecesBLength
                       && !modifications.Old[posA]
                       && !modifications.New[posB])
                {
                    posA++;
                    posB++;
                }

                var beginA = posA;
                var beginB = posB;
                // Find the end of this sequence of modifications in A
                for (; posA < piecesALength && modifications.Old[posA]; posA++) ;

                // Find the end of this sequence of modifications in B
                for (; posB < piecesBLength && modifications.New[posB]; posB++) ;

                // Create a block from the consecutive modifications on each side.
                var deleteCount = posA - beginA;
                var insertCount = posB - beginB;
                if (deleteCount > 0 || insertCount > 0)
                {
                    lineDiffs.Add(new DiffBlock(beginA, deleteCount, beginB, insertCount));
                }
            } while (posA < piecesALength && posB < piecesBLength);
            return lineDiffs;
        }
    }
}