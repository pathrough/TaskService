using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF
{
    public abstract class DALBase<T> : IDALBase<T> where T : class
    {
        protected BidContext _Context;
        public DALBase()
        {
            _Context = new BidContext();
        }
        public virtual void Insert(T entity)
        {
            //利用反射
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


        public virtual T GetEntity(long ID)
        {
            throw new NotImplementedException();
        }
    }
}
