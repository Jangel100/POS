using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebPOS.Utilities;

namespace WebPOS.Controllers.EntregaPedido
{
    public class EntregaPedidoController : Controller
    {
        
        private string connstringWEB;
        private string connstringSAP;
        // GET: EntregaPedido1      
        public ActionResult EntregaPedido(string id)
        {
            string idventa="";
            string idstore="";
            var listapagos = new List<string[]>();
            DBMaster oDB = new DBMaster();
            string sQueryP, sQueryCk, sQueryU, identrega, numchk, numintentos, sQueryU1, sQueryCp, sQueryPt, sQueryppd, sQueryEF;
            DataTable dtp = new DataTable(), dtck = new DataTable(), dtv = new DataTable(), dto = new DataTable();
            string sError2 = "";
            string url = id;
            string[] content;
            string Statusve, sQueryOri;
            bool Completo;
            bool Consigna = false;
            bool IsDormicredit = false;
            string Notificacion_Correo, NoCelular_User, NoPedido, Notificacion_Client, NoCelular_Client, Tipodeventa;

            ViewBag.PanelFranquicia = false;
            ViewBag.label1 = true;
            ViewBag.label2 = true;
            ViewBag.label3 = true;
            ViewBag.label4 = true;
            ViewBag.label5 = true;
            ViewBag.label6 = true;
            ViewBag.label7 = true;
            ViewBag.label8 = true;
            ViewBag.label9 = true;
            ViewBag.label10 = true;
            ViewBag.label11 = true;

            if (!string.IsNullOrEmpty(id))//valida que no venga el id en null
                    {
                var idv =id;
                content = idv.Split('.');
                idventa = content[0];
                idstore = content[1];

                ViewBag.Idstore = idstore;
                ViewBag.Idventa = idventa;

                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                sQueryP = $"select v.Facturado,v.StatusVenta,(Case WHEN a.CorreoElectronico IS NULL THEN '' ELSE a.CorreoElectronico END) as COrreo,v.Folio,(Case WHEN a.NoCelular IS NULL THEN '' ELSE a.NoCelular END) as NoCelularUser,(Case WHEN c.Correo IS NULL THEN '' ELSE c.Correo END) as CorreoClient,(Case WHEN c.NoCelular IS NULL THEN '' ELSE c.NoCelular END) as NoCelularClient,v.TipodeVenta from VentasEncabezado v LEFT OUTER JOIN AdminUser a ON v.IDUser = a.AdminUserID INNER JOIN Clientes c ON c.id = v.IDCliente where v.ID={idventa}";// AND v.IDStore={idstore}
                // verifica si esta completado o no el pedido
                
                dtp = oDB.EjecutaQry_Tabla(sQueryP, CommandType.Text, "checkcompletado", connstringWEB);
                Completo = Convert.ToBoolean(dtp.Rows[0][0]);
                Statusve = dtp.Rows[0][1].ToString();
                Statusve = Statusve.TrimEnd();
                Notificacion_Correo = dtp.Rows[0][2].ToString();
                NoPedido = dtp.Rows[0][3].ToString();
                NoCelular_User = dtp.Rows[0][4].ToString();
                Notificacion_Client = dtp.Rows[0][5].ToString();
                NoCelular_Client = dtp.Rows[0][6].ToString();
                if (Statusve.Equals("CA") || Statusve.Equals("D"))
                {
                    //    Label11.Hidden = False -------------revisar
                    //PanelFranquicia.Hidden = True

                    ViewBag.label1 = false;
                    ViewBag.PanelFranquicia = true;
                }
                try
                {
                    Tipodeventa = dtp.Rows[0][7].ToString();
                }
                catch (Exception ex)
                {
                    Tipodeventa = "ok";
                }
                if (string.IsNullOrEmpty(Tipodeventa))
                {
                    Tipodeventa = "ok";
                }
                // Completo = IIf(Completado = 1, True, False)
                sQueryCk = "select top 1 identrega,Numerochecks, Numdiasintentos from EntregaPedidos where Idventa=" + idventa ;
                dtck = oDB.EjecutaQry_Tabla(sQueryCk, CommandType.Text, "Consultarchecks", connstringWEB);
                identrega = dtck.Rows[0][0].ToString();
                numchk = dtck.Rows[0][1].ToString();
                numintentos = dtck.Rows[0][2].ToString();
                Session["Pedido"] = NoPedido;
                if (numintentos == "0")
                {
                    numintentos = "1";
                }
                else
                {
                    //numintentos = numintentos;
                    if (numintentos == "3")
                    {
                        ViewBag.PanelFranquicia = true;
                        ViewBag.label7 = false;
                        return View();

                    }
                }
                IsDormicredit = Tipodeventa == "Dormicredit" ? true : false; // Es venta de dormicredit si es no validamos pagos solo que este facturado
                //var Utilidad = new Utilidades();
                //string Codigo = "";
                
                var textCode = "";//variable vacia y se una sin ningun valor!!!!!!

                // validamos que el saldo sea menor o igual que 0.00
                sQueryU1 = "select (select SUM(TotalLinea) -  (select SUM(Monto) from VentasPagos where VentasPagos.IDVenta = VE.ID) from VentasDetalle where VentasDetalle.IDVenta =VE.ID) as [Saldo] from VentasEncabezado VE INNER JOIN VentasDetalle VD ON  VE.ID = VD.IDVenta where  VE.IDStore=" + idstore + "and VE.ID=" + idventa;
                dtv = oDB.EjecutaQry_Tabla(sQueryU1, CommandType.Text, "validasaldo", connstringWEB);
                decimal saldo = Convert.ToDecimal(dtv.Rows[0][0]);
                if (saldo > 0 && !IsDormicredit)
                {
                    //PanelFranquicia.Hidden = True
                    //Label8.Hidden = False
                    ViewBag.PanelFranquicia = true;
                    ViewBag.label8 = false;
                    return View();
                }
                if (!Completo)
                {
                    //    PanelFranquicia.Hidden = True
                    //Label10.Hidden = False
                    ViewBag.PanelFranquicia = true;
                    ViewBag.label10 = false;
                    return View();
                }
                // valida que metodo de pago sea ppd
                sQueryppd = "select top 1 MetodoPago33, FormaPago33 from ClientesFacturacion where IDVenta=" + idventa + "order by 1 asc";
                dtv = oDB.EjecutaQry_Tabla(sQueryppd, CommandType.Text, "Validametododepago", connstringWEB);
                try
                {
                    var metodopago = dtv.Rows[0][0].ToString();
                }
                catch (Exception ex)
                {
                    // valida buscano en POS ligero ventas encabezado
                    sQueryppd = "select top 1  FormaPago33 , MetodoPago33 from VentasEncabezado where ID=" + idventa + " order by 1 asc";
                    dtv = oDB.EjecutaQry_Tabla(sQueryppd, CommandType.Text, "Validametododepago", connstringWEB);
                    //return View();
                }
                if (dtv.Rows[0][0].ToString().Equals("PPD") && !IsDormicredit)
                {
                    dtv.Clear();
                    // validamos que ya se encuentren sus complementos de pago antes de completar el pedido
                    sQueryCp = "select COUNT(*) from VentasPagos where IDVenta=" + idventa;
                    dtv = oDB.EjecutaQry_Tabla(sQueryCp, CommandType.Text, "Validacomplemento", connstringWEB);
                    decimal count = Convert.ToDecimal(dtv.Rows[0][0]);
                    if (count >= 1)
                    {
                        sQueryPt = "select ID,FechaCFDI,Facturado,Archivo_FE from VentasPagos where IDVenta=" + idventa + " and Monto > 0";
                        dtv.Clear();
                        dtv = oDB.EjecutaQry_Tabla(sQueryPt, CommandType.Text, "Listadeabonos", connstringWEB);
                        if (dtv.Rows.Count >= 1)
                        {
                            foreach (DataRow dt in dtv.Rows)
                            {
                                listapagos.Add(new string[] { dt["ID"].ToString(), dt["FechaCFDI"].ToString(), dt["Facturado"].ToString(), dt["Archivo_FE"].ToString() });
                            }
                            foreach (var rowitem in listapagos)
                            {
                                string item = rowitem[0];
                                string factura = rowitem[2];
                                var fact = (factura == null || factura == "") ? false : true;//Interaction.IIf(factura == null || factura == "", false, true);
                                if (!fact)
                                {
                                    //    PanelFranquicia.Hidden = True
                                    //Label9.Hidden = False

                                    ViewBag.PanelFranquicia = true;
                                    ViewBag.label9 = false;
                                    return View();
                                    //Exit Sub
                                }
                            }
                        }
                    }
                }
                if (numchk == "0")
                {
                    ViewBag.PanelFranquicia = true;//PanelFranquicia.Hidden = true;
                    ViewBag.label6 = false;//Label6.Hidden = false;
                    sQueryU = "update EntregaPedidos set Numerochecks='1', Numdiasintentos='" + numintentos + "', Ultimafechaintento=Getdate() where Identrega=" + identrega + "";
                    oDB.EjecutaQry(sQueryU, CommandType.Text, connstringWEB, sError2);

                    // ********************envia mensaje
                    string textmessagevend = "Estimado Vendedor buen dia, Nos es grato notificarte que el pedido N° " + NoPedido + ", ha sido embarcado";
                    string textmessageclie = "Estimado Cliente su pedido " + NoPedido + " ha sido embarcado, " + textCode;
                    Threads Message1 = new Threads("Message1", 1, NoCelular_User, textmessagevend, Notificacion_Correo, NoCelular_Client, Notificacion_Client, textmessageclie, 2);
                    Thread hilo = new Thread(Message1.Message1service);
                    hilo.Start();
                    // ********************envia mensaje
                    // ********************Entrega Wbservice
                    // Dim obj2 As New Threads(idventa, idstore)
                    // Dim hilo2 As New Thread(AddressOf obj2.tarea2)
                    // hilo2.Start()
                    // revisa si viene origen de consigna 
                    sQueryOri = "Select t.Articulo,t.Idstore,t.Origen from (select (select ItemName from DORMIMUNDO_PRODUCTIVA.DBO.oitm where itemcode=(select ArticuloSBO from Articulos where IDArticulo= vd.IDArticulo)) AS 'Articulo' ,vd.IDStore AS 'Idstore' ,CASE isnull(VD.JaliscoConsigna,0) when 0 then case vd.idstore when ve.idstore then 'Tienda' else 'Bodega Consignacion' end  else 'Consignacion' end as [Origen] from ventasEncabezado VE, VentasDetalle VD where VD.IDVenta =" + idventa + " and ve.ID=vd.IDVenta AND vd.StatusLinea='O' ) as t GROUP BY t.Articulo, t.Idstore, t.Origen";
                    dto = oDB.EjecutaQry_Tabla(sQueryOri, CommandType.Text, "Origen", connstringWEB);
                    foreach (DataRow Drow in dto.Rows)
                    {
                        string origen = Drow[2].ToString();
                        if (Drow[2].ToString().Equals("Bodega Consignacion"))
                        {
                            Consigna = true;
                        }
                    }
                    if (Consigna)
                    {
                        sQueryEF = "insert into LOGS (TipoOperacion, IdOperacion, IdTienda, Almacen, FechaOperacion) values ('EF'," + idventa + ",0,0,GETDATE())";
                        oDB.EjecutaQry(sQueryEF, CommandType.Text, connstringWEB, sError2);
                    }

                }
                if (Completo & Statusve.Equals("O") || Statusve.Equals("ENC"))//* TODO ERROR: Skipped SkippedTokensTrivia */ // cuando el pedido ya se completo)
                {
                    //    PanelFranquicia.Hidden = True
                    //Label5.Hidden = False
                    ViewBag.PanelFranquicia = true;
                    ViewBag.label5 = false;
                    //Exit Sub
                    return View();
                }
                return View();
            }
            else
            { 
                return View("~"); 
            }
           
        }
        public JsonResult Mostrar(string latitud, string longitud, string idventa, string idstore)
        {
            connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {
                //PanelFranquicia.Hidden = true;//Revisar
                DBMaster oDB = new DBMaster();
                DataTable dt = new DataTable(), dtv = new DataTable(), dtck = new DataTable();
                string sQuery, sQueryC, sQueryU, sQueryV, sQueryE, sQueryCk, identrega, numchk, numintentos, sQueryES;
                string facturado = null;
                string sError2 = "";
                bool existefactura=false;
                bool IsDormicredit = false;
                string Notificacion_Correo, NoCelular_User, NoPedido, Notificacion_Client, NoCelular_Client, Tipoventa;
                sQueryV = "select v.Archivo_FE,(Case WHEN a.CorreoElectronico IS NULL THEN '' ELSE a.CorreoElectronico END) as COrreo,v.Folio,(Case WHEN a.NoCelular IS NULL THEN '' ELSE a.NoCelular END) as NoCelularUser,(Case WHEN c.Correo IS NULL THEN '' ELSE c.Correo END) as CorreoClient,(Case WHEN c.NoCelular IS NULL THEN '' ELSE c.NoCelular END) as NoCelularClient,v.TipodeVenta from VentasEncabezado v LEFT OUTER JOIN AdminUser a ON v.IDUser = a.AdminUserID INNER JOIN Clientes c ON c.id = v.IDCliente where v.ID=" + idventa + " AND v.IDStore=" + idstore + "";
                dtv = oDB.EjecutaQry_Tabla(sQueryV, CommandType.Text, "checkfacturado", ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                facturado = Convert.ToString(dtv.Rows[0][0].ToString()); // verifica si esta facturado
                Notificacion_Correo = dtv.Rows[0][1].ToString();
                NoPedido = dtv.Rows[0][2].ToString();
                NoCelular_User = dtv.Rows[0][3].ToString();
                Notificacion_Client = dtv.Rows[0][4].ToString();
                NoCelular_Client = dtv.Rows[0][5].ToString();
                try
                {
                    Tipoventa = dtv.Rows[0][6].ToString();
                }
                catch (Exception ex)
                {
                    Tipoventa = "ok";
                }
                if (string.IsNullOrEmpty(Tipoventa))
                {
                    Tipoventa = "ok";
                }
                IsDormicredit = Tipoventa == "Dormicredit" ? true : false; // Es venta de dormicredit si es no validamos pagos solo que este facturado
                sQueryCk = "select top 1 identrega,Numerochecks, Numdiasintentos from EntregaPedidos where Idventa=" + idventa + "";
                dtck = oDB.EjecutaQry_Tabla(sQueryCk, CommandType.Text, "Consultarchecks", ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                identrega = dtck.Rows[0][0].ToString();
                numchk = dtck.Rows[0][1].ToString();
                numintentos = dtck.Rows[0][2].ToString();

                switch (numchk)
                {
                    case "0"://CASO 0 CUANDO SE ESCANEA POR PRIMERA VEZ EL MISMO DIA 
                    
                        return Json("label6", JsonRequestBehavior.AllowGet);

                    case "1"://CASO EN QUE YA SE ESCANEO UNA VEZ EL CODIGO EN EL MISMO DIA
                        if (!string.IsNullOrEmpty(facturado))
                        {
                            existefactura = true;
                            if (IsDormicredit)
                            {
                                sQuery = $"update ventasencabezado set FechaConfirmacion=getdate(),StatusVenta='ENC' where id={idventa}"; //ENTREGADO NO COBRADO
                            }
                            else
                            {
                                sQuery = $"update ventasencabezado set FechaConfirmacion=getdate(),StatusVenta='O' where id={idventa}";
                            }
                            oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError2);
                            if (string.IsNullOrEmpty(sError2))//cero errores
                            {
                                sQueryU = "update EntregaPedidos set Statusen='1', Comentarios='Completado', Numerochecks='2', latitud='" + latitud + "', longitud='" + longitud + "'where Identrega=" + identrega + "";
                                oDB.EjecutaQry(sQueryU, CommandType.Text, connstringWEB, sError2);
                                string textmessagevend = "Estimado Vendedor buen dia, el pedido N° " + NoPedido + ", ha sido entregado al cliente";
                                string textmessageclie = "Estimado Cliente buen dia, el pedido N° " + NoPedido + ", ha sido entregado";
                                // ********************envia mensaje
                                Threads Message2 = new Threads("Message2", 1, NoCelular_User, textmessagevend, Notificacion_Correo, NoCelular_Client, Notificacion_Client, textmessageclie, 2);
                                Thread hilo2 = new Thread(Message2.Message2service);
                                hilo2.Start();
                                // ********************envia mensaje
                                // ********************Entrega Wbservice
                                // Dim obj1 As New Threads(idventa, idstore)
                                // Dim hilo1 As New Thread(AddressOf obj1.tarea1)
                                // hilo1.Start()
                                sQueryES = "insert into LOGS (TipoOperacion, IdOperacion, IdTienda, Almacen, FechaOperacion) values ('ES'," + idventa + ",0,0,GETDATE())";
                                oDB.EjecutaQry(sQueryES, CommandType.Text, connstringWEB, sError2);
                             return   Json("label1", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {

                                // aqui poner el error en RutaPDF
                                sQueryU = "update EntregaPedidos set RutaPDF=" + sError2 + " where Identrega=" + identrega + "";
                                oDB.EjecutaQry(sQueryU, CommandType.Text, connstringWEB, sError2);
                       
                              return  Json("label4", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            existefactura = false;
                           return Json("label3", JsonRequestBehavior.AllowGet);
                        }
                        
                    default:
                       return Json("label6", JsonRequestBehavior.AllowGet);
                        
                }
            }
            catch (Exception)
            {
               
                throw;
            }

        }
    }
}