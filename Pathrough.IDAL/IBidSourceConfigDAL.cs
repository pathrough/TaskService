using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.IDAL
{
    public interface IBidSourceConfigDAL : IDALBase<BidSourceConfig>
    {
        List<BidSourceConfig> GetList(string areaNo, int pageIndex, int pageSize, out int pageCount, out int recordCount);
        BidSourceConfig GetEntityByUrl(string url);
    }
}
