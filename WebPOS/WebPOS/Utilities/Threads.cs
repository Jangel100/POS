
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Threading;
using System.Data;
using System.Configuration;
using WebPOS.Controllers;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Entities.Models.Ventas;
using WebPOS.Utilities;
using Entities.viewsModels;
using WebPOS.Controllers.Ventas;


namespace WebPOS.Utilities
{

    public class Threads
    {
        private string name;
        private int idenvio;
        private string usernumber;
        private string messagevend;
        private string useremail;
        private string clientnumber;
        private string clientemail;
        private string messageclien;
        private int idclient;
        // Wservice1
        private string idVenta;
        private string IdStore;

        private string connWEB;
        private string DireccionFis;
        public string DirPDF;
        private MailMessage correos = new MailMessage();
        private SmtpClient envios = new SmtpClient();
        private AddSale Infoventa = new AddSale();
        private string IDPRINTVENTA;

        private string emisor;
        private string password;
        private string mensaje;
        private string asunto;
        private string destinatario;
        private string RutaArchivoPDF;

        private string IDVENTA;

        PaybackController payback = new PaybackController();
        String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";

        public Threads(string name, int idenvio, string usernumber, string messagevend, string useremail, string clientnumber, string clientemail, string messageclien, int idclient)
        {
            this.name = name;
            this.idenvio = idenvio;
            this.usernumber = usernumber;
            this.messagevend = messagevend;
            this.useremail = useremail;
            this.clientnumber = clientnumber;
            this.clientemail = clientemail;
            this.messageclien = messageclien;
            this.idclient = idclient;
        }
        public Threads(string idVenta, string IdStore)
        {
            // Wservice1
            this.idVenta = idVenta;
            this.IdStore = IdStore;
        }
        public Threads(string IDVENTA)
        {
            // Wservice1
            this.IDVENTA = IDVENTA;
        }
        public Threads(AddSale Infoventa, string IDPRINTVENTA)
        {
            // Pedido urgente
            this.Infoventa = Infoventa;
            this.IDPRINTVENTA = IDPRINTVENTA;

        }
        public Threads(string emisor, string password, string mensaje, string asunto, string destinatario, string IDVENTA)
        {
            // Envio de Email y crea pdf
            this.emisor = emisor;
            this.password = password;
            this.mensaje = mensaje;
            this.asunto = asunto;
            this.destinatario = destinatario;
            this.IDVENTA = IDVENTA;

        }

        public void Message1service()
        {
            try
            {
                Thread.Sleep(2 * 1000);
            var Message = new WsMessage.WsMessageSoapClient();
            Message.RecibeDateMessage(idenvio, usernumber, messagevend, useremail, clientnumber, clientemail, messageclien, idclient);
                    }
            catch (Exception ex) { }

        }
        public void Message2service()
        {
            try
            {
                Thread.Sleep(2 * 1000);
            var Message = new WsMessage.WsMessageSoapClient();
            Message.RecibeDateMessage(idenvio, usernumber, messagevend, useremail, clientnumber, clientemail, messageclien, idclient);
            }
            catch (Exception ex) { }
        }
        public void Messagepedido()
        {
            try { 
            Thread.Sleep(2 * 1000);
            var Message = new WsMessage.WsMessageSoapClient();
            Message.RecibeDateMessage(idenvio, usernumber, messagevend, useremail, clientnumber, clientemail, messageclien, idclient);
            }
            catch (Exception ex) { }
        }
        public static string ConvertTo2D(string a, bool requerido = true, bool zeros = true, bool comas = false, bool pesosSign = false)
        {
            var result = a;
            var format = zeros ? (!comas ? "{0:0.00}" : "{0:#,##0.00}") : (!comas ? "{0:0.##}" : "{0:#,##.##}");
            var cifra = (!string.IsNullOrEmpty(result) ? result : "").Replace(",", "").Trim();

            if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(cifra))
            {
                decimal b = 0;

                if (decimal.TryParse(cifra, out b))
                    result = string.Format(format, b);
            }
            else
                result = requerido ? "0.00" : a;

            if (!string.IsNullOrEmpty(result) && !result.Equals("N/A") && pesosSign)
                result = "$ " + result;
            return result;
        }

        //public void tarea1()
        //{
        //    // Logarchivo = archivo & "Realiza las entregas de siesta mundo al cliente"
        //    // Realiza las entregas de siesta mundo al cliente
        //    string connstringWEB;
        //    DBMaster oDB = new DBMaster(), oDB2 = new DBMaster();
        //    string sError = "";
        //    string sQuery, sQuery2, Response;
        //    DataTable dt = new DataTable();
        //    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        //    Thread.Sleep(3 * 1000);
        //    try
        //    {
        //        // Logarchivo = archivo & " Comienza conexion"
        //        var InvoiceSmu = new WsService1.Service1SoapClient();
        //        var Request = InvoiceSmu.InsertSalesDeliverySiestaMundo("Manager", "admin", idVenta);
        //        Response = Request.InnerText;
        //        // Log archivo = archivo & Response
        //        sQuery = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values (" + idVenta + "," + IdStore + "," + "'InsertSalesDeliverySiestaMundo'" + ",'" + Response.Replace(",", "") + "'" + ",Getdate())";
        //        oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError);
        //    }
        //    catch (Exception ex)
        //    {
        //        sQuery = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values (" + idVenta + "," + IdStore + "," + "'ERRORSMU'" + ",'" + Convert.ToString(ex) + "'" + ",Getdate())";
        //        oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError);
        //    }
        //}
        //public void tarea2()
        //{
        //    // Realiza las entregas de dormimundo a siesta mundo 
        //    // Log archivo = archivo & "Realiza las entregas de dormimundo a siesta mundo"
        //    string connstringWEB;
        //    DBMaster oDB = new DBMaster(), oDB2 = new DBMaster();
        //    string sError = "";
        //    string sQuery, sQuery2;
        //    DataTable dt = new DataTable();
        //    connstringWEB = ConfigurationManager.ConnectionStrings("DBConn").ConnectionString;
        //    Thread.Sleep(3 * 1000);
        //    try
        //    {
        //        // Log   archivo = archivo & " Comienza conexion"
        //        var InvoiceDormi = new WsService1.Service1SoapClient();
        //        var Request = InvoiceDormi.InsertSalesDeliveryDorminundo("Manager", "dormi", idVenta);
        //        var Response = Request.InnerText;
        //        // Logarchivo = archivo & Response
        //        sQuery = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values (" + idVenta + "," + IdStore + "," + "'InsertSalesDeliveryDorminundo'" + ",'" + Response.Replace(",", "") + "'" + ",Getdate())";
        //        oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError);
        //    }
        //    catch (Exception ex)
        //    {
        //        sQuery = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values (" + idVenta + "," + IdStore + "," + "'ERRORFDO'" + ",'" + Convert.ToString(ex.Message) + "'" + ",Getdate())";
        //        oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError);
        //    }
        //}

        public void PedidoUrgente()
        {
            try
            {
                CheckOrder(Infoventa, IDPRINTVENTA);
            }
            catch (Exception ex) { }

        }

        public void CreaPDFventaYenviaCorreo()
        {
            try
            {
                var idventa = IDVENTA;
                var RutaFisicaPDF = ReporteVentasSub(idventa);
                enviarCorreo(emisor, password, mensaje, asunto, destinatario, RutaFisicaPDF);

            }
            catch (Exception ex) { }

        }
        public void CreaPDFventaYenviaCorreoFranq()
        {
            try
            {
                var idventa = IDVENTA;
                var RutaFisicaPDF = ReporteVentasSubFranq(idventa);
                enviarCorreo(emisor, password, mensaje, asunto, destinatario, RutaFisicaPDF);

            }
            catch (Exception ex) { }

        }

        public void CheckOrder(AddSale Infoventa, string IDPRINTVENTA)
        {

            try
            {
                DataTable dt, dto;
                DBMaster obj = new DBMaster();
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                int MontoAplicaOrder = Convert.ToInt32(ConfigurationManager.AppSettings["MontoPedidoU"]);
                int MaximoDias = 1;
                bool ShopParticipates = false;
                List<System.Data.SqlClient.SqlParameter> lsParametersTienda = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Idstore", Value = Infoventa.idstore }
                    };
                dto = obj.EjecutaQry_Tabla("InfoStore", lsParametersTienda, CommandType.StoredProcedure, connWEB);
                var Tienda = dto.Rows[0][1].ToString();
                string Asunto = "PEDIDO URGENTE TIENDA " + Tienda.ToUpper();
                string CuerpoMensaje = "Esta es una solicitud de un pedido urgente: <br> Pedido POS numero: " + IDPRINTVENTA + " <br> Fecha de entrega: " + Infoventa.Fechaentrega;
                string Mensaje = "";
                DateTime FechaEntrega = Convert.ToDateTime(Infoventa.Fechaentrega);
                DateTime FechaHoy = DateTime.Now;
                TimeSpan DifFechas = FechaEntrega - FechaHoy;

                int Dias = DifFechas.Days;
                bool NotificationUrgent = (Dias >= 0 && Dias <= MaximoDias) ? true : false;

                double TotalxVenta = Infoventa.ArrayArticulos.Sum(t => Convert.ToDouble(t.Total.Replace("$", ""))); //Total del Pedido

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>() { };
                dt = obj.EjecutaQry_Tabla("StorePartakerUrgentOrder", lsParameters, CommandType.StoredProcedure, connWEB); //Lista de tiendas participantes
                var TiendasParticipante = new List<string>();
                foreach (DataRow line in dt.Rows)
                {
                    TiendasParticipante.Add(line["AdminStoreID"].ToString());
                }
                ShopParticipates = (TiendasParticipante.Contains(Infoventa.idstore)) ? true : false; //Revisa si pertenece a las tiendas participantes

                if (NotificationUrgent && TotalxVenta >= MontoAplicaOrder && ShopParticipates)
                {
                    payback.anade_linea_archivo2(LogWeb, "Entra metodo CheckOrder idventa: " + IDPRINTVENTA);
                    #region Detalles
                    CuerpoMensaje += "<br /> Detalles: <br /> <Table border=" + 1 + "> <Tr><Th>Articulo</Th><Th>CANTIDAD</Th></Tr>";
                    Mensaje += " <br /> <Table border=" + 2 + "> <Tr><Th colspan=" + 2 + "> COMPLEMENTO DEL PEDIDO </Th></Tr>";
                    Mensaje += "<Tr><Th>Articulo</Th><Th>CANTIDAD</Th></Tr>";
                    List<System.Data.SqlClient.SqlParameter> lsParametersArt = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDTIENDA", Value = Convert.ToInt32(Infoventa.idstore) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDVENTA", Value = IDPRINTVENTA }
                    };
                    dto = obj.EjecutaQry_Tabla("ArticulosxPedido", lsParametersArt, CommandType.StoredProcedure, connWEB);
                    string namefranqui;
                    string franqui;
                    double cantidad = 0;
                    // insert de hoja roja
                    namefranqui = dto.Rows[0][2].ToString();
                    franqui = dto.Rows[0][3].ToString();
                    cantidad = Convert.ToDouble(dto.Rows[0][1]);
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
                                CuerpoMensaje += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + resta + ".00" + "</td></Tr>";

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
                                CuerpoMensaje += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + resta + ".00" + "</td></Tr>";

                                cantidadparainsert = stockk.ToString();
                                cantidadpara = Drow["IdArticulo"].ToString();
                                descarticulo = Drow["Articulo"].ToString();
                                band = false;
                                Cantidadex = exbodega;
                            }
                        }


                        if (Cantidadex > 0.0)
                        {
                            Mensaje += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + Cantidadex + ".00" + "</td></Tr>";

                            cantidadparainsert = Cantidadex.ToString();
                            cantidadpara = Drow["IdArticulo"].ToString();
                            descarticulo = Drow["Articulo"].ToString();
                        }

                    }
                    CuerpoMensaje += "</table>";
                    Mensaje += "</table>";
                    var MensajeCompleto = CuerpoMensaje + Mensaje;
                    #endregion
                    ReporteVentasSub(IDPRINTVENTA.ToString());
                    List<System.Data.SqlClient.SqlParameter> lsInsertPar = new List<System.Data.SqlClient.SqlParameter>() {
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value = Convert.ToInt32(Infoventa.idstore) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdVenta", Value = Convert.ToInt32(IDPRINTVENTA) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaPedido", Value = Convert.ToDateTime(FechaHoy) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaEntrega", Value = Convert.ToDateTime(FechaEntrega) }
                    };
                    dt = obj.EjecutaQry_Tabla("InsertPedidoUrgente", lsInsertPar, CommandType.StoredProcedure, connWEB);
                    var mail = ConfigurationManager.AppSettings.Get("CorreosPedidosUrgentes");
                    enviarCorreo("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", MensajeCompleto, Asunto, mail, DireccionFis);
                }
                else
                {
                    //nothing
                }
            }
            catch (Exception ex)
            {
                payback.anade_linea_archivo2(LogWeb, "Error en metodo CheckOrder: " + ex.ToString());
            }


        }
        public string ReporteVentasSub(string IDVENTA)
        {
            string url;
            AbonoMontoView abonoMontoView;
            PedidoMontoView pedidoMontoView;
            Decimal MONTOABONOS, MONTOVENTA, SALDO;
            DBMaster oDB = new DBMaster();
            DataTable dt = new DataTable();
            var RPTS = new VentasRPTSController();
            string downloadAsFilename;
            var connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                var path = ConfigurationManager.AppSettings["RutaFisicaRPTS"].ToString();
                ReportDocument rd = new ReportDocument();
                //var path = System.Web.HttpContext.Current.Server.MapPath("~/Reports/Venta/VentaDormimundo.rpt");
                rd.Load(path);
                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IDVENTA) };
                pedidoMontoView = RPTS.GetClienteSumaTotalPedido(abonoMontoView);

                MONTOABONOS = pedidoMontoView.MONTOABONOS;
                MONTOVENTA = pedidoMontoView.MONTOVENTA;
                SALDO = MONTOVENTA - MONTOABONOS;

                rd.SetParameterValue(0, IDVENTA);
                rd.SetParameterValue(1, SALDO);
                rd.SetParameterValue(2, MONTOABONOS);
                rd.SetParameterValue("@Idventa", IDVENTA);


                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                DirPDF = "C://PDF";
                if (!Directory.Exists(DirPDF))
                {

                    Directory.CreateDirectory(DirPDF);
                    if (!Directory.Exists(DirPDF))
                        Directory.CreateDirectory(DirPDF);
                }

                downloadAsFilename = "C://PDF/HojaRoja_" + IDVENTA + "_.pdf";
                DireccionFis = downloadAsFilename;
              
                ExportToPDF(rd, downloadAsFilename);
                return downloadAsFilename;
            }
            catch (Exception ex)
            {
                payback.anade_linea_archivo2(LogWeb, "Error en metodo ReporteVentasSub: " + ex.ToString());
                return "";

            }
        }
        public string ReporteVentasSubFranq(string IDVENTA)
        {
            string url;
            AbonoMontoView abonoMontoView;
            PedidoMontoView pedidoMontoView;
            Decimal MONTOABONOS, MONTOVENTA, SALDO;
            DBMaster oDB = new DBMaster();
            DataTable dt = new DataTable();
            var RPTS = new VentasRPTSController();
            string downloadAsFilename;
            var connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                var path = ConfigurationManager.AppSettings["RutaFisicaFranquiciatarioRPTS"].ToString();
                ReportDocument rd = new ReportDocument();
                //var path = System.Web.HttpContext.Current.Server.MapPath("~/Reports/Venta/VentaDormimundo.rpt");
                rd.Load(path);
                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IDVENTA) };
                pedidoMontoView = RPTS.GetClienteSumaTotalPedido(abonoMontoView);

                MONTOABONOS = pedidoMontoView.MONTOABONOS;
                MONTOVENTA = pedidoMontoView.MONTOVENTA;
                SALDO = MONTOVENTA - MONTOABONOS;

                rd.SetParameterValue(0, IDVENTA);
                rd.SetParameterValue(1, SALDO);
                rd.SetParameterValue(2, MONTOABONOS);
                rd.SetParameterValue("@Idventa", IDVENTA);


                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                DirPDF = "C://PDF";
                if (!Directory.Exists(DirPDF))
                {

                    Directory.CreateDirectory(DirPDF);
                    if (!Directory.Exists(DirPDF))
                        Directory.CreateDirectory(DirPDF);
                }

                downloadAsFilename = "C://PDF/HojaRoja_" + IDVENTA + "_.pdf";
                DireccionFis = downloadAsFilename;

                ExportToPDF(rd, downloadAsFilename);
                return downloadAsFilename;
            }
            catch (Exception ex)
            {
                payback.anade_linea_archivo2(LogWeb, "Error en metodo ReporteVentasSub: " + ex.ToString());
                return "";

            }
        }
        public string ExportToPDF(ReportDocument rpt, string NombreArchivo)
        {
            DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
            try
            {
                {
                    var withBlock = rpt.ExportOptions;
                    withBlock.ExportDestinationType = ExportDestinationType.DiskFile;
                    withBlock.ExportFormatType = ExportFormatType.PortableDocFormat;
                }

                if (File.Exists(NombreArchivo))
                    File.Delete(NombreArchivo);
                diskOpts.DiskFileName = NombreArchivo;
                rpt.ExportOptions.DestinationOptions = diskOpts;
                rpt.Export();
            }
            catch (Exception ex)
            {
                if (ex.ToString() != "")
                {
                    NombreArchivo = "";
                }
            }
            return NombreArchivo;
        }
        /// <summary>
        /// envoy the email of sheet red 
        /// </summary>
        /// <param name="emisor"></param>
        /// <param name="password"></param>
        /// <param name="mensaje"></param>
        /// <param name="asunto"></param>
        /// <param name="destinatario"></param>
        /// <param name="RutaArchivoPDF",></param>
        public void enviarCorreo(string emisor, string password, string mensaje, string asunto, string destinatario, string RutaArchivoPDF)
        {
            try
            {
                correos.To.Clear();
                correos.Body = "";
                correos.Subject = "";
                correos.Body = mensaje;
                correos.Subject = asunto;
                correos.IsBodyHtml = true;
                //
                correos.Attachments.Clear();
                Attachment archivoAdjunto = new System.Net.Mail.Attachment(RutaArchivoPDF);
                correos.Attachments.Add(archivoAdjunto);
                //
                correos.To.Add(Strings.Trim(destinatario));
                correos.From = new MailAddress(emisor);
                envios.Credentials = new NetworkCredential(emisor, password);
                envios.Host = "smtp.dormimundo.com.mx";
                envios.Port = 587;
                envios.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
                envios.Send(correos);
            }
            catch (Exception ex)
            {
                payback.anade_linea_archivo2(LogWeb, "Error en metodo enviarCorreo: " + ex.ToString());
            }
        }
    }

}