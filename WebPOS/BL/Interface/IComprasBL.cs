using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Entities;
using Entities.viewsModels;
namespace BL.Interface
{
    public interface IComprasBL
    {
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        List<ListArticuloView> GetArticulos();
        List<ListModeloView> GetModelos(string Tipo);
        List<ListModeloView> GetModelosCveCorta(string Tipo);
    }
}
