using Pathrough.TaskService.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pathrough.TaskService
{
    public class TaskManager
    {
        static TaskManager _instance = new TaskManager();
        private TaskManager()
        {
           
        }

        public static TaskManager Current
        {
            get
            {
                return _instance;
            }
        }

        public void Init()
        {
            var tasks = GetTaskList();
            foreach (var task in tasks)
            {
                ThreadStart myThreadDelegate = new ThreadStart(task.StartRun);
                Thread myThread = new Thread(myThreadDelegate);
                myThread.Start();
            }
        }
        List<TaskBase> GetTaskList()
        {
            return new List<TaskBase> { new BidSourceAccessTask() };
        }

    }
}
