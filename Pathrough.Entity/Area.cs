using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pathrough.Entity
{
    public class Area
    {
        [Key]
        public int AreaID { get; set; }
        public string AreaNo { get; set; }
        public string AreaName { get; set; }
    }
}
