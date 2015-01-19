using Pathrough.BLL;
using Pathrough.BLL.Spider;
using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.TaskService.Tasks
{
    public class BidSourceAccessTask : TaskBase
    {
        protected override void Run()
        {
            BidWebsiteSpider sp = new BidWebsiteSpider();
            
            var config = new BidSourceConfig
            {
                ListUrl = "http://www.chinabidding.com/zbzx.jhtml?method=outlineOne&type=biddingProjectGG&channelId=205"
                ,
                DetailUrlPattern = @"http://www.chinabidding.com/zbzx-detail-\d+.html"
                ,
                TitleXpath = "/html/body/div/div[2]/div[2]/div[1]/div/h2"
                ,
                ContentXpath = "/html/body/div/div[2]/div[2]/div[1]/div/div[2]"
                ,
                PubishDateXpath = "/html/body/div/div[2]/div[2]/div[1]/div/div[1]"
                ,
                PubishDatePattern = @"(\d{4}\.\d{2}\.\d{2})"
            };

            //BidSourceConfigBLL configService = new BidSourceConfigBLL();

            //configService.Insert(config);

            var bidList = sp.DownLoadBids(config);
            BidBLL bidService = new BidBLL();
            foreach (var entity in bidList)
            {
                bidService.Insert(entity);
            }
        }

        protected override DateTime RunStartTime
        {
            get { return DateTime.Now.AddSeconds(-5); }
        }

        protected override DateTime RunEndTime
        {
            get { return DateTime.Now.AddYears(10); }
        }

        public override int RunPeriod
        {
            get { return 1000 * 60 * 15; }
        }
    }
}
