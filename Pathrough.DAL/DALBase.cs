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
        protected IDALBase<T> dalActive;

        public bool Insert(T entity)
        {
            return dalActive.Insert(entity);
        }

        public bool Update(T entity)
        {
            return dalActive.Update(entity);
        }

        public bool Delete(T entity)
        {
            return dalActive.Delete(entity);
        }

        public T Query(int ID)
        {
            return dalActive.Query(ID);
        }
    }
}
