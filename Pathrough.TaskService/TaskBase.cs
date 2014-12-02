using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pathrough.TaskService
{
   
    public abstract class TaskBase
    {
        public TaskBase()
        {
            RunEnabled = true;
        }
        public void StartRun()
        {
            while (true)
            {
                if (AbortBeforeNextRun)
                {
                    Console.WriteLine("Abort:" + this.GetType());
                    Thread.CurrentThread.Abort();
                }
                else
                {
                    var now = DateTime.Now;
                    if (RunEnabled && RunSartTime < now && RunEndTime > now)
                    {
                        Console.WriteLine("Run:" + this.GetType());
                        Run();
                    }
                    else
                    {
                        Console.WriteLine("NotRun:" + this.GetType());
                    }
                    Thread.Sleep(this.RunPeriod);
                }
               
            };
        }
        protected abstract void Run();
        protected abstract DateTime RunSartTime { get; }
        protected abstract DateTime RunEndTime { get; }
        public abstract int RunPeriod { get; }
        public bool RunEnabled { get; set; }

        public bool AbortBeforeNextRun { get; set; }

    }

    
}
