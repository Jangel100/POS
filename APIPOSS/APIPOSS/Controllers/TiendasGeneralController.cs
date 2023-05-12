using APIPOSS.Models.AdminStore;
using APIPOSS.Models.Configuracion;
using Newtonsoft.Json.Linq;
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

namespace APIPOSS.Controllers
{
    public class TiendasGeneralController : ApiController
    {
        private string DefaultList;
        private string IDDefaultList;
        private string DefaultCustomer;
        private bool checkGlobal;
        private string connWEB;
        [Route("api2/GetConsultaTiendas")]
        public JsonResult<List<TiendaConfiguracionView>> GetConsultaTiendas()
        {
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreListN", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var tiendas = (from DataRow rows in dt.Rows
                                   select new TiendaConfiguracionView
                                   {
                                       AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] is DBNull ? 0 : rows["AdminStoreID"]),
                                       CanBeDeleted = (Boolean)rows["CanBeDeleted"],
                                       Status = (string)rows["Status"],
                                       StoreName = (string)rows["StoreName"],
                                       StoreType = (string)rows["StoreType"]
                                   }
                                    ).ToList();

                    if (tiendas != null)
                    {
                        return Json<List<TiendaConfiguracionView>>(tiendas);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        [Route("api2/GetTiendaConfig")]
        public JsonResult<TiendaConfigView> GetTiendaConfig(string idStore)
        {
            List<TypeStore> lstypeStores;
            List<SAPWS> lsSapWS;
            List<Entidades> lsEntidades;
            List<Store> lsStoreClasificaciones;
            List<Store> lsStoreBodegaPropia;
            List<StoreIVA> lsStoreIvas;
            TiendaConfigView lsTiendaConfigView;
            List<SelectedFolios> lsFolios;
            List<ListPrice> lsListaPrecios;
            List<Franquicias> lsFranquicias;
            try
            {
                lstypeStores = GetTypeStore();
                lsSapWS = GetSAPWS();
                lsEntidades = GetEntidades();
                lsStoreClasificaciones = GetStoreClasificaciones();
                lsStoreBodegaPropia = GetStoreBodegaPropia();
                lsStoreIvas = GetStoreIVA();
                lsFolios = SelectedFolios(idStore);
                lsListaPrecios = getListPrice(idStore);
                lsFranquicias = GetListFranquicias();

                lsTiendaConfigView = new TiendaConfigView()
                {
                    lsTypeStore = lstypeStores,
                    lsSAPWS = lsSapWS,
                    lsEntidades = lsEntidades,
                    lsStoreClasificaciones = lsStoreClasificaciones,
                    lsStoreBodegaPropia = lsStoreBodegaPropia,
                    lsIVA = lsStoreIvas,
                    lsfolios = lsFolios,
                    lsListaPrecios = lsListaPrecios,
                    DefaultList = DefaultList,
                    DefaultCustomer = DefaultCustomer,
                    IDDefaultList = IDDefaultList,
                    checkGlobal = checkGlobal,
                    lsFranquicias = lsFranquicias
                };

                return Json<TiendaConfigView>(lsTiendaConfigView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TypeStore> GetTypeStore()
        {

            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreType", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new TypeStore
                              {
                                  StoreTypeID = Convert.ToInt32(rows["StoreTypeID"] is DBNull ? 0 : rows["StoreTypeID"]),
                                  StoreTypeName = (string)rows["StoreTypeName"]
                              }
                                    ).ToList();

                    if (ls != null)
                    {
                        return ls;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        public List<SAPWS> GetSAPWS()
        {

            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreAlmacenSap", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new SAPWS
                              {
                                  WhsCode = rows["WhsCode"] is DBNull ? "" : (string)rows["WhsCode"],
                                  WhsName = rows["WhsName"] is DBNull ? "" : (string)rows["WhsName"]
                              }
                                    ).ToList();

                    if (ls != null)
                    {
                        return ls;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        public List<Entidades> GetEntidades()
        {

            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreEntidades", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new Entidades
                              {
                                  EntidadId = Convert.ToInt32(rows["EntidadId"] is DBNull ? 0 : rows["EntidadId"]),
                                  EntidadDesc = (string)rows["EntidadDesc"]
                              }
                              ).ToList();

                    if (ls != null)
                    {
                        return ls;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        public List<Store> GetStoreClasificaciones()
        {

            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreClasificaciones", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new Store
                              {
                                  AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] is DBNull ? 0 : rows["AdminStoreID"]),
                                  StoreName = (string)rows["StoreName"]
                              }
                              ).ToList();

                    if (ls != null)
                    {
                        return ls;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        public List<Store> GetStoreBodegaPropia()
        {

            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreBodegaPropia", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new Store
                              {
                                  AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] is DBNull ? 0 : rows["AdminStoreID"]),
                                  StoreName = (string)rows["StoreName"]
                              }
                              ).ToList();

                    if (ls != null)
                    {
                        return ls;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        public List<StoreIVA> GetStoreIVA()
        {
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreIVA", null, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new StoreIVA
                              {
                                  COD_IVA = (string)rows["COD_IVA"],
                                  PORCENTAJE = Convert.ToInt32(rows["PORCENTAJE"] is DBNull ? 0 : rows["PORCENTAJE"])
                              }
                              ).ToList();

                    if (ls != null)
                    {
                        return ls;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }

        [Route("api2/AddTiendas")]
        [HttpPost]
        public JsonResult<StoreAdmin> AddUsuarios(StoreAdmin store)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                if (store.NumExt == null) store.NumExt = "0";
                if (store.NumInt == null) store.NumInt = "0";
                if (store.StoreName == null) store.StoreName = " ";
                if (store.Telefono == null) store.Telefono = " ";
                if (store.CalleNumero == null) store.CalleNumero = " ";
                if (store.CodigoPostal == null) store.CodigoPostal = " ";
                if (store.Delegacion == null) store.Delegacion = " ";
                if (store.Colonia == null) store.Colonia = " ";
                if (store.FolderPDF == null) store.FolderPDF = " ";
                if (store.emailTienda == null) store.emailTienda = " ";
                if (store.AlmacenSapPropio == null) store.AlmacenSapPropio = " ";

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@StoreName", Value = store.StoreName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Status", Value = store.Status },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@StoreTypeID", Value = Convert.ToInt32(store.StoreTypeID) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@WHSID", Value = store.WHSID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Telefono", Value = store.Telefono },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumero", Value = store.CalleNumero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CodigoPostal", Value = store.CodigoPostal },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Delegacion", Value = store.Delegacion },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoId", Value = Convert.ToInt32(store.EstadoId) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Colonia", Value = store.Colonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Socios", Value = false },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Clientes", Value = false },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actIDPV", Value = string.Empty },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@whsConsigID", Value = Convert.ToInt32(store.whsConsigID) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCodeZonaImpo", Value = string.Empty },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actIVA", Value = store.actIVA },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAdendaLiverpool", Value = false },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FolderPDF", Value = store.FolderPDF },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@emailTienda", Value = store.emailTienda },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AlmacenSapPropio", Value = store.AlmacenSapPropio },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NumExt", Value = store.NumExt },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NumInt", Value = store.NumInt },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@BodegaPropia", Value = store.BodegaPropia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@MontoReabasto", Value = string.Empty }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int result = obj.EjecutaQry_Tabl("AdminStoreInsert", "@AdminStoreID", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (result != -1)
                {
                    StoreAdmin admin = new StoreAdmin() { AdminStoreID = result };
                    return Json<StoreAdmin>(admin);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetConsultaUsuarios")]
        public JsonResult<List<Entidades>> GetConsultaUsuarios()
        {
            List<Entidades> lsEntidades;
            try
            {
                lsEntidades = GetEntidades();

                return Json<List<Entidades>>(lsEntidades);
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }

        [Route("api2/GetConsultaTiendasxId")]
        [HttpPost]
        public JsonResult<StoreAdmin> GetConsultaTiendasxId(StoreAdmin storeAdmin)
        {
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminStoreID", Value = storeAdmin.AdminStoreID }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminStoreListxIdN", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new StoreAdmin
                              {
                                  AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] is DBNull ? 0 : rows["AdminStoreID"]),
                                  StoreName = rows["StoreName"] is DBNull ? "" : (string)rows["StoreName"],
                                  StoreTypeID = rows["StoreTypeID"] is DBNull ? "" : Convert.ToInt32(rows["StoreTypeID"]).ToString(),
                                  WHSID = rows["WHSID"] is DBNull ? "" : (string)rows["WHSID"],
                                  Status = rows["Status"] is DBNull ? "" : (string)rows["Status"],
                                  BodegaPropia = rows["BodegaPropia"] is DBNull ? "" : Convert.ToInt32(rows["BodegaPropia"]).ToString(),
                                  whsConsigID = rows["whsConsigID"] is DBNull ? "" : Convert.ToInt32(rows["whsConsigID"]).ToString(),
                                  Telefono = rows["Telefono"] is DBNull ? "" : (string)rows["Telefono"],
                                  CalleNumero = rows["CalleNumero"] is DBNull ? "" : (string)rows["CalleNumero"],
                                  NumExt = rows["NumExt"] is DBNull ? "" : (string)rows["NumExt"],
                                  NumInt = rows["NumInt"] is DBNull ? "" : (string)rows["NumInt"],
                                  Colonia = rows["Colonia"] is DBNull ? "" : (string)rows["Colonia"],
                                  CodigoPostal = rows["CodigoPostal"] is DBNull ? "" : (string)rows["CodigoPostal"],
                                  Delegacion = rows["Delegacion"] is DBNull ? "" : (string)rows["Delegacion"],
                                  EstadoId = rows["EstadoId"] is DBNull ? "" : Convert.ToInt32(rows["EstadoId"]).ToString(),
                                  FolderPDF = rows["FolderPDF"] is DBNull ? "" : (string)rows["FolderPDF"],
                                  emailTienda = rows["emailTienda"] is DBNull ? "" : (string)rows["emailTienda"],
                                  AlmacenSapPropio = rows["AlmacenSapPropio"] is DBNull ? "" : (string)rows["AlmacenSapPropio"],
                                  actIVA = Convert.ToInt32(rows["actIVA"])
                              }).FirstOrDefault();

                    if (ls != null)
                    {
                        return Json<StoreAdmin>(ls);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }
        [Route("api2/UpdateStores")]
        public JsonResult<int> UpdateStore(StoreAdmin store)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            try
            {
              var UpdateMenu = AdminStoreMenuOptionUpdate(store); //UpdateListas
                var UpdateFolios =  AdminFolios(store.StoreAdminFolios, store.AdminStoreID); //UpdateFolios
                if (UpdateMenu && UpdateFolios)
                {
                    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@StoreName", Value = store.StoreName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Status", Value = store.Status },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@StoreTypeID", Value = Convert.ToInt32(store.StoreTypeID) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@WHSID", Value = store.WHSID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Telefono", Value = store.Telefono },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumero", Value = store.CalleNumero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CodigoPostal", Value = string.IsNullOrEmpty(store.CodigoPostal) ? "":store.CodigoPostal},
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Delegacion", Value = store.Delegacion },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoId", Value = Convert.ToInt32(store.EstadoId) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Colonia", Value = store.Colonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Socios", Value = 0 },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Clientes", Value = 0 },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actIDPV", Value = "NULL" },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@whsConsigID", Value = Convert.ToInt32(store.whsConsigID) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCodeZonaImpo", Value = 0},
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actIVA", Value = store.actIVA },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAdendaLiverpool", Value = 0 },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FolderPDF", Value = string.IsNullOrEmpty(store.FolderPDF) ? "":store.FolderPDF },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@emailTienda", Value = string.IsNullOrEmpty(store.emailTienda)? "":store.emailTienda },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AlmacenSapPropio", Value = string.IsNullOrEmpty(store.AlmacenSapPropio) ? "":store.AlmacenSapPropio},
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NumExt", Value = string.IsNullOrEmpty(store.NumExt) ? "NULL" : store.NumExt /*Convert.ToInt32(store.NumExt)*/ },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NumInt", Value = string.IsNullOrEmpty(store.NumInt) ? "NULL" : store.NumInt /*Convert.ToInt32(store.NumInt)*/ },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@BodegaPropia", Value = Convert.ToInt32(store.BodegaPropia) },
                       // new System.Data.SqlClient.SqlParameter(){ ParameterName = "@MontoReabasto", Value = "NULL" },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminStoreID", Value = store.AdminStoreID }
                    };

                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    int result = obj.EjecutaQry_Tabl("AdminStoreUpdate", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                    return Json<int>(result);
                }                            
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<SelectedFolios> SelectedFolios(string IdStore)
        {
            string connstringWEB = string.Empty;
            List<SelectedFolios> Folios = null;
            DataTable dt;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminStoreID", Value = IdStore == null? "": IdStore }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                dt = obj.EjecutaQry_Tabla("SelectStoreFolios", lsParameters, CommandType.StoredProcedure, "Folios", connstringWEB);
                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new SelectedFolios
                              {
                                  TipoFolio = rows["Tipo Folio"] is DBNull ? "" : rows["Tipo Folio"].ToString().Trim(),
                                  Tienda_Name = rows[1] is DBNull ? "" : rows[1].ToString().Trim(),
                                  PrimerFolio = rows["Primer Folio"] is DBNull ? 0 : Convert.ToInt32(rows["Primer Folio"].ToString().Trim()),
                                  UltimoFolio = rows["Ultimo Folio"] is DBNull ? 0 : Convert.ToInt32(rows["Ultimo Folio"].ToString().Trim()),
                                  UltimoFolioAsignado = rows["Ultimo Folio asignado"] is DBNull ? 0 : Convert.ToInt32(rows["Ultimo Folio asignado"]),
                                  AdminFolioType = rows["AdminFolioType"] is DBNull ? "" : rows["AdminFolioType"].ToString().Trim(),
                                  TiendaId = rows[6] is DBNull ? 0 : Convert.ToInt32(rows[6].ToString().Trim()),
                                  Prefijo = rows["Prefijo"] is DBNull ? "" : rows["Prefijo"].ToString().Trim(),
                                  NoAprobacion = rows["NoAprobacion"] is DBNull ? "" : rows["NoAprobacion"].ToString().Trim(),
                                  AñoAprobacion = rows["AnioAprobacion"] is DBNull ? "" : rows["AnioAprobacion"].ToString().Trim()
                              }).ToList();

                    return (ls);
                }
                return Folios;

            }
            catch (Exception ex) { return null; }
        }
        public List<ListPrice> getListPrice(string AdminstoreID)
        {
            Utilities.DBMaster obj = new Utilities.DBMaster();
            int idBtn = 0;
            string connstringWEB = string.Empty;
            string connstringSAP = string.Empty;
            DataTable dt = new DataTable();
            List<ListPrice> ls = new List<ListPrice>();
            //int AdminRoleID;
            DataTable daLists = null;/* TODO Change to default(_) if this is not a reference type */;
            DataTable daSelected = null;/* TODO Change to default(_) if this is not a reference type */;
            DataTable daSelectedCustomer = null;/* TODO Change to default(_) if this is not a reference type */;
            string sQuery = "";
            try
            {
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                sQuery = "" + "SELECT  listnum,listname,'true' as isAvailable FROM opln" + Environment.NewLine + "";
                daLists = obj.EjecutaQry_Tabla(sQuery, CommandType.Text, "daLists", connstringSAP);
                #region object
                if (daLists != null)
                {

                    ls = (from DataRow rows in daLists.Rows
                          select new ListPrice
                          {
                              listnum = rows["listnum"] is DBNull ? 0 : Convert.ToInt32(rows["listnum"]),
                              listname = rows["listname"] is DBNull ? "" : rows["listname"].ToString(),
                              isAvailable = rows["isAvailable"] is DBNull ? false : Convert.ToBoolean(rows["isAvailable"].ToString()),
                              idBtn = "Btn" + idBtn++
                          }).ToList();
                    // return (ls);
                }
                #endregion

                sQuery = "" + "SELECT SL.*, ADS.DefaultList " + Environment.NewLine + "FROM StoreList SL " + Environment.NewLine + "RIGHT JOIN AdminStore ADS " + "ON SL.AdminStoreID = ADS.AdminStoreID " + Environment.NewLine + "WHERE ADS.AdminStoreID = " + AdminstoreID + Environment.NewLine + "";
                daSelected = obj.EjecutaQry_Tabla(sQuery, CommandType.Text, "daSelected", connstringWEB);

                //CustomerStore.DataSource = daCustomer;
                //CustomerStore.DataBind();
                //StoreLists.DataSource = daLists;
                sQuery = "" + "IF EXISTS (SELECT ListGlobal FROM StoreList WHERE AdminStoreID = 5 GROUP BY ListGlobal) " + "BEGIN " + "IF (SELECT ListGlobal FROM StoreList WHERE AdminStoreID = 5 GROUP BY ListGlobal) = 1 " + "SELECT 'True' " + "ELSE " + "SELECT 'False' " + "End " + "ELSE " + "SELECT 'False'";
                dt = obj.EjecutaQry_Tabla(sQuery, CommandType.Text, "Listas", connstringWEB);
                /*checkGlobal.Checked = dt.Rows.Item(0).Item(0);*///ver
                checkGlobal = Convert.ToBoolean(dt.AsEnumerable().FirstOrDefault().ItemArray[0]);
                //string defaultList = "";
                if (daSelected.Rows.Count > 0) //descomentar
                {
                    DefaultList = daSelected.AsEnumerable().Select(x => x["DefaultList"].ToString()).FirstOrDefault();
                    //Tables(0).Rows(0)("DefaultList").ToString;
                }
                foreach (ListPrice rowList in ls)
                {
                    if (rowList.listnum.ToString() == DefaultList)
                    {
                        DefaultList = rowList.listname;
                        IDDefaultList = rowList.listnum.ToString();
                    }
                    foreach (DataRow rowSelected in daSelected.Rows)
                    {
                        if (rowList.listnum.ToString() == rowSelected["ListID"].ToString())
                        {
                            rowList.CheckedList = true;
                        }
                    }
                } //descomentar
                //Ext.Net.RowSelectionModel smMySelector = GridPanel1.SelectionModel.Primary;//ver

                //smMySelector.ClearSelections();//ver
                //smMySelector.UpdateSelection();//ver

                //foreach (DataRow dr in daSelected.Tables(0).Rows)
                //    smMySelector.SelectedRows.Add(new Ext.Net.SelectedRow(dr("ListID").ToString));//descomentar

                //smMySelector.UpdateSelection();//ver
                //StoreLists.DataBind();//ver

                daSelectedCustomer = obj.EjecutaQry_Tabla("SELECT DefaultCustomer FROM AdminStore WHERE AdminStoreID = " + AdminstoreID, CommandType.Text, "Listas", connstringWEB);
                if (daSelectedCustomer.Rows.Count > 0)//descomentar
                {
                    string strCustomer = daSelectedCustomer.AsEnumerable().FirstOrDefault().ItemArray[0].ToString();
                    //daSelectedCustomer.Tables(0).Rows(0)("DefaultCustomer").ToString;
                    if (strCustomer.Length > 0)
                    {
                        DefaultCustomer = strCustomer.ToString();
                    }
                }
                return ls;
            }
            catch (Exception ex) { return null; }
            // return null;
        }
        public List<Franquicias> GetListFranquicias()
        {
            Utilities.DBMaster obj = new Utilities.DBMaster();
            string connstringSAP = string.Empty;
            string sQuery = string.Empty;
            DataTable daCustomer = null;/* TODO Change to default(_) if this is not a reference type */;
            try
            {
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                sQuery = "" + "SELECT CardName + ' '  + cardcode as 'CardName',LicTradNum,CardCode FROM ocrd  " + Environment.NewLine + "WHERE  GroupCode in " + Environment.NewLine + "(SELECT GroupCode  FROM ocrg WHERE GroupName in ('CLIENTES FRANQUICIAS','CLIENTES DEL GRUPO', 'SUBURBIA'))" + Environment.NewLine + "";
                daCustomer = obj.EjecutaQry_Tabla(sQuery, CommandType.Text, "daSelected", connstringSAP);
                #region object
                if (daCustomer != null)
                {

                    var ls = (from DataRow rows in daCustomer.Rows
                              select new Franquicias
                              {
                                  CardName = rows["CardName"] is DBNull ? "" : rows["CardName"].ToString(),
                                  CardCode = rows["CardCode"] is DBNull ? "" : rows["CardCode"].ToString(),
                                  LicTradNum = rows["LicTradNum"] is DBNull ? "" : rows["LicTradNum"].ToString()
                              }).ToList();
                    return (ls);
                }
                else { return null; }
                #endregion
            }
            catch (Exception)
            {

                return null;
            }

        }
        private bool AdminStoreMenuOptionUpdate(StoreAdmin Store)
        {
            Utilities.DBMaster obj = new Utilities.DBMaster();
            Utilities.admin Master = new Utilities.admin();
            DataSet daLists1 = null/* TODO Change to default(_) if this is not a reference type */;
            DataSet daLists1s = null/* TODO Change to default(_) if this is not a reference type */;
            Int16 check = 0;
            string connstringSAP = string.Empty;
            string connstringWEB = string.Empty;
            string strWhere = string.Empty;
            try
            {
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                var ListaSelect = Store.StoreAdminListas.Where(x => x.ChekChecked == true).OrderBy(x => Convert.ToInt32(x.idCheck));
                if (Store.checkGlobal)
                {
                    check = 1;
                }
                if (!Store.checkfranquicia)
                {
                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminStoreID", Value = Store.AdminStoreID.ToString() == null? "": Store.AdminStoreID.ToString() }
                    };
                    var dt = obj.EjecutaQry_Tabl("AdminStoreListOptionDeleteByStore", "", lsParameters, CommandType.StoredProcedure, connstringWEB);  

                    //strWhere = "SELECT listnum,listname FROM opln " + "WHERE listnum in (";

                    //foreach (var selRow in List) //obtiene informacion sobre los cheks seleccionados
                    //    strWhere = strWhere + selRow.listnum + ",";

                    //var Squery = obj.EjecutaQry_Tabla(strWhere,CommandType.Text, "daSelectedSAP", connstringSAP);///ejecuta Query                  
                    foreach (var lista in ListaSelect)
                    {
                        Master.DBConn.ExecuteCMD("AdminStoreListInsert " + Store.AdminStoreID + ", " + lista.idCheck + ", '" + lista.TexDescripcion.Trim() + "'");
                    }
                    Master.DBConn.ExecuteCMD("UPDATE adminstore set DefaultList = " + Store.hdnDefault + " WHERE AdminStoreID = " + Store.AdminStoreID);

                    System.Data.SqlClient.SqlDataReader daCustomer = null;

                    Master.DBConnSAP.GetQuerydtr("SELECT * FROM ocrd WHERE CardCode = '" + Store.SelectedFranquicia + "'", ref daCustomer);///ejecuta Query
                    if (daCustomer.HasRows)
                    {
                        while (daCustomer.Read())
                            Master.DBConn.ExecuteCMD(" EXEC InsertCustomer '" + daCustomer["CardName"].ToString() + "','" + daCustomer["LicTradNum"].ToString() + "','" + daCustomer["Address"].ToString().ToUpper() + "','" + daCustomer["Block"].ToString() + "','" + daCustomer["ZipCode"].ToString() + "','" + daCustomer["Phone1"].ToString() + "','" + daCustomer["Phone2"].ToString() + "','" + daCustomer["E_Mail"].ToString() + "','" + daCustomer["State1"].ToString() + "','" + daCustomer["County"].ToString() + "','" + daCustomer["CardCode"].ToString() + "','" + "M'");

                        Master.DBConn.ExecuteCMD("UPDATE AdminStore SET DefaultCustomer = '" + Store.SelectedFranquicia + "' WHERE AdminStoreID = " + Store.AdminStoreID);
                    }
                }
                else
                {
                    string squery1s = string.Empty;
                    string franqui = string.Empty;
                    string datoFranquicia = string.Empty;
                    string squery2s = string.Empty;
                    string AdminStoreIDTienda = string.Empty;
                    // se obtiene la franquicia
                    squery1s = " select DefaultCustomer from AdminStore where AdminStoreID =" + Store.AdminStoreID;
                    daLists1 = Master.DBConn.GetQuerydts(squery1s);
                    datoFranquicia = daLists1.Tables[0].Rows[0].ItemArray[0].ToString();

                    // se seleccionan las tiendas que se van a cambiar y entra ciclo                   
                    squery2s = "select AdminStoreID from AdminStore where DefaultCustomer = '" + datoFranquicia + "'";
                    daLists1s = Master.DBConn.GetQuerydts(squery2s);

                    foreach (DataRow dr in daLists1s.Tables[0].Rows)
                    {
                        AdminStoreIDTienda = dr.ItemArray[0].ToString();
                        Master.DBConn.ExecuteCMD("AdminStoreListOptionDeleteByStore " + AdminStoreIDTienda);

                    
                        foreach (var lista in ListaSelect)
                        {
                            Master.DBConn.ExecuteCMD("AdminStoreListInsert " + AdminStoreIDTienda + ", " + lista.idCheck + ", '" + lista.TexDescripcion.Trim() + "'");
                        } //revisar

                        Master.DBConn.ExecuteCMD("UPDATE adminstore set DefaultList = " + Store.hdnDefault + " WHERE AdminStoreID = " + AdminStoreIDTienda);
                        System.Data.SqlClient.SqlDataReader daCustomer = null;
                        Master.DBConnSAP.GetQuerydtr("SELECT * FROM ocrd WHERE CardCode = '" + Store.SelectedFranquicia + "'", ref daCustomer);
                        if (daCustomer.HasRows)
                        {
                            while (daCustomer.Read())
                                Master.DBConn.ExecuteCMD(" EXEC InsertCustomer '" + daCustomer["CardName"].ToString() + "','" + daCustomer["LicTradNum"].ToString() + "','" + daCustomer["Address"].ToString().ToUpper() + "','" + daCustomer["Block"].ToString() + "','" + daCustomer["ZipCode"].ToString() + "','" + daCustomer["Phone1"].ToString() + "','" + daCustomer["Phone2"].ToString() + "','" + daCustomer["E_Mail"].ToString() + "','" + daCustomer["State1"].ToString() + "','" + daCustomer["County"].ToString() + "','" + daCustomer["CardCode"].ToString() + "','" + "M'");


                            Master.DBConn.ExecuteCMD("UPDATE AdminStore SET DefaultCustomer = '" + Store.SelectedFranquicia + "' WHERE AdminStoreID = " + AdminStoreIDTienda);
                            daCustomer.Close();
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        private bool AdminFolios(List<StoreAdminFolios> Folios, int AdminStoreID)
        {
            try
            {
                string FID = string.Empty;
                int FF;
                int LF;
                string Pre = string.Empty;
                int CF;
                string NA = string.Empty;
                string AA = string.Empty;
                Utilities.admin Master = new Utilities.admin();
                foreach (StoreAdminFolios dgRow in Folios)
                {
                    FF = dgRow.PrimerFolio;
                    LF = dgRow.UltimoFolio;
                    Pre = dgRow.Prefijo;
                    CF = dgRow.UltimoFolioAsignado;
                    FID = dgRow.AdminFolioType;

                    NA = string.IsNullOrEmpty(dgRow.NoAprobacion) ? "" : dgRow.NoAprobacion;

                    AA = string.IsNullOrEmpty(dgRow.AñoAprobacion) ? "" : dgRow.AñoAprobacion;

                    // grdFolios.Columns.Item(5).Visible = False
                    if (FF.ToString() != "" & LF.ToString() != "" & Pre != "" & CF.ToString() != "")
                        Master.DBConn.ExecuteCMD("AdminStoreFolios " + AdminStoreID + "," + FID + "," + FF + "," + LF + ",'" + Pre + "'," + CF + ",'" + NA + "','" + AA + "'");
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        [Route("api/GetUpdateMessage")]
        public string GetFechaEntrega(string Parametros)
        {
            try
            {
                dynamic datos = JObject.Parse(Parametros);
                string Texto = datos.mensaje;
                string franquicia = datos.Franquicia;
                string mensaje = "";
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery = "";
                squery = "Delete From Marquesina where Franquicia is null or franquicia = '" + franquicia + "'" + Environment.NewLine;
                squery = squery + "INSERT INTO Marquesina (TextoMarquesina, Franquicia) " + Environment.NewLine;
                squery = squery + "VALUES ('" + Texto + "', '" + franquicia + "')";
                if (oDB.EjecutaQry(squery, CommandType.Text, connWEB, mensaje) == 2)
                {
                    mensaje = "Se encontro un problema al actualizar el mensaje. " + mensaje;
                    return "0";
                }
                else {
                    mensaje = "Mensaje actualizado con éxito";
                    return "1";
                }
               
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
    }
}