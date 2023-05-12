using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.IO;

namespace APIPOSS.Utilities
{
    public class Log
    {
        private string Path = "";


        public Log(string Path)
        {
            this.Path = Path;
        }

        public void Add(string sLog)
        {
            CreateDirectory();
            string nombre = GetNameFile();
            string cadena = "";

            cadena += DateTime.Now + " - " + sLog + Environment.NewLine;

            StreamWriter sw = new StreamWriter(Path + "/" + nombre, true);
            sw.Write(cadena);
            sw.Close();

        }

        #region HELPER
        private string GetNameFile()
        {
            string nombre = "";

            nombre = "logApi_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".txt";

            return nombre;
        }

        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);


            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion
    }
    public class GeneralClass
    {

        private string connstringWEB;

        public string InsertaLog(Tipos TipoOperacion, int IdOperacion, int IdTienda, string Almacen)
        {
            DBMaster oDB = new DBMaster();
            string sError = "";
            //string path = HttpContext.Current.Request.MapPath("~");
            //Log oLog = new Log(path);
            //oLog.Add("Hola LOG");
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

            string sQuery;
            DataTable dt = new DataTable();

            sQuery = "Insert into LOGS (TipoOperacion,IdOperacion,IdTienda,Almacen,FechaOperacion) values (" + "'" + TipoOperacion.ToString() + "'," + IdOperacion + "," + IdTienda + "," + "'" + Almacen + "'," + "getdate())";

            if (oDB.EjecutaQry(sQuery, CommandType.Text, connstringWEB, sError) == 1)
                return "1";
            else
                return sError;
        }

        public static string ConvertDataTable(DataTable dt)
        {
            try
            {
                string JSONString = string.Empty;
                JSONString = JsonConvert.SerializeObject(dt);

                return JSONString;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    public enum Tipos
    {
        VE // Venta
,
        DE // devolucion venta
,
        ED // entrada directa
,
        EC // entrada por compra
,
        RE // recepcion envio
,
        SA // salida directa
,
        AB // abono
,
        CO // compra
,
        CE // cancelacion envio
,
        FA // factura
    }
    public struct returnventa
    {
        public int IDPRINTVENTA;
        public int IDSTORE;
        public string WHSID;
        public returnventa(int idventa, int idstore, string whsid)
        {
            IDPRINTVENTA = idventa;
            IDSTORE = idstore;
            WHSID = whsid;
        }
    }

    public struct returnpago
    {
        public int IDPRINTPAGO;
        public int IDSTORE;
        public string WHSID;
        public returnpago(int idabono, int idstore, string whsid)
        {
            IDPRINTPAGO = idabono;
            IDSTORE = idstore;
            WHSID = whsid;
        }
    }
    public struct returncompra
    {
        public int IDPRINTCOMPRA;
        public int IDSTORE;
        public string WHSID;
        public returncompra(int idcompra, int idstore, string whsid)
        {
            IDPRINTCOMPRA = idcompra;
            IDSTORE = idstore;
            WHSID = whsid;
        }
    }


}