using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Http;
using System.Web.Http.Results;
using APIPOSS.Models.Garantias;
using System.Linq;
using APIPOSS.Utilities;
using System.Threading;
using Newtonsoft.Json;

namespace APIPOSS.Controllers
{
    public class GarantiasController : ApiController
    {
        private string _connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        private string _connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
        private string _NameBDPos = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
        private string _NameBDSap = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
        [Route("api/GetGarantias")]
        public JsonResult<List<GarantiasIn>> GetGarantias()
        {
            DataTable dt;
            try
            {
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NameBDSAP", Value = _NameBDSap}
                    };
                DBMaster obj = new DBMaster();
                dt = obj.EjecutaQry_Tabla("GarantiasInfo", lsParameters, CommandType.StoredProcedure, "GARANTIAS", _connstringWEB);
                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new GarantiasIn
                              {
                                  id_garantia = Convert.ToString(rows["# Solicitud"]),
                                  Fecha = (DateTime)rows["Fecha Solicitud"],
                                  Franquicia = (string)rows["Franquicia"],
                                  Tienda = (string)rows["Tienda"],
                                  FechaVenta = (DateTime)rows["Fecha de Venta"],
                                  FechaGaranti = (DateTime)rows["FechaGaranti"],
                                  Cliente = (string)rows["Cliente"],
                                  Direccion = (string)rows["Dirección"],
                                  QuienSol = (string)rows["QuienSol"],
                                  ItemName = (string)rows["Modelo"],
                                  Auditor = (string)rows["Auditor"],
                                  e_mail = (string)rows["e-mail"],
                                  Estatus = (string)rows["Estatus"]
                              }).ToList();

                    return Json<List<GarantiasIn>>(ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/GetGarantiasxAprobar")]
        public JsonResult<List<GarantiasIn>> GetGarantiasxAprobar()
        {
            DataTable dt;
            try
            {
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NameBDSAP", Value = _NameBDSap}
                    };
                DBMaster obj = new DBMaster();
                dt = obj.EjecutaQry_Tabla("GarantiasVentas", lsParameters, CommandType.StoredProcedure, "GARANTIASV", _connstringWEB);
                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new GarantiasIn
                              {
                                  id_garantia = Convert.ToString(rows["# Solicitud"]),
                                  Fecha = (DateTime)rows["Fecha Solicitud"],
                                  Franquicia = (string)rows["Franquicia"],
                                  Tienda = (string)rows["Tienda"],
                                  FechaVenta = (DateTime)rows["Fecha de Venta"],
                                  FechaGaranti = (DateTime)rows["FechaGaranti"],
                                  Cliente = (string)rows["Cliente"],
                                  Direccion = (string)rows["Dirección"],
                                  QuienSol = (string)rows["QuienSol"],
                                  ItemName = (string)rows["Modelo"],
                                  Auditor = (string)rows["Auditor"],
                                  e_mail = (string)rows["e-mail"],
                                  Estatus = (string)rows["Estatus"]
                              }).ToList();

                    return Json<List<GarantiasIn>>(ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/GetResponseAction")]
        public JsonResult<GarantiasIn> GetResponseAction(string JsonString)
        {
            try
            {
                var ModeloGarantia = JsonConvert.DeserializeObject<GarantiasIn>(JsonString);
                string IdAbono = ModeloGarantia.id_garantia;
                string tipo = ModeloGarantia.Tipo;
                string Comments = ModeloGarantia.Comentarios;
                string Error = string.Empty;
                string sQuery;
                DataTable dt = new DataTable();
                string MensajeCorreo, Correos;
                string Tienda, email, RutaArchivo, Articulo, Hun, Res, Tela, otros, comotros;
                DBMaster oDB1 = new DBMaster();
                sQuery = "select " + Environment.NewLine;
                sQuery = sQuery + "Franquicia as [Franquicia] " + Environment.NewLine;
                sQuery = sQuery + ",UPPER(Tienda) as [Tienda] " + Environment.NewLine;
                sQuery = sQuery + ",e_mail as [email] " + Environment.NewLine;
                sQuery = sQuery + ",Archivos as [Archivos] " + Environment.NewLine;
                sQuery = sQuery + ",(select ItemName from DORMIMUNDO_PRODUCTIVA.DBO.oitm where itemcode=Modelo) as [Articulo] " + Environment.NewLine;
                sQuery = sQuery + ",case Hundimiento when 1 then 'Hundimiento' else ''end as [Hundimiento]" + Environment.NewLine;
                sQuery = sQuery + ",case Resortes when 1 then 'Resortes' else ''end as [Resortes]" + Environment.NewLine;
                sQuery = sQuery + ",case Tela when 1 then 'Tela' else ''end as [Tela]" + Environment.NewLine;
                sQuery = sQuery + ",case Otros when 1 then 'Otros' else ''end as [Otros]" + Environment.NewLine;
                sQuery = sQuery + ",Otros_c as [OtrosComentarios] " + Environment.NewLine;
                sQuery = sQuery + "from garantias where id_garantia=" + IdAbono;

                dt = oDB1.EjecutaQry_Tabla(sQuery, CommandType.Text, "DatosCorreo", _connstringWEB);
                Tienda = (string)dt.Rows[0]["Tienda"];
                email = (string)dt.Rows[0]["email"];
                RutaArchivo = (string)dt.Rows[0]["Archivos"];
                Articulo = (string)dt.Rows[0]["Articulo"];
                Hun = (string)dt.Rows[0]["Hundimiento"];
                Res = (string)dt.Rows[0]["Resortes"];
                Tela = (string)dt.Rows[0]["Tela"];
                otros = (string)dt.Rows[0]["Otros"];
                comotros = (string)dt.Rows[0]["OtrosComentarios"];
                // mmendoza@dormimundo.com.mx
                // logis@dormimundo.com.mx
                // rperez@dormimundo.com.mx
                string defectos;


                if (Hun == "")
                    defectos = "";
                else
                    defectos = Hun;

                if (Res == "")
                    defectos = defectos + "";
                else
                    defectos = defectos + ", " + Res;

                if (Tela == "")
                    defectos = defectos + "";
                else
                    defectos = defectos + ", " + Tela;

                if (otros == "")
                    defectos = defectos + "";
                else
                    defectos = defectos + ", " + otros + ": " + comotros;

                //Correos = "logis@dormimundo.com.mx";
                Correos = ConfigurationManager.AppSettings.Get("CorreosGarantias");
                switch (tipo)
                {
                    case "AprobarVentas":
                        {
                            sQuery = "update Garantias set Estatus=1, C1='" + Comments + "' where id_garantia=" + IdAbono;
                            oDB1.EjecutaQry(sQuery, CommandType.Text, _connstringWEB, Error);
                            if (Comments == "")
                                Comments = "Ninguno";
                            MensajeCorreo = "<p>Tienes una nueva Solicitud de Aprobaci&oacute;n de Garant&iacute;a con n&uacute;mero: <Strong> " + IdAbono + ".</Strong> de la Tienda: " + Tienda + "</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Previamente Aprobada por el Departamento de ventas.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Descripci&oacute;n de la Garant&iacute;a:</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Art&iacute;culo: " + Articulo + ".</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Defecto(s): " + defectos + ".</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Adjunto encontrar&aacute;s los archivos correspondientes a esta solicitud.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p><a href= http://10.0.128.108/DMSITE/AdminLogin/Login.aspx> Link de Acceso</a>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<br>Usuario: GARANTIAS" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<br>Contrase&ntilde;a: ALMACEN</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Comentarios Adicionales de la Aprobaci&oacute;n: <br>" + Comments + "</p>";
                            MensajeCorreo = MensajeCorreo + "<p>No es Necesario responder a este correo, solo es con fines informativos.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Saludos.";
                            PreparaCorreoCliente(IdAbono, email, Comments, defectos, Tienda, Articulo, RutaArchivo);
                            Threads MailGarantia = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", MensajeCorreo, "Aprobación de Garantía: " + IdAbono + " de la Tienda: " + Tienda, Correos, RutaArchivo);
                            Thread SendMail = new Thread(MailGarantia.enviarCorreo);
                            SendMail.Start();
                            break;
                        }

                    case "AprovarAlmacen":
                        {
                            sQuery = "update Garantias set Estatus=3, C2='" + Comments + "' where id_garantia=" + IdAbono;
                            oDB1.EjecutaQry(sQuery, CommandType.Text, _connstringWEB, Error);
                            if (Comments == "")
                                Comments = "Ninguno";
                            MensajeCorreo = "<p>Tienes una nueva Solicitud de Aprobaci&oacute;n de Garant&iacute;a con n&uacute;mero: <Strong> " + IdAbono + ".</Strong></p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Previamente Aprobada por el Departamento de Almac&eacute;n.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Adjunto encontrar&aacute;s los archivos correspondientes.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Comentarios Adicionales de la Aprobaci&oacute;n: <br>" + Comments + "</p>";
                            MensajeCorreo = MensajeCorreo + "<p>No es Necesario responder a este correo, solo es con fines informativos.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Saludos.";
                            Threads MailGarantia = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", MensajeCorreo, "Aprobación de Garantía: " + IdAbono, Correos, RutaArchivo);
                            Thread SendMail = new Thread(MailGarantia.enviarCorreo);
                            SendMail.Start();
                            break;
                        }

                    case "Cancelada":
                        {
                            sQuery = "update Garantias set Estatus=4, C1='" + Comments + "'  where id_garantia=" + IdAbono;
                            oDB1.EjecutaQry(sQuery, CommandType.Text, _connstringWEB, Error);
                            if (Comments == "")
                                Comments = "Ninguno";
                            MensajeCorreo = "<p>La Solicutud de Garant&iacute;a con n&uacute;mero: <strong>" + IdAbono + "</strong> Ha sido Cancelada.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Comentarios Adicionales de la Cancelaci&oacute;n: <br>" + Comments + "</p>";
                            MensajeCorreo = MensajeCorreo + "<p>No es Necesario responder a este correo, solo es con fines informativos.</p>" + Environment.NewLine;
                            MensajeCorreo = MensajeCorreo + "<p>Saludos.</p>";
                            Threads MailGarantia = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", MensajeCorreo, "Cancelación de Garantía No.: " + IdAbono + " de la Tienda: " + Tienda, email, RutaArchivo);
                            Thread SendMail = new Thread(MailGarantia.enviarCorreo);
                            SendMail.Start();
                            break;
                        }

                    case "DescargaArchivos":
                        {
                            string DescargarArc;
                            string RutaServer, NuevaRuta;

                            sQuery = "Select Archivos from Garantias where id_garantia=" + IdAbono;
                            dt = oDB1.EjecutaQry_Tabla(sQuery, CommandType.Text, "DatosCorreo", _connstringWEB);
                            //DescargarArc = dt.Rows.Item(0).Item("Archivos");

                            //string ii = DescargarArc.LastIndexOf(@"\");
                            //if (ii != -1)
                            //    NuevaRuta = @"\" + DescargarArc.Substring(ii + 1, DescargarArc.Length - ii - 1);
                            //RutaServer = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + NuevaRuta;

                            //if (System.IO.File.Exists(RutaServer))
                            //    System.IO.File.Delete(RutaServer);

                            //My.Computer.Network.DownloadFile(DescargarArc, RutaServer);

                            //try
                            //{
                            //    Ext.Net.Notification msg = new Ext.Net.Notification();
                            //    Ext.Net.NotificationConfig nconf = new Ext.Net.NotificationConfig();
                            //    nconf.IconCls = "icon-information";
                            //    nconf.Html = "Se ha descargado el archivo en la siguiente ruta " + RutaServer;
                            //    nconf.Title = "Descarga de Archivos";
                            //    msg.Configure(nconf);
                            //    msg.Show();
                            //}
                            //// WindowDescarga.Close()

                            //catch (Exception ex)
                            //{
                            //    Interaction.MsgBox(ex.Message, MsgBoxStyle.OkOnly);
                            //}

                            break;
                        }

                }
                GarantiasIn ResponseGarantia = new GarantiasIn();
                ResponseGarantia.ResponseAPI = true;

                return Json<GarantiasIn>(ResponseGarantia);

            }
            catch (Exception EX)
            {
                throw;
            }
        }


        private void PreparaCorreoCliente(string idGarantia, string correo, string comentarios, string Defectos, string Tienda, string Modelo, string Archivos)
        {
            string MensajeCorreo;
            string sQuery;
            DataTable dt = new DataTable();
            // Correos = "mmendoza@dormimundo.com.mx"
            // mmendoza@dormimundo.com.mx
            // logis@dormimundo.com.mx
            // rperez@dormimundo.com.mx

            MensajeCorreo = "<p>La solicutud con n&uacute;mero: <Strong> " + idGarantia + "</Strong> ha sido aprobada por el departamento de ventas.</p>" + Environment.NewLine;
            MensajeCorreo = "<p>El siguiente paso es realizar el cambio del producto al cliente y posteriormente enviar a Lerma la mercancía.</p>" + Environment.NewLine;
            MensajeCorreo = MensajeCorreo + "<p>Descripción de la Garantía:</p>" + Environment.NewLine;
            MensajeCorreo = MensajeCorreo + "<p>Artículo: " + Modelo + ".</p>" + Environment.NewLine;
            MensajeCorreo = MensajeCorreo + "<p>Defecto(s): " + Defectos + ".</p>" + Environment.NewLine;
            MensajeCorreo = MensajeCorreo + "<p>Comentarios Adicionales de la Aprobaci&oacute;n: <br>" + comentarios + "</p>";
            MensajeCorreo = MensajeCorreo + "<p>No es Necesario responder a este correo, solo es con fines informativos.</p>" + Environment.NewLine;
            MensajeCorreo = MensajeCorreo + "<p>Saludos.";
            Threads MailGarantia = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", MensajeCorreo, "Aprobación de Garantía: " + idGarantia + " de la Tienda: " + Tienda, correo, Archivos);
            Thread SendMail = new Thread(MailGarantia.enviarCorreo);
            SendMail.Start();

        }

    }
}
