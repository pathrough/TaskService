using Pathrough.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.IDAL
{
    public interface IBidDAL : IDALBase<Bid>
    {
        //可以添加bid相关的抽象方法
        Bid GetEntityByUrl(string url);
    }
}
