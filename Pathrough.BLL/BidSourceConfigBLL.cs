using Pathrough.EF;
using Pathrough.Entity;
using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.BLL
{
    public interface IInsertHandler
    {
        void ParameterInvalid();
    }
    public interface IBidSourceConfigBLL
    {
        List<BidSourceConfig> GetAll();
        void Insert(BidSourceConfig entity, IInsertHandler handler);

        List<BidSourceConfig> GetList(string areaNo,int pageIndex,int pageSize,out int pageCount,out int recordCount);
    }
    public class BidSourceConfigBLL : BLLBase<BidSourceConfig>, IBidSourceConfigBLL
    {
        public BidSourceConfigBLL()
            : base(new BidSourceConfigDAL())
        {
            //base.dalService = new BidSourceConfigDAL();
        }
        public List<BidSourceConfig> GetAll()
        {
            BidSourceConfigDAL dal = this.dalService as BidSourceConfigDAL;
            return dal.GetAll();
        }

        public void Insert(BidSourceConfig entity,IInsertHandler handler)
        {
            if (entity == null
                ||string.IsNullOrWhiteSpace(entity.ListUrl)
                ||string.IsNullOrWhiteSpace(entity.DetailUrlPattern)
                ||string.IsNullOrWhiteSpace(entity.TitleXpath)
                || string.IsNullOrWhiteSpace(entity.ContentXpath)
                || string.IsNullOrWhiteSpace(entity.PubishDateXpath)
                || string.IsNullOrWhiteSpace(entity.PubishDatePattern)
                )
            {
                handler.ParameterInvalid();
                return;
            }           
            dalService.Insert(entity);
        }


        public List<BidSourceConfig> GetList(string areaNo,int pageIndex,int pageSize,out int pageCount,out int recordCount)
        {
            return (dalService as IBidSourceConfigDAL).GetList(areaNo, pageIndex, pageSize, out pageCount, out recordCount);
        }
    }
}
