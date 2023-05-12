//using APIPOSS.Models.AdminStore;
using APIPOSS.Models.Configuracion;
using APIPOSS.Models.Home;
using APIPOSS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIPOSS.Controllers
{
    public class TiendasApiController : ApiController
    {
        private String _token = ConfigurationManager.AppSettings["SecretWebToken"].ToString();
        private string _NameBDPos = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
        private string _NameBDSap = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
        // GET api/TiendasApi
        public JsonResult<List<TiendaView>> Get(string tiendaLoginJson)
        {
            StringBuilder UserQuery = new StringBuilder();
            TiendaJsonView tiendaView = null;
            DataTable dt;
            string JsonTiendaView = string.Empty;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                if (tiendaLoginJson != null)
                {
                    tiendaView = JsonConvert.DeserializeObject<TiendaJsonView>(tiendaLoginJson);

                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = tiendaView.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = tiendaView.Franquicia }
                    };

                    UserQuery.Append("SELECT ADS.AdminStoreID, ADS.StoreName,ADS.WhsId FROM AdminStore ADS LEFT JOIN UserStores US ON US.AdminStoreID = ADS.AdminStoreID and US.AdminUserID = @AdminUserID  LEFT JOIN UserStores US2 ON US2.AdminStoreToSendID = ADS.AdminStoreID and US2.AdminUserID = @AdminUserID WHERE (US.AdminUserId > 0) and ADS.Status='A' AND DefaultCustomer= @Franquicia ORDER BY ADS.StoreName");
                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), lsParameters, CommandType.Text, "Tiendas", connstringWEB);

                    if (dt != null)
                    {
                        var dtTiendas = (from DataRow rows in dt.Rows
                                         select new TiendaView
                                         {
                                             AdminStoreID = Convert.ToInt32(rows["AdminStoreID"]),
                                             StoreName = (string)rows["StoreName"],
                                             WhsId = Convert.ToInt32(rows["WhsId"])
                                         }).ToList();

                        return Json<List<TiendaView>>(dtTiendas);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }


        [Route("api/GetStore")]
        public JsonResult<TiendaSelectedView> GetStore(string valuetiendaJson)
        {
            StringBuilder UserQuery = new StringBuilder();
            string valueTienda = null;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                var response = JsonConvert.DeserializeObject<string>(valuetiendaJson);
                var cadena = response.Split(',');
                valueTienda = cadena[0];
                if (_token.Equals(cadena[1]))
                {
                    if (valueTienda != null)
                    {
                        UserQuery.Append("SELECT  Top 1 ");
                        UserQuery.Append("ADS.AdminStoreID, ");
                        UserQuery.Append("ADS.StoreName, ");
                        UserQuery.Append("ADS.WhsId, ");
                        UserQuery.Append("ADS.DefaultList, ");
                        UserQuery.Append("ADS.StoreTypeID, ");
                        UserQuery.Append("ADS.actIVA, ");
                        UserQuery.Append("ADS.whsConsigID,");
                        UserQuery.Append("ADS.DefaultCustomer, ");
                        UserQuery.Append("ADF.Franquicia,");
                        UserQuery.Append("ADF.DBName, ");
                        UserQuery.Append("ADF.SBOUser, ");
                        UserQuery.Append("ADF.SBOPAss, ");
                        UserQuery.Append("ADF.TransitWhsID ");
                        UserQuery.Append("FROM AdminStore ADS ");
                        UserQuery.Append("     LEFT JOIN UserStores US ");
                        UserQuery.Append("     ON US.AdminStoreID = ADS.AdminStoreID ");
                        UserQuery.Append("     LEFT JOIN UserStores US2 ");
                        UserQuery.Append("     ON US2.AdminStoreToSendID = ADS.AdminStoreID ");
                        UserQuery.Append("     inner join AdminFranquicia ADF on ADS.DefaultCustomer=ADF.Franquicia ");
                        UserQuery.Append("Where ADS.AdminStoreID = @valueTienda");

                        List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@valueTienda", Value = valueTienda }
                    };

                        Utilities.DBMaster obj = new Utilities.DBMaster();
                        dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), lsParameters, CommandType.Text, "Tiendas", connstringWEB);

                        if (dt != null)
                        {
                            var dtTiendas = (from DataRow rows in dt.Rows
                                             select new TiendaSelectedView
                                             {
                                                 WhsId = (string)rows["WhsId"],
                                                 AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] == null ? 0 : rows["AdminStoreID"]),
                                                 DefaultList = Convert.ToInt32(rows["DefaultList"] == null ? 0 : rows["DefaultList"]),
                                                 StoreTypeID = Convert.ToInt32(rows["StoreTypeID"] == null ? 0 : rows["StoreTypeID"]),
                                                 whsConsigID = Convert.ToInt32(rows["whsConsigID"] == null ? 0 : rows["whsConsigID"]),
                                                 actIVA = Convert.ToInt32(rows["actIVA"] == null ? 0 : rows["actIVA"]),
                                                 DefaultCustomer = (string)rows["DefaultCustomer"],
                                                 Franquicia = (string)rows["Franquicia"],
                                                 DBName = (string)rows["DBName"],
                                                 SBOUser = (string)rows["SBOUser"],
                                                 SBOPAss = (string)rows["SBOPAss"],
                                                 TransitWhsID = (string)rows["TransitWhsID"],
                                                 ESCONSIGNA = Convert.ToInt32(rows["whsConsigID"] == null ? 0 : rows["whsConsigID"]) == 0 ? false : true
                                             }).FirstOrDefault<TiendaSelectedView>();

                            return Json<TiendaSelectedView>(dtTiendas);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        [Route("api/GetTextoMarquesina")]
        public JsonResult<string> GetTextoMarquesina(string frCardCodeJson)
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            string frCardCode = string.Empty;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                if (frCardCode != null)
                {
                    frCardCode = JsonConvert.DeserializeObject<string>(frCardCodeJson);

                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = frCardCode }
                    };

                    UserQuery.Append("Select top 1 TextoMarquesina from Marquesina WHERE franquicia = @Franquicia");
                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), lsParameters, CommandType.Text, "Marquesina", connstringWEB);

                    if (dt != null)
                    {
                        var dtTiendas = (from DataRow rows in dt.Rows
                                         select new
                                         {
                                             TextoMarquesina = rows["TextoMarquesina"]
                                         }).FirstOrDefault();


                        return Json(dtTiendas.TextoMarquesina.ToString());
                    }
                    return null;
                }

            }
            catch (Exception ex)
            {
                return Json("false");
            }

            return Json("false");
        }

        [Route("api/GetEnvioToday")]
        public JsonResult<List<PedidoDiaView>> GetEnvioToday(string StoreJson)
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            PaginationTableView JsonIdStoreView;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                if (StoreJson != null)
                {

                    JsonIdStoreView = JsonConvert.DeserializeObject<PaginationTableView>(StoreJson);

                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value =JsonIdStoreView.Id }
                    };

                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    dt = obj.EjecutaQry_Tabla("PedidoxToday", lsParameters, CommandType.StoredProcedure, connstringWEB);

                    if (dt != null)
                    {
                        var dtPedidos = (from DataRow rows in dt.Rows
                                         select new PedidoDiaView
                                         {
                                             Pedido = (string)rows["PEDIDO"] == null ? "" : (string)rows["PEDIDO"],
                                             Vendedor = (string)rows["VENDEDOR"] == null ? "" : (string)rows["VENDEDOR"],
                                             Fecha_Creacion = (string)rows["FECHA_DE_CREACION"] == null ? "" : (string)rows["FECHA_DE_CREACION"],
                                             Tienda = (string)rows["TIENDA"] == null ? "" : (string)rows["TIENDA"],
                                             Estatus = (string)rows["ESTATUS"] == null ? "" : (string)rows["ESTATUS"],
                                             Fecha_Entrega = (string)rows["FECHA_ENTREGA"] == null ? "" : (string)rows["FECHA_ENTREGA"],
                                             IdVenta = Convert.ToInt32(rows["IDVENTA"] == null ? 0 : rows["IDVENTA"]),
                                             IdStore = Convert.ToInt32(rows["IDStore"] == null ? 0 : rows["IDStore"])
                                         }).ToList();
                        return Json<List<PedidoDiaView>>(dtPedidos);
                    }
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("api/GetFranquicias")]
        public JsonResult<List<FranquiciasView>> GetFranquicias()
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            string JsonView = string.Empty;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                UserQuery.Append($"SELECT OC.CardCode,OC.CardName FROM {_NameBDSap}.dbo.OCRD OC JOIN {_NameBDPos}.dbo.AdminFranquicia AF ON (AF.Franquicia = OC.CardCode)");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Franquicias", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new FranquiciasView
                              {
                                  CardCode = (string)rows["CardCode"],
                                  CardName = (string)rows["CardName"]
                              }).ToList();

                    return Json<List<FranquiciasView>>(ls);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        [Route("api/GetFranquiciaTiendas")]
        public JsonResult<List<TiendaView>> GetFranquiciaTiendas(string tiendaLoginJson)
        {
            StringBuilder UserQuery = new StringBuilder();
            TiendaJsonView tiendaView = null;
            DataTable dt;
            string JsonTiendaView = string.Empty;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                if (tiendaLoginJson != null)
                {
                    tiendaView = JsonConvert.DeserializeObject<TiendaJsonView>(tiendaLoginJson);

                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = tiendaView.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = tiendaView.Franquicia }
                    };

                    UserQuery.Append("SELECT ADS.AdminStoreID, ADS.StoreName,ADS.WhsId FROM AdminStore ADS inner JOIN UserStores US ON US.AdminStoreID = ADS.AdminStoreID and US.AdminUserID = @AdminUserID AND WHERE DefaultCustomer = @Franquicia AND Status = 'A' ORDER BY ADS.StoreName");
                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), lsParameters, CommandType.Text, "Tiendas", connstringWEB);

                    if (dt != null)
                    {
                        var dtTiendas = (from DataRow rows in dt.Rows
                                         select new TiendaView
                                         {
                                             AdminStoreID = Convert.ToInt32(rows["AdminStoreID"]),
                                             StoreName = (string)rows["StoreName"],
                                             WhsId = Convert.ToInt32(rows["WhsId"])
                                         }).ToList();

                        return Json<List<TiendaView>>(dtTiendas);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        [Route("api4/GetInfoApDesc")]

        public JsonResult<List<AprobacionDescuentosView>> GetInfoApDesc()
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            string JsonView = string.Empty;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                UserQuery.Append("SELECT ad.Iddescuento,u.FirstName + ' ' + u.LastName as [NameUser],ad.IdArticulo,(select top 1 OITM.itemName from DORMIMUNDO_PRODUCTIVA.dbo.OITM OITM  where OITM.itemCode = ad.IdArticulo) as [itemName],ad.Fechas,s.StoreName,ad.Cantdescuento,ad.Observaciones,ad.PriceFD,ad.PriceSMU,ad.PriceFD - ((ad.PriceFD * ad.Cantdescuento)/100) as PriceWhithDesc FROM Autdescuentos ad INNER JOIN AdminUser u on ad.Iduser = u.AdminUserID INNER JOIN AdminStore s on ad.Idstored = s.AdminStoreID where ad.Statushide = 0 and ad.Statusdesc = 0 order by 1 desc");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "AprobDesc", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new AprobacionDescuentosView
                              {
                                  ID = rows["Iddescuento"].ToString(),
                                  Vendedor = (string)rows["NameUser"],
                                  Articulo = (string)rows["IdArticulo"],
                                  itemName = (string)rows["itemName"],
                                  Fecha = (string)rows["Fechas"].ToString(),
                                  Tienda = (string)rows["StoreName"],
                                  Descuento = (string)rows["Cantdescuento"],
                                  Observaciones = rows["Observaciones"] is DBNull ? "" : (string)rows["Observaciones"],
                                 PrecioFDO = rows["PriceFD"] is DBNull ? 0 : Convert.ToDouble(rows["PriceFD"]),
                                 PrecioSMU = rows["PriceSMU"] is DBNull ? 0 : Convert.ToDouble(rows["PriceSMU"]),
                                 Margen = rows["PriceFD"] is DBNull ? 0 : Convert.ToDouble(Threads.ConvertTo2D((1-(Convert.ToDouble(rows["PriceSMU"])/(Convert.ToDouble(rows["PriceWhithDesc"])/1.16))).ToString()))
                              }).ToList();  

                    return Json<List<AprobacionDescuentosView>>(ls);
                }
                return null;
            }
            catch (Exception ex)
            {

                 return null;
            }
        }
        [Route("api4/GetAceptar")]
        public JsonResult<int> GetAceptar(string iddescuento, int Status, string Descuento, string AdminUserID)
        {
            StringBuilder Userquery = new StringBuilder();
            string connstringWEB;
            string Email = string.Empty;
            string Articulo = string.Empty;
            DataTable dt = new DataTable();
            DataTable dtable = new DataTable();
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdDescuento", Value = iddescuento },
                          new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Statusdesc", Value = Status },
                          new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Descuento", Value = string.IsNullOrEmpty(Descuento) ? 0 : Convert.ToInt32(Descuento.Trim())}
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                var result = obj.EjecutaQry_Tabla("UpdateAcepDescuento", lsParameters, CommandType.StoredProcedure, connstringWEB);

                #region
                try
                {
                    var squeryAut = "select ad.Iduser,(select s.emailTienda from AdminStore s where s.AdminStoreID = ad.Idstored) as[EmailT],(select top 1 OITM.itemName from DORMIMUNDO_PRODUCTIVA.dbo.OITM OITM  where OITM.itemCode = ad.IdArticulo) as Articulo from Autdescuentos ad where ad.Iddescuento =" + iddescuento;
                    dtable = oDB.EjecutaQry_Tabla(squeryAut, CommandType.Text, "Correo1", connstringWEB);
                    var nuevoiduser = dtable.Rows[0][0].ToString();
                    var EmailT = dtable.Rows[0][1].ToString();
                     Articulo = dtable.Rows[0][2].ToString();

                    var squery = "select CorreoElectronico from AdminUser where AdminUserID ="+ nuevoiduser + "";
                    dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "Correo", connstringWEB);
                    if (dt != null)
                    {
                        foreach (DataRow em in dt.Rows)
                        {
                            Email = em[0].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(EmailT))
                    {
                        if (EmailT.Contains("@"))
                        {
                            Email = EmailT + (Email.Contains("@") ? "," + Email : "");
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion

                if (result != null)
                {
                    var mesageText = "";
                    if (Status.Equals(1) && string.IsNullOrEmpty(Descuento))
                    {
                        mesageText = "Su descuento con descripcion : "+Articulo+" fue autorizado , por favor introduzca su codigo : ";
                    }
                    else if (Status.Equals(1) && !string.IsNullOrEmpty(Descuento))
                    {
                        mesageText = "Su descuento con descripcion : " + Articulo + " fue autorizado, pero fue modificado por el porcentaje : " + Descuento.Trim() + " , por favor introduzca su codigo : ";
                    }
                    else
                    {
                        mesageText = "Su descuento con descripcion : " + Articulo + " no fue autorizado , por favor contacte a un Administrador";
                    }
                    Threads MailsEnvoy = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", mesageText + (Status == 2 ? "" : result.Rows[0].ItemArray[0].ToString()), "Notificaciones Dormimundo", Email.Trim());
                    Thread EnviaMail = new Thread(MailsEnvoy.SoloenviaCorreo);
                    EnviaMail.Start();
                    //Threads Messagepedido = new Threads("MessagePedido", 1, "7223098497", "", "", "7223098497", Email.Trim(), mesageText + (Status == 2 ? "" : result.Rows[0].ItemArray[0].ToString()) + "", 1); ;
                    //Thread Messped = new Thread(Messagepedido.Messagepedido);
                    //Messped.Start();

                    return Json<int>(1);
                }
                //  return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
