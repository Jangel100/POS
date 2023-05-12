using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class CPSepomex
    {
        public int c_colonia { get; set; }
        public int d_codigo { get; set; }
        public string d_asenta { get; set; }
        public string d_tipo_asenta { get; set; }
        public string d_mnpio { get; set; }
        public string d_estado { get; set; }
        public string d_ciudad { get; set; }
        public string d_CP { get; set; }
        public int c_estado { get; set; }
        public int c_oficina { get; set; }
        public string c_CP { get; set; }
        public int c_tipo_asenta { get; set; }
        public int c_mnpio { get; set; }
        public int id_asenta_cpcons { get; set; }
        public string d_zona { get; set; }
        public int c_cve_ciudad { get; set; }
    }
}