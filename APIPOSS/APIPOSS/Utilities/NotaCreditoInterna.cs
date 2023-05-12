using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using APIPOSS.Models.Ventas;

namespace APIPOSS.Utilities
{
    public class NotaCreditoInterna
    {
        private string connstringWEB;
        private DBMaster oDB;
        private bool Response;

        public NotaCreditoInterna()
        {
            this.connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            this.oDB = new DBMaster();
        }
        public ResponseCancelacionPedido CancelaPedido(RequestCancelacion pedido)
        {
            string squery = string.Empty;
            string sError = string.Empty;
            try
            {
                //Inserta en DevolucionesEncabezado
                squery = "Insert into devolucionesencabezado (IDCliente,IDVenta,IDUser,IDStore,Folio,Prefijo,SUFIJO,Fecha,TipoDevolucion,StatusCierre,Vendedor,UUID_Rel) values (";
                // squery = squery & "'" & cmbCliente.SelectedItem.Value & "'"
                squery = squery + "(select IDCliente from VentasEncabezado where id=" + pedido.idVenta + ")";
                squery = squery + ",'" + pedido.idVenta + "'";
                squery = squery + ",'" + pedido.AdminUserID + "'";
                squery = squery + ",'" + pedido.IdStore + "'";
                squery = squery + ",'" + getCurrentFolio(pedido.IdStore.ToString(), "5") + "'";
                squery = squery + ",'" + getPrefix(pedido.IdStore.ToString(), "5") + "'";
                squery = squery + ",'" + getSufix(pedido.IdStore.ToString(), "5") + "'";
                squery = squery + ",getdate()";
                squery = squery + ",'CA'";
                squery = squery + ",'O'";
                squery = squery + ",'" + getUserCancel(pedido.AdminUserID) + "'";
                squery = squery + ",'" + "" + "')";

                oDB.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                var Details = getDatesEncabezado(pedido.idVenta);
                var IDDevolucion = getLastDevolucion(pedido.AdminUserID);
                if(Details != null)
                {
                    squery = "";
                    foreach (var detail in Details)
                    {
                        //Inserta en DevolucionesDetalle

                        squery = squery + "Insert into devolucionesdetalle (IDDevolucion,IDLinea,IDArticulo,Lista,PrecioUnitario,IVA,Observaciones,Descuento,TotalLinea,Cantidad,StatusLinea,IDStore,Fecha) values(";
                        squery = squery + "'" + IDDevolucion + "'";
                        squery = squery + ",'" + detail.IDLinea + "'";
                        squery = squery + ",'" + detail.IDArticulo + "'";
                        squery = squery + ",'" + detail.Lista + "'";
                        squery = squery + ",'" + detail.PrecioUnitario + "'";
                        squery = squery + ",'" + detail.IVA + "'";
                        squery = squery + ",'" + detail.Observaciones + "'";
                        squery = squery + ",'" + detail.Descuento + "'";
                        squery = squery + ",'" + detail.TotalLinea + "'";

                        squery = squery + ",'" + detail.Cantidad + "'";
                        squery = squery + ",'O'";
                        squery = squery + ",'" + detail.IDStore + "'";
                        squery = squery + ",getdate()";
                        squery = squery + ")" + Environment.NewLine;
                        squery = squery + "update ventasdetalle set statuslinea='CA' where id=" + detail.ID + Environment.NewLine;
                        squery = squery + "update VentasEncabezado set StatusVenta='CA' where id=" + pedido.idVenta + Environment.NewLine;
                        squery = squery + "update VentasPagos set StatusPago='CA' where idVenta=" + pedido.idVenta + Environment.NewLine;
                    }
                    oDB.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                    string sError1 = string.Empty;
                    squery = "INSERT INTO PushMoney2 " + Environment.NewLine;
                    squery = squery + "(UserId,Tienda,Franquicia,IDVenta,Origen,FechaVenta,FechaCFDI,Factura,Marca,IdArticulo,PushMoney,Cantidad,Total) " + Environment.NewLine;
                    squery = squery + "SELECT VE.IDUser,ADS.AdminStoreID,OC.CardCode,'" + pedido.idVenta + "','Nota de Credito', VE.Fecha,VE.FechaCFDI,(VE.SUFIJO+' '+VE.Folio),OI.U_BXP_MARCA,OI.ItemCode,OI.U_PM,VD.Cantidad,((VD.Cantidad * OI.U_PM)*-1)AS TotalPush  " + Environment.NewLine;
                    squery = squery + "FROM DORMIMUNDOPOS.dbo.VentasEncabezado VE JOIN DORMIMUNDOPOS.dbo.VentasDetalle VD ON VD.IDVenta = VE.ID" + Environment.NewLine;
                    squery = squery + "JOIN DORMIMUNDOPOS.dbo.AdminStore ADS ON ADS.AdminStoreID = VE.IDStore " + Environment.NewLine;
                    squery = squery + "JOIN DORMIMUNDO_PRODUCTIVA.dbo.OCRD OC ON OC.CardCode = ADS.DefaultCustomer " + Environment.NewLine;
                    squery = squery + "JOIN DORMIMUNDOPOS.dbo.Articulos ART ON ART.IDArticulo = VD.IDArticulo " + Environment.NewLine;
                    squery = squery + "JOIN DORMIMUNDO_PRODUCTIVA.dbo.OITM OI ON ART.ArticuloSBO = OI.ItemCode " + Environment.NewLine;
                    squery = squery + "WHERE VE.ID =" + pedido.idVenta;
                    oDB.EjecutaQry(squery, CommandType.Text, connstringWEB, sError1);
                    var ResponseCanc = new ResponseCancelacionPedido();
                    ResponseCanc.idDevolucion = IDDevolucion;
                    ResponseCanc.statusCancelacion = true;
                    ResponseCanc.InfoPaypackCancel = new List<InfoPaypackCancel>();
                    ResponseCanc.InfoPaypackCancel = GetInfoPayback(pedido.idVenta);
                    return ResponseCanc;

                }


            }
            catch (Exception ex) { return null; }

            return null;
        }
        public bool omiteNC(string id, string idStore)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "select AdminFolioType from StoreFolios where AdminStoreID=" + idStore + " and Prefijo=(select prefijo from VentasEncabezado where ID=" + id + ")";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "LastSale", connstringWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if (Drow["AdminFolioType"].ToString() == "4")
                    return false;
            }

            return true;

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public string getItemID(string Item)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "select IDArticulo from articulos where ArticuloSBO = '" + Item + "'";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "IDArticulo", connstringWEB);

            foreach (DataRow Drow in dt.Rows)

                return Drow["IDArticulo"].ToString();

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public string getLastDevolucion(string user)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "select max(id) as LastSale from devolucionesencabezado where iduser=" + user;

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "LastSale", connstringWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if (!DBNull.Value.Equals(Drow["LastSale"]))
                    return Drow["LastSale"].ToString();
            }

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public bool getFE(string folio)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "select count(*) as Devolucion from ventasencabezado where folio='" + folio[1].ToString().Split('-') + "' and prefijo='" + folio[0].ToString().Split('-') + "' and facturado=1";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Devolucion", connstringWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if (Drow["Devolucion"].ToString() == "0")
                    return false;
                else
                    return true;
            }

            return false;

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public string getCurrentFolio(string AdminStoreID, string AdminFolioType)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "(select (currentfolio + 1) as [NuevoFolio] from storeFolios where AdminStoreID=" + AdminStoreID + " and AdminFolioType=" + AdminFolioType + ")";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "UltimoFolio", connstringWEB);

            foreach (DataRow Drow in dt.Rows)

                return Drow["NuevoFolio"].ToString();

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public string getPrefix(string AdminStoreID, string AdminFolioType)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "(select prefijo from storeFolios where AdminStoreID=" + AdminStoreID + " and AdminFolioType=" + AdminFolioType + ")";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Prefijo", connstringWEB);

            foreach (DataRow Drow in dt.Rows)

                return Drow["prefijo"].ToString();

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public string getSufix(string AdminStoreID, string AdminFolioType)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "(select NoAprobacion from storeFolios where AdminStoreID=" + AdminStoreID + " and AdminFolioType=" + AdminFolioType + ")";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Sufijo", connstringWEB);

            foreach (DataRow Drow in dt.Rows)

                return Drow["NoAprobacion"].ToString();

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public bool inventoryExist(string IDArticulo, string WHSID)
        {
            string sQuery;
            DataTable dt = new DataTable();


            sQuery = "select count(*) as exist from Inventarios where IDArticulo=" + IDArticulo + " and Almacen='" + WHSID + "'";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Exist", connstringWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if (Drow["exist"].ToString() == "0")
                    return false;
                else
                    return true;
            }

            return false;

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public void GetPeriods(string Value)
        {
            DataTable dt = new DataTable();
            string squery;

            squery = "select distinct CAST(DATENAME(month, VE.Fecha) AS varchar) + '-' + CAST(CAST(DATEPART(year, VE.Fecha) AS varchar) AS varchar) as Periodo ";
            squery = squery + " from ventasEncabezado VE";
            squery = squery + " where VE.IDCliente =" + Value;

            dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "Periodos", connstringWEB);

            // cmbFolio.Visible = False


            // cmbTipo.Disabled = True
            // cmbTipo.SelectedIndex = 1

            //foreach (DataRow row in dt.Rows)
            //    cmbFecha.Items.Add(new Ext.Net.ListItem(row("Periodo"), row("Periodo")));


        }
        public bool UpdateStoreFolios(string idStore)
        {
            string sQuery;
            string sError = string.Empty;
            try
            {
                sQuery = "Update storeFolios set currentfolio=currentfolio+1 where AdminStoreID=" + idStore + " and AdminFolioType=" + 5 + "";

                var execute = oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError);//EjecutaQry
                return true;
            }
            catch (Exception ex) { return false; }
        }
        public string getUserCancel(string idUser)
        {
            string sQuery;
            DataTable dt = new DataTable();

            sQuery = "(select FirstName +' '+ LastName as [UserCancel]  from AdminUser where AdminUserID=" + idUser + ")";

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "UserCancel", connstringWEB);

            foreach (DataRow Drow in dt.Rows)

                return Drow["UserCancel"].ToString();

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }

        public List<DetailsSales> getDatesEncabezado(string idVenta)
        {
            string UserQuery = string.Empty;
            DataTable dt;
            try
            {

                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery = ("select ID,IDLinea,IDArticulo,Juego,Lista,PrecioUnitario,IVA,Observaciones,Descuento,TotalLinea,Cantidad,StatusLinea,Importado,IDStore,Fecha from VentasDetalle where IDVenta = " + idVenta + "");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Line", connstringWEB);

                if (dt != null)
                {
                    var ListDetails = (from DataRow rows in dt.Rows
                                       select new DetailsSales
                                       {
                                           ID = (int)rows["ID"],
                                           IDLinea = (int)rows["IDLinea"],
                                           IDArticulo = (int)rows["IDArticulo"],
                                           Juego = (string)rows["Juego"],
                                           Lista = (string)rows["Lista"],
                                           PrecioUnitario = rows["PrecioUnitario"].ToString(),
                                           IVA = rows["IVA"].ToString(),
                                           Observaciones = (string)rows["Observaciones"],
                                           Descuento = rows["Descuento"].ToString(),
                                           TotalLinea = rows["TotalLinea"].ToString(),
                                           Cantidad = rows["Cantidad"].ToString(),
                                           StatusLinea = (string)rows["StatusLinea"],
                                           Importado = (bool)rows["Importado"],
                                           IDStore = (int)rows["IDStore"]
                                       }).ToList();

                    return ListDetails;
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<InfoPaypackCancel> GetInfoPayback(string idVenta)
        {
            string sQuery;
            DataTable dt = new DataTable();

            try
            {
                sQuery = "select idTransaction,PartnerShortName,BranchShortName,ReceiptNumber,RedemptionNumber,Monedero,StatusTrans,TypeTransaction from Dat_TransactionPayback where idVenta = " + idVenta + " and StatusTrans = 1";

                dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Dat_TransactionPayback", connstringWEB);
                if (dt != null)
                {
                    foreach (DataRow Drow in dt.Rows)
                    {
                        var ListDetails = (from DataRow rows in dt.Rows
                                           select new InfoPaypackCancel
                                           {
                                               idTransaction = (int)rows["idTransaction"],
                                               PartnerShortName = (string)rows["PartnerShortName"],
                                               BranchShortName = (string)rows["BranchShortName"],
                                               ReceiptNumber = (string)rows["ReceiptNumber"],
                                               RedemptionNumber = (string)rows["RedemptionNumber"],
                                               Monedero = (string)rows["Monedero"],
                                               StatusTrans = (bool)rows["StatusTrans"],
                                               TypeTransaction = (string)rows["TypeTransaction"]
                                           }
                                           ).ToList();
                        return ListDetails;
                    }

                    dt = null/* TODO Change to default(_) if this is not a reference type */;
                    GC.Collect();
                }
                return null;
            }
            catch (Exception ex) { return null; }         
        }
    }
}