using APIPOSS.Models.Payback;
using APIPOSS.Models.Ventas;
//using APIPOSS.WsRedemption;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIPOSS.Controllers
{
    public class PaybackController : ApiController
    {
        [Route("api/GetAdminPayback")]
        public AdminPaybackView GetAdminPayback(string JsonWs)
        {
            StringBuilder UserQuery = new StringBuilder();
            AdminPaybackView AdminInfo = null;
            DataTable dt;
            string JsonGetArticulosView = string.Empty;
            RequestAdminInfo data = null;
            try
            {
                data = JsonConvert.DeserializeObject<RequestAdminInfo>(JsonWs);

                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery.Append("SELECT AdminUser,Password,PartnerShortName,ValueInPoints,(select Identificador from Cat_IdentificadorPayback where TipoWs = '" + data.TypeWs + "' ) as TipoWs from AdminPayback WHERE Wservice = '" + data.ws + "'");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "AdminInfo", connstringWEB);

                if (dt != null)
                {
                    AdminInfo = new AdminPaybackView();
                    foreach (DataRow row in dt.Rows)
                    {
                        AdminInfo.AdminUser = row["AdminUser"].ToString();
                        AdminInfo.Password = row["Password"].ToString();
                        AdminInfo.PartnerShortName = row["PartnerShortName"].ToString();
                        AdminInfo.ValueInPoints = Convert.ToDecimal(row["ValueInPoints"].ToString());
                        AdminInfo.TypeWs = row["TipoWs"] == null ? "" : row["TipoWs"].ToString();
                    }
                }
                return AdminInfo;

            }
            catch (Exception ex)
            {
                return AdminInfo;
            }
        }
        [Route("api/GetMessageErrorPayback")]
        public string GetMessageErrorPayback(string code)
        {
            string Message = string.Empty;
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            string JsonGetArticulosView = string.Empty;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery.Append("select Mensajepos from  Cat_Errorespayback where CodigoError = '" + code + "'");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "AdminInfo", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Message = row["Mensajepos"].ToString();
                    }
                }
                return Message;

            }
            catch (Exception ex)
            {
                return Message;
            }
        }
        [Route("api/GetPrefijoVenta")]
        public string GetPrefijoVenta(string idStore)
        {
            string UserQuery = string.Empty;
            string Prefijo = string.Empty;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery = "select prefijo from storeFolios where AdminStoreID=" + idStore + " and AdminFolioType=1";
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Getprefijo", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Prefijo = row["prefijo"].ToString();
                        return Prefijo;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("api/GetDateSale")]
        public string GetDateSale(string idVenta)
        {
            string UserQuery = string.Empty;
            string Date = string.Empty;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery = "select Fecha from VentasEncabezado where ID = '" + idVenta + "'";
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "GetDate", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Date = row["Fecha"].ToString();
                        return Date;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("api/GetFolioSaleId")]
        public string GetFolioSaleId(string idVenta)
        {
            string UserQuery = string.Empty;
            string Folio = string.Empty;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery = "select Folio from VentasEncabezado where ID = '" + idVenta + "'";
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "GetFolio", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Folio = row["Folio"].ToString();
                        return Folio;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("api/GetFolioSale")]
        public string GetFolioSale(string idStore)
        {
            string UserQuery = string.Empty;
            string Folio = string.Empty;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery = "(select (currentfolio + 1) as [NuevoFolio] from storeFolios where AdminStoreID=" + idStore + " and AdminFolioType=1)";
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "GetFolio", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Folio = row["NuevoFolio"].ToString();
                        return Folio;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("api/GetFolioAbono")]
        public string GetFolioAbono(string idStore)
        {
            string UserQuery = string.Empty;
            string Folio = string.Empty;
            DataTable dt;
            try
            {
                string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery = "(select (currentfolio + 1) as [NuevoFolio] from storeFolios where AdminStoreID=" + idStore + " and AdminFolioType=2)";
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "GetFolio", connstringWEB);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Folio = row["NuevoFolio"].ToString();
                        return Folio;
                    }
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("api/GetRegisterTransactionPBK")]
        public bool GetRegisterTransactionPBK(string TPBK)
        { 
            StringBuilder UserQuery = new StringBuilder();
            StringBuilder UserQueryDet = new StringBuilder();
            RequestRegisterPaybackSale Transaction1 = null;
            DataTable dt;
            string sError = string.Empty;
            string idtrans = string.Empty;
            string SQuery = string.Empty;
            string Monedero = "";
            bool Existeacomulacion = false;
            bool Mismomonedero = true;
            string idtransaction = "";
            DataTable dtvalida;
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            Utilities.DBMaster obj = new Utilities.DBMaster();
            string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {

                if (TPBK != null)
                {
                    Transaction1 = JsonConvert.DeserializeObject<RequestRegisterPaybackSale>(TPBK);
                    SQuery = "select Monedero, idTransaction from Dat_TransactionPayback where idVenta =" + Transaction1.Idventa + " and TypeTransaction = 'AcumulacionVenta'";
                    dtvalida = oDB.EjecutaQry_Tabla(SQuery.ToString(), CommandType.Text, "Existeacomulacion", connstringWEB);
                    if (dtvalida.Rows.Count >= 1)
                    {
                        Existeacomulacion = true;
                        foreach (DataRow row in dtvalida.Rows)
                        {
                            Monedero = row["Monedero"].ToString();
                            Mismomonedero = Monedero.Equals(Transaction1.monedero) ? true : false;
                            idtransaction = row["idTransaction"].ToString();
                        }
                    }
                    if (Mismomonedero == false) { return false; }

                    if (Existeacomulacion = false || dtvalida.Rows.Count <= 0)

                    {
                        List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idVenta", Value = Transaction1.Idventa },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@PartnerShortName", Value = Transaction1.PartnerShortName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@BranchShortName", Value = Transaction1.BranchShortName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EfectiveTime", Value = Transaction1.EfectiveTime },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ReceiptNumber", Value = Transaction1.ReceipNumber },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@RedemptionNumber", Value = Transaction1.RedemptionNumber == null? "": Transaction1.RedemptionNumber},
                         new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ReferenceReceiptNumber", Value = Transaction1.ReferenceReceipNumber == null ? "": Transaction1.ReferenceReceipNumber },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@LegalAmountRLegalV", Value = Transaction1.LegalAmountRLegalV },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@LoyaltyAmountRLoyalV", Value = Transaction1.LoyaltyAmountRLoyalV },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@LegalAmountTPV", Value = Transaction1.LegalAmountTPV },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Monedero", Value = Transaction1.monedero },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nip", Value = Transaction1.NIP },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TypeTransaction", Value = Transaction1.TypeTransaction },
                         new System.Data.SqlClient.SqlParameter(){ ParameterName = "@StatusTrans", Value = Transaction1.Status ? 1 : 0},
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@MessageError", Value = Transaction1.MessageError }
                    };

                        UserQuery.Append("insert into Dat_TransactionPayback (idVenta,PartnerShortName,BranchShortName,EfectiveTime,ReceiptNumber,RedemptionNumber,ReferenceReceiptNumber,LegalAmountRLegalV, LoyaltyAmountRLoyalV, LegalAmountTPV, Monedero, Nip, TypeTransaction,StatusTrans,MessageError) output inserted.idTransaction Values(@idVenta, @PartnerShortName, @BranchShortName, @EfectiveTime, @ReceiptNumber,     @RedemptionNumber, @ReferenceReceiptNumber, @LegalAmountRLegalV, @LoyaltyAmountRLoyalV, @LegalAmountTPV, @Monedero, @Nip, @TypeTransaction,@StatusTrans,@MessageError)");

                        dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), lsParameters, CommandType.Text, "Transaction", connstringWEB);
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                foreach (DataRow rows in dt.Rows)
                                {
                                    idtrans = rows["idTransaction"].ToString();
                                }
                                if (!string.IsNullOrEmpty(idtrans) && Transaction1.ListDetails.Count > 0)
                                {
                                    foreach (var Detail in Transaction1.ListDetails)
                                    {
                                        var query = ("Insert Into Dat_DetailsTransactionPayback(idTransPK, ArticleEanCode, PartnerProductGroupCode, PartnerProductCategoryCode, Quantity, TotalTurnoverAmount, TotalRewardableAmount)values(" + idtrans + ", '" + Detail.ArticleEanCode + "', '" + Detail.PartnerProductGroupCode + "', '" + Detail.PartnerProductCategoryCode + "'," + Detail.Quantity + ", " + Detail.TotalTurnoverAmount + ", " + Detail.TotalRewardableAmount + ")");
                                        var result = obj.EjecutaQry(query, CommandType.Text, connstringWEB, sError);
                                    }
                                }

                            }
                            catch (Exception ex) { }
                            InsertPoints(Transaction1, connstringWEB);
                            return true;
                        }
                        else { return false; }

                        // return true;
                    }
                    else if (dtvalida.Rows.Count == 1)
                    {
                        string Error = string.Empty;
                        string Squeyupdate = "";
                        try
                        {
                            if (!string.IsNullOrEmpty(idtransaction))
                            {
                                string Status = Transaction1.Status ? "1" : "0";
                                Squeyupdate = "Update Dat_TransactionPayback SET ReceiptNumber = '" + Transaction1.ReceipNumber + "' , LegalAmountRLegalV = '" + Transaction1.LegalAmountRLegalV + "' , LoyaltyAmountRLoyalV = '" + Transaction1.LoyaltyAmountRLoyalV + "' , StatusTrans = " + Status + " WHERE idTransaction = " + idtransaction;
                                var dtpbk = oDB.EjecutaQry(Squeyupdate, CommandType.Text, connstringWEB, Error);

                                if (!string.IsNullOrEmpty(Transaction1.Idabono))
                                {
                                    if (Transaction1.TotalPuntosPayback == 0 && Transaction1.PuntosPaybackAcumulados == 0 && Transaction1.PuntosPaybackRedimidos == 0)
                                    {

                                    }
                                    else
                                    {
                                        var Squey = "Update VentasPagos SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackAcumulados = " + Transaction1.PuntosPaybackAcumulados + " , PuntosPaybackRedimidos = " + Transaction1.PuntosPaybackRedimidos + " WHERE ID = " + Transaction1.Idabono + "";
                                        var dtup = oDB.EjecutaQry(Squey, CommandType.Text, connstringWEB, sError);
                                    }

                                }
                             
                            }

                        }
                        catch (Exception ex) { }
                        return true;
                    }
                    else { return false; }
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool InsertPoints(RequestRegisterPaybackSale Transaction1, string connstringWEB)
        {
            bool inserto = false;
            string sError = string.Empty;
            Utilities.DBMaster objPbk = new Utilities.DBMaster();
            try
            {
                if (string.IsNullOrEmpty(Transaction1.Idabono))
                {
                    string QueryPoints = string.Empty;
                    if (!Transaction1.SuccesAcumulation && !Transaction1.SuccesRedemption && Transaction1.TotalPuntosPayback == 0)
                    {
                        //nothing
                        return true;
                    }
                    else if (Transaction1.SuccesAcumulation && Transaction1.SuccesRedemption)
                    {
                        QueryPoints = "Update VentasEncabezado SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackAcumulados = " + Transaction1.PuntosPaybackAcumulados + " WHERE ID = " + Transaction1.Idventa + "";
                    }
                    else if (Transaction1.SuccesAcumulation)
                    {

                        QueryPoints = "Update VentasEncabezado SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackAcumulados = " + Transaction1.PuntosPaybackAcumulados + " WHERE ID = " + Transaction1.Idventa + "";

                    }
                    else if (Transaction1.SuccesRedemption)
                    {
                        QueryPoints = "Update VentasEncabezado SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackRedimidos = " + Transaction1.PuntosPaybackRedimidos + " WHERE ID = " + Transaction1.Idventa + "";
                    }
                    else
                    {
                        QueryPoints = "Update VentasEncabezado SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackAcumulados = " + Transaction1.PuntosPaybackAcumulados + " , PuntosPaybackRedimidos = " + Transaction1.PuntosPaybackRedimidos + " WHERE ID = " + Transaction1.Idventa + "";
                    }
                    var dtpbk = objPbk.EjecutaQry(QueryPoints, CommandType.Text, connstringWEB, sError);
                }
                else
                {
                    if (Transaction1.TotalPuntosPayback == 0 && Transaction1.PuntosPaybackAcumulados == 0 && Transaction1.PuntosPaybackRedimidos == 0)
                    {

                    }
                    else {
                        var Squey = "Update VentasPagos SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackAcumulados = " + Transaction1.PuntosPaybackAcumulados + " , PuntosPaybackRedimidos = " + Transaction1.PuntosPaybackRedimidos + " WHERE ID = " + Transaction1.Idabono + "";
                        var dtpbk = objPbk.EjecutaQry(Squey, CommandType.Text, connstringWEB, sError);
                    }

                }

            }
            catch (Exception ex) { return false; }
            return true;
        }
        [Route("api/GetDetailsPayback")]
        [HttpPost]
        public List<DetailsAcumulationRA> GetDetailsPayback(List<ArrayArticulos> ArrayArticulos)
        {
            DataTable result;
            List<DetailsAcumulationRA> ListaItemsAcumulation = new List<DetailsAcumulationRA>();
            string connstringSAP;
            try
            {
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;


                foreach (var ListArt in ArrayArticulos)
                {
                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@item", Value = ListArt.Linea }
                    };

                    Utilities.DBMaster obj = new Utilities.DBMaster();
                    result = obj.EjecutaQry_Tabla("artur", lsParameters, CommandType.StoredProcedure, "Group", connstringSAP);// obj.EjecutaQry_Tabl("artur", "", lsParameters, CommandType.StoredProcedure, connstringSAP);

                    if (result != null)
                    {
                        foreach (DataRow rows in result.Rows)
                        {
                            var ls = new DetailsAcumulationRA();
                            ls.ArticleEanCode = ListArt.Linea;
                            ls.PartnerProductCategoryCode = rows["U_BXP_MARCA"].ToString();
                            ls.PartnerProductGroupCode = rows["Clase art"].ToString();
                            ls.QuantityUnitCode = ListArt.Cantidad;
                            ls.SingleTurnoverAmount = Convert.ToDecimal(ListArt.Total.Replace("$", ""));
                            ls.TotalRewardableAmount = Convert.ToDecimal(ListArt.Total.Replace("$", ""));
                            ListaItemsAcumulation.Add(ls);

                        }
                    }
                }
                if (ListaItemsAcumulation.Count > 0) { return ListaItemsAcumulation; }

            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
