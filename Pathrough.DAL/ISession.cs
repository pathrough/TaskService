using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.DAL
{
    public interface ISession
    {
        IBidDAL OrderDal { get; }
        //更多实体dal
        
    }
}
