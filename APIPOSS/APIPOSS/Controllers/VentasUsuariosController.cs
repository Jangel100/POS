using APIPOSS.Models;
using APIPOSS.Models.Configuracion;
using APIPOSS.Models.Ventas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIPOSS.Controllers
{
    public class VentasUsuariosController : ApiController
    {
        [Route("api2/GetInfoCliente")]
        [HttpPost]
        public Task<List<Clientes>> GetInfoCliente(ClienteInputView clientes)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                object Nombre;
                if (clientes.Nombre == "") Nombre = DBNull.Value; else Nombre = clientes.Nombre;

                object RFC;
                if (clientes.RFC == "") RFC = DBNull.Value; else RFC = clientes.RFC;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = Nombre },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = RFC }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClientesInfo", lsParameters, CommandType.StoredProcedure, "Clientes", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new Clientes
                              {
                                  Id = rows["Id"] is DBNull ? "" : Convert.ToInt32(rows["Id"]).ToString(),
                                  actAPEPATERNO = rows["actAPEPATERNO"] is DBNull ? "" : (string)rows["actAPEPATERNO"],
                                  actNOMBRE = rows["actNOMBRE"] is DBNull ? "" : (string)rows["actNOMBRE"],
                                  actCALLE = rows["actCALLE"] is DBNull ? "" : (string)rows["actCALLE"],
                                  actCIUDAD = rows["actCIUDAD"] is DBNull ? "" : (string)rows["actCIUDAD"],
                                  Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"],
                                  actNUMEXT = rows["actNUMEXT"] is DBNull ? "" : (string)rows["actNUMEXT"],
                                  actNUMINT = rows["actNUMINT"] is DBNull ? "" : (string)rows["actNUMINT"],
                                  CalleNumero = rows["CalleNumero"] is DBNull ? "" : (string)rows["CalleNumero"],
                                  Colonia = rows["Colonia"] is DBNull ? "" : (string)rows["Colonia"],
                                  Correo = rows["Correo"] is DBNull ? "" : (string)rows["Correo"],
                                  CP = rows["CP"] is DBNull ? "" : (string)rows["CP"],
                                  DelMun = rows["DelMun"] is DBNull ? "" : (string)rows["DelMun"],
                                  Estado = rows["Estado"] is DBNull ? "" : (string)rows["Estado"],
                                  NoCelular = rows["NoCelular"] is DBNull ? "" : (string)rows["NoCelular"],
                                  RFC = rows["RFC"] is DBNull ? "" : (string)rows["RFC"],
                                  TelCasa = rows["TelCasa"] is DBNull ? "" : (string)rows["TelCasa"],
                                  CardCode = rows["CardCode"] is DBNull ? "" : (string)rows["CardCode"],
                                  IDRef = rows["IDRef"] is DBNull ? "" : (string)rows["IDRef"],
                                  Referencia = rows["Referencia"] is DBNull ? "" : (string)rows["Referencia"],
                                  TelOfc = rows["TelOfc"] is DBNull ? "" : (string)rows["TelOfc"],
                                  FormaPago33 = rows["FormaPago33"] is DBNull ? "" : (string)rows["FormaPago33"],
                                  CFDI_Rel33 = rows["CFDI_Rel33"] is DBNull ? "" : (string)rows["CFDI_Rel33"],
                                  actCod_CIUDAD = rows["actCod_CIUDAD"] is DBNull ? "" : (string)rows["actCod_CIUDAD"],
                                  MetodoPago33 = rows["MetodoPago33"] is DBNull ? "" : (string)rows["MetodoPago33"],
                                  TipoCliente = rows["TipoCliente"] is DBNull ? "" : (string)rows["TipoCliente"],
                                  TipoComp33 = rows["TipoComp33"] is DBNull ? "" : (string)rows["TipoComp33"],
                                  TipoRel33 = rows["TipoRel33"] is DBNull ? "" : (string)rows["TipoRel33"],
                                  UsoCFDI33 = rows["TipoRel33"] is DBNull ? "" : (string)rows["TipoRel33"],
                                  RegimenFiscal = rows["RegimenFiscal"] is DBNull ? "0" : Convert.ToInt32(rows["RegimenFiscal"]).ToString(),
                                  actCod_COMUNAColonia = rows["actCod_COMUNAcolonia"] is DBNull ? "" : (string)rows["actCod_COMUNAcolonia"],
                                  actCod_PROVINCIAmunicipio = rows["actCod_PROVINCIAmunicipio"] is DBNull ? "" : (string)rows["actCod_PROVINCIAmunicipio"],
                                  actCod_REGIONestado = rows["actCod_REGIONestado"] is DBNull ? "" : (string)rows["actCod_REGIONestado"],
                                  actFCHNACIMIENTOTemp = rows["actFCHNACIMIENTO"] is DBNull ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(rows["actFCHNACIMIENTO"]).ToString("yyyy/MM/dd")
                              }).ToList();

                    return Task.Run(() => ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetClienteFacturacion")]
        [HttpPost]
        public Task<ClientesView> GetClienteFacturacion(ClienteInputView clienteInputView)
        {
            ClientesView clientesView;
            Clientes clientesN;
            Clientes clientesFacturacion;
            try
            {
                clientesN = GetClientexId(clienteInputView);
                clientesFacturacion = GetClienteFacturacionxId(clienteInputView);

                clientesView = new ClientesView() { Clientes = clientesN, ClientesFacturacion = clientesFacturacion };

                return Task.Run(() => clientesView);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        //OBTENER EL REGISTRO DE CLIENTE POR EL ID
        public Clientes GetClientexId(ClienteInputView clienteInputView)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Id", Value = clienteInputView.Id }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClientesxIdInfo", lsParameters, CommandType.StoredProcedure, "Clientes", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new Clientes
                              {
                                  Id = rows["Id"] is DBNull ? "" : Convert.ToInt32(rows["Id"]).ToString(),
                                  actAPEPATERNO = rows["actAPEPATERNO"] is DBNull ? "" : (string)rows["actAPEPATERNO"],
                                  actAPEMATERNO = rows["actAPEMATERNO"] is DBNull ? "" : (string)rows["actAPEMATERNO"],
                                  actNOMBRE = rows["actNOMBRE"] is DBNull ? "" : (string)rows["actNOMBRE"],
                                  actCALLE = rows["actCALLE"] is DBNull ? "" : (string)rows["actCALLE"],
                                  actCIUDAD = rows["actCIUDAD"] is DBNull ? "" : (string)rows["actCIUDAD"],
                                  Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"],
                                  actNUMEXT = rows["actNUMEXT"] is DBNull ? "" : (string)rows["actNUMEXT"],
                                  actNUMINT = rows["actNUMINT"] is DBNull ? "" : (string)rows["actNUMINT"],
                                  CalleNumero = rows["CalleNumero"] is DBNull ? "" : (string)rows["CalleNumero"],
                                  Colonia = rows["Colonia"] is DBNull ? "" : (string)rows["Colonia"],
                                  Correo = rows["Correo"] is DBNull ? "" : (string)rows["Correo"],
                                  CP = rows["CP"] is DBNull ? "" : (string)rows["CP"],
                                  DelMun = rows["DelMun"] is DBNull ? "" : (string)rows["DelMun"],
                                  Estado = rows["Estado"] is DBNull ? "" : (string)rows["Estado"],
                                  NoCelular = rows["NoCelular"] is DBNull ? "" : (string)rows["NoCelular"],
                                  RFC = rows["RFC"] is DBNull ? "" : (string)rows["RFC"],
                                  TelCasa = rows["TelCasa"] is DBNull ? "" : (string)rows["TelCasa"],
                                  CardCode = rows["CardCode"] is DBNull ? "" : (string)rows["CardCode"],
                                  IDRef = rows["IDRef"] is DBNull ? "" : (string)rows["IDRef"],
                                  Referencia = rows["Referencia"] is DBNull ? "" : (string)rows["Referencia"],
                                  TelOfc = rows["TelOfc"] is DBNull ? "" : (string)rows["TelOfc"],
                                  FormaPago33 = rows["FormaPago33"] is DBNull ? "" : (string)rows["FormaPago33"],
                                  CFDI_Rel33 = rows["CFDI_Rel33"] is DBNull ? "" : (string)rows["CFDI_Rel33"],
                                  actCod_CIUDAD = rows["actCod_CIUDAD"] is DBNull ? "" : (string)rows["actCod_CIUDAD"],
                                  MetodoPago33 = rows["MetodoPago33"] is DBNull ? "" : (string)rows["MetodoPago33"],
                                  TipoCliente = rows["TipoCliente"] is DBNull ? "" : (string)rows["TipoCliente"],
                                  TipoComp33 = rows["TipoComp33"] is DBNull ? "" : (string)rows["TipoComp33"],
                                  TipoRel33 = rows["TipoRel33"] is DBNull ? "" : (string)rows["TipoRel33"],
                                  UsoCFDI33 = rows["TipoRel33"] is DBNull ? "" : (string)rows["TipoRel33"],
                                  actCod_COMUNAColonia = rows["actCod_COMUNAcolonia"] is DBNull ? "" : (string)rows["actCod_COMUNAcolonia"],
                                  actCod_PROVINCIAmunicipio = rows["actCod_PROVINCIAmunicipio"] is DBNull ? "" : (string)rows["actCod_PROVINCIAmunicipio"],
                                  actCod_REGIONestado = rows["actCod_REGIONestado"] is DBNull ? "" : (string)rows["actCod_REGIONestado"],                                
                                  actFCHNACIMIENTOTemp = rows["actFCHNACIMIENTO"] is DBNull ? DateTime.Now.ToString("yyyy-MM-dd") : Convert.ToDateTime(rows["actFCHNACIMIENTO"]).ToString("yyyy-MM-dd"),
                                  RegimenFiscal = rows["RegimenFiscal"] is DBNull ? "0" : Convert.ToInt32(rows["RegimenFiscal"]).ToString(),
                              }).FirstOrDefault();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public Clientes GetClienteFacturacionxId(ClienteInputView clienteInputView)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = clienteInputView.Id }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClientesFacturacionxIdInfo", lsParameters, CommandType.StoredProcedure, "Clientes", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new Clientes
                              {
                                  Id = rows["Id"] is DBNull ? "" : Convert.ToInt32(rows["Id"]).ToString(),
                                  actAPEPATERNO = rows["actAPEPATERNO"] is DBNull ? "" : (string)rows["actAPEPATERNO"],
                                  actAPEMATERNO = rows["actAPEMATERNO"] is DBNull ? "" : (string)rows["actAPEMATERNO"],
                                  actNOMBRE = rows["actNOMBRE"] is DBNull ? "" : (string)rows["actNOMBRE"],
                                  actCALLE = rows["actCALLE"] is DBNull ? "" : (string)rows["actCALLE"],
                                  actCIUDAD = rows["actCIUDAD"] is DBNull ? "" : (string)rows["actCIUDAD"],
                                  Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"],
                                  actNUMEXT = rows["actNUMEXT"] is DBNull ? "" : (string)rows["actNUMEXT"],
                                  actNUMINT = rows["actNUMINT"] is DBNull ? "" : (string)rows["actNUMINT"],
                                  CalleNumero = rows["CalleNumero"] is DBNull ? "" : (string)rows["CalleNumero"],
                                  Colonia = rows["Colonia"] is DBNull ? "" : (string)rows["Colonia"],
                                  Correo = rows["Correo"] is DBNull ? "" : (string)rows["Correo"],
                                  CP = rows["CP"] is DBNull ? "" : (string)rows["CP"],
                                  DelMun = rows["DelMun"] is DBNull ? "" : (string)rows["DelMun"],
                                  Estado = rows["Estado"] is DBNull ? "" : (string)rows["Estado"],
                                  RFC = rows["RFC"] is DBNull ? "" : (string)rows["RFC"],
                                  TelCasa = rows["TelCasa"] is DBNull ? "" : (string)rows["TelCasa"],
                                  CardCode = rows["CardCode"] is DBNull ? "" : (string)rows["CardCode"],
                                  IDRef = rows["IDRef"] is DBNull ? "" : (string)rows["IDRef"],
                                  Referencia = rows["Referencia"] is DBNull ? "" : (string)rows["Referencia"],
                                  TelOfc = rows["TelOfc"] is DBNull ? "" : (string)rows["TelOfc"],
                                  FormaPago33 = rows["FormaPago33"] is DBNull ? "" : (string)rows["FormaPago33"],
                                  CFDI_Rel33 = rows["CFDI_Rel33"] is DBNull ? "" : (string)rows["CFDI_Rel33"],
                                  actCod_CIUDAD = rows["actCod_CIUDAD"] is DBNull ? "" : (string)rows["actCod_CIUDAD"],
                                  MetodoPago33 = rows["MetodoPago33"] is DBNull ? "" : (string)rows["MetodoPago33"],
                                  TipoCliente = rows["TipoCliente"] is DBNull ? "" : (string)rows["TipoCliente"],
                                  TipoComp33 = rows["TipoComp33"] is DBNull ? "" : (string)rows["TipoComp33"],
                                  TipoRel33 = rows["TipoRel33"] is DBNull ? "" : (string)rows["TipoRel33"],
                                  UsoCFDI33 = rows["TipoRel33"] is DBNull ? "" : (string)rows["TipoRel33"],
                                  actCod_COMUNAColonia = rows["actCod_COMUNAcolonia"] is DBNull ? "" : (string)rows["actCod_COMUNAcolonia"],
                                  actCod_PROVINCIAmunicipio = rows["actCod_PROVINCIAmunicipio"] is DBNull ? "" : (string)rows["actCod_PROVINCIAmunicipio"],
                                  actCod_REGIONestado = rows["actCod_REGIONestado"] is DBNull ? "" : (string)rows["actCod_REGIONestado"],
                                  actFCHNACIMIENTO = rows["actFCHNACIMIENTO"] is DBNull ? DateTime.Now : Convert.ToDateTime(rows["actFCHNACIMIENTO"]),
                                  RegimenFiscal = rows["RegimenFiscal"] is DBNull ? "0" : Convert.ToInt32(rows["RegimenFiscal"]).ToString(),
                              }).FirstOrDefault();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        [Route("api2/GetInsertInfoCliente")]
        [HttpPost]
        public Task<int> GetInsertInfoCliente(Clientes clientes)
        {
            string connstringWEB;
            Clientes clientesFact = new Clientes();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                SqlParameter paremeterFecha;

                clientesFact.Id = clientes.Id;
                clientesFact.actAPEMATERNO = clientes.actAPEMATERNO;
                clientesFact.actAPEPATERNO = clientes.actAPEPATERNO;
                clientesFact.actCALLE = clientes.actCALLE;
                clientesFact.actCIUDAD = clientes.actCIUDAD;
                clientesFact.actCod_COMUNAColonia = clientes.actCod_COMUNAColonia;
                clientesFact.actCod_COMUNAColoniaFact = clientes.actCod_COMUNAColoniaFact;
                clientes.actCod_PROVINCIAmunicipio = clientes.actCod_PROVINCIAmunicipio;
                clientesFact.actCod_REGIONestado = clientes.actCod_REGIONestado;
                clientesFact.actCod_REGIONestadoFact = clientes.actCod_REGIONestadoFact;
                clientesFact.actFCHNACIMIENTO = clientes.actFCHNACIMIENTO;
                clientesFact.actFCHNACIMIENTOTemp = clientes.actFCHNACIMIENTOTemp;
                clientesFact.actNOMBRE = clientes.actNOMBRE;
                clientesFact.actNUMEXT = clientes.actNUMEXT;
                clientesFact.actNUMEXTFact = clientes.actNUMEXTFact;
                clientesFact.actNUMINT = clientes.actNUMINT;
                clientesFact.actNUMINTFact = clientes.actNUMINTFact;
                clientesFact.CalleNumero = clientes.CalleNumero;
                clientesFact.CalleNumeroFact = clientes.CalleNumeroFact;
                clientesFact.CardCode = clientes.CardCode;
                clientesFact.CFDI_Rel33 = clientes.CFDI_Rel33;
                clientesFact.Colonia = clientes.Colonia;
                clientesFact.ColoniaFact = clientes.ColoniaFact;
                clientesFact.Correo = clientes.Correo;
                clientesFact.CP = clientes.CP;
                clientesFact.CPFact = clientes.CPFact;
                clientesFact.DelMun = clientes.DelMun;
                clientesFact.DelMunFact = clientes.DelMunFact;
                clientesFact.Domicilio = clientes.Domicilio;
                clientesFact.DomicilioFact = clientes.DomicilioFact;
                clientesFact.Estado = clientes.Estado;
                clientesFact.FormaPago33 = clientes.FormaPago33;
                clientesFact.IDRef = clientes.IDRef;
                clientesFact.IdStore = clientes.IdStore;
                clientesFact.MetodoPago33 = clientes.MetodoPago33;
                clientesFact.NoCelular = clientes.NoCelular;
                clientesFact.Nombre = clientes.Nombre;
                clientesFact.Referencia = clientes.Referencia;
                clientesFact.RFC = clientes.RFC;
                clientesFact.TelCasa = clientes.TelCasa;
                clientesFact.TelOfc = clientes.TelOfc;
                clientesFact.TipoCliente = clientes.TipoCliente;
                clientesFact.TipoComp33 = clientes.TipoComp33;
                clientesFact.TipoRel33 = clientes.TipoRel33;
                clientesFact.UsoCFDI33 = clientes.UsoCFDI33;
                clientesFact.RegimenFiscal = clientes.RegimenFiscal;
                

                if (string.IsNullOrEmpty(clientes.actFCHNACIMIENTOTemp))
                {

                    paremeterFecha = new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaNacimiento", Value = null };
                }
                else
                {
                    paremeterFecha = new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaNacimiento", Value = Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp) };
                }


                if (string.IsNullOrEmpty(clientes.actAPEPATERNO))
                {
                    clientes.actAPEPATERNO = "";
                }
                if (string.IsNullOrEmpty(clientes.actAPEMATERNO))
                {
                    clientes.actAPEMATERNO = "";
                }

                if (string.IsNullOrEmpty(clientes.actNUMINT))
                {
                    clientes.actNUMINT = "";
                }

                if (string.IsNullOrEmpty(clientes.actNUMINTFact))
                {
                    clientes.actNUMINTFact = "";
                }

                if (clientes.actCod_COMUNAColonia.Equals("-1") ||
                    clientes.Colonia.Equals("Seleccione Colonia"))
                {
                    if (clientes.RFC.Equals("XAXX010101000"))
                    {
                        if (clientes.IdStore != 0 ||
                           clientes.IdStore != -1)
                        {
                            //var store = GetStore(clientes.IdStore.ToString());
                            //clientes.CalleNumero = store.CalleNumero;
                            //clientes.actCALLE = store.CalleNumero;
                            //clientes.actNUMEXT = store.NumExt;
                            //clientes.actNUMINT = store.NumInt;
                            //clientes.Estado = store.Delegacion;
                            //clientes.DelMun = store.Delegacion;
                            //clientes.Colonia = store.Colonia;
                            //clientes.CP = store.CodigoPostal;
                            //clientes.actCod_REGIONestado = store.EstadoId;
                            //clientes.Correo = store.emailTienda;

                            clientes.CalleNumero = clientes.CalleNumeroFact;
                            clientes.actCALLE = clientes.CalleNumeroFact;
                            clientes.actNUMEXT = clientes.actNUMEXTFact;
                            clientes.actNUMINT = clientes.actNUMINTFact;
                            clientes.actCod_COMUNAColonia = clientes.actCod_COMUNAColoniaFact;
                            clientes.DelMun = clientes.DelMunFact;
                            clientes.Colonia = clientes.ColoniaFact;
                            clientes.CP = clientes.CPFact;
                            clientes.actCod_REGIONestado = clientes.actCod_REGIONestadoFact;
                        }
                    }
                }

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.Nombre },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = clientes.RFC },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumero", Value = clientes.CalleNumero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNOMBRE", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEPATERNO", Value = clientes.actAPEPATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEMATERNO", Value = clientes.actAPEMATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXT", Value = clientes.actNUMEXT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINT", Value = clientes.actNUMINT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Colonia", Value = clientes.Colonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_COMUNAColonia", Value = clientes.actCod_COMUNAColonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CP", Value = clientes.CP },

                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelCasa", Value = clientes.TelCasa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelOfc", Value = clientes.TelOfc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = clientes.NoCelular },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Correo", Value = clientes.Correo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estado", Value = clientes.actCod_REGIONestado },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_REGIONestado", Value = clientes.actCod_REGIONestado },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMun", Value = clientes.DelMun },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoCliente", Value = clientes.TipoCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RegimenFiscal", Value = clientes.RegimenFiscal },

                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumeroFact", Value = clientes.CalleNumeroFact },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXTFact", Value = clientes.actNUMEXTFact },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINTFact", Value = clientes.actNUMINTFact },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ColoniaFact", Value = clientes.ColoniaFact },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CPFact", Value = clientes.CPFact },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoFact", Value = Convert.ToInt32(clientes.actCod_REGIONestadoFact) },
                    //    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMunFact", Value = clientes.DelMunFact },
                    };

                lsParameters.Add(paremeterFecha);

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int i = obj.EjecutaQry_Tabl("InsertCustomersNew", "@IdCliente", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (i > 0)
                {
                    clientes.Id = i.ToString();
                    clientesFact.Id = i.ToString();
                    int Fact = GetInsertInfoClienteFacturacion(clientesFact);
                }

                return Task.Run(() => i);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public int GetInsertInfoClienteFacturacion(Clientes clientes)
        {
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlParameter paremeterFecha;

                if (string.IsNullOrEmpty(clientes.RFC))
                {
                    clientes.RFC = "XAXX010101000";
                }

                if (string.IsNullOrEmpty(clientes.actFCHNACIMIENTOTemp))
                {
                    paremeterFecha = new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaNacimiento", Value = null };
                }
                else
                {
                    paremeterFecha = new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaNacimiento", Value = Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp) };
                }


                if (string.IsNullOrEmpty(clientes.actNUMINTFact))
                {
                    clientes.actNUMINTFact = "";
                }
                if (string.IsNullOrEmpty(clientes.actNUMINT))
                {
                    clientes.actNUMINT = "";
                }
                if (string.IsNullOrEmpty(clientes.NoCelular))
                {
                    clientes.NoCelular = "";
                }
                if (string.IsNullOrEmpty(clientes.TelCasa))
                {
                    clientes.TelCasa = "";
                }
                if (string.IsNullOrEmpty(clientes.TelOfc))
                {
                    clientes.TelOfc = "";
                }

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.Nombre },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = clientes.RFC },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNOMBRE", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEPATERNO", Value = clientes.actAPEPATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEMATERNO", Value = clientes.actAPEMATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_COMUNAColoniaFact", Value = clientes.actCod_COMUNAColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelCasa", Value = clientes.TelCasa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelOfc", Value = clientes.TelOfc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = clientes.NoCelular },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Correo", Value = clientes.Correo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoCliente", Value = clientes.TipoCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumeroFact", Value = clientes.CalleNumeroFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXTFact", Value = clientes.actNUMEXTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINTFact", Value = clientes.actNUMINTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ColoniaFact", Value = clientes.ColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CPFact", Value = clientes.CPFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoFact", Value = clientes.actCod_REGIONestadoFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMunFact", Value = clientes.DelMunFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = clientes.Id }
                    };
                lsParameters.Add(paremeterFecha);

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int i = obj.EjecutaQry_Tabl("InsertCustomersFacturacion", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                return (i);
            }
            catch (Exception ex)
            {
                return 3;
            }
            return 3;
        }
        public int GetUpdateInfoClienteFacturacion(Clientes clientes)
        {
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                SqlParameter paremeterFecha;

                if (string.IsNullOrEmpty(clientes.RFC))
                {
                    clientes.RFC = "XAXX010101000";
                }

                if (string.IsNullOrEmpty(clientes.actFCHNACIMIENTOTemp))
                {
                    paremeterFecha = new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaNacimiento", Value = null };
                }
                else
                {
                    paremeterFecha = new System.Data.SqlClient.SqlParameter() { ParameterName = "@FechaNacimiento", Value = Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp) };
                }
                if (string.IsNullOrEmpty(clientes.actNUMINTFact))
                {
                    clientes.actNUMINTFact = "";
                }

                if (clientes.actFCHNACIMIENTO == null)
                {
                    clientes.actFCHNACIMIENTO = DateTime.Now;
                }


                if (string.IsNullOrEmpty(clientes.actAPEPATERNO))
                {
                    clientes.actAPEPATERNO = "";
                }
                if (string.IsNullOrEmpty(clientes.actAPEMATERNO))
                {
                    clientes.actAPEMATERNO = "";
                }

                if (string.IsNullOrEmpty(clientes.actNUMINTFact))
                {
                    clientes.actNUMINTFact = "";
                }

                if (string.IsNullOrEmpty(clientes.actNUMEXTFact))
                {
                    clientes.actNUMEXTFact = "";
                }

                if (string.IsNullOrEmpty(clientes.NoCelular)) { clientes.NoCelular = ""; }
                if (string.IsNullOrEmpty(clientes.TelCasa)) { clientes.TelCasa = ""; }
                if (string.IsNullOrEmpty(clientes.TelOfc)) { clientes.TelOfc = ""; }
                if (string.IsNullOrEmpty(clientes.actNOMBRE)) { clientes.actNOMBRE = ""; }
                if (string.IsNullOrEmpty(clientes.CalleNumeroFact)) { clientes.CalleNumeroFact = ""; }
                if (string.IsNullOrEmpty(clientes.actNUMINTFact)) { clientes.actNUMINTFact = ""; }
                if (string.IsNullOrEmpty(clientes.actNUMEXTFact)) { clientes.actNUMEXTFact = ""; }
                if (string.IsNullOrEmpty(clientes.actCod_COMUNAColoniaFact)) { clientes.actCod_COMUNAColoniaFact = ""; }
                if (string.IsNullOrEmpty(clientes.actCod_REGIONestadoFact)) { clientes.actCod_REGIONestadoFact = ""; }
                if (string.IsNullOrEmpty(clientes.CPFact)) { clientes.CPFact = ""; }
                if (string.IsNullOrEmpty(clientes.ColoniaFact)) { clientes.ColoniaFact = ""; }
                if (string.IsNullOrEmpty(clientes.DelMunFact)) { clientes.DelMunFact = ""; }
                if (string.IsNullOrEmpty(clientes.CPFact)) { clientes.CPFact = ""; }


                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.Nombre },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = clientes.RFC },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNOMBRE", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEPATERNO", Value = clientes.actAPEPATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEMATERNO", Value = clientes.actAPEMATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_COMUNAColoniaFact", Value = clientes.actCod_COMUNAColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelCasa", Value = clientes.TelCasa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelOfc", Value = clientes.TelOfc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = clientes.NoCelular },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Correo", Value = clientes.Correo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoFact", Value = clientes.actCod_REGIONestadoFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_REGIONestadoFact", Value = clientes.actCod_REGIONestadoFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoCliente", Value = clientes.TipoCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumeroFact", Value = clientes.CalleNumeroFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXTFact", Value = clientes.actNUMEXTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINTFact", Value = clientes.actNUMINTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ColoniaFact", Value = clientes.ColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CPFact", Value = clientes.CPFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMunFact", Value = clientes.DelMunFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Id", Value = Convert.ToInt32(clientes.Id) }
                       // new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaNacimiento", Value = clientes.actFCHNACIMIENTO },
                };
                lsParameters.Add(paremeterFecha);

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int i = obj.EjecutaQry_Tabl("UpdateCustomersFacturacion", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                return (i);
            }
            catch (Exception ex)
            {
                return 3;
            }
            return 3;
        }
        [Route("api2/GetUpdateInfoCliente")]
        [HttpPost]
        public Task<int> GetUpdateInfoCliente(Clientes clientes)
        {
            string connstringWEB;
            Clientes clientesFact = new Clientes();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                clientesFact.Id = clientes.Id;
                clientesFact.actAPEMATERNO = clientes.actAPEMATERNO;
                clientesFact.actAPEPATERNO = clientes.actAPEPATERNO;
                clientesFact.actCALLE = clientes.actCALLE;
                clientesFact.actCIUDAD = clientes.actCIUDAD;
                clientesFact.actCod_COMUNAColonia = clientes.actCod_COMUNAColonia;
                clientesFact.actCod_COMUNAColoniaFact = clientes.actCod_COMUNAColoniaFact;
                clientesFact.actCod_PROVINCIAmunicipio = clientes.actCod_PROVINCIAmunicipio;
                clientesFact.actCod_REGIONestado = clientes.actCod_REGIONestado;
                clientesFact.actCod_REGIONestadoFact = clientes.actCod_REGIONestadoFact;
                clientesFact.actFCHNACIMIENTO = clientes.actFCHNACIMIENTO;
                clientesFact.actFCHNACIMIENTOTemp = clientes.actFCHNACIMIENTOTemp;
                clientesFact.actNOMBRE = clientes.actNOMBRE;
                clientesFact.actNUMEXT = clientes.actNUMEXT;
                clientesFact.actNUMEXTFact = clientes.actNUMEXTFact;
                clientesFact.actNUMINT = clientes.actNUMINT;
                clientesFact.actNUMINTFact = clientes.actNUMINTFact;
                clientesFact.CalleNumero = clientes.CalleNumero;
                clientesFact.CalleNumeroFact = clientes.CalleNumeroFact;
                clientesFact.CardCode = clientes.CardCode;
                clientesFact.CFDI_Rel33 = clientes.CFDI_Rel33;
                clientesFact.Colonia = clientes.Colonia;
                clientesFact.ColoniaFact = clientes.ColoniaFact;
                clientesFact.Correo = clientes.Correo;
                clientesFact.CP = clientes.CP;
                clientesFact.CPFact = clientes.CPFact;
                clientesFact.DelMun = clientes.DelMun;
                clientesFact.DelMunFact = clientes.DelMunFact;
                clientesFact.Domicilio = clientes.Domicilio;
                clientesFact.DomicilioFact = clientes.DomicilioFact;
                clientesFact.Estado = clientes.Estado;
                clientesFact.FormaPago33 = clientes.FormaPago33;
                clientesFact.IDRef = clientes.IDRef;
                clientesFact.IdStore = clientes.IdStore;
                clientesFact.MetodoPago33 = clientes.MetodoPago33;
                clientesFact.NoCelular = clientes.NoCelular;
                clientesFact.Nombre = clientes.Nombre;
                clientesFact.Referencia = clientes.Referencia;
                clientesFact.RFC = clientes.RFC;
                clientesFact.TelCasa = clientes.TelCasa;
                clientesFact.TelOfc = clientes.TelOfc;
                clientesFact.TipoCliente = clientes.TipoCliente;
                clientesFact.TipoComp33 = clientes.TipoComp33;
                clientesFact.TipoRel33 = clientes.TipoRel33;
                clientesFact.UsoCFDI33 = clientes.UsoCFDI33;                
                clientesFact.RegimenFiscal = clientes.RegimenFiscal;                

                if (!string.IsNullOrEmpty(clientes.actFCHNACIMIENTOTemp))
                {
                    clientes.actFCHNACIMIENTO = Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp);
                }

                if (clientes.actCod_COMUNAColoniaFact == null ||
                     clientes.Colonia.Equals("Seleccione"))
                {
                    if (clientes.IdStore != 0 ||
                       clientes.IdStore != -1)
                    {
                        var store = GetStore(clientes.IdStore.ToString());
                        clientes.CalleNumero = store.CalleNumero;
                        clientes.actCALLE = store.CalleNumero;
                        clientes.actNUMEXT = store.NumExt;
                        clientes.actNUMINT = store.NumInt;
                        clientes.Estado = store.Delegacion;
                        clientes.DelMun = store.Delegacion;
                        clientes.Colonia = store.Colonia;
                        clientes.CP = store.CodigoPostal;
                        clientes.actCod_REGIONestado = store.EstadoId;
                        clientes.Correo = store.emailTienda;
                    }
                }


                if (string.IsNullOrEmpty(clientes.NoCelular)) { clientes.NoCelular = ""; }
                if (string.IsNullOrEmpty(clientes.TelCasa)) { clientes.TelCasa = ""; }
                if (string.IsNullOrEmpty(clientes.TelOfc)) { clientes.TelOfc = ""; }
                if (string.IsNullOrEmpty(clientes.actNOMBRE)) { clientes.actNOMBRE = ""; }
                if (string.IsNullOrEmpty(clientes.CalleNumero)) { clientes.CalleNumero = ""; }
                if (string.IsNullOrEmpty(clientes.actNUMINT)) { clientes.actNUMINT = ""; }
                if (string.IsNullOrEmpty(clientes.actNUMEXT)) { clientes.actNUMEXT = ""; }
                if (string.IsNullOrEmpty(clientes.actCod_COMUNAColonia)) { clientes.actCod_COMUNAColonia = ""; }
                if (string.IsNullOrEmpty(clientes.actCod_REGIONestado)) { clientes.actCod_REGIONestado = ""; }
                if (string.IsNullOrEmpty(clientes.CPFact)) { clientes.CP = ""; }
                if (string.IsNullOrEmpty(clientes.ColoniaFact)) { clientes.Colonia = ""; }
                if (string.IsNullOrEmpty(clientes.DelMunFact)) { clientes.DelMun = ""; }
                if (string.IsNullOrEmpty(clientes.CPFact)) { clientes.CP = ""; }


                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Id", Value = Convert.ToInt32(clientes.Id) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = clientes.RFC },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumero", Value = clientes.CalleNumero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNOMBRE", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEPATERNO", Value = clientes.actAPEPATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEMATERNO", Value = clientes.actAPEMATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXT", Value = clientes.actNUMEXT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINT", Value = clientes.actNUMINT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Colonia", Value = clientes.Colonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName ="@actCod_COMUNAColonia", Value = clientes.actCod_COMUNAColonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CP", Value = clientes.CP },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Correo", Value = clientesFact.Correo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estado", Value = clientes.actCod_REGIONestado },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMun", Value = clientes.DelMun },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoCliente", Value = clientes.TipoCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaNacimiento", Value = clientes.actFCHNACIMIENTO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelCasa", Value = clientes.TelCasa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelOfc", Value = clientes.TelOfc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = clientes.NoCelular },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RegimenFiscal", Value=clientes.RegimenFiscal}


                        //new System.Data.SqlClient.SqlParameter(){ ParameterName ="@actCod_COMUNAColoniaFact", Value = clientes.actCod_COMUNAColoniaFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumeroFact", Value = clientesFact.CalleNumeroFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXTFact", Value = clientesFact.actNUMEXTFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINTFact", Value = clientesFact.actNUMINTFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ColoniaFact", Value = clientesFact.ColoniaFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CPFact", Value = clientesFact.CPFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoFact", Value = clientesFact.actCod_REGIONestadoFact },
                        //new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMunFact", Value = clientesFact.DelMunFact }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int i = obj.EjecutaQry_Tabl("UpdateCustomers", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (i > 0)
                {
                    int j = GetUpdateInfoClienteFacturacion(clientesFact);
                }
                return Task.Run(() => i);
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        [Route("api2/GetInsertInfoClienteNew")]
        [HttpPost]
        public Task<int> GetInsertInfoClienteNew(Clientes clientes)
        {
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.Nombre },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = clientes.RFC },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumero", Value = clientes.CalleNumero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNOMBRE", Value = clientes.actNOMBRE  },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEPATERNO", Value = clientes.actAPEPATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEMATERNO", Value = clientes.actAPEMATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXT", Value = clientes.actNUMEXT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINT", Value = clientes.actNUMINT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Colonia", Value = clientes.Colonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_COMUNAColonia", Value = clientes.actCod_COMUNAColonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CP", Value = clientes.CP },

                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelCasa", Value = clientes.TelCasa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelOfc", Value = clientes.TelOfc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = clientes.NoCelular },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Correo", Value = clientes.Correo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estado", Value = clientes.actCod_REGIONestado },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actCod_REGIONestado", Value = clientes.actCod_REGIONestado },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMun", Value = clientes.DelMun },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoCliente", Value = clientes.TipoCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaNacimiento", Value = Convert.ToDateTime(clientes.actFCHNACIMIENTOTemp) },

                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumeroFact", Value = clientes.CalleNumeroFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXTFact", Value = clientes.actNUMEXTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINTFact", Value = clientes.actNUMINTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ColoniaFact", Value = clientes.ColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CPFact", Value = clientes.CPFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoFact", Value = clientes.actCod_REGIONestadoFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMunFact", Value = clientes.DelMunFact }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int i = obj.EjecutaQry_Tabl("InsertCustomersN", "@IdCliente", lsParameters, CommandType.StoredProcedure, connstringWEB);

                return Task.Run(() => i);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetInfoCP")]
        [HttpPost]
        public Task<List<CPSepomex>> GetInfoCP(CPSepomex CPSepomex)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@d_codigo", Value = CPSepomex.d_codigo }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("CPCodigoPostal", lsParameters, CommandType.StoredProcedure, "CPSepomex", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CPSepomex
                              {
                                  c_colonia = Convert.ToInt32(rows["c_colonia"]),
                                  d_estado = (string)rows["d_estado"],
                                  c_estado = Convert.ToInt32(rows["c_estado"]),
                                  d_mnpio = (string)rows["d_mnpio"],
                                  c_mnpio = Convert.ToInt32(rows["c_mnpio"]),
                                  c_cve_ciudad = rows["c_cve_ciudad"] is DBNull ? 0 : Convert.ToInt32(rows["c_cve_ciudad"]),
                                  d_ciudad = rows["d_ciudad"] is DBNull ? (string)rows["d_mnpio"] : (string)rows["d_ciudad"],
                                  d_asenta = (string)rows["d_asenta"],
                                  id_asenta_cpcons = Convert.ToInt32(rows["id_asenta_cpcons"])
                              }).ToList();

                    return Task.Run(() => ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetInfoCPAsentamiento")]
        [HttpPost]
        public Task<List<CPSepomex>> GetInfoCPAsentamiento(CPSepomex CPSepomex)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;


                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@d_asenta", Value = CPSepomex.d_asenta }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("CPCodigoPostalAsentamientos", lsParameters, CommandType.StoredProcedure, "CPSepomex", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new CPSepomex
                              {
                                  c_colonia = Convert.ToInt32(rows["c_colonia"]),
                                  d_estado = (string)rows["d_estado"],
                                  c_estado = Convert.ToInt32(rows["c_estado"]),
                                  d_mnpio = (string)rows["d_mnpio"],
                                  c_mnpio = Convert.ToInt32(rows["c_mnpio"]),
                                  c_cve_ciudad = rows["c_cve_ciudad"] is DBNull ? 0 : Convert.ToInt32(rows["c_cve_ciudad"]),
                                  d_ciudad = rows["d_ciudad"] is DBNull ? (string)rows["d_mnpio"] : (string)rows["d_ciudad"],
                                  d_asenta = (string)rows["d_asenta"],
                                  id_asenta_cpcons = Convert.ToInt32(rows["id_asenta_cpcons"]),
                                  d_codigo = Convert.ToInt32(rows["d_codigo"])
                              }).ToList();

                    return Task.Run(() => ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetClienteMostrador")]
        [HttpPost]
        public Task<List<Clientes>> GetClienteMostrador(ClienteInputView clientes)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteMostradorInfo", CommandType.StoredProcedure, "Clientes", connstringWEB);

                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              select new Clientes
                              {
                                  Id = rows["Id"] is DBNull ? "" : Convert.ToInt32(rows["Id"]).ToString(),
                                  Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"],
                                  RFC = rows["RFC"] is DBNull ? "" : (string)rows["RFC"],
                                  Domicilio = rows["Domicilio"] is DBNull ? "" : (string)rows["Domicilio"],
                                  DomicilioFact = rows["DomicilioFact"] is DBNull ? "" : (string)rows["DomicilioFact"],
                                  Correo = rows["Correo"] is DBNull ? "" : (string)rows["Correo"]
                              }).ToList();

                    return Task.Run(() => ls);
                }

                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api2/GetAdminUserInfoAll")]
        [HttpPost]
        public IEnumerable<Users> GetAdminUserInfoAll(UsersView users)
        {

            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminUserInfoAll", CommandType.StoredProcedure, "AdminUser", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new Users
                              {
                                  usuario = rows["NTUserAccount"] is DBNull ? "" : (string)rows["NTUserAccount"],
                                  password = rows["NTUserDomain"] is DBNull ? "" : (string)rows["NTUserDomain"]
                              }).ToList();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetUpdateFactInfoCliente")]
        [HttpPost]
        public Task<int> GetUpdateFactInfoCliente(Clientes clientes)
        {
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                if (string.IsNullOrEmpty(clientes.actFCHNACIMIENTOTemp))
                {
                    clientes.actFCHNACIMIENTO = DateTime.Now;
                }

                if (string.IsNullOrEmpty(clientes.TelCasa))
                {
                    clientes.TelCasa = "";
                }

                if (string.IsNullOrEmpty(clientes.TelOfc))
                {
                    clientes.TelOfc = "";
                }

                if (string.IsNullOrEmpty(clientes.NoCelular))
                {
                    clientes.NoCelular = "";
                }

                if (string.IsNullOrEmpty(clientes.actNUMEXT))
                {
                    clientes.actNUMEXT = "";
                }
                if (string.IsNullOrEmpty(clientes.actNUMEXTFact))
                {
                    clientes.actNUMEXTFact = "";
                }
                if (string.IsNullOrEmpty(clientes.actNUMINT))
                {
                    clientes.actNUMINT = "";
                }
                if (string.IsNullOrEmpty(clientes.actNUMINTFact))
                {
                    clientes.actNUMINTFact = "";
                }

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Id", Value = Convert.ToInt32(clientes.Id) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RFC", Value = clientes.RFC },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumero", Value = clientes.CalleNumero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNOMBRE", Value = clientes.actNOMBRE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEPATERNO", Value = clientes.actAPEPATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actAPEMATERNO", Value = clientes.actAPEMATERNO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXT", Value = clientes.actNUMEXT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINT", Value = clientes.actNUMINT },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Colonia", Value = clientes.Colonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName ="@actCod_COMUNAColonia", Value = clientes.actCod_COMUNAColonia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CP", Value = clientes.CP },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Correo", Value = clientes.Correo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Estado", Value = clientes.actCod_REGIONestado },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMun", Value = clientes.DelMun },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoCliente", Value = clientes.TipoCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FechaNacimiento", Value = clientes.actFCHNACIMIENTO },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelCasa", Value = clientes.TelCasa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TelOfc", Value = clientes.TelOfc },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = clientes.NoCelular },

                        new System.Data.SqlClient.SqlParameter(){ ParameterName ="@actCod_COMUNAColoniaFact", Value = clientes.actCod_COMUNAColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CalleNumeroFact", Value = clientes.CalleNumeroFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMEXTFact", Value = clientes.actNUMEXTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@actNUMINTFact", Value = clientes.actNUMINTFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ColoniaFact", Value = clientes.ColoniaFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CPFact", Value = clientes.CPFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EstadoFact", Value = clientes.actCod_REGIONestadoFact },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@DelMunFact", Value = clientes.DelMunFact }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int i = obj.EjecutaQry_Tabl("UpdateCustomers", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                return Task.Run(() => i);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public StoreAdmin GetStore(string idStore)
        {
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value = Convert.ToInt32(idStore)},
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                DataTable dt = obj.EjecutaQry_Tabla("StoreInfo", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new StoreAdmin
                              {
                                  StoreName = rows["StoreName"] is DBNull ? "" : (string)rows["StoreName"],
                                  CalleNumero = rows["CalleNumero"] is DBNull ? "" : (string)rows["CalleNumero"],
                                  CodigoPostal = rows["CodigoPostal"] is DBNull ? "" : (string)rows["CodigoPostal"],
                                  Colonia = rows["Colonia"] is DBNull ? "" : (string)rows["Colonia"],
                                  EstadoId = rows["EstadoId"] is DBNull ? "" : Convert.ToInt32(rows["EstadoId"]).ToString(),
                                  Delegacion = rows["Delegacion"] is DBNull ? "" : (string)rows["Delegacion"],
                                  NumExt = rows["NumExt"] is DBNull ? "" : (string)rows["NumExt"],
                                  NumInt = rows["NumInt"] is DBNull ? "" : (string)rows["NumInt"],
                                  emailTienda = rows["emailTienda"] is DBNull ? "" : (string)rows["emailTienda"]
                              }).ToList().FirstOrDefault();

                    return ls;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


    }
}