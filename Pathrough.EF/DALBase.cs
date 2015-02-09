using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathrough.EF.Common;
using System.Data.Entity.Core.Objects.DataClasses;

namespace Pathrough.EF
{
    public abstract class DALBase<T> : IDALBase<T> where T : class
    {
        protected BidContext _Context;
        public DALBase()
        {
            _Context = new BidContext();
        }
        //public virtual void Insert(T entity)
        public  void Insert(T entity)
        {
            _Context.Set<T>().Add(entity);
            _Context.SaveChanges();          
        }

        //public virtual void Update(T entity)
        public  void Update(T entity)
        {
            //_Context.Entry<T>(entity).GetValidationResult();
            if(_Context.Entry<T>(entity).State==EntityState.Modified)
            {
                _Context.SaveChanges();
            }            
        }

        //public virtual void Delete(T entity)
        public  void Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
            _Context.SaveChanges();
        }


        public  T GetEntity(long ID)
        {
            return _Context.Set<T>().Find(ID);
        }


        protected List<T> GetPageList(IQueryable<T> query, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {            
            return query.TakePage<T>(pageIndex, pageSize, out pageCount, out recordCount).ToList();
        }
    }
}
