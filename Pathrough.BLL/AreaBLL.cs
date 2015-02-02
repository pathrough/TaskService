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
    public class AreaBLL:BLLBase<Area>
    {
        public AreaBLL()
            : base(new AreaDAL())
        {            
        }
    }
}
