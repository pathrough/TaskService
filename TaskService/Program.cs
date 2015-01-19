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

            //Dictionary<string, string> patern = new Dictionary<string, string>{
            //    {"{obj}和{obj}有很大的不同",""},
            //    {"对于{obj}的情形",""},
            //    {"在{}时",""},
            //    {"首先，{}","首先，如果你家庭条件负担得起的话，一定要和麻麻申请购买衣服的资金！"},
            //    {"如果{}的话，{}","如果你家庭条件负担得起的话，一定要和麻麻申请购买衣服的资金！"}
                
                
            //};
            string instance = "精算和风险管理 有很大的不同。";
        }
    }
}
