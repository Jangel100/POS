﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.Compras
{
    public class ParametersReqReimpCO
    {
        public string IdStore { get; set; }
        public string FechaIN { get; set; }
        public string FechaFin { get; set; }
        public string SesionSAPDB { get; set; }
        public string AdminUserID { get; set; }
        public string SesionFRCARDCODE { get; set; }
    }
}
