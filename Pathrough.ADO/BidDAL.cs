using Pathrough.DAL;
using Pathrough.Entity;
using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.ADO
{
    public class BidDAL : DALBase<Bid>, IBidDAL
    {
        public BidDAL(IDbConnection dbConnection)
        {

        }
    }
}
