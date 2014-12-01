using Pathrough.TaskService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskService
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskManager.Current.Init();
        }
    }
}
