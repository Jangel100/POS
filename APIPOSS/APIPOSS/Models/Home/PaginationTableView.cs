using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Home
{
    public class PaginationTableView
    {
        public int Id { get; set; }
        public int? page { get; set; }
        public int? pageSize { get; set; }
    }
}