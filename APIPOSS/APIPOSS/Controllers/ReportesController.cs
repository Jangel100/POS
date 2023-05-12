using APIPOSS.Models;
using APIPOSS.Models.Configuracion;
using APIPOSS.Models.Home;
using APIPOSS.Models.Reportes;
using APIPOSS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIPOSS.Controllers
{
    public class ReportesController : ApiController
    {
        // log
        private string patherror = AppDomain.CurrentDomain.BaseDirectory + @"logErrApi";
        [Route("api2/GetStorebyUserNew")]
        [HttpPost]
        public List<TiendaJsonView> GetStorebyUserNew(TiendaJsonView tienda)
        {
            DataTable dt;
            try
            {
                return GetDataStorebyUser(tienda);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetVendedorByStore")]
        [HttpPost]
        public List<VendedorView> GetVendedorByStore(VendedorView vendedor)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminStoreID", Value = vendedor.@AdminStoreID }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("VendedorByStore", lsParameters, CommandType.StoredProcedure, "AdminUser", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new VendedorView
                              {
                                  AdminUserID = rows["AdminUserID"] is DBNull ? 0 : Convert.ToInt32(rows["AdminUserID"]),
                                  Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsVentas")]
        [HttpPost]
        public string GetReportsVentas(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                var culture = new CultureInfo("es-MX");

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1"; 

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estatus", Value = Reports.Estatus.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsVentas", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var result = GeneralClass.ConvertDataTable(dt);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsFacturacion")]
        [HttpPost]
        public List<ReportsFacturacionView> GetReportsFacturacion(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsFacturacion", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsFacturacionView
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  FechaPedido = rows["FechaPedido"] is DBNull ? null : Convert.ToDateTime(rows["FechaPedido"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  FechaFactura = rows["FechaFactura"] is DBNull ? null : Convert.ToDateTime(rows["FechaFactura"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  FechaCompletado = rows["FechaCompletado"] is DBNull ? null : Convert.ToDateTime(rows["FechaCompletado"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  EstatusVenta = rows["Estatus Venta"] is DBNull ? "" : (string)rows["Estatus Venta"],
                                  Facturado = rows["Facturado"] is DBNull ? "" : (string)rows["Facturado"],
                                  Total_Venta = rows["Total_Venta"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total_Venta"])),
                                  Monto_pagado = rows["Monto_pagado"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Monto_pagado"])),
                                  Saldo = rows["Saldo"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Saldo"])),
                                  Estatus_Factura = rows["Estatus_Factura"] is DBNull ? "" : (string)rows["Estatus_Factura"],
                                  Comentarios = rows["Comentarios"] is DBNull ? "" : (string)rows["Comentarios"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsClienteAvisa")]
        [HttpPost]
        public List<ReportsClienteAvisaView> GetReportsClienteAvisa(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estatus", Value = Reports.Estatus.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsClienteAvisa", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsClienteAvisaView
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  Fecha = rows["Fecha"] is DBNull ? null : rows["Fecha"].ToString(),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  Estatus_Venta = rows["Estatus Venta"] is DBNull ? "" : (string)rows["Estatus Venta"],
                                  Codigo_Articulo = rows["Codigo Articulo"] is DBNull ? "" : (string)rows["Codigo Articulo"],
                                  Articulo = rows["Articulo"] is DBNull ? "" : (string)rows["Articulo"],
                                  Lista_de_Precios = rows["Lista de Precios"] is DBNull ? "" : (string)rows["Lista de Precios"],
                                  Cantidad = rows["Cantidad"] is DBNull ? 0 : Convert.ToInt32(rows["Cantidad"]),
                                  Precio_Unitario = rows["Precio unitario"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Precio unitario"])),
                                  Descuento = rows["Descuento"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Descuento"])),
                                  IVA = rows["IVA"] is DBNull ? 0 : Convert.ToInt32(rows["IVA"]),
                                  Total_Linea = rows["Total Linea"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total Linea"])),
                                  Total_Venta = rows["Total Venta"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total Venta"])),
                                  Monto_Pagado = rows["Monto pagado"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Monto pagado"])),
                                  Saldo = rows["Saldo"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Saldo"])),
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsTotalVenta")]
        [HttpPost]
        public List<ReportsTotalVentaView> GetReportsTotalVenta(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estatus", Value = Reports.Estatus.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsTotalVenta", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsTotalVentaView
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  Fecha = rows["Fecha"] is DBNull ? null : rows["Fecha"].ToString(),
                                  FechaCFDI = rows["FechaCFDI"] is DBNull ? null : rows["FechaCFDI"].ToString(),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  Estatus_Venta = rows["Estatus Venta"] is DBNull ? "" : (string)rows["Estatus Venta"],
                                  Total_Venta = rows["Total Venta"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total Venta"])),
                                  Monto_pagado = rows["Monto pagado"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Monto pagado"])),
                                  Saldo = rows["Saldo"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Saldo"])),
                                  Estatus_Factura = rows["Estatus Factura"] is DBNull ? "" : (string)rows["Estatus Factura"],
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<TiendaJsonView> GetDataStorebyUser(TiendaJsonView tienda)
        {
            DataTable dt;
            string connstringWEB;
            Log logApi = new Log(patherror);
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = tienda.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = tienda.Franquicia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                dt = obj.EjecutaQry_Tabla("StoreByUserNew", lsParameters, CommandType.StoredProcedure, "AdminStore", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new TiendaJsonView
                              {
                                  AdminUserID = rows["AdminStoreID"] is DBNull ? "" : Convert.ToInt32(rows["AdminStoreID"]).ToString(),
                                  Franquicia = rows["StoreName"] is DBNull ? "" : (string)rows["StoreName"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {
                logApi.Add("Error en metodo GetDataStorebyUser: " + ex.Message);
            }
            return null;
        }
        public List<CatalogoView> GetDataModelo()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ModeloInfo", CommandType.StoredProcedure, "OITM", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CatalogoView
                              {
                                  Code = rows["code"] is DBNull ? "" : (string)rows["code"],
                                  Name = rows["name"] is DBNull ? "" : (string)rows["name"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<CatalogoView> GetDataArticulo()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ArticuloInfo", CommandType.StoredProcedure, "OITM", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CatalogoView
                              {
                                  Code = rows["code"] is DBNull ? "" : Convert.ToInt32(rows["code"]).ToString(),
                                  Name = rows["name"] is DBNull ? "" : (string)rows["name"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<CatalogoView> GetDataMarca()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("MarcaInfo", CommandType.StoredProcedure, "OITM", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CatalogoView
                              {
                                  Code = rows["code"] is DBNull ? "" : (string)rows["code"],
                                  Name = rows["name"] is DBNull ? "" : (string)rows["name"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<CatalogoView> GetDataMedida()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("MedidaInfo", CommandType.StoredProcedure, "OITM", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CatalogoView
                              {
                                  Code = rows["code"] is DBNull ? "" : (string)rows["code"],
                                  Name = rows["name"] is DBNull ? "" : (string)rows["name"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetDataReportsVentaxArticulo")]
        [HttpPost]
        public ReportsVentasxArticulo GetDataReportsVentaxArticulo(TiendaJsonView tienda)
        {
            ReportsVentasxArticulo reportsVentasxArticulo;
            try
            {
                reportsVentasxArticulo = new ReportsVentasxArticulo();
                reportsVentasxArticulo.lsTienda = GetDataStorebyUser(tienda);
                reportsVentasxArticulo.lsModelo = GetDataModelo();
                reportsVentasxArticulo.lsArticulo = GetDataArticulo();
                reportsVentasxArticulo.lsLinea = GetDataMarca();
                reportsVentasxArticulo.lsMedida = GetDataMedida();
                return reportsVentasxArticulo;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsVentaxArticulo")]
        [HttpPost]
        public List<ReportsVentaxArticuloView> GetReportsVentaxArticulo(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estatus", Value = Reports.Estatus.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Articulo", Value = Reports.Articulo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Linea", Value = Reports.Linea },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Medida", Value = Reports.Medida },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Modelo", Value = Reports.Modelo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsVentasxArticulo", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsVentaxArticuloView
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  FechaFactura = rows["FechaFactura"] is DBNull ? null : rows["FechaFactura"].ToString(),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  Estatus_Venta = rows["Estatus Venta"] is DBNull ? "" : (string)rows["Estatus Venta"],
                                  Codigo_Articulo = rows["Codigo Articulo"] is DBNull ? "" : (string)rows["Codigo Articulo"],
                                  Articulo = rows["Articulo"] is DBNull ? "" : (string)rows["Articulo"],
                                  Lista_de_Precio = rows["Lista de Precios"] is DBNull ? "" : (string)rows["Lista de Precios"],
                                  Cantidad = rows["Cantidad"] is DBNull ? 0 : Convert.ToInt32(rows["Cantidad"]),
                                  Precio_unitario = rows["Precio unitario"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Precio unitario"])),
                                  Descuento = rows["Descuento"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Descuento"])),
                                  IVA = rows["IVA"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["IVA"])),
                                  Total_Linea = rows["Total Linea"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total Linea"])),
                                  Total_Venta = rows["Total Venta"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total Venta"])),
                                  Monto_pagado = rows["Monto pagado"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Monto pagado"])),
                                  Saldo = rows["Saldo"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Saldo"])),
                                  Push_Money = rows["Push Money"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Push Money"])),
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsIngresos")]
        [HttpPost]
        public IEnumerable<ReportsIngresosView> GetReportsIngresos(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoPago", Value = Reports.TipoPago.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsIngresos", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsIngresosView
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  //Fecha = rows["Fecha"] is DBNull ? null : rows["Fecha"].ToString(),
                                  Fecha = rows["Fecha"] is DBNull ? null : Convert.ToDateTime(rows["Fecha"], CultureInfo.InvariantCulture).ToString("dd/MM/yyyy").ToString(),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  Monto = rows["Monto"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",(decimal)(rows["Monto"])),
                                  Forma_de_Pago = rows["Forma de Pago"] is DBNull ? "" : (string)rows["Forma de Pago"],
                                  Recibo = rows["Recibo"] is DBNull ? "" : (string)rows["Recibo"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsVTotalxTienda")]
        [HttpPost]
        public string GetReportsVTotalxTienda(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsVentaTotalxtienda", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var result = GeneralClass.ConvertDataTable(dt);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsVTotalxVendedor")]
        [HttpPost]
        public string GetReportsVTotalxVendedor(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsVentaTotalxVendedor", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var result = GeneralClass.ConvertDataTable(dt);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsTransferencia")]
        [HttpPost]
        public IEnumerable<ReportsTransferenciasView> GetReportsTransferencia(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsVentaTotalxTransferencias", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsTransferenciasView
                              {
                                  IDEnvio = rows["IDEnvio"] is DBNull ? 0 : Convert.ToInt32(rows["IDEnvio"]),
                                  ArticuloSBO = rows["ArticuloSBO"] is DBNull ? "" : (string)rows["ArticuloSBO"],
                                  Descripcion = rows["Descripcion"] is DBNull ? "" : (string)rows["Descripcion"],
                                  Usuario = rows["Usuario"] is DBNull ? "" : (string)rows["Usuario"],
                                  FechaEnvio = rows["FechaEnvio"] is DBNull ? null : rows["FechaEnvio"].ToString(),
                                  Cantidad = rows["Cantidad"] is DBNull ? 0 : Convert.ToInt32(rows["Cantidad"]),
                                  TiendaOrigen = rows["TiendaOrigen"] is DBNull ? "" : (string)rows["TiendaOrigen"],
                                  TiendaDestino = rows["TiendaDestino"] is DBNull ? "" : (string)rows["TiendaDestino"],
                                  FechaDif = rows["FechaDif"] is DBNull ? 0 : Convert.ToInt32(rows["FechaDif"]),
                                  Status = rows["Status"] is DBNull ? "" : (string)rows["Status"],
                                  Comentarios = rows["Comentarios"] is DBNull ? "" : (string)rows["Comentarios"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportscompras")]
        [HttpPost]
        public IEnumerable<ReportsComprasView> GetReportscompras(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsCompras", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsComprasView
                              {
                                  IDEntrada = rows["IDEntrada"] is DBNull ? 0 : Convert.ToInt32(rows["IDEntrada"]),
                                  DocumentoSAP = rows["DocumentoSAP"] is DBNull ? "" : Convert.ToInt32(rows["DocumentoSAP"]).ToString(),
                                  Artículo = rows["Artículo"] is DBNull ? "" : (string)rows["Artículo"],
                                  Cantidad = rows["Cantidad"] is DBNull ? 0 : Convert.ToInt32(rows["Cantidad"]),
                                  LineaSBO = rows["LineaSBO"] is DBNull ? 0 : Convert.ToInt32(rows["LineaSBO"]),
                                  FechaSBO = rows["FechaSBO"] is DBNull ? "" : rows["FechaSBO"].ToString(),
                                  FechaEntWeb = rows["FechaEntWeb"] is DBNull ? "" : rows["FechaEntWeb"].ToString(),
                                  SNSBO = rows["SNSBO"] is DBNull ? "" : (string)rows["SNSBO"],
                                  Usuario = rows["Usuario"] is DBNull ? "" : (string)rows["Usuario"],
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  FolioPOS = rows["FolioPOS"] is DBNull ? "" : (string)rows["FolioPOS"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsKardex")]
        [HttpPost]
        public IEnumerable<ReportsKardexView> GetReportsKardex(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Whs", Value = Reports.Whs },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Item", Value = Reports.Articulo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FE1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FE2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tienda", Value = Reports.Store }
                };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("Kardex", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsKardexView
                              {
                                  Id = rows["rn"] is DBNull ? "    " : Convert.ToInt32(rows["rn"]).ToString(),
                                  Tipo_Mov = rows["TipoMovimiento"] is DBNull ? "    " : (string)rows["TipoMovimiento"],
                                  Fecha_Movimiento = rows["FechaMovimiento"] is DBNull ? "    " : (string)rows["FechaMovimiento"],
                                  Tienda = rows["Tienda"] is DBNull ? "   " : (string)rows["Tienda"],
                                  Desc_Articulo = rows["Dscription"] is DBNull ? "    " : (string)rows["Dscription"],
                                  //Existencias = rows["SALDO_INICIAL"] is DBNull ? "0" : Convert.ToDecimal(rows["SALDO_INICIAL"]).ToString().Split('.')[0],
                                  Cantidad = rows["Cantidad"] is DBNull ? "0" : Convert.ToDecimal(rows["Cantidad"]).ToString().Split('.')[0],
                                  Cantidad_Acumulada = rows["ACUMULADA"] is DBNull ? "" : Convert.ToInt32(rows["ACUMULADA"]).ToString().Split('.')[0],
                                  Referencia = rows["ReferenciaSAP"] is DBNull ? "" : (string)rows["ReferenciaSAP"],
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public IEnumerable<CatalogoView> GetDataArticuloKardex()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ArticuloKardexInfo", CommandType.StoredProcedure, "OITM", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CatalogoView
                              {
                                  Code = rows["code"] is DBNull ? "" : (string)rows["code"].ToString(),
                                  Name = rows["name"] is DBNull ? "" : (string)rows["name"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public IEnumerable<SAPWS> GetStoreAlmacenes()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("StoreALmacenes", CommandType.StoredProcedure, "OITM", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new SAPWS
                              {
                                  WhsCode = rows["WhsCode"] is DBNull ? "" : (string)rows["WhsCode"].ToString(),
                                  WhsName = rows["WhsName"] is DBNull ? "" : (string)rows["WhsName"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api2/GetFilterKardex")]
        [HttpPost]
        public ReportsFilterKardexView GetFilterKardex(ReportsVentasInputView Reports)
        {
            ReportsFilterKardexView kardexView;
            List<TiendaJsonView> lsTiendas;
            IEnumerable<CatalogoView> lsArticulos;
            TiendaJsonView tienda = new TiendaJsonView(); ;
            try
            {
                tienda.AdminUserID = Reports.AdminUserID;
                tienda.Franquicia = Reports.Franquicia;

                lsTiendas = GetDataStoreWhsID(tienda);
                lsArticulos = GetDataArticuloKardex();

                kardexView = new ReportsFilterKardexView()
                {
                    lsTiendas = lsTiendas,
                    lsArticulo = lsArticulos
                };

                return kardexView;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [Route("api2/GetFranquiciasPushMoney")]
        [HttpPost]
        public IEnumerable<AdminFranquiciasView> GetFranquiciasPushMoney(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminListFranquicia", CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new AdminFranquiciasView
                              {
                                  CardCode = rows["CardCode"] is DBNull ? "" : (string)rows["CardCode"],
                                  CardName = rows["CardName"] is DBNull ? "" : (string)rows["CardName"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetVendedorPushMoney")]
        [HttpPost]
        public IEnumerable<UsersView> GetVendedorPushMoney(UsersView usersView)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = usersView.Franquicia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("VendedorFranquicia", lsParameters, CommandType.StoredProcedure, "Vendedores", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new UsersView
                              {
                                  AdminUserID = rows["AdminUserID"] is DBNull ? "" : Convert.ToInt32(rows["AdminUserID"]).ToString(),
                                  UserName = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsPushMoney")]
        [HttpPost]
        public IEnumerable<ReportsPushMoneyVendedorView> GetReportsPushMoney(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoReporte", Value = Reports.TipoReporte },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Vendedor", Value = Reports.Vendedor },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsPushMoney", lsParameters, CommandType.StoredProcedure, "PushMoney", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsPushMoneyVendedorView
                              {
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Fecha = rows["FechaVenta"] is DBNull ? "" : Convert.ToDateTime(rows["FechaVenta"]).ToString(),
                                  Franquicia = rows["Fran"] is DBNull ? "" : (string)rows["Fran"],
                                  Factura = rows["Factura"] is DBNull ? "" : (string)rows["Factura"],
                                  Producto = rows["Articulo"] is DBNull ? "" : (string)rows["Articulo"],
                                  PushMoney = rows["PushMoney"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                               Convert.ToDecimal((rows["PushMoney"]))),
                                  Cantidad = rows["Cantidad"] is DBNull ? "0" : Convert.ToInt32(rows["Cantidad"]).ToString(),
                                  Total = rows["Total"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              Convert.ToDecimal((rows["Total"])))
                              }).ToList();

                    return ls;
                }

                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsPushMoneyxVendedor")]
        [HttpPost]
        public IEnumerable<ReportsPushMoneyVendedor> GetReportsPushMoneyxVendedor(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoReporte", Value = Reports.TipoReporte },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Vendedor", Value = Reports.Vendedor },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsPushMoney", lsParameters, CommandType.StoredProcedure, "PushMoney", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsPushMoneyVendedor
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"],
                                  TotalVendedor = rows["TotalVendedor"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["TotalVendedor"]))
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsPushMoneyxGralFranquicia")]
        [HttpPost]
        public IEnumerable<ReportsPushMoneyGralFranquicia> GetReportsPushMoneyxGralFranquicia(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                if (Reports.TipoReporte.Equals("0"))
                    Reports.FRCARDCODE = "-1";
                if (Reports.TipoReporte.Equals("0"))
                    Reports.Vendedor = "-1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoReporte", Value = Reports.TipoReporte },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Vendedor", Value = Reports.Vendedor },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsPushMoney", lsParameters, CommandType.StoredProcedure, "PushMoney", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsPushMoneyGralFranquicia
                              {
                                  CardCode = rows["CardCode"] is DBNull ? "" : (string)rows["CardCode"],
                                  Franquicia = rows["Franquicia"] is DBNull ? "" : (string)rows["Franquicia"],
                                  Subtotal = rows["Subtotal"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Subtotal"])),
                                  Iva = rows["Iva"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Iva"])),
                                  TotalPush = rows["TotalPush"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["TotalPush"]))
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsFactPagos")]
        [HttpPost]
        public IEnumerable<ReportsFactPagosView> GetReportsFactPagos(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Vendedor.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };


                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsFactPagos", lsParameters, CommandType.StoredProcedure, "VentasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsFactPagosView
                              {
                                  FechaPago = rows["Fecha Pago"] is DBNull ? "" : rows["Fecha Pago"].ToString(),
                                  FechaFactura = rows["Fecha Factura"] is DBNull ? "" : rows["Fecha Factura"].ToString(),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  FolioPago = rows["Folio Pago"] is DBNull ? "" : (string)rows["Folio Pago"],
                                  FormaDePago = rows["FormaPago"] is DBNull ? "" : (string)rows["FormaPago"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  EstatusPago = rows["Estatus Pago"] is DBNull ? "" : (string)rows["Estatus Pago"],
                                  TotalVenta = rows["Total Venta"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total Venta"])),
                                  Saldo = rows["Saldo"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Saldo"])),
                                  MontoPagado = rows["Monto Pagado"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Monto Pagado"])),
                                  EstatusFactura = rows["Estatus Factura"] is DBNull ? "" : (string)rows["Estatus Factura"],
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetDataStoreWhsID")]
        [HttpPost]
        public List<TiendaJsonView> GetDataStoreWhsID(TiendaJsonView tienda)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = tienda.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = tienda.Franquicia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("StoreByWhsIDNew", lsParameters, CommandType.StoredProcedure, "AdminStore", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new TiendaJsonView
                              {
                                  WhsID = rows["AlmacenSapPropio"] is DBNull ? " " : (string)rows["AlmacenSapPropio"],
                                  Franquicia = rows["StoreName"] is DBNull ? "" : (string)rows["StoreName"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetReportsNomina")]
        [HttpPost]
        public string GetReportsNomina(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                var culture = new CultureInfo("es-MX");

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1"; 

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsNomina", lsParameters, CommandType.StoredProcedure, "ventasEncabezado", connstringWEB);

                if (dt != null)
                {
                    var result = GeneralClass.ConvertDataTable(dt);

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api3/GetReportsFacturacionPayback")]
        [HttpPost]
        public List<ReportsFacturacionPayback> GetReportsFacturacionPayback(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsFacturacionPayback", lsParameters, CommandType.StoredProcedure, "ReportsFacturacionPayback", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsFacturacionPayback
                              {
                                  ID = rows["ID"] is DBNull ? 0 : Convert.ToInt32(rows["ID"]),
                                  FechaPedido = rows["FechaPedido"] is DBNull ? null : Convert.ToDateTime(rows["FechaPedido"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  FechaFactura = rows["FechaFactura"] is DBNull ? null : Convert.ToDateTime(rows["FechaFactura"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  FechaCompletado = rows["FechaCompletado"] is DBNull ? null : Convert.ToDateTime(rows["FechaCompletado"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Venta = rows["Venta"] is DBNull ? "" : (string)rows["Venta"],
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  EstatusVenta = rows["Estatus Venta"] is DBNull ? "" : (string)rows["Estatus Venta"],
                                  Facturado = rows["Facturado"] is DBNull ? "" : (string)rows["Facturado"],
                                  Total_Venta = rows["Total_Venta"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Total_Venta"])),
                                  Monto_pagado = rows["Monto_pagado"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Monto_pagado"])),
                                  Saldo = rows["Saldo"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}",
                                                                                              (decimal)(rows["Saldo"])),
                                  Estatus_Factura = rows["Estatus_Factura"] is DBNull ? "" : (string)rows["Estatus_Factura"],
                                  NumeroRecibo = rows["NUMERO DE RECIBO"] is DBNull ? "" : (string)rows["NUMERO DE RECIBO"],
                                  TipoTransaccion = rows["TypeTransaction"] is DBNull ? "" : (string)rows["TypeTransaction"],
                                  PagadoPuntos = rows["Pago_en_puntos"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}", (decimal)(rows["Pago_en_puntos"])),
                                  PagadoPesos = rows["Pago_en_pesos"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}", (decimal)(rows["Pago_en_pesos"])),
                                  AbonadoPuntos = rows["Abonado_en_puntos"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}", (decimal)(rows["Abonado_en_puntos"])),
                                  AbonadoPesos = rows["Abonado_en_pesos"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}", (decimal)(rows["Abonado_en_pesos"])),
                                  AcumuladoPuntos = rows["Acumulado_en_puntos"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}", (decimal)(rows["Acumulado_en_puntos"])),
                                  AcumuladoPesos = rows["Acumulado_en_pesos"] is DBNull ? "0" : "$" + string.Format("{0:#,0.00}", (decimal)(rows["Acumulado_en_pesos"])),
                                  ErrorMessage = rows["MessageError"] is DBNull ? "" : (string)rows["MessageError"],
                                  Conciliado = rows["Conciliado"] is DBNull ? "" : (string)rows["Conciliado"],
                                  FechaPayback = rows["Fecha"] is DBNull ? null : Convert.ToDateTime(rows["Fecha"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  FechaConciliacion = rows["FechaConciliacion"] is DBNull ? null : Convert.ToDateTime(rows["FechaConciliacion"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  Socio = rows["Socio"] is DBNull ? "" : (string)rows["Socio"],
                                  NombreArchivo = rows["NombreArchivo"] is DBNull ? "" : (string)rows["NombreArchivo"],
                                  FechaTrans = rows["EfectiveTime"] is DBNull ? null : Convert.ToDateTime(rows["EfectiveTime"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  EstatusTransaccion = rows["StatusTrans"] is DBNull ? "" : (string)rows["StatusTrans"],
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("api4/GetReportsDesc")]
        [HttpPost]
        public List<ReportsFacturacionPayback> GetReportsDesc(ReportsVentasInputView Reports)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //if (Reports.Usuarios == 0)
                //    Reports.AdminUserID = "1";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = Reports.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FRCARDCODE", Value = Reports.FRCARDCODE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Usuarios", Value = Reports.Usuarios.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Tiendas", Value = Reports.Tiendas.ToString() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha1", Value = Convert.ToDateTime(Reports.Fecha1).ToString("yyyyMMdd") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha2", Value = Convert.ToDateTime(Reports.Fecha2).ToString("yyyyMMdd") }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ReportsAutorizacionDescuentos", lsParameters, CommandType.StoredProcedure, "ReportsAutorizacionDescuentos", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new ReportsFacturacionPayback
                              {
                                  ID = rows["Iddescuento"] is DBNull ? 0 : Convert.ToInt32(rows["Iddescuento"]),
                                  Vendedor = rows["Vendedor"] is DBNull ? "" : (string)rows["Vendedor"],
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  Articulo = rows["Articulo"] is DBNull ? "" : (string)rows["Articulo"],
                                  FechaFactura = rows["Fecha"] is DBNull ? null : Convert.ToDateTime(rows["Fecha"]).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                                  Descuento = rows["Cantdescuento"] is DBNull ? "" : (string)rows["Cantdescuento"],
                                  Codigo = rows["Codigo"] is DBNull ? "" : (string)rows["Codigo"],
                                  Utilizado = rows["Utilizado"] is DBNull ? "" : (string)rows["Utilizado"],
                                  Cliente = rows["Cliente"] is DBNull ? "" : (string)rows["Cliente"],
                                  Observaciones = rows["Observaciones"] is DBNull ? "" : (string)rows["Observaciones"],
                                  DescuentoModificado = rows["Descuento Modificado"] is DBNull ? "" : Convert.ToString(rows["Descuento Modificado"]),
                                  PrecioFDO = rows["PriceFD"] is DBNull ? "" : "$" + Threads.ConvertTo2D(rows["PriceFD"].ToString()),
                                  PrecioSMU = rows["PriceSMU"] is DBNull ? "" : "$" + Threads.ConvertTo2D(rows["PriceSMU"].ToString()),
                                  Margen = rows["PriceFD"] is DBNull ? "" : (Convert.ToDouble(Threads.ConvertTo2D((1 - (Convert.ToDouble(rows["PriceSMU"]) / (Convert.ToDouble(rows["PriceWhithDesc"]) / 1.16))).ToString())) * 100).ToString() + "%"
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}