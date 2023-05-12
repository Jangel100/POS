using BL.Interface;
using BL.Ventas;
using Entities.Models.Configuracion;
using Entities.Models.Franquicias;
using Entities.Models.Ventas;
using Entities.viewsModels;
using Newtonsoft.Json;
using RfcFacil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebPOS.Security;

namespace WebPOS.Controllers
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class VentasUsuarioController : Controller
    {

        readonly IVentasUsuarioBL _VentasUsuarioBL;
        public VentasUsuarioController(VentasUsuarioBL login)
        {
            _VentasUsuarioBL = login;
        }

        public VentasUsuarioController()
        {
            _VentasUsuarioBL = new VentasUsuarioBL();
        }
        // GET: VentasUsuario
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetViewClientes()
        {

            ClientesView clientesView = new ClientesView();
            try
            {
                //lsEstados = GetEstados();

                return PartialView("../ViewPartial/Ventas/_PartialClientesIndex", clientesView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetViewClientesF()
        {

            ClientesView clientesView = new ClientesView();
            try
            {
                return PartialView("../ViewPartial/VentasFranquicias/_PartialClientesIndex", clientesView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private List<Entidades> GetEstados()
        {

            string JsonResult = string.Empty;
            string url = string.Empty;
            try
            {
                url = "api2/GetConsultaUsuarios";

                HttpResponseMessage response = _VentasUsuarioBL.GetResponseAPIVentasUsuarios(url);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<Entidades>>().Result;

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult GetCliente(ClienteInputView clienteInputView)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/GetInfoCliente";

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = _VentasUsuarioBL.ReadAsStringAsyncAPI(url, clienteInputView);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<Clientes>>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        public async Task<JsonResult> GetClienteFacturacion(ClienteInputView clienteInputView)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/GetClienteFacturacion";

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, clienteInputView);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<ClientesView>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        public async Task<JsonResult> GetInsertInfoCliente(Clientes clientes)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetInsertInfoCliente";

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, clientes);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<int>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        public async Task<JsonResult> GetUpdateInfoCliente(Clientes clientes)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetUpdateInfoCliente";

                if (string.IsNullOrEmpty(clientes.RFC))
                {
                    clientes.RFC = GetRFC(clientes);
                }

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, clientes);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<int>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        [HttpGet]
        public ActionResult GetViewClientesAdd()
        {

            ClientesView clientesView;
            List<Entidades> lsEstados;
            try
            {
                lsEstados = GetEstados();
                clientesView = new ClientesView() { Estados = lsEstados, };

                return PartialView("../ViewPartial/Ventas/_PartialClientesAdd", clientesView);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult GetViewClientesAddF()
        {

            ClientesView clientesView;
            List<Entidades> lsEstados;
            try
            {
                lsEstados = GetEstados();
                clientesView = new ClientesView() { Estados = lsEstados, };

                return PartialView("../ViewPartial/VentasFranquicias/_PartialClientesAdd", clientesView);
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
        public async Task<JsonResult> GetInsertInfoClienteNew(Clientes clientes)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetInsertInfoClienteNew";
                if (string.IsNullOrEmpty(clientes.RFC))
                {
                    clientes.RFC = GetRFC(clientes);
                    //clientes.RFC = "XAXX010101000";
                }

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, clientes);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<int>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        public async Task<JsonResult> GetInfoCP(CPSepomex cpSepomex)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/GetInfoCP";

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, cpSepomex);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<CPSepomex>>().Result;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        public async Task<JsonResult> GetInfoCPAsentamiento(CPSepomex cpSepomex)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/GetInfoCPAsentamiento";

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, cpSepomex);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<CPSerchaAsentamientoView>>().Result;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        public string GetRFC(Clientes clientes)
        {
            try
            {
               return RfcBuilder.ForNaturalPerson()
                                .WithName(clientes.Nombre)
                                .WithFirstLastName(clientes.actAPEPATERNO)
                                .WithSecondLastName(clientes.actAPEMATERNO)
                                .WithDate(Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp).Year,
                                          Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp).Month,
                                          Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp).Day)
                                .Build().ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
         public ActionResult GetClienteMostrador()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;
            ClienteInputView clienteInputView =  null;

            try
            {
                url = "api2/GetClienteMostrador";

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = _VentasUsuarioBL.ReadAsStringAsyncAPI(url, clienteInputView);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<Clientes>>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
        [HttpPost]
        public List<Users> GetAdminUserInfoAll()
        {
            UsersView users = new UsersView() { UserName = "" };
            try
            {
                string url = string.Empty;

                url = "api2/GetAdminUserInfoAll";
                HttpResponseMessage response = _VentasUsuarioBL.ReadAsStringAsyncAPI(url, users);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<Users>>().Result;

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public string GetUsersSecurity(UserEncripty users)
        {
            SqlDataReader dr;
            StringBuilder UserQuery = new StringBuilder();

            try
            {
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@UserName", Value = users.UserName.ToUpper() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Paswoord", Value = users.Paswoord.ToUpper() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@PaswoordHassh", Value = users.PaswoordHassh },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Salt",  Value = users.Salt}
                    };


                UserQuery.Append("InsertUserSecurity");
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(UserQuery.ToString(), con))
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddRange(lsParameters.ToArray());
                        int i = com.ExecuteNonQuery();

                        if (i > 0)
                        {
                            return "true";
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<JsonResult> GetUpdateFactInfoCliente(Clientes clientes)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetUpdateFactInfoCliente";

                if (string.IsNullOrEmpty(clientes.RFC))
                {
                    clientes.RFC = GetRFC(clientes);
                }

                HttpClient client = _VentasUsuarioBL.HttpClientBL();
                HttpResponseMessage responses = await client.PostAsJsonAsync(url, clientes);

                if (responses.IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<int>().Result;

                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            return null;
        }
    }
}