﻿using System;
using System.Collections.Generic;
using System.Text;
using Entities.Models.Ventas;
namespace Entities.Models.Compras
{
    public class AddPurchase
    {
        public string Idcompra { get; set; }
        public string Idstore { get; set; }
        public string Idusuario { get; set; }
        public List<ArrayArticulos> ArrayArticulos { get; set; }
        public string webToken { get; set; }
        public bool statusresponse { get; set; }
        public string WhsID { get; set; }
        public string DEFAULTLIST { get; set; }
    }
}
