using BL.Interface;
using DAL.Configuracion;
using System;
using System.Net.Http;

namespace BL.Configuracion
{
    public class TiendasBL : ITiendasBL
    {
        readonly DAL.Utilities.IResponseAPI _tiendas;
        public TiendasBL(TiendasDAL usuariosDAL)
        {
            _tiendas = usuariosDAL;
        }

        public TiendasBL()
        {
            _tiendas = new TiendasDAL();
        }

        public HttpResponseMessage GetResponseAPITiendas(string url)
        {
            try
            {
                return _tiendas.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return _tiendas.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string UpdateMessage(string Parametros)
        {
            try
            {
                var ResponseUpdate = GetResponseAPITiendas($"api/GetUpdateMessage?Parametros=" + Parametros + "");
                ResponseUpdate.EnsureSuccessStatusCode();
                var Responseupdate = ResponseUpdate.Content.ReadAsStringAsync().Result;

                return Responseupdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}