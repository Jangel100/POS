using BL.Interface;
using BL.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using Entities.viewsModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;

namespace WebPOS.Controllers
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class HomeController : Controller
    {
        private string connstringSAP;
        private string connstringWEB;
        readonly IReportesBL _VentasReportesBL;
        public HomeController(ReportesBL login)
        {
            _VentasReportesBL = login;
        }

        public HomeController()
        {
            _VentasReportesBL = new ReportesBL();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Download_EntregaPedido_PDF(string IdStore)
        {
            DBMaster oDB = new DBMaster();
            string QRENTRE = "0";
            EntregaPedidoView pedido = new EntregaPedidoView();
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                var parametros = IdStore.Split(',');
                ReportDocument rd = new ReportDocument();
                var path = Server.MapPath("~/Reports/Inicio/Entregaped.rpt");
                rd.Load(path);
                pedido.IdVenta = Convert.ToInt32(parametros[0]);
                pedido.IdStore = GetStorexVenta(pedido.IdVenta);
                //pedido.IdStore = parametros[1];
                QRENTRE = EntregaPedidosInsert(pedido);  //Se realiza la insercion/actualizacion en la tabla Entregapedidos
                //rd.SetParameterValue("imageurl", QRENTRE);
                rd.SetParameterValue(0, Convert.ToInt32(pedido.IdStore));
                rd.SetParameterValue(1, pedido.IdVenta);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDPOS, false);
                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "Archivo_" + parametros[0] + ".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string EntregaPedidosInsert(EntregaPedidoView pedido)
        {
            StringBuilder stringQuery = new StringBuilder();
            try
            {
                pedido = GeneraCodigoQR(pedido);

                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public EntregaPedidoView GeneraCodigoQR(EntregaPedidoView pedido)
        {
            StringBuilder stringQuery = new StringBuilder();
            string connstringWEB;
            string letras,
                   ROUTEQR = "",
                   ruta = "C:/QRENTRE",
                   link = pedido.IdVenta + "." + pedido.IdStore;
            try
            {
                MessagingToolkit.QRCode.Codec.QRCodeEncoder QR_Generator = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
                //POS Pruebas
                //letras = "http://170.245.190.28/ReporteVentasApp/EntregaPedido/EntregaPedido1.aspx?datoDx=" + link; //pruebas
                ROUTEQR = @"C:/QRENTRE/" + (pedido.IdVenta + ".jpg");

                byte[] imageBytes = null;
                object img = new MemoryStream();
                DateTime Datetimes = DateTime.Now;

                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                if (Directory.Exists(ruta))
                {
                    if (!existeQRinTableEntregaPedidos(pedido.IdVenta))
                    {
                        try
                        {
                            Bitmap Image = QR_Generator.Encode(letras);
                            System.Drawing.Image QR = ((System.Drawing.Image)(Image));

                            using (MemoryStream ms = new MemoryStream())
                            {
                                QR.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imageBytes = ms.ToArray();
                                Image.Save(ROUTEQR, System.Drawing.Imaging.ImageFormat.Jpeg);

                                if (System.IO.File.Exists(ROUTEQR))
                                {
                                    pedido.IdVenta = Convert.ToInt32(pedido.IdVenta);
                                    pedido.RutaQR = ROUTEQR;
                                    pedido.Statusen = "0";
                                    pedido.imagen = imageBytes;
                                    pedido.Numdiasintentos = "0";
                                    pedido.Numerochecks = "0";

                                    #region InsertQR
                                    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                                    stringQuery.Append("insert into EntregaPedidos(Fechaentrega,Idventa, RutaQR, Statusen, imagen, Numerochecks, Numdiasintentos) values (@Fechaentrega, @Idventa, @RutaQR, @Statusen, @imagen, @Numerochecks, @Numdiasintentos) ");

                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                                    {
                                        con.Open();

                                        using (SqlCommand com = new SqlCommand(stringQuery.ToString(), con))
                                        {
                                            com.CommandType = System.Data.CommandType.Text;
                                            com.Parameters.Add("@IdVenta", System.Data.SqlDbType.Int).Value = pedido.IdVenta;
                                            com.Parameters.Add("@RutaQR", System.Data.SqlDbType.VarChar).Value = pedido.RutaQR;
                                            com.Parameters.Add("@Statusen", System.Data.SqlDbType.VarChar).Value = pedido.Statusen;
                                            com.Parameters.Add("@Numerochecks", System.Data.SqlDbType.VarChar).Value = pedido.Numerochecks;
                                            com.Parameters.Add("@imagen", System.Data.SqlDbType.VarBinary).Value = imageBytes;
                                            com.Parameters.Add("@Numdiasintentos", System.Data.SqlDbType.VarChar).Value = pedido.Numdiasintentos;
                                            com.Parameters.Add("@Fechaentrega", System.Data.SqlDbType.DateTime).Value = DateTime.Now;

                                            int i = com.ExecuteNonQuery();

                                            return null;
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //else
                    //{
                    //    try
                    //    {
                    //        if (System.IO.File.Exists(ROUTEQR))  //Elimino la imagen para despues volverla a crear
                    //        {
                    //            System.IO.File.Delete(ROUTEQR);  
                    //        }

                    //        //actualizacion de la imagen nueva del QR
                    //        #region 
                    //            Bitmap Image = QR_Generator.Encode(letras);
                    //            System.Drawing.Image QR = ((System.Drawing.Image)(Image));

                    //            using (MemoryStream ms = new MemoryStream())
                    //            {
                    //                QR.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //                imageBytes = ms.ToArray();
                    //                Image.Save(ROUTEQR, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //                if (System.IO.File.Exists(ROUTEQR))
                    //                {
                    //                    #region UpdateQR
                    //                    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                    //                    stringQuery.Append("UPDATE EntregaPedidos SET imagen = @imagen WHERE IdVenta = @IdVenta");

                    //                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                    //                    {
                    //                        con.Open();

                    //                        using (SqlCommand com = new SqlCommand(stringQuery.ToString(), con))
                    //                        {
                    //                            com.CommandType = System.Data.CommandType.Text;
                    //                            com.Parameters.Add("@IdVenta", System.Data.SqlDbType.Int).Value = pedido.IdVenta;
                    //                            com.Parameters.Add("@imagen", System.Data.SqlDbType.VarBinary).Value = imageBytes;

                    //                            int i = com.ExecuteNonQuery();

                    //                            return null;
                    //                        }
                    //                    }
                    //                    #endregion
                    //                }
                    //            }
                    //        #endregion
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}
                }

                return pedido;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public string GetStorexVenta(int idVenta)
        {
            string connstringWEB;
            string idStore = "";
            StringBuilder stringQuery = new StringBuilder();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                stringQuery.Append("SELECT IDStore FROM VentasEncabezado WHERE ID=@id");

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(stringQuery.ToString(), con))
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = idVenta;

                        SqlDataReader dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            idStore = Convert.ToInt32(dr["IDStore"]).ToString();
                        }

                        return idStore;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public bool existeQRinTableEntregaPedidos(int idVenta)
        {
            string connstringWEB;
            string Identrega = "";
            StringBuilder stringQuery = new StringBuilder();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                stringQuery.Append("SELECT Identrega FROM  EntregaPedidos WHERE Idventa=@idVenta");

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(stringQuery.ToString(), con))
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.Parameters.Add("@idVenta", System.Data.SqlDbType.Int).Value = idVenta;

                        SqlDataReader dr = com.ExecuteReader();

                        while (dr.Read())
                        {
                            Identrega = Convert.ToInt32(dr["Identrega"]).ToString();
                        }

                        return Identrega == "" ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
