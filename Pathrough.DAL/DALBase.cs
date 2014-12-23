using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.DAL
{
    public class DALBase<T> : IDALBase<T> where T : class
    {

        public virtual void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual T Query(long ID)
        {
            throw new NotImplementedException();
        }
    }
}
