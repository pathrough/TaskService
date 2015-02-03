using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathrough.EF.Common
{
    public static class QueryableExt
    {
        public static IQueryable<T> TakePage<T>(this IQueryable<T> query, int iPageIndex, int iPageSize, out int iPageCount, out int iRecordCount)
        {
            iPageIndex = iPageIndex <= 0 ? 1 : iPageIndex;
            int iSkinCount = (iPageIndex - 1) * iPageSize;
            iRecordCount = query.Count();
            iPageCount = iRecordCount / iPageSize;
            if (iPageSize * iPageCount < iRecordCount)
                iPageCount++;
            return query = query.Skip(iSkinCount)
                .Take(iPageSize);
        }

    }
}
