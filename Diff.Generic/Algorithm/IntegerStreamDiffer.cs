namespace Diff.Generic.Algorithm
{
    public class IntegerStreamDiffer
    {
        public static readonly IntegerStreamDiffer Instance = new IntegerStreamDiffer();

        public Modifications GetModifications(int[] old, int[] @new)
        {
            return new ModificationData(old, @new).Build();
        }

        class ModificationData
        {
            private readonly int[] old;
            private readonly int[] @new;
            private readonly EditLengthCalculator calc;
            private readonly Modifications modifications;

            public ModificationData(int[] old, int[] @new)
            {
                this.old = old;
                this.@new = @new;
                calc = new EditLengthCalculator(old, @new);
                modifications = new Modifications(old.Length, @new.Length);
            }

            public Modifications Build()
            {
                BuildModificationData(0, old.Length, 0, @new.Length);
                return modifications;
            }

            private void BuildModificationData(int startA, int endA, int startB, int endB)
            {
                while (startA < endA && startB < endB && old[startA] == @new[startB])
                {
                    startA++;
                    startB++;
                }
                while (startA < endA && startB < endB && old[endA - 1] == @new[endB - 1])
                {
                    endA--;
                    endB--;
                }

                var aLength = endA - startA;
                var bLength = endB - startB;
                if (aLength > 0 && bLength > 0)
                {
                    var res = calc.CalculateEditLength(startA, endA, startB, endB);
                    if (res.EditLength <= 0) return;

                    if (res.LastEdit == Edit.DeleteRight && res.StartX - 1 > startA)
                        modifications.Old[--res.StartX] = true;
                    else if (res.LastEdit == Edit.InsertDown && res.StartY - 1 > startB)
                        modifications.New[--res.StartY] = true;
                    else if (res.LastEdit == Edit.DeleteLeft && res.EndX < endA)
                        modifications.Old[res.EndX++] = true;
                    else if (res.LastEdit == Edit.InsertUp && res.EndY < endB)
                        modifications.New[res.EndY++] = true;

                    BuildModificationData(startA, res.StartX, startB, res.StartY);

                    BuildModificationData(res.EndX, endA, res.EndY, endB);
                }
                else if (aLength > 0)
                {
                    for (var i = startA; i < endA; i++)
                        modifications.Old[i] = true;
                }
                else if (bLength > 0)
                {
                    for (var i = startB; i < endB; i++)
                        modifications.New[i] = true;
                }
            }
        }
    }
}