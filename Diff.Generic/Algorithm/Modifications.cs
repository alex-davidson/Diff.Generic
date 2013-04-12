namespace Diff.Generic.Algorithm
{
    public class Modifications
    {
        public Modifications(int oldLength, int newLength)
        {
            Old = new bool[oldLength];
            New = new bool[newLength];
        }
        
        public bool[] Old { get; private set; }
        public bool[] New { get; private set; }
    }
}