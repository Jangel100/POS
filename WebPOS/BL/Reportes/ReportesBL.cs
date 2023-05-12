using BL.Interface;
using BL.Utilities;
using DAL.Reportes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BL.Reportes
{
    public class ReportesBL : HttpClientBL, IReportesBL
    {
        readonly DAL.Utilities.IResponseAPI _VentasUsuarios;
        public ReportesBL(ReportesDAL VentasUsuarioDAL)
        {
            _VentasUsuarios = VentasUsuarioDAL;
        }
        public ReportesBL()
        {
            _VentasUsuarios = new ReportesDAL();
        }
        public HttpResponseMessage GetResponseAsycAPI(string url)
        {
            try
            {
                return _VentasUsuarios.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpClient HttpClientBL()
        {
            try
            {
                return GetHttpClientBL(); ;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
