using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.TaskService.Tasks
{
    public class NewsEmailRemindTask : TaskBase
    {

        protected override void Run()
        {
            Console.WriteLine(DateTime.Now.ToString());
        }

        protected override DateTime RunStartTime
        {
            get 
            {
                return DateTime.Now.AddDays(-1);
            }
        }

        protected override DateTime RunEndTime
        {
            get 
            {
                return DateTime.Now.AddDays(1);
            }
        }

        public override int RunPeriod
        {
            get 
            {
                return 1000;
            }
        }
    }
}
