using System;

namespace Diff.Generic.Performance
{
    public class ProfilerTask<TWorkload>
    {
        private readonly Action<TWorkload> run;
        public string Name { get; private set; }

        public ProfilerTask(string name, Action<TWorkload> run)
        {
            this.run = run;
            Name = name;
        }

        public void Run(TWorkload workload)
        {
            run(workload);
        }
    }
}