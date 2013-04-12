using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Diff.Generic.Performance
{
    public class Profiler<TWorkload>
    {
        private readonly Func<TWorkload> workloadGenerator;
        private readonly List<ProfilerTask<TWorkload>> tasks;

        public Profiler(Func<TWorkload> workloadGenerator, params ProfilerTask<TWorkload>[] tasks)
        {
            this.workloadGenerator = workloadGenerator;
            this.tasks = tasks.ToList();
        }

        /// <summary>
        /// Warms up the task implementations and the workload generator. Ensures that everything is jitted.
        /// </summary>
        public void WarmUp()
        {
            var workload = workloadGenerator();
            foreach (var task in tasks)
            {
                task.Run(workload);
            }
        }

        public void Run(TextWriter log, int iterations = 5)
        {
            log.Write("Generating {0} workloads...", iterations);
            var workloads = Enumerable.Range(0, iterations).Select(_ => workloadGenerator()).ToList();
            log.WriteLine("done.");
            log.WriteLine();
            log.WriteLine("Profiling {0} tasks.", tasks.Count);

            foreach (var task in tasks)
            {
                log.WriteLine();
                log.WriteLine("Task: {0}", task.Name);
                Profile(log, task.Run, workloads);
            }
            log.WriteLine();
        }

        public void Profile(TextWriter log, Action<TWorkload> task, IList<TWorkload> workloads)
        {
            log.WriteLine("Time before run: {0}", DateTime.Now);
            var times = new List<double>();
            var iterationTimer = new Stopwatch();
            foreach (var workload in workloads)
            {
                iterationTimer.Restart();
                task(workload);
                iterationTimer.Stop();
                times.Add(iterationTimer.ElapsedMilliseconds);
            }
            var totalTime = times.Sum();
            log.WriteLine("Time after run: {0}", DateTime.Now);
            log.WriteLine("Workloads Per Second: {0}", (workloads.Count/totalTime)*1000);
            log.WriteLine("Average: {0}ms", times.Average());
            log.WriteLine("Max time for a workload: {0}ms", times.Max());
            log.WriteLine("Min time for a workload: {0}ms", times.Min());
        }
    }
}
