using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using APIPOSS.Models.Ventas;
using APIPOSS.Models.Facturacion;
using System.Text;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;

namespace APIPOSS.Controllers
{
    public class VentasFranquiciasController : ApiController
    {
        private string connstringWEB;
        private string connstringSAP;
        private string _NameBDPos = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
        private string _NameBDSap = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
        [Route("api/GetArticulosF")]
        public JsonResult<List<ListArticuloView>> GetArticulosF()
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            try
            {
                 connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                UserQuery.Append("select ItmsTypCod as code,ItmsGrpNam as name from OITG where ItmsGrpNam not like '%Artículos propiedad%' and ItmsTypCod in (2,3,6) or ItmsTypCod between 9 and 22");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "R", connstringWEB);

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
        [Route("api/GetArticulosFCve")]
        public JsonResult<List<ListArticuloCVEView>> GetArticulosFCve()
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            try
            {
                 connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                UserQuery.Append("select distinct ItemCode as code, IsNull(U_BXP_CVEB, 0) As name  from OITM where QryGroup7='Y' and (QryGroup43='N' or QryGroup42='Y')");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "R", connstringWEB);

                if (dt != null)
                {
                    var dtArticulosCVE = (from DataRow rows in dt.Rows
                                       select new ListArticuloCVEView
                                       {
                                           code = Convert.ToString(rows["code"]),
                                           name = (string)rows["name"]
                                       }).ToList();
                    return Json<List<ListArticuloCVEView>>(dtArticulosCVE);
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;

        }
        [Route("api/GetArticulosFCode")]
        public JsonResult<List<ListArticuloCodeView>> GetArticulosFCode()
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            try
            {
                 connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                UserQuery.Append("select distinct ItemCode as code, IsNull(ItemCode, 0) As name  from OITM where QryGroup7='Y' and (QryGroup43='N' or QryGroup42='Y')");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "R", connstringWEB);

                if (dt != null)
                {
                    var dtArticulosCode = (from DataRow rows in dt.Rows
                                          select new ListArticuloCodeView
                                          {
                                              code = Convert.ToString(rows["code"]),
                                              name = (string)rows["name"]
                                          }).ToList();
                    return Json<List<ListArticuloCodeView>>(dtArticulosCode);
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api/GetPriceF")]
        public JsonResult<List<ListListOfPriceView>> GetPriceF(string StoreSap)
        {
            string UserQuery = string.Empty;
            Utilities.DBMaster obj = new Utilities.DBMaster();
            GetPriceView PriceView = null;
            DataTable dt = new DataTable();
            int Listas = 0;
            string ListName = string.Empty;
            string JsonGetArticulosView = string.Empty;
            try
            {
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
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
                            dt = obj.EjecutaQry_Tabla(UserQuery, CommandType.Text, "Listas", connstringWEB);
                            if (dt != null)
                            {
                                var dtPrice = (from DataRow rows in dt.Rows
                                               select new ListListOfPriceView
                                               {
                                                   ListID = Convert.ToInt32(rows["ListID"] == null ? 0 : rows["ListID"]),
                                                   ListName = (string)rows["ListName"]
                                               }).ToList();
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
                                return Json<List<ListListOfPriceView>>(dtPrice);
                            }
                            return null;
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
        [Route("api/GetModelosF")]
        public JsonResult<List<ListModeloView>> GetModelosF(string Tipo)
        {
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
            Utilities.DBMaster obj = new Utilities.DBMaster();
            string UserQuery = string.Empty;
            ListModeloView ClientesView = null;
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
                    UserQuery = ("select distinct ItemCode as code, ItemName as name from OITM where QryGroup7='N' and QryGroup43='N'");
                }

                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Modelos", connstringWEB);

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
        [Route("api/GetQuantityStoreAndwineryF")]
        public Existencias GetQuantityStoreAndwineryF(string JsonParametrs)
        {
            StringBuilder UserQuery = new StringBuilder();
            StringBuilder UserQuery1 = new StringBuilder();
            StringBuilder UserQuery2 = new StringBuilder();
            Existencias QuantityStorewinery = new Existencias();
            DataTable dt;
            DataTable dt1;
            DataTable dt2;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                var parameter = JsonConvert.DeserializeObject<ExistenciasParametrs>(JsonParametrs);

                UserQuery.Append("select Cantidad from Inventarios where IDArticulo=(select IDArticulo from Articulos where ArticuloSBO='" + parameter.ItemCode + "') and Almacen=" + parameter.IdStore);
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Linea", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        QuantityStorewinery.ExistenciaTienda = Convert.ToInt32(row["Cantidad"] == null ? 0 : row["Cantidad"]);
                    }
                }

                UserQuery1.Append("select OITW.OnHand as Cantidad from DORMIMUNDO_PRODUCTIVA.dbo.OITW OITW where OITW.WhsCode = (select WhsID from AdminStore where AdminStoreID= " + parameter.IdStore + ") and OITW.ItemCode = '" + parameter.ItemCode + "'");
                Utilities.DBMaster obj1 = new Utilities.DBMaster();
                dt1 = obj1.EjecutaQry_Tabla(UserQuery1.ToString(), CommandType.Text, "Linea", connstringWEB);

                if (dt1 != null)
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        QuantityStorewinery.ExistenciaBodega = Convert.ToInt32(row["Cantidad"] == null ? 0 : row["Cantidad"]);
                    }
                }

                UserQuery2.Append("select Cantidad from Inventarios where IDArticulo=(select IDArticulo from Articulos where ArticuloSBO='" + parameter.ItemCode + "') and Almacen=244");
                dt2 = obj.EjecutaQry_Tabla(UserQuery2.ToString(),CommandType.Text,"Linea", connstringWEB);
                if (dt2 != null)
                {
                    foreach (DataRow row in dt2.Rows)
                    {
                        QuantityStorewinery.ExistenciaAlmacen = Convert.ToInt32(row["Cantidad"] == null ? 0 : row["Cantidad"]);
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
        [Route("api/GetUnitPriceF")]
        public double GetUnitPriceF(string JsonUnitPrice)
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
        [Route("api/GetIvaF")]
        public int GetIvaF(string idStore)
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

        [Route("api/GetComboLineF")]
        public ResponseJgoView GetComboLineF(string JsonComboLine)
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
                        CAlmacen= "0",
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
                        CAlmacen = "0",
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
    }
}
