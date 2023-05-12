using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using APIPOSS.Models.Ventas;
using System.Globalization;
using APIPOSS.Utilities;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Threading;
using APIPOSS.Views.Facturacion;
using APIPOSS.Models.Facturacion;
using System.Data.SqlClient;
using System.Net.Mail;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using APIPOSS.Models.ConsultasFacturacion;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using APIPOSS.Models.Configuracion;

namespace APIPOSS.Controllers
{
    public class VentasController : ApiController
    {
        returnventa infoventas = new returnventa();
        private string connWEB;
        private string connSAP;
        private string _token = ConfigurationManager.AppSettings["SecretWebToken"].ToString();
        private string _NameBDPos = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
        private string _NameBDSap = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
        private string DireccionFis;
        private string stock;
        public string IDPRINTVENTA = "";
        public Boolean btnPDF = false;
        private MailMessage correos = new MailMessage();
        private SmtpClient envios = new SmtpClient();
        public string DirPDF;
        // log
        private string patherror = AppDomain.CurrentDomain.BaseDirectory + @"logErrApi";
        // GET: api/Ventas
        [Route("api/GetModelos")]
        public JsonResult<List<ListModeloView>> GetModelos(string Tipo)
        {            
            //conexión a base de datos pruebas
            connSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;

            //conexion a base de datos productiva para hacer pruebas con datos reales
            //connSAP = ConfigurationManager.ConnectionStrings["DBConnSAPProductiva"].ConnectionString;


            Utilities.DBMaster obj = new Utilities.DBMaster();
            string UserQuery = string.Empty;
            ListModeloView 
                
                
                View = null;
            DataTable dt;
            string JsonGetArticulosView = string.Empty;
            try
            {
                if (Tipo.Equals("Linea"))
                {
                    UserQuery = ("select distinct ItemCode as code, ItemName as name from OITM where QryGroup7='Y' and (QryGroup43='N' or QryGroup42='Y')");
                }
                else
                {
                    UserQuery = ("select distinct ItemCode as code, ItemName as name from OITM where QryGroup7='N'");
                }

                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Modelos", connSAP);

                if (dt != null)
                {
                    var dtModelos = (from DataRow rows in dt.Rows
                                     select new ListModeloView
                                     {
                                         code = Convert.ToString(rows["code"]),
                                         name = (string)rows["name"]
                                     }).ToList();
                    return Json<List<ListModeloView>>(dtModelos);
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetArticulos")]
        public JsonResult<List<ListArticuloView>> GetArticulos()
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            try
            {
                //conexión para base de datos de prueba
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;

                //conexión para base de datos productiva para hacer pruebas con articulos reales y precios reales
                //string connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAPProductiva"].ConnectionString;


                UserQuery.Append("select ItmsTypCod as code,ItmsGrpNam as name from OITG where ItmsGrpNam not like '%Artículos propiedad%' and ItmsTypCod in (2, 3, 6) or ItmsTypCod between 9 and 22");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Articulos", connstringWEB);

                if (dt != null)
                {
                    var dtArticulos = (from DataRow rows in dt.Rows
                                       select new ListArticuloView
                                       {
                                           code = Convert.ToString(rows["code"]),
                                           name = (string)rows["name"]
                                       }).ToList();
                    return Json<List<ListArticuloView>>(dtArticulos);
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetLinea")]
        public JsonResult<List<ListLineaView>> GetLinea(string code)
        {
            StringBuilder UserQuery = new StringBuilder();
            //ListArticuloView ClientesView = null;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;


                UserQuery.Append("select distinct U_BXP_MARCA as code ,U_BXP_MARCA  as name from OITM where QryGroup" + code + "='Y' and U_BXP_MARCA is not null order by U_BXP_MARCA");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Linea", connstringWEB);

                if (dt != null)
                {
                    var dtLinea = (from DataRow rows in dt.Rows
                                   select new ListLineaView
                                   {
                                       code = (string)rows["code"],
                                       name = (string)rows["name"]
                                   }).ToList();
                    //dtArticulos.Insert(0,new ListArticuloView {code="0",name = "Seleccione artículo"});
                    return Json<List<ListLineaView>>(dtLinea);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetMedida")]
        public JsonResult<List<ListMedidaView>> GetMedida(string JsonMedida)
        {
            StringBuilder UserQuery = new StringBuilder();
            MedidaView MedidaView = null;
            DataTable dt;
            try
            {
                if (JsonMedida != null)
                {
                    string connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;

                    MedidaView = JsonConvert.DeserializeObject<MedidaView>(JsonMedida);

                    //List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdFranquicia", Value = MedidaView.code },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TextName", Value = MedidaView.name }
                    //};
                    UserQuery.Append("select distinct U_BXP_MEDIDA as code, U_BXP_MEDIDA as name from OITM where QryGroup" + MedidaView.code + " ='Y' and U_BXP_MARCA= '" + MedidaView.name + "'");
                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Line", connstringWEB);

                    if (dt != null)
                    {
                        var dtMedida = (from DataRow rows in dt.Rows
                                        select new ListMedidaView
                                        {
                                            code = (string)rows["code"],
                                            name = (string)rows["name"]
                                        }).ToList();
                        //dtArticulos.Insert(0,new ListArticuloView {code="0",name = "Seleccione artículo"});
                        return Json<List<ListMedidaView>>(dtMedida);
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
        [Route("api/GetPrice")]
        public JsonResult<List<ListListOfPriceView>> GetPrice(string StoreSap)
        {
            //StringBuilder UserQuery = new StringBuilder();
            string UserQuery = string.Empty;
            Utilities.DBMaster obj = new Utilities.DBMaster();
            GetPriceView PriceView = null;
            DataTable dt = new DataTable();
            int Listas = 0;
            string ListName = string.Empty;
            string JsonGetArticulosView = string.Empty;
            try
            {
                //string connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                
                //conexion a la db de pruebas
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                
                //conexion a Db productiva aunque esté apuntando a la DB de pruebas
                //string connstringWEB = ConfigurationManager.ConnectionStrings["DBConnProductiva"].ConnectionString;
                PriceView = JsonConvert.DeserializeObject<GetPriceView>(StoreSap);

                UserQuery = ("SELECT COUNT(*) FROM StoreList WHERE AdminStoreID = " + PriceView.Store + "");
                dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Listas", connstringWEB);
                if (dt != null)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        Listas = Convert.ToInt32(row[0] == null ? 0 : row[0]);
                    }
                    if (Listas == 0)
                    {
                        UserQuery = ("SELECT ListID,ListName FROM StoreListGlobal");
                        dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Listas", connstringWEB);
                        if (dt != null)
                        {
                            var dtPrice = (from DataRow rows in dt.Rows
                                           select new ListListOfPriceView
                                           {
                                               ListID = Convert.ToInt32(rows["ListID"] == null ? 0 : rows["ListID"]),
                                               ListName = (string)rows["ListName"]
                                           }).ToList();
                            //dtArticulos.Insert(0,new ListArticuloView {code="0",name = "Seleccione artículo"});
                            return Json<List<ListListOfPriceView>>(dtPrice);
                        }
                        return null;
                    }
                    else
                    {
                        UserQuery = ("SELECT isnull(ListGlobal,0) FROM StoreList WHERE AdminStoreID = " + PriceView.Store + " GROUP BY ListGlobal");
                        dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Listas", connstringWEB);
                        foreach (DataRow row in dt.Rows)
                        {
                            Listas = Convert.ToInt32(row[0] == null ? 0 : row[0]);
                        }
                        if (Listas == 0)
                        {

                            UserQuery = ($"SELECT ST.ListID,ST.ListName FROM [{_NameBDPos}].dbo.StoreList ST JOIN [{_NameBDSap}].dbo.OPLN OP ON OP.ListNum = ST.ListID WHERE ST.AdminStoreID = " + PriceView.Store + "");
                            //UserQuery = ($"SELECT ST.ListID,ST.ListName FROM ['DORMIMUNDOPOS'].dbo.StoreList ST JOIN ['DORMIMUNDO_PRODUCTIVA'].dbo.OPLN OP ON OP.ListNum = ST.ListID WHERE ST.AdminStoreID = " + PriceView.Store + "");
                            dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Listas", connstringWEB);
                            if (dt != null)
                            {
                                var dtPrice = (from DataRow rows in dt.Rows
                                               select new ListListOfPriceView
                                               {
                                                   ListID = Convert.ToInt32(rows["ListID"] == null ? 0 : rows["ListID"]),
                                                   ListName = (string)rows["ListName"]
                                               }).ToList();
                                //dtArticulos.Insert(0,new ListArticuloView {code="0",name = "Seleccione artículo"});
                                return Json<List<ListListOfPriceView>>(dtPrice);
                            }
                            return null;
                        }
                        else
                        {
                            UserQuery = ("select ListID listnum,isnull(ListName,'') listname from storelist where AdminStoreID='" + PriceView.Store + "' and listid in (select ListID from StoreList where AdminStoreID='" + PriceView.Store + "') union select defaultlist,(select ListName from " + PriceView.SAPDB + ".OPLN where OPLN.ListNum=defaultlist) from AdminStore where AdminStoreID=" + PriceView.Store + " union select listid,listname from StoreListGlobal");
                            dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Listas", connstringWEB);
                            if (dt != null)
                            {
                                var dtPrice = (from DataRow rows in dt.Rows
                                               select new ListListOfPriceView
                                               {
                                                   ListID = Convert.ToInt32(rows["ListID"] == null ? 0 : rows["ListID"]),
                                                   ListName = (string)rows["ListName"]
                                               }).ToList();
                                //dtArticulos.Insert(0,new ListArticuloView {code="0",name = "Seleccione artículo"});
                                return Json<List<ListListOfPriceView>>(dtPrice);
                            }
                            return null;
                        }
                    }
                    //if (dt != null)
                    //{
                    //    var dtModelos = (from DataRow rows in dt.Rows
                    //                     select new ListModeloView
                    //                     {
                    //                         code = Convert.ToString(rows["code"]),
                    //                         name = (string)rows["name"]
                    //                     }).ToList();
                    //    //dtArticulos.Insert(0,new ListArticuloView {code="0",name = "Seleccione artículo"});
                    //    //return Json<List<ListListOfPriceView>>(dtModelos);
                    //}
                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetQuantityStoreAndwinery")]
        public Existencias GetQuantityStoreAndwinery(string JsonParametrs)
        {
            StringBuilder UserQuery = new StringBuilder();
            StringBuilder UserQuery1 = new StringBuilder();
            Existencias QuantityStorewinery = new Existencias();
            DataTable dt;
            DataTable dt1;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                var parameter = JsonConvert.DeserializeObject<ExistenciasParametrs>(JsonParametrs);

                UserQuery.Append("select OITW.OnHand as Cantidad from SMU_VF_SI.dbo.OITW OITW where OITW.WhsCode = (select AlmacenSapPropio from AdminStore where AdminStoreID = " + parameter.IdStore + ") and OITW.ItemCode = '" + parameter.ItemCode + "'");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Linea", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        QuantityStorewinery.ExistenciaTienda = Convert.ToInt32(row["Cantidad"] == null ? 0 : row["Cantidad"]);
                    }
                }

                UserQuery1.Append("select OITW.OnHand as Cantidad from DORMIMUNDO_PRODUCTIVA.dbo.OITW OITW where OITW.WhsCode = (select WhsID from AdminStore where AdminStoreID = " + parameter.IdStore + ") and OITW.ItemCode = '" + parameter.ItemCode + "'");
                Utilities.DBMaster obj1 = new Utilities.DBMaster();
                dt1 = obj1.EjecutaQry_Tabla(UserQuery1.ToString(), CommandType.Text, "Linea", connstringWEB);

                if (dt1 != null)
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        QuantityStorewinery.ExistenciaBodega = Convert.ToInt32(row["Cantidad"] == null ? 0 : row["Cantidad"]);
                    }
                }
                return QuantityStorewinery;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetIva")]
        public int GetIva(string idStore)
        {
            StringBuilder UserQuery = new StringBuilder();
            int Iva = 0;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                UserQuery.Append("select actIVA from AdminStore where AdminStoreID=" + idStore + "");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Iva", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Iva = Convert.ToInt32(row["actIVA"] == null ? 0 : row["actIVA"]);
                    }
                    return Iva;
                }
            }
            catch (Exception ex)
            {
                return Iva;
            }
            return Iva;
        }
        [Route("api/GetUnitPrice")]
        public double GetUnitPrice(string JsonUnitPrice)
        {
            StringBuilder UserQuery = new StringBuilder();
            JsonUnitPriceView UnitPriceView = null;
            double price = 0;
            DataTable dt;
            try
            {
                string connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                UnitPriceView = JsonConvert.DeserializeObject<JsonUnitPriceView>(JsonUnitPrice);
                UserQuery.Append("SELECT isnull(price,0) AS price FROM itm1 IT WHERE IT.ItemCode='" + UnitPriceView.ItemCode + "' AND IT.PriceList = (SELECT ListNum FROM OPLN WHERE ListName='" + UnitPriceView.IdList + "' OR ListNum='" + UnitPriceView.Ion + "')");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "UnitPrice", connstringSAP);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        price = Convert.ToDouble(row["price"] == null ? 0 : row["price"]);
                    }
                    return price;
                }
            }
            catch (Exception ex)
            {
                return price;
            }
            return price;
        }
        [Route("api/GetExistenciasTiendaBodega")]
        public DataTable GetExistenciasTiendaBodega(string idstore, string Modelo, bool BodegaConsigna)
        {
            string squery = string.Empty;
            BodegaConsigna = false;
            try
            {


                squery = "select OnHand as C_Tienda,";
                if (BodegaConsigna)
                {
                    squery += "(select OITW.OnHand as Cantidad from DORMIMUNDO_PRODUCTIVA.dbo.OITW OITW where OITW.WhsCode = (select WhsID from AdminStore where AdminStoreID=" + idstore + ")";
                }
                else
                {
                    squery += "(select OITW.OnHand as Cantidad from DORMIMUNDO_PRODUCTIVA.dbo.OITW OITW where OITW.WhsCode = (select WhsID from AdminStore where AdminStoreID=(select CediCorrespondiente from AdminStore where AdminStoreID=" + idstore + "))";
                }
                squery += " and OITW.ItemCode ='" + Modelo + "')as C_Bodega," + " isnull((select OITM.U_BXPRANGO from DORMIMUNDO_PRODUCTIVA.dbo.OITM where OITM.ItemCode ='" + Modelo + "'),'D')as Rango, " + " isnull((select OITM.U_BXP_NICHO from DORMIMUNDO_PRODUCTIVA.dbo.OITM where OITM.ItemCode ='" + Modelo + "'),'D')as Nicho " + "from SMU_VF_SI.dbo.OITW INV" + "where INV.ItemCode='" + Modelo + "' and INV.WhsCode=(select AlmacenSapPropio from AdminStore where AdminStoreID=" + idstore + ")";
                DataTable dt;
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(squery.ToString(), CommandType.Text, "Query", connstringWEB);

                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) { return null; }

        }
        [Route("api/GetCombo")]
        public ResponseJuego GetCombo(string Parameters)
        {
            string UserQuery = string.Empty;
            Utilities.DBMaster obj = new Utilities.DBMaster();
            DataTable dt = new DataTable();
            RequestJuego RequestJgo = null;
            //DataTable dt1 = new DataTable();
            //DataTable dt2 = new DataTable();
            //DataTable dt3 = new DataTable();
            //DataTable dt4 = new DataTable();
            int Listas = 0;
            ResponseJuego Response = new ResponseJuego();
            Response.ListPrice = new List<ListaPrecio>();
            string JsonGetArticulosView = string.Empty;
            string ListaJuego = ConfigurationManager.AppSettings["ListaJuego"];
            string ListaJuegoFin = ConfigurationManager.AppSettings["ListaJuegoFin"];
            try
            {
                string connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                RequestJgo = JsonConvert.DeserializeObject<RequestJuego>(Parameters);
                //Consulta informacion del Combo
                UserQuery = "select ItemName,itemcode from DORMIMUNDO_PRODUCTIVA.DBO.oitm where itemcode= (select cast(U_juego as varchar) from DORMIMUNDO_PRODUCTIVA.DBO.OITM where ItemCode='" + RequestJgo.Modelo + "')";
                dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Combo", connstringSAP);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        Response.itemName = item[0].ToString();
                        Response.itemCode = item[1].ToString();
                    }
                    if (Response.itemName == "")
                        Response.MessageError = "El articulo del combo ya no existe en la base de datos";
                }
                else
                {
                    Response.MessageError = "No se encontro informacion del combo";

                }

                //consulta informacion de ET          
                bool normal = false;
                UserQuery = "select count(*) from StoreListGlobal where ListID IN (5)";
                dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "ET", connstringWEB);
                var val = dt.AsEnumerable().FirstOrDefault();
                var value1 = val[0].ToString();
                if (val[0].ToString() == "1")
                {
                    UserQuery = "select count(*) from StoreListGlobal where ListID IN (" + ListaJuego + ",11)";
                    dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "ET", connstringWEB);



                    var id = dt.AsEnumerable().FirstOrDefault();
                    //if (dt.Rows.Item(0).Item(0) = 1)
                    if (id[0].ToString() == "1")
                    {
                        normal = true;
                    }
                    UserQuery = "select ListName from OPLN where ListNum=" + ListaJuegoFin;
                    dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "ET", connstringSAP);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            ListaPrecio Listprice = new ListaPrecio();
                            Listprice.ListID = item[0].ToString();
                            Listprice.ListName = item[0].ToString();
                            Response.ListPrice.Add(Listprice);
                        }
                        return Response;
                    }
                    // Lista.AddItem(dt.Rows.Item(0).Item(0), ListaJuegoFin);

                }
                else
                {
                    normal = true;
                }
                if (normal)
                {

                    UserQuery = "IF EXISTS (SELECT ISNULL(ListGlobal,0)AS Valor FROM StoreList WHERE AdminStoreID = " + RequestJgo.IdStore + " GROUP BY ListGlobal) " + "BEGIN " + "IF (SELECT ISNULL(ListGlobal,0)AS Valor FROM StoreList WHERE AdminStoreID = " + RequestJgo.IdStore + " GROUP BY ListGlobal) = 1 " + "IF (SELECT COUNT(*) FROM StoreList WHERE AdminStoreID = " + RequestJgo.IdStore + ") > 0 " + "SELECT ListName FROM DORMIMUNDO_PRODUCTIVA.dbo.OPLN WHERE ListNum = " + ListaJuego + " UNION SELECT ListName FROM StoreList WHERE AdminStoreID = " + RequestJgo.IdStore + " AND ListName LIKE '%Juego%'" + " UNION SELECT ListName FROM StoreListGlobal WHERE ListName LIKE '%Juego%'" + "ELSE " + "SELECT ListName FROM DORMIMUNDO_PRODUCTIVA.dbo.OPLN WHERE ListNum = " + ListaJuego + " ELSE " + "SELECT OP.ListName FROM DORMIMUNDO_PRODUCTIVA.dbo.OPLN OP JOIN StoreList ST ON OP.ListNum= ST.ListID WHERE AdminStoreID = " + RequestJgo.IdStore + " AND OP.ListName LIKE '%Juego%'" + "End " + "ELSE " + "SELECT OP.ListName FROM DORMIMUNDO_PRODUCTIVA.dbo.OPLN OP JOIN StoreListGlobal ST ON ST.ListID = OP.ListNum WHERE OP.ListName LIKE '%Juego%'";
                    dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "ET", connstringWEB);
                    if (dt.Rows.Count > 0)
                    {

                        //for (int index = 0; index <= dt4.Rows.Count - 1; index++)
                        foreach (DataRow item in dt.Rows)
                        {
                            ListaPrecio Listprice = new ListaPrecio();
                            Listprice.ListID = item[0].ToString();
                            Listprice.ListName = item[0].ToString();
                            Response.ListPrice.Add(Listprice);
                        }
                        return Response;
                        //Response.ListPrice.Add  Lista.AddItem(dt.Rows.Item(index).Item(0), dt4.Rows.Item(index).Item(0));
                    }
                }

                return Response;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("api/GetComboLine")]
        public ResponseJgoView GetComboLine(string JsonComboLine)
        {

            string squery = string.Empty;
            double c_boxTienda = 0;
            double c_boxBodega = 0;
            double c_XJuego = 0;
            double minimoBox = 0;
            double maximoBox = 0;
            string Rango = string.Empty;
            string Nicho = string.Empty;
            int tiempodesurtido = 0;
            bool Esbodegac = false;
            string solcompra = string.Empty;
            string AlmJuego = string.Empty;
            string AlmacenBox = string.Empty;
            double pCantidadTienda = 0;
            double pCantidadBodega = 0;
            string pCantidad = "";
            double dPrecioUnitario = 0;
            double _dMonto = 0;
            double _dIVATotal = 0;
            double dPrecioConDescuento = 0;
            double desc = 0;
            double dIvaUnitario = 0;
            RequestJsonJgo RequestJgo = null;
            ResponseJgoView responseJgoView = null;
            DataTable dt = new DataTable();
            DataTable dts = new DataTable();
            DataTable dtx = new DataTable();
            DataTable dtt = new DataTable();
            DataTable dts1 = new DataTable();
            Utilities.DBMaster obj = new Utilities.DBMaster();
            try
            {
                //crea objeto para deserealizar              
                RequestJgo = JsonConvert.DeserializeObject<RequestJsonJgo>(JsonComboLine);
                //Asignacion de valores
                desc = string.IsNullOrEmpty(RequestJgo.Descuento) ? 0 : Convert.ToDouble(RequestJgo.Descuento);
                pCantidad = RequestJgo.cantidad;
                //if (RequestJgo.Origen.Equals("Tienda"))
                //{
                //    pCantidadTienda = Convert.ToDouble(pCantidad);
                //}
                //else { pCantidadBodega = Convert.ToDouble(pCantidad); }
                dPrecioUnitario = RequestJgo.Price / (1 + (RequestJgo.Iva / 100));
                dPrecioConDescuento = dPrecioUnitario * (1 - (desc / 100));
                dIvaUnitario = (RequestJgo.Price * (1 - (desc / 100)) - dPrecioConDescuento);
                //end
                string connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                squery = "select OnHand as C_Tienda," + Environment.NewLine;
                squery = squery + "(select OITW.OnHand from DORMIMUNDO_PRODUCTIVA.dbo.OITW OITW where OITW.WhsCode = (select WhsID from AdminStore where AdminStoreID=5)" + Environment.NewLine;
                squery = squery + "and OITW.ItemCode =(select cast(U_juego as varchar) from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "'))as C_Bodega," + Environment.NewLine;
                squery = squery + "(select U_boxesXjuego from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "')as CantXJuego" + Environment.NewLine;
                squery = squery + "from SMU_VF_SI.dbo.OITW INV" + Environment.NewLine;
                squery = squery + "where ItemCode=(select cast(ArticuloSBO as varchar) from Articulos where ArticuloSBO=(select cast(U_juego as varchar) from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "')and WhsCode = (select AlmacenSapPropio from AdminStore where AdminStoreID=" + RequestJgo.idStore + "))";
                dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "ExistenciasTiendaBodega", connstringWEB);

                if (dt.Rows.Count == 0)
                {
                    squery = "";
                    squery = "select U_boxesXjuego from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "'";
                    dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "ExistenciasTiendaBodega", connstringWEB);
                    c_boxTienda = 0; // dt.Rows.Item(0).Item("C_Tienda")
                    foreach (DataRow Item in dt.Rows)
                    {
                        c_boxBodega = Convert.ToDouble(Item[0]);
                        c_XJuego = Convert.ToDouble(Item[0]);
                    }

                }
                else
                {

                    foreach (DataRow Item in dt.Rows)
                    {
                        c_boxTienda = Convert.ToDouble(Item["C_Tienda"]); //dt.Rows.Item(0).Item("C_Tienda") comentar temporalmente tienda 0
                        c_boxBodega = Convert.ToDouble(Item["C_Bodega"]);
                        c_XJuego = Convert.ToDouble(Item["CantXJuego"]);
                    }
                }
                string squeryMM = string.Empty;

                squeryMM = "select top 1 Rango " + Environment.NewLine;
                squeryMM = squeryMM + "from VentasDetalle VD full join VentasEncabezado VE on VE.ID = VD.IDVenta " + Environment.NewLine;
                squeryMM = squeryMM + "where VD.IDArticulo = (select IDArticulo from Articulos where ArticuloSBO = (select cast(U_juego as varchar) from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "') ) AND VE.IDStore IN (" + RequestJgo.idStore + ",(SELECT whsConsigID FROM AdminStore WHERE AdminStoreID=" + RequestJgo.idStore + ")) and Rango != '' " + Environment.NewLine;
                dts1 = obj.EjecutaQry_Tabla(squeryMM, CommandType.Text, "Rango", connstringWEB);
                if (dts1.Rows.Count == 0)
                {
                    Rango = "D";
                }
                else
                {
                    foreach (DataRow Item in dts1.Rows)
                    {
                        Rango = Item["Rango"].ToString();
                    }

                }
                if (Rango == "A" || Rango == "B")
                {
                    string squeryMinMax = string.Empty;
                    double VentasAnteriores = 0;
                    double ConsumoPromedio;
                    double leadtime;

                    squeryMinMax = "SELECT SUM(Cantidad)as [PromedioVentas] FROM VentasDetalle" + Environment.NewLine;
                    squeryMinMax = squeryMinMax + " WHERE IDArticulo=(SELECT IDArticulo FROM Articulos WHERE ArticuloSBO= (select cast(U_juego as varchar) from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "'))" + Environment.NewLine;
                    squeryMinMax = squeryMinMax + " AND IDStore IN (" + RequestJgo.idStore + ",(SELECT whsConsigID FROM AdminStore WHERE AdminStoreID=" + RequestJgo.idStore + "))" + Environment.NewLine;
                    // squeryMinMax = squeryMinMax & " AND Fecha BETWEEN (SELECT DATEADD(wk,DATEDIFF(wk,0,GETDATE()-7),0))AND (SELECT DATEADD(wk,DATEDIFF(wk,0,GETDATE()-7),6))" & vbNewLine
                    squeryMinMax = squeryMinMax + "AND Fecha >= DATEADD (MONTH,-3,GETDATE())" + Environment.NewLine;
                    squeryMinMax = squeryMinMax + " AND IDVenta in (select ID from VentasEncabezado where Facturado=1 and IDStore=" + RequestJgo.idStore + ")";
                    dt = obj.EjecutaQry_Tabla(squeryMinMax, CommandType.Text, "ExistenciasTiendaBodega", connstringWEB);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow Item in dt.Rows)
                        {
                            //var value = Item["Rango"] == null ? "" : Item["Rango"]
                            if (Item[0].ToString() == "")
                            { VentasAnteriores = 0; }
                            else
                            {
                                VentasAnteriores = Convert.ToDouble(Item[0]);
                            }
                        }
                    }
                    //double sevent = 7;
                    ConsumoPromedio = VentasAnteriores / 7;
                    string squeryProxEnvio;
                    string tiendaDestino;
                    string squeryTienda;
                    tiendaDestino = RequestJgo.idStore;
                    squeryTienda = "select BodegaReabastecimiento from AdminStore where AdminStoreID = " + tiendaDestino + "";
                    dtx = obj.EjecutaQry_Tabla(squeryTienda, CommandType.Text, "BodegaReabastecimiento", connstringWEB);
                    var va1 = dtx.AsEnumerable().FirstOrDefault();
                    tiendaDestino = va1[0].ToString();
                    squeryProxEnvio = "select top 1 DATEDIFF(DAY, GETDATE(), FechaDeEnvio) as DiasParaEnvio  from ReabastecimientoLine where FechaDeEnvio > GETDATE() and " + tiendaDestino + " = '1'";
                    dts = obj.EjecutaQry_Tabla(squeryProxEnvio, CommandType.Text, "DiasParaEnvio" + tiendaDestino + " ", connstringWEB);
                    if (dts.Rows.Count == 0)
                    {
                        tiempodesurtido = 0;
                    }
                    else
                    {
                        var val = dt.AsEnumerable().FirstOrDefault();
                        tiempodesurtido = Convert.ToInt32(val[0]);
                    }
                    leadtime = tiempodesurtido + 1 + 1;

                    minimoBox = ConsumoPromedio * leadtime;
                    maximoBox = (ConsumoPromedio * leadtime) + minimoBox;

                    minimoBox = Convert.ToInt32(Math.Ceiling(minimoBox));
                    maximoBox = Convert.ToInt32(Math.Ceiling(maximoBox));
                    minimoBox = 0;//temporal
                }
                string squeryValBox = string.Empty;
                bool BanderaDeExibicionBox = false;
                DataTable dtBox = new DataTable();
                squeryValBox = "select IdArticulo, Cantidad from RegistroExhibiciones  where IdArticulo = (select cast(U_juego as varchar) from DORMIMUNDO_PRODUCTIVA.dbo.OITM where ItemCode='" + RequestJgo.modelo + "') and idstore = " + RequestJgo.idStore + "";
                dtBox = obj.EjecutaQry_Tabla(squeryValBox, CommandType.Text, "IdArticulo", connstringWEB);
                if (dtBox.Rows.Count == 0)
                {
                    BanderaDeExibicionBox = true;
                }
                else
                {
                    int CantidadExhibicionBox = 0;
                    foreach (DataRow Item in dtBox.Rows)
                    {
                        CantidadExhibicionBox = Convert.ToInt32(Item["Cantidad"]);
                    }
                    if (c_boxTienda > CantidadExhibicionBox)
                    { }
                    else if (c_boxTienda == CantidadExhibicionBox)
                    {
                        if (c_boxTienda == 0)
                            BanderaDeExibicionBox = true;
                        else
                            BanderaDeExibicionBox = false;
                    }
                    else if (c_boxTienda < CantidadExhibicionBox)
                    {
                        c_boxTienda = c_boxTienda - CantidadExhibicionBox;
                        BanderaDeExibicionBox = true;
                    }
                }
                if ((c_boxTienda >= minimoBox) || (BanderaDeExibicionBox == false))
                {
                    if ((c_XJuego > c_boxTienda && BanderaDeExibicionBox == true) || Esbodegac == true)
                    {
                        // AlmJuego.SelectedItem.Value = "B";
                        AlmJuego = "B";
                    }
                    else
                    {
                        if ((c_boxTienda - c_XJuego) <= minimoBox)
                        {
                            solcompra = solcompra + Environment.NewLine + "itemcode " + (maximoBox - (c_boxTienda - c_XJuego)) + " " + RequestJgo.idStore;
                        }
                        //AlmJuego.SelectedItem.Value = "T";
                        AlmJuego = "B";  // Deshabilitado por regla de negocio para que todo el tiempo lo saque de Dodega y no de tienda ,.. cambiar la letra B por T cuando ya no se requiera
                    }
                }
                else
                {
                    if (c_XJuego >= c_boxTienda)
                    {
                        //AlmJuego.SelectedItem.Value = "B";
                        AlmJuego = "B";
                    }
                    else
                    {
                        if ((c_boxTienda - c_XJuego) <= minimoBox)
                        {
                            //agregar una solicitud de compra para la tienda
                            //notificar al usuario que tiene que comprar mercancia para su tienda.
                            solcompra = solcompra + Environment.NewLine + "itemcode " + (maximoBox - (c_boxTienda - c_XJuego)) + " " + RequestJgo.idStore;
                        }
                        // AlmJuego.SelectedItem.Value = "T";
                        AlmJuego = "T";
                    }
                }
                if (AlmJuego.Equals(""))
                {
                    var Error = "Debe seleccionar el origen del box incluido en el combo";
                }
                else
                {
                    squery = "select (select ItmsGrpNam from OITB where oitb.ItmsGrpCod=oitm.ItmsGrpCod) as [Articulo],ItemName,itemcode,(select Price from ITM1 where ITM1.ItemCode=oitm.ItemCode and PriceList=(select listnum from OPLN where ListNum='" + ConfigurationManager.AppSettings["ListaBox"] + "')) as Precio,(select U_BoxesXJuego from OITM A where A.ItemCode='" + RequestJgo.modelo + "') as [Cantidad] from oitm where itemcode= (select cast(U_juego as varchar) from OITM where ItemCode='" + RequestJgo.modelo + "')";

                    obj.ConectaDBConnString(connstringSAP);
                    dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "Combo", connstringSAP);

                    //ChangeABOX();
                    if (AlmJuego.Equals("T"))
                    {
                        foreach (DataRow Item in dt.Rows)
                        {
                            //if ((Item["Cantidad"] * (pCantidadTienda + pCantidadBodega)) > System.Convert.ToDouble(CantidadBox.Text))
                            //{

                            //}
                        }

                    }
                }
                if (AlmJuego.Equals("T"))
                {

                    pCantidadTienda = Convert.ToDouble(pCantidad);

                    //        squery = "select OITW.OnHand as Cantidad from SMU_VF_SI.dbo.OITW OITW where OITW.WhsCode = (select AlmacenSapPropio from AdminStore where AdminStoreID=" + RequestJgo.idStore + ") and OITW.ItemCode = '" + RequestJgo.modelo + "'";
                    //        dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "ET", connstringWEB);

                    //        Dim ctemp As Double
                    //If dt.Rows.Count = 0 Then
                    //    ctemp = 0
                    //Else
                    //    ctemp = dt.Rows.Item(0).Item("Cantidad")
                    //End If
                    //For Each oVenta In CurrentData
                    //    If oVenta.CantidadTienda > 0 Then
                    //        If oVenta.Linea = pArtCode Then
                    //            ctemp = ctemp - CDbl(oVenta.CantidadTienda)
                    //        End If
                    //    End If
                    //Next
                    //CantidadBox.Text = ctemp
                }
                else if (AlmJuego.Equals("B"))
                {
                    pCantidadBodega = Convert.ToDouble(pCantidad);
                    //squery = "select OITW.OnHand as Cantidad from DORMIMUNDO_PRODUCTIVA.dbo.OITW OITW where OITW.WhsCode = (select WhsID from AdminStore where AdminStoreID=" + RequestJgo.idStore + ") and OITW.ItemCode = '" + RequestJgo.modelo + "'";
                    //dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "EB", connstringWEB);
                }

                squery = "select (select ItmsGrpNam from OITB where oitb.ItmsGrpCod=oitm.ItmsGrpCod) as [Articulo],ItemName,itemcode,(select Price / " + (1 + (RequestJgo.Iva / (double)100)) + " from ITM1 where ITM1.ItemCode=oitm.ItemCode and PriceList=(select listnum from OPLN where ListNum='" + ConfigurationManager.AppSettings["ListaBox"] + "')) as Precio,(select U_BoxesXJuego from OITM A where A.ItemCode='" + RequestJgo.modelo + "') as [Cantidad] from oitm where itemcode= (select cast(U_juego as varchar) from OITM where ItemCode='" + RequestJgo.modelo + "')";

                obj.ConectaDBConnString(connstringSAP);
                dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "Combo", connstringSAP);
                AlmacenBox = "";
                double cantidad = 0;
                double cantidad_ = 0;
                string NameArt = "";
                string ItemCode = "";
                string Precio = "";
                string Cantidad1 = "";
                var cantidadBoxJgo = 0.01;
                //double _Descuento = 100;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow Item in dt.Rows)
                    {
                        cantidad_ = (pCantidadBodega + pCantidadTienda) * Convert.ToDouble(Item["Cantidad"]);
                        cantidad = Convert.ToDouble(pCantidad) * Convert.ToDouble(Item["Cantidad"]);
                        Cantidad1 = Item["Cantidad"].ToString();
                        NameArt = Item["ItemName"].ToString();
                        ItemCode = Item["itemcode"].ToString();
                        Precio = Item["Precio"].ToString();
                    }
                }

                if (AlmJuego == "B")
                {
                    var precioColchon = Convert.ToDouble(RequestJgo.PriceUnit.Replace("$", "")) * Convert.ToInt32(RequestJgo.cantidad);
                    var cantidadAddBox = (cantidad_ * cantidadBoxJgo);
                    //Descuenta los decimales de acuerdo a la cantidad
                    var DiscounDecimalPU = Convert.ToDouble(RequestJgo.PriceUnit.Replace("$", "")) - cantidadBoxJgo;
                    var DiscountDecimalSbT = precioColchon - cantidadAddBox;
                    var DiscounDecimalT = Convert.ToDouble(RequestJgo.Total.Replace("$", "")) - cantidadAddBox;
                    responseJgoView = (new ResponseJgoView
                    {
                        Id = "2",
                        Modelo = NameArt,
                        Juego = "JGO",
                        Cantidad = (cantidad_).ToString(),
                        CTienda = "0", //pCantidadTienda.ToString(),
                        CBodega = (cantidad_).ToString(),
                        Lista = RequestJgo.Lista,
                        PrecioUnitario = string.Format("{0:c}", cantidadBoxJgo),
                        Descuento = string.Format("{0:c}", 0),
                        Subtotal = string.Format("{0:c}", cantidadAddBox),
                        IVA = "$0.00",
                        Total = string.Format("{0:c}", cantidadAddBox),
                        ItemCode = ItemCode,
                        AjustePriceUnit = string.Format("{0:c}", Utilities.Threads.ConvertTo2D(DiscounDecimalPU.ToString())),
                        AjustePriceSub = string.Format("{0:c}", Utilities.Threads.ConvertTo2D(DiscountDecimalSbT.ToString())),
                        AjusteTotal = string.Format("{0:c}", Utilities.Threads.ConvertTo2D(DiscounDecimalT.ToString())),
                    });
                    return responseJgoView;
                }
                else
                {

                    responseJgoView = (new ResponseJgoView
                    {
                        Id = "2",
                        Modelo = NameArt,
                        Juego = "JGO",
                        Cantidad = (cantidad_ * pCantidadTienda).ToString(),
                        CTienda = (cantidad_ * pCantidadTienda).ToString(),
                        CBodega = "0", //pCantidadBodega.ToString(),
                        Lista = RequestJgo.Lista,
                        PrecioUnitario = string.Format("{0:c}", Convert.ToDouble((cantidad_ * pCantidadTienda) * 0.01)),
                        Descuento = string.Format("{0:c}", Convert.ToDouble(Precio) * (pCantidadBodega + pCantidadTienda) * Convert.ToDouble(Cantidad1)),
                        Subtotal = "$0.00",
                        IVA = "$0.00",
                        Total = "$0.00",
                        ItemCode = ItemCode

                    });
                    return responseJgoView;
                }
            }
            catch (Exception ex) { return null; }
        }
        [Route("api/AddSale")]
        [HttpPost]
        public JsonResult<AddSale> AddSale(AddSale Sale)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            Log logApi = new Log(patherror);
            var ExitDescTotal = Sale.ArrayArticulos.Any(x => x.Juego != "JGO");
            var ExistTotalCero = Sale.ArrayArticulos.Any(x => Convert.ToDouble(x.Total.Replace("$", "")) == 0);
            if (ExitDescTotal && ExistTotalCero)
            {
                var GetArrayArt = GetNewObjAdd(Sale.ArrayArticulos);
                if (GetArrayArt != null)
                {
                    Sale.ArrayArticulos = GetArrayArt;
                }
                else { return null; }
            }
            try
            {
                if (Sale.webToken.Equals(_token))
                {
                    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                    logApi.Add("Crear venta en metodo AddSale");
                    if (addVenta(Sale) == true)
                    {
                        logApi.Add("Crear venta Correcta idventa: " + Convert.ToString(infoventas.IDPRINTVENTA));
                        ServiceMessage(infoventas.IDSTORE, infoventas.IDPRINTVENTA);   //Envia mensajes de texto
                        Utilities.GeneralClass objlog = new Utilities.GeneralClass();
                        objlog.InsertaLog(Tipos.VE, infoventas.IDPRINTVENTA, infoventas.IDSTORE, infoventas.WHSID);
                        CFactura(infoventas.IDPRINTVENTA, infoventas.IDSTORE);   //Envia hoja Roja
                        CheckOrder(Sale, infoventas.IDPRINTVENTA.ToString()); //Revisa si es pedido urgente y envia notificacion


                        AddSale ResponseVenta = new AddSale();
                        ResponseVenta.saleresponse = true;
                        ResponseVenta.Idventa = infoventas.IDPRINTVENTA.ToString();
                        return Json<AddSale>(ResponseVenta);
                    }
                }
            }
            catch (Exception ex)
            {
                logApi.Add("Error en metodo AddSale: " + ex.Message);
            }
            return null;
        }

        public bool addVenta(AddSale AddVenta)
        {
            string value; //COMO SE ENTERO DE NOSOTROS
            value = AddVenta.Medios;
            string typesale = AddVenta.Tipodeventa; //TIPO DE VENTA
            string campo;
            string fechanueva = AddVenta.Fechaentrega; //FECHA DE ENTREGA
            string anexo; //FECHA DE ENTREGA
            campo = ",FechaEntrega";
            anexo = ",'" + fechanueva + "'";
            string squery;
            string idcliente = AddVenta.Idcliente;
            connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            DataTable dt = new DataTable();
            Utilities.DBMaster obj = new Utilities.DBMaster();

            Log logApi = new Log(patherror);
            try
            {
                #region
                if (AddVenta.IsRequiredFactura.Equals("false"))
                {
                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                                new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = idcliente }
                            };

                    int i = obj.EjecutaQry_Tabl("UpdateRFCGenericoCliente", "", lsParameters, CommandType.StoredProcedure, connWEB);
                }
                else
                {
                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                                    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = idcliente }
                                };

                    int i = obj.EjecutaQry_Tabl("UpdateClienteDatosFacturacion", "", lsParameters, CommandType.StoredProcedure, connWEB);
                }
                #endregion

                squery = "DECLARE @intErrorCode INT" + Environment.NewLine + Environment.NewLine;
                squery = squery + " begin transaction" + Environment.NewLine + Environment.NewLine;
                squery = squery + " DECLARE @Tienda as int" + Environment.NewLine;
                squery = squery + " DECLARE @TipoFolio as int" + Environment.NewLine;
                squery = squery + " DECLARE @Folio as int" + Environment.NewLine;
                squery = squery + " DECLARE @Prefijo as varchar(20)" + Environment.NewLine;
                squery = squery + " DECLARE @Sufijo as varchar(20)" + Environment.NewLine;
                squery = squery + " DECLARE @UltimaVenta as int" + Environment.NewLine;
                squery = squery + " DECLARE @LineasAInsertar as int" + Environment.NewLine;
                squery = squery + " DECLARE @LineasInsertadas as int" + Environment.NewLine + Environment.NewLine;
                squery = squery + " DECLARE @ICCS as VARCHAR(200)=''" + Environment.NewLine;
                squery = squery + " DECLARE @IMEIS as VARCHAR(200)=''" + Environment.NewLine;
                squery = squery + " DECLARE @Inventario as int" + Environment.NewLine;
                squery = squery + " DECLARE @EmailTienda as VARCHAR(200)=''" + Environment.NewLine + Environment.NewLine;
                squery = squery + " DECLARE @ERROR as VARCHAR(200)" + Environment.NewLine + Environment.NewLine;


                squery = squery + " SET @ERROR = 'Ocurrio un error'" + Environment.NewLine;

                string STipoCliente = "";

                STipoCliente = "C";

                string sCurrentFolio;
                sCurrentFolio = "1";
                squery = squery + " SET @TipoFolio = " + sCurrentFolio + Environment.NewLine;
                squery = squery + " SET @Tienda = " + AddVenta.idstore + Environment.NewLine;
                squery = squery + " SET @Folio= (select (currentfolio + 1) as [NuevoFolio] from storeFolios where AdminStoreID=@Tienda and AdminFolioType=@TipoFolio)" + Environment.NewLine;
                squery = squery + " SET @Prefijo = LTRIM(RTRIM((select prefijo from storeFolios where AdminStoreID=@Tienda and AdminFolioType=@TipoFolio)))" + Environment.NewLine;
                squery = squery + " SET @Sufijo = LTRIM(RTRIM((select NoAprobacion from storeFolios where AdminStoreID=@Tienda and AdminFolioType=@TipoFolio)))" + Environment.NewLine;
                squery = squery + " SET @EmailTienda= (select emailTienda from Adminstore where adminstoreid=" + AddVenta.idstore + ")" + Environment.NewLine;
                squery = squery + " print @Folio" + Environment.NewLine;
                squery = squery + " print @Prefijo" + Environment.NewLine;
                // Captura de encabezado----------------------------------------------------------------------------------
                squery = squery + "Insert into ventasencabezado (IDCliente,IDUser,IDStore,Fecha,Folio,Prefijo,SUFIJO,TipoVenta,StatusCierre,StatusVenta,Comentarios" + campo + ",Entero,Tipodeventa, Correocliente, CorreoUsuario, FormaPago33, MetodoPago33, TipoComp33, UsoCFDI33, TipoRel33, CFDI_Rel33) values (";
                squery = squery + "'" + idcliente + "'";
                squery = squery + ",'" + AddVenta.Idusuario + "'"; ;
                squery = squery + ",@Tienda";
                squery = squery + ",getdate()";

                STipoCliente = "C";

                squery = squery + ",@Folio";
                squery = squery + ",@Prefijo";
                squery = squery + ",@Sufijo";
                squery = squery + ",'VM'";

                squery = squery + ",'O','O',UPPER('" + AddVenta.Comentarios + "')" + anexo + "," + value + ",'" + typesale + "', '" + AddVenta.CorreoUsuario + "'," + "@EmailTienda " + ",'" + AddVenta.FormaPago33 + "','" + AddVenta.MetodoPago33 + "','" + AddVenta.TipoComp33 + "','" + AddVenta.UsoCFDI33 + "','" + AddVenta.TipoRel33 + "','" + AddVenta.CFDI_Rel33 + "' )" + Environment.NewLine + Environment.NewLine;
                squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;

                squery = squery + " SET @UltimaVenta = (select MAX(ve.ID) from VentasEncabezado ve)" + Environment.NewLine;
                squery = squery + " Print @UltimaVenta" + Environment.NewLine;

                squery = squery + "Update storeFolios set currentfolio=@Folio where AdminStoreID=" + AddVenta.idstore + " and AdminFolioType=" + sCurrentFolio + "" + Environment.NewLine;
                squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;

                string sQueryEfectivo = "";
                double dEfectivo = 0;
                string FormaPago33 = "";
                string MetodoPago33 = "";
                string TipoComp33 = "";
                string UsoCFDI33 = "";
                string TipoRel33 = "";
                string CFDI_Rel33 = "";
                string CorreoCliente = "";
                string CorreoUsuario = "";
                int Parcialidad = 0;

                foreach (ArrayPagos pagos in AddVenta.ArrayPagos)
                {
                    var Forma = Obtenformapago(pagos);
                    if (pagos.Formapago != "Efectivo")
                    {
                        squery = squery + " insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,Afiliacion,TipoTarjeta,MSI,FormaPago33,MetodoPago33,TipoComp33,UsoCFDI33,TipoRel33,CFDI_Rel33,CorreoCliente,CorreoUsuario,Parcialidad,FechaPago) values (@UltimaVenta,@Tienda,'VT','" + pagos.Formapago + "'," + pagos.Monto + ",getdate(),'O','" + pagos.Cuenta + "',(select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2),'" + pagos.Afiliacion + "','" + pagos.Tipotarjeta + "','" + pagos.MSISub + "', '" + pagos.FormaPago33 + "','" + pagos.MetodoPago33 + "', '" + pagos.TipoComp33 + "','" + pagos.UsoCFDI33 + "', '" + pagos.TipoRel33 + "'" + ",CONCAT((select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2), '-', @Folio)" + ",'" + pagos.CorreoCliente + "'," + "(select emailTienda from Adminstore where adminstoreid=" + AddVenta.idstore + ")," + pagos.Parcialidad + ", getdate())" + Environment.NewLine;
                        squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                        squery = squery + " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=@Tienda and AdminFolioType=2" + Environment.NewLine;
                        squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                        squery = squery + " update VentasPagos set TipoTarjeta='' where TipoTarjeta is null" + Environment.NewLine;
                    }
                    else
                        dEfectivo = dEfectivo + Convert.ToDouble(pagos.Monto);
                    FormaPago33 = pagos.FormaPago33;
                    MetodoPago33 = pagos.MetodoPago33;
                    TipoComp33 = pagos.TipoComp33;
                    UsoCFDI33 = pagos.UsoCFDI33;
                    TipoRel33 = pagos.TipoRel33;
                    CFDI_Rel33 = pagos.CFDI_Rel33;
                    CorreoCliente = pagos.CorreoCliente;
                    CorreoUsuario = pagos.CorreoUsuario;

                }

                if (dEfectivo > 0)
                {
                    var result = AddVenta.ArrayPagos.Where(x => x.Formapago == "Efectivo").ToList();
                    foreach (var item in result)
                    {
                        Parcialidad = item.Parcialidad;
                    }
                    squery = squery + "insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,TipoTarjeta,MSI,FormaPago33,MetodoPago33,TipoComp33,UsoCFDI33,TipoRel33,CFDI_Rel33,CorreoCliente,CorreoUsuario,Parcialidad,FechaPago) values (@UltimaVenta,@Tienda,'VT','Efectivo'," + dEfectivo + ",getdate(),'O','',(select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2),'','','" + FormaPago33 + "','" + MetodoPago33 + "', '" + TipoComp33 + "','" + UsoCFDI33 + "', '" + TipoRel33 + "'" + ",CONCAT((select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2), '-', @Folio)" +
                ",'" + CorreoCliente + "'," + "(select emailTienda from Adminstore where adminstoreid=" + AddVenta.idstore + ")," + Parcialidad + ", getdate())" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=@Tienda and AdminFolioType=2" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " update VentasPagos set TipoTarjeta='' where TipoTarjeta is null" + Environment.NewLine;
                }
                var abonocero = Convert.ToBoolean(ConfigurationManager.AppSettings["abonocero"]);
                if (abonocero)
                {
                    dEfectivo = 0;
                    squery = squery + "insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,TipoTarjeta,MSI) values (@UltimaVenta,@Tienda,'VT','Efectivo'," + dEfectivo + ",getdate(),'O','',(select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2),'','')" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=@Tienda and AdminFolioType=2" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " update VentasPagos set TipoTarjeta='' where TipoTarjeta is null" + Environment.NewLine;
                }


                squery = squery + Environment.NewLine;

                // Captura de detalle y actualiza Inventarios/Series-------------------------------------------------------------------------

                string subtotal4Decimales;
                string subtotal2Decimales;

                string impuesto2Decimales;
                string impuesto4Decimales;

                string totalLinea2Decimales;

                int rowCount = 1;
                int lineasAInsertar = 0;
                int Detalles = 0;
                Detalles = AddVenta.ArrayArticulos.Count();
                if (Detalles > 0)
                {
                    foreach (ArrayArticulos oVenta in AddVenta.ArrayArticulos)
                    {
                    tagCheck:
                        ;
                        try
                        {
                            squery = squery + "Insert into ventasdetalle (";

                            squery = squery + "IDVenta,";
                            squery = squery + "IDLinea,";
                            squery = squery + "IDArticulo,";
                            squery = squery + "Juego,";
                            squery = squery + "Lista,";
                            squery = squery + "PrecioUnitario,";
                            squery = squery + "IVA,";
                            squery = squery + "Observaciones,";
                            squery = squery + "Descuento,";
                            if (!string.IsNullOrEmpty(oVenta.DescuentoJgo)) { squery = squery + "DescuentoJgo,"; }
                            squery = squery + "TotalLinea,";
                            squery = squery + "Cantidad,";
                            squery = squery + "StatusLinea,";
                            squery = squery + "IDStore,";
                            squery = squery + "Fecha";
                            squery = squery + ",JaliscoConsigna";
                            squery = squery + ",CodigoIva";
                            squery = squery + ") values(";
                            squery = squery + "@UltimaVenta"; // IDVenta
                            squery = squery + ",'" + rowCount + "'"; // IDLinea
                            squery = squery + ",'" + getItemID(oVenta.Linea) + "'"; // IDArticulo
                            squery = squery + ",'" + oVenta.Juego + "'"; // Juego
                            squery = squery + ",'" + oVenta.Lista + "'"; // Lista

                            double digito = double.Parse(oVenta.PrecioUnitario, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string Punitario = digito.ToString("F4");
                            double digitoiva = double.Parse(oVenta.IVA, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string Iva = digitoiva.ToString("F4");
                            double digitodesc = double.Parse(oVenta.Descuento, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string descuento = digitodesc.ToString("F4");
                            double digitototal = double.Parse(oVenta.Total, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string total = digitototal.ToString("F4");

                            squery = squery + ",'" + Punitario + "'";
                            squery = squery + ",'" + Iva + "'"; // IVA
                            squery = squery + ",'" + "v2" + "'"; // Observaciones
                            squery = squery + ",'" + descuento + "'"; // Descuento
                            if (!string.IsNullOrEmpty(oVenta.DescuentoJgo))
                            {
                                double digitodescJgo = double.Parse(oVenta.DescuentoJgo, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                                string descuentoJgo = digitodescJgo.ToString("F4");
                                squery = squery + ",'" + descuentoJgo + "'";
                            }
                            squery = squery + ",'" + total + "'"; // TotalLinea
                            var cantidadgeneral = (Convert.ToInt32(oVenta.CantidadTienda) + Convert.ToInt32(oVenta.CantidadBodega));
                            double digitocantge = double.Parse(cantidadgeneral.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string cantigen = digitocantge.ToString("F4");
                            squery = squery + ",'" + cantigen + "'"; // Cantidad NUEVA FORMA
                            squery = squery + ",'O'"; // StatusLinea

                            if (Convert.ToInt32(oVenta.CantidadTienda) > 0)
                                squery = squery + "," + AddVenta.idstore; // IDStore
                            else
                                squery = squery + ",(select whsconsigid from AdminStore where AdminStoreID=" + AddVenta.idstore + ")";
                            squery = squery + ",getdate()"; // Fecha
                            squery = squery + ",0" + Environment.NewLine + Environment.NewLine;
                            squery = squery + ",(select COD_IVA_SAP from IVA where PORCENTAJE=(select actIVA from AdminStore where AdminStoreID=" + AddVenta.idstore + ")))" + Environment.NewLine + Environment.NewLine;
                            squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;

                            lineasAInsertar = lineasAInsertar + 1;

                            rowCount = rowCount + 1;
                        }
                        catch (Exception ex)
                        {
                            // log
                            string sQuery1 = "";
                            // aproximado a 1 idventa
                            sQuery1 = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values ( (select MAX(ve.ID) from VentasEncabezado ve) ," + AddVenta.idstore + "," + "'PEDIDO SIN DETALLAES'" + ",'" + Detalles + "'" + ",Getdate())";
                            dt = obj.EjecutaQry_Tabla(sQuery1.ToString(), CommandType.Text, "Error", connWEB);
                            // 'cometemos eror de la siguiente linea para que no inserte nada
                            squery = squery + "Insert into ventasdetalle (";
                        }
                    }
                }
                else
                {
                    // log
                    string sQuery1 = "";
                    // aproximado a 1 idventa
                    sQuery1 = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values ( (select MAX(ve.ID) from VentasEncabezado ve) ," + AddVenta.idstore + "," + "'PEDIDO SIN DETALLAES'" + ",'" + Detalles + "'" + ",Getdate())";
                    dt = obj.EjecutaQry_Tabla(sQuery1.ToString(), CommandType.Text, "Error", connWEB);
                    // 'cometemos eror de la siguiente linea para que no inserte nada
                    squery = squery + "Insert into ventasdetalle (";
                }
                squery = squery + " SET @LineasAInsertar = " + lineasAInsertar + Environment.NewLine;

                squery = squery + " SET @LineasInsertadas = (select COUNT(*) from VentasDetalle where IDVenta=@UltimaVenta)" + Environment.NewLine;
                squery = squery + " PRINT @LineasInsertadas" + Environment.NewLine + Environment.NewLine;

                squery = squery + " IF (@LineasAInsertar <> @LineasInsertadas) GOTO PROBLEM" + Environment.NewLine;

                squery = squery + " IF (" + idcliente + " = 70) GOTO PROBLEM" + Environment.NewLine; ///Aqui va el id de cliente

                squery = squery + " Print 'Transaccion Exitosa'" + Environment.NewLine + Environment.NewLine;

                squery = squery + " commit transaction" + Environment.NewLine + Environment.NewLine;

                squery = squery + " PROBLEM:" + Environment.NewLine;
                squery = squery + " IF (@intErrorCode <> 0) or (@LineasAInsertar <> @LineasInsertadas) OR (@IMEIS <> '') OR (@ICCS<>'') OR (@intErrorCode<>0) or (" + idcliente + " = 70) BEGIN" + Environment.NewLine;
                squery = squery + " Print 'Ocurrio un error'" + Environment.NewLine;
                squery = squery + " ROLLBACK transaction" + Environment.NewLine;
                squery = squery + "  RAISERROR(@ERROR,18,1)" + Environment.NewLine;
                squery = squery + " End" + Environment.NewLine + Environment.NewLine;

                string sError = "";

                if (obj.EjecutaQry(squery, CommandType.Text, connWEB, sError) == 2)
                {
                    logApi.Add("Error al insertar venta:" + sError);
                    //Ext.Net.Notification msg = new Ext.Net.Notification();
                    //Ext.Net.NotificationConfig nconf = new Ext.Net.NotificationConfig();
                    //nconf.IconCls = "icon-exclamation";

                    //if (sError.Contains("EL IMEI") | sError.Contains("EL ICC"))
                    //    nconf.Html = sError;
                    //else
                    //    nconf.Html = "Se encontro un problema al procesar la venta, por favor seleccione nuevamente al cliente e intente de nuevo <br> Si el problema persiste por favor cancele la venta y vuélvalo a intentar<BR>" + sError;

                    //nconf.Title = "Error";
                    //msg.Configure(nconf);
                    //msg.Show();
                    return false;
                }
                try
                {
                    System.Data.SqlClient.SqlParameter Parameter = new System.Data.SqlClient.SqlParameter()
                    {
                        ParameterName = "@Id",
                        Value = Convert.ToInt32(idcliente)
                    };
                    int res = obj.EjecutaQry_T("UpdateImportSAPCustomer", "", Parameter, CommandType.StoredProcedure, connWEB);
                }
                catch (Exception exce)
                {
                    logApi.Add("Error al actualizar cliente importado id=: " + idcliente + "Error: " + exce.Message);
                }
                var ventaid = getLastSale(AddVenta.Idusuario);
                int IDPRINTVENTA = Convert.ToInt32(ventaid);
                int IDSTORE = Convert.ToInt32(AddVenta.idstore);
                var WHSID = AddVenta.WhsID;
                returnventa infoventa = new returnventa(IDPRINTVENTA, IDSTORE, WHSID);
                infoventas = infoventa;
                return true;
            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                logApi.Add("Error en metodo addVenta:" + error);
                return false;
            }
        }
        public string getItemID(string Item)
        {
            connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            string sQuery;
            DataTable dt = new DataTable();
        retry:
            ;
            sQuery = "select IDArticulo from articulos where ArticuloSBO = '" + Item + "'";
            Utilities.DBMaster obj = new Utilities.DBMaster();
            dt = obj.EjecutaQry_Tabla(sQuery.ToString(), CommandType.Text, "IDArticulo", connWEB);

            foreach (DataRow Drow in dt.Rows)
                return Drow["IDArticulo"].ToString();
            goto retry;
            return "0";
            dt = null;
            GC.Collect();
        }
        public string getLastSale(string user)
        {
            string sQuery;
            DataTable dt = new DataTable();
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            sQuery = "select max(id) as LastSale from ventasencabezado where iduser=" + user;

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "LastSale", connWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if (Convert.IsDBNull(Drow["LastSale"]) == false)
                    return Drow["LastSale"].ToString();
            }

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        [Route("api/GetModelsSelected")]
        public JsonResult<List<ListModeloView>> GetModelsSelected(string jsonparams)
        {
            string sQuery = "";
            DataTable dt;
            string jsonpa = string.Empty;
            dynamic data = JObject.Parse(jsonparams);
            string articulo = data.articulo;
            string modelo = data.modelo;
            string medida = data.medida;
            try
            {
                connSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                sQuery = "select distinct ItemCode as code, ItemName as name, isnull(U_BXP_CVEB,'N/A') as CCorta from OITM where QryGroup" + articulo + "='Y' and U_BXP_MARCA='" + modelo + "'and U_BXP_MEDIDA='" + medida + "' and (QryGroup7='Y')";
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(sQuery, CommandType.Text, "Modelos", connSAP);

                if (dt != null)
                {
                    var dtModels = (from DataRow rows in dt.Rows
                                    select new ListModeloView
                                    {
                                        code = Convert.ToString(rows["code"]),
                                        name = (string)rows["name"],
                                        clavecorta = (string)rows["CCorta"]
                                    }).ToList();
                    return Json<List<ListModeloView>>(dtModels);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api2/UpdateSale")]
        [HttpPost]
        public JsonResult<int> UpdateSale(SaleView SaleView)
        {
            StringBuilder Userquery = new StringBuilder();
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdOperacion", Value = SaleView.idVenta },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoOperacion", Value = SaleView.TipoOperacion },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaOperacion", Value = DateTime.Now }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int result = obj.EjecutaQry_Tabl("VentaUpdateTipoOperacion", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (result != -1)
                {
                    return Json<int>(result);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetSearchVenta")]
        [HttpPost]
        public JsonResult<List<SaleSearchView>> GetSearchVenta(SaleSearchView SaleView)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            List<SaleSearchView> lsS = null;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value = SaleView.IdStore.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Folio", Value = SaleView.Folio == null?"":SaleView.Folio },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha", Value = SaleView.Fecha }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                dt = obj.EjecutaQry_Tabla("VentaSearchVentaC", lsParameters, CommandType.StoredProcedure, "VentasEncabezado", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new SaleSearchView
                              {
                                  ID = Convert.ToInt32(rows["ID"]),
                                  Folio = (string)rows["Folio"]
                              }).ToList();

                    return Json<List<SaleSearchView>>(ls);
                }
                return Json<List<SaleSearchView>>(lsS);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api2/GetSearchDetalleVenta")]
        [HttpPost]
        public JsonResult<SalesSearchVenta> GetSearchDetalleVenta(SaleSearchView SaleView)
        {
            SalesSearchVenta DetailsVenta;
            List<SalesDetailsVenta> salesDetailsVentas;
            List<VentasPago> lsDetailsAbonosVenta;
            try
            {
                salesDetailsVentas = GetSearchVentaDetalle(SaleView);
                lsDetailsAbonosVenta = GetSearchVentaPagosDetalle(SaleView);
                DetailsVenta = new SalesSearchVenta() { lsDetailsVenta = salesDetailsVentas, lsDetailsAbonosVenta = lsDetailsAbonosVenta };

                return Json<SalesSearchVenta>(DetailsVenta);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<SalesDetailsVenta> GetSearchVentaDetalle(SaleSearchView SaleView)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            List<SalesDetailsVenta> lsS = null;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdVenta", Value = SaleView.ID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value = SaleView.IdStore }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                dt = obj.EjecutaQry_Tabla("VentaDetalleSearch", lsParameters, CommandType.StoredProcedure, "VentasEncabezado", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new SalesDetailsVenta
                              {
                                  IdVenta = Convert.ToInt32(rows["IdVenta"]),
                                  IdArticulo = Convert.ToInt32(rows["IdArticulo"]),
                                  Modelo = (string)rows["Modelo"],
                                  Juego = (string)rows["Juego"],
                                  Cantidad = (Decimal)rows["Cantidad"],
                                  Tienda = (string)rows["Tienda"],
                                  Bodega = (string)rows["Bodega"],
                                  Lista = (string)rows["Lista"],
                                  Unitario = (Decimal)rows["Unitario"],
                                  Descuento = (Decimal)rows["Descuento"],
                                  Subtotal = (Decimal)rows["Subtotal"],
                                  IVA = (Decimal)rows["IVA"],
                                  Total = (Decimal)rows["Total"],
                                  Itemcode = (string)rows["Itemcode"],
                                  Opcion = (string)rows["Opcion"]
                              }).ToList();

                    return (ls);
                }
                return lsS;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<VentasPago> GetSearchVentaPagosDetalle(SaleSearchView SaleView)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            List<VentasPago> lsS = null;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdVenta", Value = SaleView.ID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminStoreID", Value = SaleView.IdStore }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                dt = obj.EjecutaQry_Tabla("VentaDetallePagos", lsParameters, CommandType.StoredProcedure, "VentasPagos", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new VentasPago
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  IDVenta = rows["IDVenta"] is DBNull ? 0 : Convert.ToInt32(rows["IDVenta"]),
                                  AdminStoreID = rows["AdminStoreID"] is DBNull ? 0 : Convert.ToInt32(rows["AdminStoreID"]),
                                  //TipoVenta = rows["TipoVenta"] is DBNull ? '0' : (char)rows["TipoVenta"],
                                  FormaPago = rows["FormaPago"] is DBNull ? "" : (string)rows["FormaPago"],
                                  Monto = rows["Monto"] is DBNull ? 0 : (Decimal)rows["Monto"],
                                  NoCuenta = rows["NoCuenta"] is DBNull ? "" : (string)rows["NoCuenta"],
                                  Folio = rows["Folio"] is DBNull ? "" : (string)rows["Folio"],
                                  Ticket = rows["Ticket"] is DBNull ? "" : (string)rows["Ticket"],
                                  TipoTarjeta = rows["TipoTarjeta"] is DBNull ? "" : (string)rows["TipoTarjeta"],
                                  MSI = rows["MSI"] is DBNull ? 0 : Convert.ToInt32(rows["MSI"])
                              }).ToList();

                    return (ls);
                }
                return lsS;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public string ServiceMessage(object IdTienda, object IdVenta)
        {
            try
            {
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery;
                squery = "select(Case WHEN a.CorreoElectronico IS NULL THEN '' ELSE a.CorreoElectronico END) as COrreo,v.Folio,(Case WHEN a.NoCelular IS NULL THEN '' ELSE a.NoCelular END) as NoCelularUser,(Case WHEN c.Correo IS NULL THEN '' ELSE c.Correo END) as CorreoClient,(Case WHEN c.NoCelular IS NULL THEN '' ELSE c.NoCelular END) as NoCelularClient from VentasEncabezado v LEFT OUTER JOIN AdminUser a ON v.IDUser = a.AdminUserID INNER JOIN Clientes c ON c.id = v.IDCliente where v.ID=" + IdVenta.ToString() + " AND v.IDStore=" + IdTienda.ToString() + "";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "VentasEncabezado", connWEB);
                string textmessagevend = "Buen dia estimado Vendedor, el pedido N° " + dt.Rows[0][1].ToString() + " ha sido generado con exito";
                string textmessageclie = "Estimado Cliente su pedido " + dt.Rows[0][1].ToString() + " ha sido generado con exito";
                // ****************
                Threads Messagepedido = new Threads("MessagePedido", 1, dt.Rows[0][2].ToString(), textmessagevend, dt.Rows[0][0].ToString(), dt.Rows[0][4].ToString(), dt.Rows[0][3].ToString(), textmessageclie, 1);
                Thread Messped = new Thread(Messagepedido.Messagepedido);
                Messped.Start();

                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        [Route("api2/CancelaPedidoInt")]
        [HttpPost]
        public JsonResult<ResponseCancelacionPedido> CancelaPedidoInt(RequestCancelacion Dates)
        {

            var Cancelacion = new NotaCreditoInterna();
            var response = Cancelacion.CancelaPedido(Dates);
            return Json(response);
        }

        public Boolean Obtenformapago(ArrayPagos pagos)
        {
            Boolean formapago33 = true;
            switch (pagos.FormaPago33)
            {

                case "Efectivo":
                    pagos.FormaPago33 = "01";
                    break;

                case "Terminal Banamex":
                case "Terminal Bancomer":
                case "Terminal Banorte":
                case "Terminal PROSA":
                case "Terminal Santander":
                case "American Express":
                    switch (pagos.Tipotarjeta)
                    {
                        case "Credito": pagos.FormaPago33 = "04"; break;
                        case "Debito": pagos.FormaPago33 = "28"; break;
                        case "Crédito": pagos.FormaPago33 = "04"; break;
                        case "Débito": pagos.FormaPago33 = "28"; break;
                        case "": pagos.FormaPago33 = ""; break;
                    }
                    break;
                case "Deposito / Cheque":
                    pagos.FormaPago33 = "02";
                    break;
                case "Transferencia Electronica":
                    pagos.FormaPago33 = "03"; break;
                case "Transferencia Electrónica":
                    pagos.FormaPago33 = "03"; break;
                case "Transf. Elect. Dormicredit St.":
                    pagos.FormaPago33 = "03"; break;
                case "Compensación":
                    pagos.FormaPago33 = "17"; break;
                default:
                    pagos.FormaPago33 = "99";
                    break;
            }
            return formapago33;
        }
        [Route("api/Getdatepedido")]
        public JsonResult<AddSale> Getdatepedido(string JsonParameters)
        {
            try
            {
                AddSale Responseventa = new AddSale();
                dynamic datas = JObject.Parse(JsonParameters);
                string IDstore = datas.IdStore;
                string Folio = datas.folio;

                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery = "";

                squery = squery + "select id," + Environment.NewLine;
                squery = squery + "IDUser," + Environment.NewLine;
                squery = squery + "StatusVenta, " + Environment.NewLine;
                squery = squery + "FechaEntrega," + Environment.NewLine;
                squery = squery + "Prefijo + '-' + Folio as Documento," + Environment.NewLine;
                squery = squery + "Fecha as FechaVenta" + Environment.NewLine;
                squery = squery + " from VentasEncabezado " + Environment.NewLine;
                squery = squery + "where Folio='" + Folio + "'" + Environment.NewLine;
                squery = squery + "and idstore='" + IDstore + "'";

                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "SelectFecha", connWEB);

                if (dt.Rows.Count > 0)
                {
                    Responseventa.Idventa = dt.Rows[0][0].ToString();
                    Responseventa.Idusuario = dt.Rows[0][1].ToString();
                    Responseventa.StatusVenta = dt.Rows[0][2].ToString();
                    Responseventa.Fechaentrega = dt.Rows[0][3].ToString();
                    Responseventa.FolioPOS = dt.Rows[0][4].ToString();
                    Responseventa.fechaventa = dt.Rows[0][5].ToString();
                }
                else
                {
                    //  nconf.Html = "No se Encontro Ningun Pedido con Este Número de Folio"
                }

                return Json<AddSale>(Responseventa);
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api2/GetFechaEntrega")]
        public string GetFechaEntrega(string Parametros)
        {
            try
            {
                AddSale Responseventa = new AddSale();
                dynamic datas = JObject.Parse(Parametros);
                string idventa = datas.idventa;
                string fechaentrega = datas.fechaentrega;
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery = "";
                squery = "update VentasEncabezado set FechaEntrega='" + fechaentrega + "' where ID=" + idventa;
                oDB.EjecutaQry(squery, CommandType.Text, connWEB, "");

                return "1";
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetModelosCveCorta")]
        public JsonResult<List<ListModeloView>> GetModelosCveCorta(string Tipo)
        {
            //conexión a base de datos pruebas
            connSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;

          

            Utilities.DBMaster obj = new Utilities.DBMaster();
            string UserQuery = string.Empty;
            ListModeloView ClientesView = null;
            DataTable dt;
            string JsonGetArticulosView = string.Empty;
            try
            {
                if (Tipo.Equals("Linea"))
                {
                    UserQuery = ("select distinct ItemCode as code, ItemName as NAME, isnull(U_BXP_CVEB,'N/A') as CCorta from OITM where QryGroup7='Y' and QryGroup8='N' and (QryGroup43='N' or QryGroup42='Y')order by NAME--Linea");
                }
                else
                {
                    UserQuery = ("select distinct ItemCode as code, ItemName as NAME, isnull(U_BXP_CVEB,'N/A') as CCorta from OITM where QryGroup7='N' and QryGroup8='Y' and (QryGroup43='N' or QryGroup42='Y')order by NAME--Descontinuados");
                }

                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Modelos", connSAP);

                if (dt != null)
                {
                    var dtModelos = (from DataRow rows in dt.Rows
                                     select new ListModeloView
                                     {
                                         code = Convert.ToString(rows["code"]),
                                         name = (string)rows["NAME"],
                                         clavecorta = (string)rows["CCorta"]
                                     }).ToList();
                    return Json<List<ListModeloView>>(dtModelos);
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public List<ArrayArticulos> GetNewObjAdd(List<ArrayArticulos> ArtS)
        {
            List<ArrayArticulos> ListarrayArticulos = new List<ArrayArticulos>();
            try
            {
                double PriceBox = 0.01;
                bool AplicateDescount = false;
                var GetTotalcero = ArtS.Where(x => Convert.ToDouble(x.Total.Replace("$", "")) == 0).ToList();
                //var CalcValue = Convert.ToInt32(GetTotalcero.FirstOrDefault().Cantidad) * PriceBox;
                var Art = ArtS.OrderByDescending(x => Convert.ToDouble(x.Total.Replace("$", "")));
                foreach (var item in Art)
                {
                    if (Convert.ToDouble(item.Total.Replace("$", "")) == 0)
                    {
                        var array = (new ArrayArticulos
                        {
                            AlmacenBox = item.AlmacenBox,
                            Total = "$" + PriceBox,
                            Articulo = item.Articulo,
                            Cantidad = item.Cantidad,
                            CantidadBodega = item.CantidadBodega,
                            CantidadBox = item.CantidadBox,
                            CantidadTienda = item.CantidadTienda,
                            comentarios = item.comentarios,
                            Descuento = "$0.00",
                            Id = item.Id,
                            IVA = item.IVA,
                            Juego = item.Juego,
                            Linea = item.Linea,
                            Lista = item.Lista,
                            Medida = item.Medida,
                            Modelo = item.Modelo,
                            Monto = "$" + PriceBox,
                            PrecioUnitario = item.PrecioUnitario,
                            subTotal = "$" + PriceBox,
                            DescuentoJgo = item.Descuento
                        });
                        ListarrayArticulos.Add(array);
                    }
                    else if (!AplicateDescount)
                    {
                        var DescPU = Convert.ToDouble(item.PrecioUnitario.Replace("$", "")) - PriceBox;
                        var DescT = Convert.ToDouble(item.Total.Replace("$", "")) - PriceBox;
                        var DescSub = Convert.ToDouble(item.subTotal.Replace("$", "")) - PriceBox;
                        var array = (new ArrayArticulos
                        {
                            AlmacenBox = item.AlmacenBox,
                            Total = "$" + DescT.ToString(),
                            Articulo = item.Articulo,
                            Cantidad = item.Cantidad,
                            CantidadBodega = item.CantidadBodega,
                            CantidadBox = item.CantidadBox,
                            CantidadTienda = item.CantidadTienda,
                            comentarios = item.comentarios,
                            Descuento = item.Descuento,
                            Id = item.Id,
                            IVA = item.IVA,
                            Juego = item.Juego,
                            Linea = item.Linea,
                            Lista = item.Lista,
                            Medida = item.Medida,
                            Modelo = item.Modelo,
                            Monto = item.Monto,
                            PrecioUnitario = "$" + DescPU.ToString(),
                            subTotal = "$" + DescSub.ToString()
                        });
                        AplicateDescount = true;
                        ListarrayArticulos.Add(array);
                    }
                    else { ListarrayArticulos.Add(item); }

                }
                return ListarrayArticulos;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public void CFactura(int IDVENTA, int IDSTORE)
        {
            try
            {
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery = "";
                int IDABONO = 0;
                squery = squery + "select top 1 ID from VentasPagos where IDVenta=" + IDVENTA;
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "SelectFecha", connWEB);
                if (dt.Rows.Count > 0)
                {
                    IDABONO = Convert.ToInt32(dt.Rows[0][0]);
                }
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDTIENDA", Value = IDSTORE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDABONO", Value = IDABONO }
                    };
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("HredInfo", lsParameters, CommandType.StoredProcedure, connWEB);
                bool sError = false;
                int exist_ = 0;
                string aux, tipoT = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Origen"].ToString() == "Tienda")
                    {
                        exist_ = Convert.ToInt32(row["ExistenciaTienda"]);
                        aux = "ExistenciaTienda";
                    }
                    else
                    {
                        exist_ = Convert.ToInt32(row["ExistenciaBodega"]);
                        aux = "ExistenciaBodega";
                    }
                    if (aux == "ExistenciaTienda")
                    {
                        //nothing
                    }
                    else
                    {
                        if (Convert.ToInt32(row["Cantidad"]) > exist_)
                        {
                            // var msg="Existencia insuficiente del articulo " & "<br>" & row["Articulo"] & "<br>" & "<br>Existencia : " & row(aux) & "<br> Origen de Venta : " & tipoT & "<br> Se a enviado la información al distribuidor para surtir el producto a la brevedad posible "
                            stock = row[aux].ToString();
                            ImprimirHojaRoja(IDABONO, IDSTORE);
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ImprimirHojaRoja(int IDABONO, int IDSTORE)
        {
            try
            {
                string mensajeME, mensajeMEFinal, mensajeME1, asuntoN, rutaPDFSend, franq, mensajeTab2;
                DataTable dt = new DataTable(), dto = new DataTable(), dtos = new DataTable(), dtosi = new DataTable(), dtosro = new DataTable();
                string squery, squery1, squery2, squery3, squery4, squery5;
                string iddeventa, banderaconfirmacion;
                DateTime Fecha;
                Nullable<DateTime> FechaCFDI;
                Nullable<int> numeroemail = 0;
                int envoy = 0;
                int idhre = 0;
                DateTime dtcreafactura = DateTime.Now;
                int Dias;
                int Hrs;
                TimeSpan Calculodia;
                TimeSpan Calculohr;
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                DBMaster oDB1 = new DBMaster();
                int horalimite = 16;
                int hora = dtcreafactura.Hour;
                bool bandhrs = false;
                string sError = "";
                if (hora >= horalimite)
                    bandhrs = true;
                DireccionFis = "";
                squery = "select IDVenta, Fecha,FechaCFDI from VentasPagos where  ID = " + IDABONO + "";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "IDVenta", connWEB);
                iddeventa = dt.Rows[0][0].ToString();
                Fecha = Convert.ToDateTime(dt.Rows[0][1]);
                try
                {
                    FechaCFDI = Convert.ToDateTime(dt.Rows[0][2]);
                }
                catch (Exception ex)
                {
                    FechaCFDI = dtcreafactura;
                }

                squery5 = "select top 1 idhre,mailenviados from HojaRojaEncabezado where IdVenta = " + iddeventa + "";
                dtosi = oDB.EjecutaQry_Tabla(squery5, CommandType.Text, "IDVenta", connWEB);
                try
                {
                    idhre = Convert.ToInt32(dtosi.Rows[0][0]);
                    numeroemail = Convert.ToInt32(dtosi.Rows[0][1]);
                }
                catch (Exception ex)
                {
                    envoy = 0;
                }
                if (numeroemail != 0)
                    envoy = Convert.ToInt32(numeroemail);
                if (dtosi.Rows.Count == 0)
                {
                    banderaconfirmacion = "";
                    asuntoN = "URGENTE STOCK";
                    if (bandhrs == true)
                    {
                        asuntoN = "INFORME DE HOJA ROJA";
                        envoy = 0;
                    }
                    else
                        envoy = 1;
                }
                else
                {
                    banderaconfirmacion = dtosi.Rows[0][0].ToString();
                    asuntoN = "RECORDATORIO URGENTE STOCK";

                    if (bandhrs == true)
                    {
                        asuntoN = "INFORME DE HOJA ROJA";
                        //envoy = envoy;

                        squery3 = "update HojaRojaEncabezado set mailenviados = " + envoy + ",ultimafechaemail =GETDATE() where idhre=" + idhre;
                        oDB1.EjecutaQry(squery3, CommandType.Text, connWEB, sError);
                    }
                    else
                    {
                        envoy = envoy + 1;
                        squery3 = "update HojaRojaEncabezado set mailenviados = " + envoy + ",ultimafechaemail =GETDATE() where idhre=" + idhre;
                        oDB1.EjecutaQry(squery3, CommandType.Text, connWEB, sError);
                    }
                }

                mensajeME1 = "<Table border=" + 1 + "> <Tr><Th>Articulo</Th><Th>CANTIDAD</Th></Tr>";
                mensajeTab2 = " <br /> <Table border=" + 2 + "> <Tr><Th colspan=" + 2 + "> COMPLEMENTO DEL PEDIDO </Th></Tr>";
                mensajeTab2 += "<Tr><Th>Articulo</Th><Th>CANTIDAD</Th></Tr>";
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDTIENDA", Value = IDSTORE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDVENTA", Value = iddeventa }
                    };
                dto = oDB.EjecutaQry_Tabla("ArticulosxPedido", lsParameters, CommandType.StoredProcedure, connWEB);
                string namefranqui;
                string franqui;
                double cantidad = 0;
                // insert de hoja roja
                namefranqui = dto.Rows[0][2].ToString();
                franqui = dto.Rows[0][3].ToString();
                cantidad = Convert.ToDouble(dto.Rows[0][1]);
                if (banderaconfirmacion == "")
                {
                    sError = "";
                    if (bandhrs == true)
                    {
                        squery3 = "insert into HojaRojaEncabezado (fecha, IdVenta, IdStore, almacen, mailenviados,ultimafechaemail) values (getdate()," + iddeventa + ",(select IDStore from VentasEncabezado  where ID = " + iddeventa + ") ," + franqui + "," + envoy + ",GETDATE())";
                        oDB1.EjecutaQry(squery3, CommandType.Text, connWEB, sError);
                    }
                    else
                    {
                        squery3 = "insert into HojaRojaEncabezado (fecha, IdVenta, IdStore, almacen, mailenviados,ultimafechaemail) values (getdate()," + iddeventa + ",(select IDStore from VentasEncabezado  where ID = " + iddeventa + ") ," + franqui + "," + envoy + ",GETDATE())";
                        oDB1.EjecutaQry(squery3, CommandType.Text, connWEB, sError);
                    }
                }
                string cantidadparainsert = "";
                string cantidadpara = "";
                string descarticulo = "";
                string tipoT = "";
                double Cantidadex;
                bool band = true;
                bool band2 = false;
                foreach (DataRow Drow in dto.Rows)
                {
                    double extienda = Convert.ToDouble(Drow["ExistenciaTienda"]);
                    double exbodega = Convert.ToDouble(Drow["ExistenciaBodega"]);
                    band = true;
                    cantidad = Convert.ToDouble(Drow["Cantidad"]);
                    Cantidadex = Convert.ToDouble(Drow["Cantidad"]);
                    if (Drow["Origen"].ToString() == "Bodega Consignacion")
                        tipoT = "Bodega";
                    else
                        tipoT = "Tienda";
                    if (tipoT == "Tienda")
                    {
                        if (extienda < cantidad)
                        {
                            double stockk = cantidad - extienda;
                            if (extienda < 0)
                                extienda = 0;
                            double resta = cantidad - extienda;
                            mensajeME1 += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + resta + ".00" + "</td></Tr>";
                            franq = Drow["IDstore"].ToString();
                            cantidadparainsert = stockk.ToString();
                            cantidadpara = Drow["IdArticulo"].ToString();
                            descarticulo = Drow["Articulo"].ToString();
                            band = false;
                            Cantidadex = extienda;
                        }
                    }
                    else if (tipoT == "Bodega")
                    {
                        if (exbodega < cantidad)
                        {
                            double stockk = cantidad - exbodega;
                            if (exbodega < 0)
                                exbodega = 0;
                            double resta = cantidad - exbodega;
                            mensajeME1 += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + resta + ".00" + "</td></Tr>";
                            franq = Drow["IDstore"].ToString();
                            cantidadparainsert = stockk.ToString();
                            cantidadpara = Drow["IdArticulo"].ToString();
                            descarticulo = Drow["Articulo"].ToString();
                            band = false;
                            Cantidadex = exbodega;
                        }
                    }


                    if (Cantidadex > 0.0)
                    {
                        mensajeTab2 += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + Cantidadex + ".00" + "</td></Tr>";
                        franq = Drow["IDstore"].ToString();
                        cantidadparainsert = Cantidadex.ToString();
                        cantidadpara = Drow["IdArticulo"].ToString();
                        descarticulo = Drow["Articulo"].ToString();
                    }

                    // insert de hoja roja
                    if (banderaconfirmacion == "")
                    {
                        sError = "";
                        squery4 = "insert into HojaRojaDetalle (IdVenta, fecha, IdArticulo, Descripcion,  Cantidad, Stock, Estatus, IdHRE) values (" + iddeventa + ", GETDATE(),'" + cantidadpara + "','" + descarticulo + "' , " + cantidadparainsert + ", " + stock + " ,'E', (select top 1 IdHRE from  HojaRojaEncabezado where IdVenta = " + iddeventa + " order by IdHRE desc))";
                        oDB1.EjecutaQry(squery4, CommandType.Text, connWEB, sError);
                    }
                }
                mensajeME1 += "</table>";
                mensajeTab2 += "</table>";
                try
                {
                    squery1 = "select top 1 fecha from HojaRojaDetalle where IdHRE = " + idhre + "";
                    dtosro = oDB.EjecutaQry_Tabla(squery1, CommandType.Text, "hojarojadetalle", connWEB);
                    DataRow columna = dtosro.Rows[0];
                    DateTime fecharoja = Convert.ToDateTime(columna["fecha"]);
                    var now = fecharoja.ToString("HH:mm");
                    now = now.Replace(":", ".");//para comparar las horas lo convertimos a decimal
                    decimal houred = Convert.ToDecimal(now);
                    decimal hourconstant = 16.00m;
                    DateTime horaActual = DateTime.Now;
                    var boool = (envoy == 1) ? true : false;
                    if (houred >= hourconstant)
                    {
                        if (boool)
                        {
                            Hrs = 0;
                        }
                        else
                        {
                            Calculohr = Convert.ToDateTime(dtcreafactura).Subtract(Convert.ToDateTime(fecharoja));
                            Hrs = Convert.ToInt32(Calculohr.TotalHours);
                        }
                    }
                    else
                    {
                        Calculohr = Convert.ToDateTime(dtcreafactura).Subtract(Convert.ToDateTime(fecharoja));
                        Hrs = Convert.ToInt32(Calculohr.TotalHours);
                    }
                }
                catch (Exception ex)
                {
                    Hrs = 0;
                }
                if (banderaconfirmacion == "" & bandhrs != true)
                {
                    mensajeME = "<br />Esta es una solicitud de un pedido urgente:";
                    mensajeME += "<br /> Numero de intentos de facturacion :  " + envoy;
                    mensajeME += "<br /> Pedido POS numero: " + iddeventa + " ";
                    mensajeME += "<br /> Fecha y hora de pedido: " + Fecha + " ";
                    mensajeME += "<br /> Fecha y hora del CFDI: " + FechaCFDI + "";
                    mensajeME += "<br />Tienda: " + namefranqui + " " + franqui + "<br />";
                    if (Hrs < 24)
                        mensajeME += "<p><font size=5 color='green'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 24 & Hrs < 36)
                        mensajeME += "<p><font size=5 color='orange'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 36)
                        mensajeME += "<p><font size=5 color='red'>" + Hrs + " Horas transcurridas </font> </p><br />";
                }
                else if (bandhrs == true)
                {
                    mensajeME = "<br />Se intento facturar el pedido:" + iddeventa + " despues de las 4 de la tarde";
                    mensajeME += "<br /> Pedido POS numero: " + iddeventa + " ";
                    mensajeME += "<br /> Fecha y hora de pedido: " + Fecha + " ";
                    mensajeME += "<br /> Fecha y hora del CFDI: " + FechaCFDI + "";
                    mensajeME += "<br />Tienda: " + namefranqui + " " + franqui + "<br />";
                    mensajeME += "<br /> NOTA: La contabilizacion de este intento comenzara el siguiente dia laboral apartir de las 8 a.m. ";
                }
                else
                {
                    mensajeME = "<br />La notificaci&oacute;n inicial de falta de mercanc&iacute;a del pedido: " + iddeventa + " seguimos en la espera de la notificaci&oacute;n del envi&oacute;, agradecemos tu pronta respuesta";
                    mensajeME += "<br /> Numero de intentos de facturacion :  " + envoy;
                    mensajeME += "<br /> Fecha y hora de pedido: " + Fecha + " ";
                    mensajeME += "<br /> Fecha y hora del CFDI: " + FechaCFDI + "";
                    mensajeME += "<br />Tienda: " + namefranqui + " " + franqui + "";
                    if (Hrs < 24)
                        mensajeME += "<p><font size=5 color='green'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 24 & Hrs < 36)
                        mensajeME += "<p><font size=5 color='orange'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 36)
                        mensajeME += "<p><font size=5 color='red'>" + Hrs + " Horas transcurridas </font> </p><br />";
                }
                mensajeME += "<br />Detalles: <br />";

                mensajeMEFinal = mensajeME + mensajeME1 + mensajeTab2;

                squery2 = "select IDVenta from VentasPagos where ID=" + IDABONO;
                dtos = oDB1.EjecutaQry_Tabla(squery2, CommandType.Text, "LastSale", connWEB);

                string idVenta = dt.Rows[0][0].ToString();
                IDPRINTVENTA = idVenta;
                btnPDF = true;
                //Threads GeneraRtpVenta = new Threads(IDPRINTVENTA);
                //Thread CreaPDF = new Thread(GeneraRtpVenta.CreaPDFventa);
                //CreaPDF.Start();
                btnPDF = false;
                var mail = ConfigurationManager.AppSettings.Get("CorreosHojaRoja");
                Threads MailsEnvoy = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", mensajeMEFinal, asuntoN, mail, IDPRINTVENTA);
                Thread EnviaMail = new Thread(MailsEnvoy.CreaPDFventaYenviaCorreo);
                EnviaMail.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public void CheckOrder(AddSale Infoventa, string IDPRINTVENTA)
        {
            try
            {
                Threads Instancia = new Threads(Infoventa, IDPRINTVENTA);
                Thread CheckOrder = new Thread(Instancia.PedidoUrgente);
                CheckOrder.Start();
            }
            catch (Exception ex)
            {

            }


        }
        [Route("api2/GetInfoApDesc")]
        [HttpPost]
        public MessageView GetInfoApDesc(RequestApDesc Apdesc)
        {
            DataTable dt;
            string connstringWEB;
            var Message = new MessageView();
            try
            {

                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Statushide", Value = Apdesc.Statushide },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Statusdesc", Value = Apdesc.Statusdesc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Iduser", Value = Apdesc.Iduser },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Idstored", Value = Apdesc.Idstored },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdArticulo", Value = Apdesc.IdArticulo },
                         new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Cantdescuento", Value = Apdesc.Cantdescuento },
                          new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = Apdesc.IdCliente },
                          new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Observaciones", Value = Apdesc.Observaciones },
                          new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdList", Value = Apdesc.idList },
                          new System.Data.SqlClient.SqlParameter(){ ParameterName = "@PriceFDO", Value = Apdesc.PriceFDO }
                          //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdDescuento", Value ="" }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                //var result = obj.EjecutaQry_Tabla("Insert_AprobacionDesc", lsParameters, CommandType.StoredProcedure, connstringWEB);
                dt = obj.EjecutaQry_Tabla("Insert_AprobacionDesc", lsParameters, CommandType.StoredProcedure, connstringWEB);

                Message.Success = true;
                return Message;
            }
            catch (Exception ex)
            {
                Message.Success = false;
                Message.Message = "Error al realizar aprobación";
                return Message;
            }
        }
        [Route("api5/GetInfoDosPorUno")]
        public List<PromocionDosPorUno> GetInfoDosPorUno()
        {
            List<PromocionDosPorUno> lsS = null;
            try
            {
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();

                //Conexion a db sa Sap pruevas
                connSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                
                //conexion a db de Sap Productiva
                //connSAP = ConfigurationManager.ConnectionStrings["DBConnSAPProductiva"].ConnectionString;

                var squery = "select itemcode, itemname from oitm where qrygroup48 = 'Y'";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "VentasEncabezado", connSAP);
                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new PromocionDosPorUno
                              {
                                  ItemCode = rows["itemcode"] is DBNull ? "" : (string)rows["itemcode"],
                                  ItemName = rows["itemname"] is DBNull ? "" : (string)rows["itemname"]
                              }).ToList();

                    return (ls);
                }
                return lsS;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
