﻿using Pathrough.Entity;
using Pathrough.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathrough.EF.Common;

namespace Pathrough.EF
{
    public class BidSourceConfigDAL : DALBase<BidSourceConfig>, IBidSourceConfigDAL
    {
        public override void Insert(BidSourceConfig entity)
        {
            _Context.BidSourceConfigs.Add(entity);
            _Context.SaveChanges();
        }

        public List<BidSourceConfig> GetAll()
        {
            return  _Context.BidSourceConfigs.ToList();
        }

        public List<BidSourceConfig> GetList(string areaNo, int pageIndex, int pageSize, out int pageCount, out int recordCount)
        {
            var query = this._Context.BidSourceConfigs.AsQueryable < BidSourceConfig>();
            if(string.IsNullOrWhiteSpace(areaNo))
            {
                query.Where(d=>d.AreaNo.StartsWith(areaNo));
            }
            query = query.OrderBy(d=>d.BscID);
            return query.TakePage<BidSourceConfig>(pageIndex, pageSize, out pageCount, out recordCount).ToList();
        }
    }
}
