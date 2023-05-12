using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ResponsePayback
    {
        public bool Succes { get; set; }
        public int Total { get; set; }
        public int Conciliados { get; set; }
        public int Existentes { get; set; }
        public int Erroneos { get; set; }
        public string Message { get; set; }
    }
}
