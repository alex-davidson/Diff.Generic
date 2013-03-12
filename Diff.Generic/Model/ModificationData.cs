using System.Collections.Generic;

namespace Diff.Generic.Model
{
    public class ModificationData<T>
    {
        public int[] HashedPieces { get; set; }

        public bool[] Modifications { get; set; }

        public T[] Pieces { get; set; }

        public ModificationData(params T[] pieces)
        {
            Pieces = pieces;
            Modifications = new bool[pieces.Length];
            HashedPieces = new int[pieces.Length];
        }

        public void PopulatePieceHash(IDictionary<T, int> pieceHash)
        {
            for (var i = 0; i < Pieces.Length; i++)
            {
                var piece = Pieces[i];
                
                if (pieceHash.ContainsKey(piece))
                {
                    HashedPieces[i] = pieceHash[piece];
                }
                else
                {
                    HashedPieces[i] = pieceHash.Count;
                    pieceHash[piece] = pieceHash.Count;
                }
            }
        }
    }
}