using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diff.Generic.Performance
{
    class Program
    {
        private class DiffWorkload
        {
            public string OldText { get; private set; }
            public string NewText { get; private set; }

            public DiffWorkload(string oldText, string newText)
            {
                OldText = oldText;
                NewText = newText;
            }
        }

        private static DiffWorkload CreateWorkload(BodyOfTextGenerator generator)
        {
            var oldLines = generator.GenerateLines(20000);
            var newLines = generator.MakeDifferent(oldLines);
            return new DiffWorkload(String.Join(Environment.NewLine, oldLines), String.Join(Environment.NewLine, newLines));
        }

        public static void Main(string[] args)
        {
            var diffplexImplementation = new Differ();
            var diffGenericImplementation = new Differ();

            var textGenerator = new BodyOfTextGenerator();

            var profiler = new Profiler<DiffWorkload>(() => CreateWorkload(textGenerator), 
                new ProfilerTask<DiffWorkload>("DiffPlex", w => diffplexImplementation.CreateLineDiffs(w.OldText, w.NewText, false, false)),
                new ProfilerTask<DiffWorkload>("Diff.Generic", w => diffGenericImplementation.CreateLineDiffs(w.OldText, w.NewText, false, false)));

            Console.WriteLine("Warming up...");
            profiler.WarmUp();

            profiler.Run(Console.Out);
        }


    }
}
