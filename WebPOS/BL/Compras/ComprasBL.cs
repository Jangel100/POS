using BL.Interface;
using DAL.Compras;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Entities.viewsModels;
using Entities.Models.Payback;
using System.Linq;
using Entities;
namespace BL.Compras
{
    public class ComprasBL : IComprasBL
    {
        readonly DAL.Utilities.IResponseAPI _Compras;
        public ComprasBL(ComprasDAL ComprasDAL)
        {
            _Compras = ComprasDAL;
        }
        public ComprasBL()
        {
            _Compras = new ComprasDAL();
        }
        public HttpResponseMessage GetResponseAPI(string ur)
        {
            return _Compras.GetResponseAPI(ur);
        }
        public HttpResponseMessage GetResponseAPIVentasUsuarios(string url)
        {
            try
            {
                return _Compras.GetResponseAPI(url);
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
                return _Compras.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ListArticuloView> GetArticulos()
        {

            string url = string.Empty;
            List<ListArticuloView> TiendasViewResult = new List<ListArticuloView>();
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetArticulos");
                ResponseVenta.EnsureSuccessStatusCode();
                TiendasViewResult = ResponseVenta.Content.ReadAsAsync<List<ListArticuloView>>().Result;

                return TiendasViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<ListModeloView> GetModelos(string Tipo)
        {

            List<ListModeloView> ModelosViewResult;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetModelos?Tipo=" + Tipo + "");
                ResponseVenta.EnsureSuccessStatusCode();
                ModelosViewResult = ResponseVenta.Content.ReadAsAsync<List<ListModeloView>>().Result;

                return ModelosViewResult;
            
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListModeloView> GetModelosCveCorta(string Tipo)
        {

            List<ListModeloView> ModelosViewResult;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetModelosCveCorta?Tipo=" + Tipo + "");
                ResponseVenta.EnsureSuccessStatusCode();
                ModelosViewResult = ResponseVenta.Content.ReadAsAsync<List<ListModeloView>>().Result;

                return ModelosViewResult;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}