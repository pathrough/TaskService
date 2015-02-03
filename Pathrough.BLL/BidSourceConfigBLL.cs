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
        void Insert(BidSourceConfig entity, IInsertHandler handler);

        List<BidSourceConfig> GetList(string areaNo,int pageIndex,int pageSize,out int pageCount,out int recordCount);
    }
    public class BidSourceConfigBLL : BLLBase<BidSourceConfig>, IBidSourceConfigBLL
    {
        IBidSourceConfigDAL dal;
        public BidSourceConfigBLL()
            : base(new BidSourceConfigDAL())
        {
            dal = (IBidSourceConfigDAL)dalService;
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
            var existEntity = dal.GetEntityByUrl(entity.ListUrl);
            if (existEntity==null)
            {
                dal.Insert(entity);
            }        
            else
            {
                existEntity.ListUrl = entity.ListUrl;
                existEntity.DetailUrlPattern = entity.DetailUrlPattern;
                existEntity.TitleXpath = entity.TitleXpath;
                existEntity.ContentXpath = entity.ContentXpath;
                existEntity.PubishDateXpath = entity.PubishDateXpath;
                existEntity.PubishDatePattern = entity.PubishDatePattern;
                existEntity.AreaName = entity.AreaName;
                existEntity.AreaNo = entity.AreaNo;
                dal.Update(existEntity);
            }
        }


        public List<BidSourceConfig> GetList(string areaNo,int pageIndex,int pageSize,out int pageCount,out int recordCount)
        {
            return dal.GetList(areaNo, pageIndex, pageSize, out pageCount, out recordCount);
        }
    }
}
