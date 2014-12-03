using Pathrough.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.ADO
{
    public class ADOSession:ISession
    {
        public IDAL.IBidDAL OrderDal
        {
            get { throw new NotImplementedException(); }
        }
    }
}
