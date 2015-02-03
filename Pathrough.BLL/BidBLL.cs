using Pathrough.Common;
using Pathrough.EF;
using Pathrough.Entity;
using Pathrough.Factory;
using Pathrough.IDAL;
using Pathrough.LuceneSE;
using Pathrough.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    public class BidBLL : BLLBase<Bid>//,IOrderBLL
    {
        IBidDAL bidDal;
        public BidBLL()
            : base(new BidDAL())
        {
            bidDal = (IBidDAL)this.dalService;
            ////初始化dal服务
            //this.dalService = DALFactory.CreateOrder<IBidDAL>
            //    (FactoryConfig.Bid.AssemblyPath, FactoryConfig.Bid.ClassName);
            //this.dalService = new BidDAL();
        }
        public void Insert(Bid bid)
        {
            var entity = bidDal.GetEntityByUrl(bid.BidSourceUrl);
            if(entity==null)
            {
                bidDal.Insert(bid);
            }            
        }

        //#region IOrderDAL的专用方法
        //public void 专用方法Dom()
        //{
        //    IBidDAL orderbll = dalService as IBidDAL;
        //    //调用IOrderDAL的专有方法
        //    //orderbll.专有方法();
        //}
        //#endregion

        public void CreateLuceneIndex(List<Bid> bidList)
        {
            foreach(var bid in bidList)
            {
                if (bid.WasIndexed == null || bid.WasIndexed == false)
                {
                    BidSearchEngine.Current.CreateIndex(new List<Bid> { bid });
                    bid.WasIndexed = true;
                    dalService.Update(bid);
                }
            }
        }
    }
}
