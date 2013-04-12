using System;

namespace Diff.Generic.Algorithm
{
    public class EditLengthCalculator
    {
        private readonly int[] old;
        private readonly int[] @new;
        private readonly int[] forwardDiagonal;
        private readonly int[] reverseDiagonal;

        public EditLengthCalculator(int[] old, int[] @new)
        {
            if (old == null) throw new ArgumentNullException("old");
            if (@new == null) throw new ArgumentNullException("new");
            this.old = old;
            this.@new = @new;
            var max = @new.Length + old.Length + 1;

            forwardDiagonal = new int[max + 1];
            reverseDiagonal = new int[max + 1];
        }

        public EditLengthResult CalculateEditLength(int startA, int endA, int startB, int endB)
        {
            if (old.Length == 0 && @new.Length == 0)
            {
                return new EditLengthResult();
            }

            var N = endA - startA;
            var M = endB - startB;
            var MAX = M + N + 1;
            var HALF = MAX / 2;
            var delta = N - M;
            var deltaEven = delta % 2 == 0;
            forwardDiagonal[1 + HALF] = 0;
            reverseDiagonal[1 + HALF] = N + 1;

            Log.WriteLine("Comparing strings");
            Log.WriteLine("\t{0} of length {1}", old, old.Length);
            Log.WriteLine("\t{0} of length {1}", @new, @new.Length);

            for (var D = 0; D <= HALF; D++)
            {
                Log.WriteLine("\nSearching for a {0}-Path", D);
                // forward D-path
                Log.WriteLine("\tSearching for foward path");
                Edit lastEdit;
                for (var k = -D; k <= D; k += 2)
                {
                    Log.WriteLine("\n\t\tSearching diagonal {0}", k);
                    var kIndex = k + HALF;
                    int x;
                    if (k == -D || (k != D && forwardDiagonal[kIndex - 1] < forwardDiagonal[kIndex + 1]))
                    {
                        x = forwardDiagonal[kIndex + 1]; // y up    move down from previous diagonal
                        lastEdit = Edit.InsertDown;
                        Log.Write("\t\tMoved down from diagonal {0} at ({1},{2}) to ", k + 1, x, (x - (k + 1)));
                    }
                    else
                    {
                        x = forwardDiagonal[kIndex - 1] + 1; // x up     move right from previous diagonal
                        lastEdit = Edit.DeleteRight;
                        Log.Write("\t\tMoved right from diagonal {0} at ({1},{2}) to ", k - 1, x - 1, (x - 1 - (k - 1)));
                    }
                    int y = x - k;
                    var startX = x;
                    var startY = y;
                    Log.WriteLine("({0},{1})", x, y);
                    while (x < N && y < M && old[x + startA] == @new[y + startB])
                    {
                        x += 1;
                        y += 1;
                    }
                    Log.WriteLine("\t\tFollowed snake to ({0},{1})", x, y);

                    forwardDiagonal[kIndex] = x;

                    if (!deltaEven)
                    {
                        if (k - delta >= (-D + 1) && k - delta <= (D - 1))
                        {
                            var revKIndex = (k - delta) + HALF;
                            var revX = reverseDiagonal[revKIndex];
                            var revY = revX - k;
                            if (revX <= x && revY <= y)
                            {
                                var res = new EditLengthResult
                                    {
                                        EditLength = 2*D - 1,
                                        StartX = startX + startA,
                                        StartY = startY + startB,
                                        EndX = x + startA,
                                        EndY = y + startB,
                                        LastEdit = lastEdit
                                    };
                                return res;
                            }
                        }
                    }
                }

                // reverse D-path
                Log.WriteLine("\n\tSearching for a reverse path");
                for (var k = -D; k <= D; k += 2)
                {
                    Log.WriteLine("\n\t\tSearching diagonal {0} ({1})", k, k + delta);
                    var kIndex = k + HALF;
                    int x;
                    if (k == -D || (k != D && reverseDiagonal[kIndex + 1] <= reverseDiagonal[kIndex - 1]))
                    {
                        x = reverseDiagonal[kIndex + 1] - 1; // move left from k+1 diagonal
                        lastEdit = Edit.DeleteLeft;
                        Log.Write("\t\tMoved left from diagonal {0} at ({1},{2}) to ", k + 1, x + 1, ((x + 1) - (k + 1 + delta)));
                    }
                    else
                    {
                        x = reverseDiagonal[kIndex - 1]; //move up from k-1 diagonal
                        lastEdit = Edit.InsertUp;
                        Log.Write("\t\tMoved up from diagonal {0} at ({1},{2}) to ", k - 1, x, (x - (k - 1 + delta)));
                    }
                    var y = x - (k + delta);

                    var endX = x;
                    var endY = y;

                    Log.WriteLine("({0},{1})", x, y);
                    while (x > 0 && y > 0 && old[startA + x - 1] == @new[startB + y - 1])
                    {
                        x -= 1;
                        y -= 1;
                    }

                    Log.WriteLine("\t\tFollowed snake to ({0},{1})", x, y);
                    reverseDiagonal[kIndex] = x;

                    if (deltaEven)
                    {
                        if (k + delta >= -D && k + delta <= D)
                        {
                            int forKIndex = (k + delta) + HALF;
                            int forX = forwardDiagonal[forKIndex];
                            int forY = forX - (k + delta);
                            if (forX >= x && forY >= y)
                            {
                                var res = new EditLengthResult
                                    {
                                        EditLength = 2 * D,
                                        StartX = x + startA,
                                        StartY = y + startB,
                                        EndX = endX + startA,
                                        EndY = endY + startB,
                                        LastEdit = lastEdit
                                    };
                                return res;
                            }
                        }
                    }
                }
            }


            throw new Exception("Should never get here");
        }
    }
}