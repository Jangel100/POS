using System;
using BL.Interface;
using BL.Configuracion;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.WsAcumulation;
using WebPOS.WsRedemption;
using Entities.viewsModels.ModelPayback;
using Newtonsoft.Json;
using Entities.Models.Payback;
using System.Net.Http;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using System.Data;
using System.Text;

namespace WebPOS.Controllers
{
    public class PaybackController : Controller
    {
        readonly IPaybackBL _Payback;
        // GET: Ventas
        public PaybackController(IPaybackBL paybackBL)
        {
            _Payback = paybackBL;
        }
        public PaybackController()
        {
            _Payback = new PaybackBL();
        }
        public ActionResult Actions()
        {
            return null;
        }

        //[CustomAuthorize(Roles = "AG,US")]
        public JsonResult GetAdminInfo(string Jsonvalues)
        {
            PintsPaybackView Resul = new PintsPaybackView();
            try
            {
                var JsonP = JsonConvert.DeserializeObject<RequestPurse>(Jsonvalues);
                var Admin = _Payback.GetInfoAdminPayback("Redemption", "");
                //Session["Monedero"] = JsonP.monederoP;
                //Session["secrettype"] = JsonP.secretP;
                var points = GetAliasDashboard(JsonP.monederoP, Admin.AdminUser, Admin.Password);


                if (points != null && points.FaultMessage == null)
                {
                    Resul.Points = _Payback.ConvertPointsToAmount(points.PointsStatementListItem.AvailablePointsAmount, !string.IsNullOrEmpty(JsonP.totalP) ? Convert.ToDecimal(JsonP.totalP) : 0, Admin.ValueInPoints);
                }
                else if (points == null)
                {
                    Resul.ErrorMessage = "Error en la comuncicacion";
                }
                else
                {
                    Resul.ErrorMessage = _Payback.GetMessageView(points.FaultMessage.Code);
                }
                var JsonR = JsonConvert.SerializeObject(Resul);
                return Json(JsonR);
            }
            catch (Exception ex) { return null; }


        }
        public JsonResult Acumulacion(string Jsonvalues)
        {
            var Admin = _Payback.GetInfoAdminPayback("Redemption", "");
            var request = _Payback.QueryDatesRquestPurchase(Session["idStore"].ToString(), Session["idAbono"].ToString(), DateTime.Now, Admin.TypeWs);

            var requestAcumulacion = ProcessDirectRedemptionEvent(Session["Monedero"].ToString(), Admin.AdminUser, Admin.Password, Session["secrettype"].ToString(), request.partnerShortName, request.branchShortName, request.Fecha, request.ReceipNumber, _Payback.AmountToPoints("total", Admin.ValueInPoints), _Payback.AmountToPoints("total", Admin.ValueInPoints), 0/*monto total*/);

            if (requestAcumulacion.FaultMessage == null)
            {
                //lbmostpd.Visible = True
                //    lbmostpb.Visible = True
                //    lbmosttp.Visible = True
            }
            else
            {

            }


            return null;
        }
        public GetAliasDashboardResponse GetAliasDashboard(string Alias, string Principal, string Credential)// Ws COnsulta de saldo
        {
            //var result = new GetAliasDashboardModel();

            try
            {
                #region
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var service = new WsRedemption.ExtintPortTypeClient();
                service.ClientCredentials.UserName.UserName = Principal;//"sie_r_mx";
                service.ClientCredentials.UserName.Password = Credential; //"v3pSuYDy_mdwe6rc";

                using (OperationContextScope scope = new OperationContextScope(service.InnerChannel))
                {
                    var httpRequestProperty = new System.ServiceModel.Channels.HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(service.ClientCredentials.UserName.UserName + ":" +
                                 service.ClientCredentials.UserName.Password));
                    OperationContext.Current.OutgoingMessageProperties[System.ServiceModel.Channels.HttpRequestMessageProperty.Name] = httpRequestProperty;

                    var request = new WsRedemption.GetAliasDashboardRequest
                    {
                        Identification = new WsRedemption.MemberIdentificationType
                        {
                            //Alias = "3086810521680143",
                            Alias = Alias,
                            AliasType = WsRedemption.PrincipalVariantType.Item1
                        },

                        ConsumerIdentification = new WsRedemption.ConsumerIdentificationType
                        {
                            ConsumerAuthentication = new WsRedemption.ConsumerAuthenticationType
                            {
                                Principal = Principal,
                                Credential = Credential
                            },
                            Version = WsRedemption.VersionType.Item1

                        },

                        //CouponFilter = new PayBackRedencion.PartnerContextType[] { },
                        //ReferenceDate = DateTime.Now
                    };
                    var response = service.GetAliasDashboard(request);

                    return response;

                }
                #endregion
            }
            catch (Exception ex) { return null; }
        }
        public ProcessDirectRedemptionEventResponse ProcessDirectRedemptionEvent(string Alias, string Principal, string Credential, string secret, string PartnerShortName, string BranchShortName, DateTime EfectiveTime, string ReceiptNumber, decimal LoyaltyAmountRLV, decimal LegalAmountRLV, decimal LegalAmountTPV)// Ws Pago con puntos
        {
            try
            {
                #region
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var service = new WsRedemption.ExtintPortTypeClient();
                service.ClientCredentials.UserName.UserName = Principal;//"sie_r_mx";
                service.ClientCredentials.UserName.Password = Credential; //"v3pSuYDy_mdwe6rc";

                using (OperationContextScope scope = new OperationContextScope(service.InnerChannel))
                {
                    var httpRequestProperty = new System.ServiceModel.Channels.HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(service.ClientCredentials.UserName.UserName + ":" +
                                 service.ClientCredentials.UserName.Password));
                    OperationContext.Current.OutgoingMessageProperties[System.ServiceModel.Channels.HttpRequestMessageProperty.Name] = httpRequestProperty;

                    var request = new WsRedemption.ProcessDirectRedemptionEventRequest
                    {
                        Authentication = new WsRedemption.MemberAliasAuthenticationType
                        {
                            Identification = new WsRedemption.MemberIdentificationType
                            {

                                Alias = Alias,
                                AliasType = WsRedemption.PrincipalVariantType.Item1
                            },
                            Security = new MemberSecurityType
                            {
                                Secret = secret,
                                SecretType = WsRedemption.PrincipalSecretType.Item4
                            },

                        },

                        ConsumerIdentification = new WsRedemption.ConsumerIdentificationType
                        {
                            ConsumerAuthentication = new WsRedemption.ConsumerAuthenticationType
                            {
                                Principal = Principal,
                                Credential = Credential
                            },
                            Version = WsRedemption.VersionType.Item1

                        },
                        RedemptionEvent = new WsRedemption.RedemptionEventType
                        {
                            Partner = new WsRedemption.PartnerContextType
                            {
                                BranchShortName = BranchShortName,
                                //BusinessUnitShortName ="",
                                PartnerShortName = PartnerShortName
                            },
                            EffectiveTime = EfectiveTime,
                            ReceiptNumber = ReceiptNumber
                        },
                        DirectRedemptionItemDetails = new WsRedemption.DirectRedemptionItemDetails
                        {
                            RedemptionLegalValue = new WsRedemption.LegalUnitType
                            {
                                LegalAmount = LegalAmountRLV
                            },
                            RedemptionLoyaltyValue = new WsRedemption.LoyaltyUnitType
                            {
                                LoyaltyAmount = LoyaltyAmountRLV
                            },
                            TotalPurchaseValue = new WsRedemption.LegalUnitType
                            {
                                LegalAmount = LegalAmountTPV
                            }
                            //TotalVatValue = new WsRedemption.LegalUnitType
                            //{
                            //    LegalAmount = LegalAmount
                            //}
                        }


                        //CouponFilter = new PayBackRedencion.PartnerContextType[] { },
                        //ReferenceDate = DateTime.Now
                    };
                    var response = service.ProcessDirectRedemptionEvent(request);
                    var stringwriter = new System.IO.StringWriter();
                    var t = new WsRedemption.ProcessDirectRedemptionEventRequest();
                    var serializer = new XmlSerializer(t.GetType());
                    serializer.Serialize(stringwriter, request);
                    var dates = stringwriter.ToString();
                    return response;

                }
                #endregion
            }
            catch (Exception ex) { return null; }
        }
        public ProcessPurchaseAndPromotionEventResponse ProcessPurchaseAndPromotionEvent(string Alias, string Principal, string Credential, string PartnerShortName, string BranchShortName, DateTime EffectiveTime, string ReceiptNumber, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LegalAmountTVLV, List<DetailsAcumulationRequest> DetailsAcumulationRequest) //Ws Acumulacion
        {


            try
            {

                #region Request ProcessPurchaseAndPromotion
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var service = new WsAcumulation.ExtintPortTypeClient();
                service.ClientCredentials.UserName.UserName = Principal;//"sie_r_mx";
                service.ClientCredentials.UserName.Password = Credential; //"v3pSuYDy_mdwe6rc";

                using (OperationContextScope scope = new OperationContextScope(service.InnerChannel))
                {
                    var httpRequestProperty = new System.ServiceModel.Channels.HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(service.ClientCredentials.UserName.UserName + ":" +
                                 service.ClientCredentials.UserName.Password));
                    OperationContext.Current.OutgoingMessageProperties[System.ServiceModel.Channels.HttpRequestMessageProperty.Name] = httpRequestProperty;

                    var request = new WsAcumulation.ProcessPurchaseAndPromotionEventRequest
                    {

                        ConsumerIdentification = new WsAcumulation.ConsumerIdentificationType
                        {
                            ConsumerAuthentication = new WsAcumulation.ConsumerAuthenticationType
                            {
                                Principal = Principal,
                                Credential = Credential
                            },
                            Version = WsAcumulation.VersionType.Item1
                        },
                        Authentication = new WsAcumulation.MemberAliasIdentificationType
                        {
                            Identification = new WsAcumulation.MemberIdentificationType
                            {
                                Alias = Alias
                            }
                        },
                        CollectEventData = new WsAcumulation.CollectEventType
                        {
                            Partner = new WsAcumulation.PartnerContextType
                            {
                                PartnerShortName = PartnerShortName,
                                BranchShortName = BranchShortName
                            },
                            EffectiveDate = EffectiveTime,
                            ReceiptNumber = ReceiptNumber,
                            CommunicationChannel = "200"
                        },
                        PurchaseEventType = 1,
                        RewardableLegalValue = new WsAcumulation.LegalUnitType
                        {
                            LegalAmount = LegalAmountRLegalV
                        },
                        TotalPurchaseLegalValue = new WsAcumulation.LegalUnitType
                        {
                            LegalAmount = LegalAmountTPV
                        },
                        //TotalVatLegalValue = new WsAcumulation.LegalUnitType
                        //{
                        //   LegalAmount = LegalAmountTVLV
                        //},
                        PurchaseItemDetails =

                        (from DetailsAcumulationRequest rows in DetailsAcumulationRequest
                         select new WsAcumulation.PurchaseItemDetails
                         {
                             //PointsAmount = LegalAmountRLegalV,
                             ArticleEanCode = rows.ArticleEanCode,
                             PartnerProductGroupCode = rows.PartnerProductGroupCode,
                             PartnerProductCategoryCode = rows.PartnerProductCategoryCode,
                             //QuantityUnitCode = rows.QuantityUnitCode,
                             QuantitySpecified = true,
                             Quantity = Convert.ToDecimal(rows.QuantityUnitCode),
                             TotalTurnoverAmountSpecified = true,
                             TotalTurnoverAmount = rows.SingleTurnoverAmount,
                             TotalRewardableAmountSpecified = true,
                             TotalRewardableAmount = rows.TotalRewardableAmount
                         }).ToArray()

                    };
                    var stringwriter = new System.IO.StringWriter();
                    var t = new WsAcumulation.ProcessPurchaseAndPromotionEventRequest();
                    var serializer = new XmlSerializer(t.GetType());
                    serializer.Serialize(stringwriter, request);
                    var dates = stringwriter.ToString();
                    

                    var response = service.ProcessPurchaseAndPromotionEvent(request);

                    //return stringwriter.ToString();
                    return response;

                }
                #endregion


            }
            catch (Exception ex) { return null; }
        }
        public decimal CalcaGet(decimal TotalPuntos, decimal pagado, decimal Porpagar, decimal porcentaje)
        {
            var amountpesos = TotalPuntos * porcentaje;
            if (TotalPuntos > Porpagar)
            {
                amountpesos = Porpagar;
            }
            else
            {
                return amountpesos;
            }
            return amountpesos;
        }
        public AdminPayback GetInfoAdminPayback1(string ws, string type)
        {
            var Admin = _Payback.GetInfoAdminPayback(ws, type);
            return Admin;
        }
        public ListRquestPurchase QueryDatesPurch1(string idstore1, string idVenta, string TypeWs)
        {
            var requestQuery = _Payback.QueryDatesRquestPurchase(idstore1, idVenta, DateTime.Now, TypeWs);
            return requestQuery;
        }
        public ListRquestPurchase QueryDatesPurch1abono(string idstore1, string idVenta, string TypeWs)
        {
            var requestQuery = _Payback.QueryDatesRquestPurchaseAbono(idstore1, idVenta, DateTime.Now, TypeWs);
            return requestQuery;
        }
        public decimal AmountToPoints1(string monto, decimal valuepoints)
        {
            var RequestPoints = _Payback.AmountToPoints(monto, valuepoints);
            return RequestPoints;
        }
        public string GetDescriptionErrorPayback(string code)
        {
            var DrescriptionMessage = _Payback.GetMessageView(code);
            return DrescriptionMessage;
        }
        public bool RegisterPaybackSales(RequestRegisterPaybackSale json)
        {
            //var url = "api/RegisterTransactionPayback";
            //HttpResponseMessage response = _Payback.ReadAsStringAsyncAPI(url, json);
            //response.EnsureSuccessStatusCode();
            //var ResponseRegisterPayback = response.Content.ReadAsAsync<bool>().Result;

            //var JsonRequest = JsonConvert.SerializeObject(json);
            //var ResponseRegisterPayback = _Payback.RegisterPaybackSale(JsonRequest);
            //return ResponseRegisterPayback;
            var ResponseRegisterPayback = GetRegisterTransactionPBK(json);
            return ResponseRegisterPayback;
        }
        public decimal PointsToAmount(decimal AmountPoints, decimal valuePoints)
        {
            decimal ValueInCurrency = 0;
            try
            {
                ValueInCurrency = AmountPoints * valuePoints;
                //if ((ValueInCurrency.ToString().Contains(".")))
                //{
                //    var pointsArray = Convert.ToString(ValueInCurrency).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                //    ValueInCurrency = Convert.ToInt32(pointsArray[0]) + 1;
                //}
            }
            catch (Exception ex)
            {
                return ValueInCurrency;
            }
            return ValueInCurrency;
        }
        public void anade_linea_archivo2(string archivo, string linea)
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback");
            }
            string escribeLog = ConfigurationManager.AppSettings.Get("LOG");
            using (StreamWriter w = System.IO.File.AppendText(archivo))
            {
                //if (escribeLog == "true")
                //{
                w.WriteLine(System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", " ") + " " + linea.Replace(Environment.NewLine, ""));
                w.Flush();
                w.Close();
                //}
            }
        }

        public ReverseRedemptionEventResponse ReverseRedemptionEvent(string Alias, string Principal, string Credential, string PartnerShortName, string BranchShortName, DateTime EfectiveTime, string ReceiptNumber, string RedemptionNumber)
        {

            try
            {
                
                #region Request ProcessPurchaseAndPromotion
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var service = new WsRedemption.ExtintPortTypeClient();
                service.ClientCredentials.UserName.UserName = Principal;//"sie_r_mx";
                service.ClientCredentials.UserName.Password = Credential; //"v3pSuYDy_mdwe6rc";

                using (OperationContextScope scope = new OperationContextScope(service.InnerChannel))
                {
                    var httpRequestProperty = new System.ServiceModel.Channels.HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(service.ClientCredentials.UserName.UserName + ":" +
                                 service.ClientCredentials.UserName.Password));
                    OperationContext.Current.OutgoingMessageProperties[System.ServiceModel.Channels.HttpRequestMessageProperty.Name] = httpRequestProperty;

                    var request = new WsRedemption.ReverseRedemptionEventRequest
                    {

                        ConsumerIdentification = new WsRedemption.ConsumerIdentificationType
                        {
                            ConsumerAuthentication = new WsRedemption.ConsumerAuthenticationType
                            {
                                Principal = Principal,
                                Credential = Credential
                            },
                            Version = WsRedemption.VersionType.Item1
                        },
                        Authentication = new WsRedemption.MemberAliasIdentificationType
                        {
                            Identification = new WsRedemption.MemberIdentificationType
                            {
                                Alias = Alias, //Monedero
                                AliasType = WsRedemption.PrincipalVariantType.Item1
                            }
                        },

                        RedemptionEvent = new WsRedemption.RedemptionEventType
                        {
                            Partner = new WsRedemption.PartnerContextType
                            {
                                PartnerShortName = PartnerShortName, //PartnerShortName
                                BranchShortName = BranchShortName  //BranchShortName
                            },

                            EffectiveTime = EfectiveTime,

                            ReceiptNumber = ReceiptNumber // ReceiptNumber
                        },
                        RedemptionNumber = RedemptionNumber  //RedemptionNumber

                    };
                    //var stringwriter = new System.IO.StringWriter();
                    //var t = new WsRedemption.ReverseRedemptionEventRequest();
                    //var serializer = new XmlSerializer(t.GetType());
                    //serializer.Serialize(stringwriter, request);
                    //var dates = stringwriter.ToString();

                    var response = service.ReverseRedemptionEvent(request);

                    return response;
                }
                #endregion

            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
        }

        public ReverseCollectEventResponse ReverseCollectEvent(string Alias, string Principal, string Credential, string PartnerShortName, string BranchShortName, DateTime EfectiveTime, string ReceiptNumber, string ReferenceReceipNumber)
        {
            try
            {
                #region Request ReverseCollectEvent
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                var service = new WsAcumulation.ExtintPortTypeClient();
                service.ClientCredentials.UserName.UserName = Principal;//"sie_r_mx";
                service.ClientCredentials.UserName.Password = Credential; //"v3pSuYDy_mdwe6rc";

                using (OperationContextScope scope = new OperationContextScope(service.InnerChannel))
                {
                    var httpRequestProperty = new System.ServiceModel.Channels.HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                 Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(service.ClientCredentials.UserName.UserName + ":" +
                                 service.ClientCredentials.UserName.Password));
                    OperationContext.Current.OutgoingMessageProperties[System.ServiceModel.Channels.HttpRequestMessageProperty.Name] = httpRequestProperty;

                    var request = new WsAcumulation.ReverseCollectEventRequest
                    {

                        ConsumerIdentification = new WsAcumulation.ConsumerIdentificationType
                        {
                            ConsumerAuthentication = new WsAcumulation.ConsumerAuthenticationType
                            {
                                Principal = Principal,
                                Credential = Credential
                            },
                            Version = WsAcumulation.VersionType.Item1
                        },
                        Authentication = new WsAcumulation.MemberAliasIdentificationType
                        {
                            Identification = new WsAcumulation.MemberIdentificationType
                            {
                                Alias = Alias, //Monedero
                                AliasType = WsAcumulation.PrincipalVariantType.Item1
                            }
                        },

                        CollectEventData = new WsAcumulation.CollectEventType
                        {
                            Partner = new WsAcumulation.PartnerContextType
                            {
                                PartnerShortName = PartnerShortName, //PartnerShortName
                                BranchShortName = BranchShortName  //BranchShortName
                            },

                            EffectiveDate = EfectiveTime,
                            ReceiptNumber = ReceiptNumber,
                            CommunicationChannel = "200"
                        },

                        ReferenceReceiptNumber = ReferenceReceipNumber  //ReceiptNumber
                    };
                    var response = service.ReverseCollectEvent(request);
                    return response;
                }
                #endregion


            }
            catch (Exception ex) { return null; }
        }
        public bool GetRegisterTransactionPBK(RequestRegisterPaybackSale Transaction1)
        {
            StringBuilder UserQuery = new StringBuilder();
            StringBuilder UserQueryDet = new StringBuilder();
            //RequestRegisterPaybackSale Transaction1 = null;
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

                if (Transaction1 != null)
                {
                    //Transaction1 = JsonConvert.DeserializeObject<RequestRegisterPaybackSale>(TPBK);
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
                                Squeyupdate = "Update Dat_TransactionPayback SET EfectiveTime = Getdate(), ReceiptNumber = '" + Transaction1.ReceipNumber + "' , LegalAmountRLegalV = '" + Transaction1.LegalAmountRLegalV + "' , LoyaltyAmountRLoyalV = '" + Transaction1.LoyaltyAmountRLoyalV + "' , StatusTrans = " + Status + " WHERE idTransaction = " + idtransaction;
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
                    else
                    {
                        var Squey = "Update VentasPagos SET TotalPuntosPayback = " + Transaction1.TotalPuntosPayback + " , PuntosPaybackAcumulados = " + Transaction1.PuntosPaybackAcumulados + " , PuntosPaybackRedimidos = " + Transaction1.PuntosPaybackRedimidos + " WHERE ID = " + Transaction1.Idabono + "";
                        var dtpbk = objPbk.EjecutaQry(Squey, CommandType.Text, connstringWEB, sError);
                    }

                }

            }
            catch (Exception ex) { return false; }
            return true;
        }

    }
}
