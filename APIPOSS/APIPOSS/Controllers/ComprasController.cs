using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Http.Results;
using APIPOSS.Models.Compras;
using System.Configuration;
using APIPOSS.Utilities;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Data;
using Microsoft.VisualBasic;
using System.Web.Http;
namespace APIPOSS.Controllers
{
    public class ComprasController : ApiController
    {
        returncompra infocompra = new returncompra();
        private string connWEB;
        // GET: Compras
        [System.Web.Http.Route("api/AddPurchase")]
        [System.Web.Http.HttpPost]
        public JsonResult<AddPurchase> AddPurchase(AddPurchase Compra)
        {
            try
            {
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                if (realizaCompra(Compra) == true)
                {
                    Utilities.GeneralClass objlog = new Utilities.GeneralClass();
                    objlog.InsertaLog(Tipos.CO, infocompra.IDPRINTCOMPRA, infocompra.IDSTORE, infocompra.WHSID);
                    AddPurchase ResponseCo = new AddPurchase();
                    ResponseCo.statusresponse = true;
                    ResponseCo.Idcompra = infocompra.IDPRINTCOMPRA.ToString();
                    return Json<AddPurchase>(ResponseCo);
                }
                else
                {
                    AddPurchase ResponseCo = new AddPurchase();
                    ResponseCo.statusresponse = false;
                    return Json<AddPurchase>(ResponseCo);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public bool realizaCompra(AddPurchase AddCompra)
        {
            object fila;
            int counter = 0;
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            string sQuery;
            string sQueryInsert = "";
            string sQueryInsert2 = "";
            int IDTienda = Convert.ToInt32(AddCompra.Idstore);
            int IDUsuario = Convert.ToInt32(AddCompra.Idusuario);
            string IdCompraWEB = "";
            int ret;
            var DEFAULTLIST = AddCompra.DEFAULTLIST;
            try
            {
                // Se verifica que el grid con el detalle de la orden de compra contenga datos
                if (AddCompra.ArrayArticulos.Count > 0)
                {
                    if (IDTienda < 1)
                    {
                        //nothing
                    }
                    else
                    {
                        // Revisamos si los articulos estan creados en web
                        foreach (var row in AddCompra.ArrayArticulos)
                        {
                            int codeBarCount = 0;
                            int IDCategoria = 0;
                            string ArticuloSBO = "";
                            string DescripcionSBO = "";
                            counter = counter + 1;

                            ArticuloSBO = row.Linea; //codigo
                            DescripcionSBO = row.Modelo; //descripcion

                            // Revisamos si el artículo esta creado en WEB
                            sQuery = "select IdArticulo from articulos " + Environment.NewLine + "where ArticuloSBO='" + ArticuloSBO + "' ";
                            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Articulos", connWEB);
                            if (dt.Rows.Count == 0)
                            {
                                // Si no hay registros lo insertamos
                                sQuery = "INSERT INTO Articulos (ArticuloSBO) " + Environment.NewLine + "VALUES ('" + ArticuloSBO + "')";

                                ret = oDB.EjecutaQry(sQuery, CommandType.Text, connWEB, "");
                            }
                            // Creamos el documento de Compra en web
                            // Creamos primero el encabezado, solo se crea una vez
                            if (counter == 1)
                            {
                                foreach (var rowHeader in AddCompra.ArrayArticulos)
                                {
                                    string fechaEnvio;
                                    fechaEnvio = Strings.Format(DateTime.Now, "dd/MM/yyyy").ToString();
                                    sQuery = "set dateformat dmy INSERT INTO CompraEncabezado (FechaComWeb, Status, IDUsuario) " + Environment.NewLine + "VALUES (getdate(),'O','" + IDUsuario + "')";
                                    oDB.EjecutaQry(sQuery, CommandType.Text, connWEB, "");
                                    // Obtenemos el id del documento de Compra creado en web
                                    sQuery = "SELECT   top 1  IDCompra " + Environment.NewLine + "FROM         CompraEncabezado order by IDCompra desc ";
                                    dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "ComprasOCLastID", connWEB);
                                    foreach (DataRow Drow in dt.Rows)
                                    {
                                        IdCompraWEB = Drow["IDCompra"].ToString();
                                        break;
                                    }
                                    break;
                                }
                            }

                            sQueryInsert += "INSERT INTO CompraDetalle (IDCompra, Linea, Articulo, Cantidad, Coments, IDTienda, Status,DefaultList) " + Environment.NewLine + "VALUES ('" + IdCompraWEB + "','" + counter + "',(Select IDArticulo from articulos where ArticuloSBO ='" + ArticuloSBO + "'),'" + row.Cantidad + "','" + row.comentarios + "','" + IDTienda + "','O','" + DEFAULTLIST + "')" + Environment.NewLine;
                        }
                        // Si llegamos a este punto podemos crear el detalle de la Compra
                        ret = oDB.EjecutaQry(sQueryInsert, CommandType.Text, connWEB, "");


                        int contadorDetOC = 0;
                        sQuery = "" + "SELECT      Articulo, Cantidad, IDTienda  " + Environment.NewLine + "FROM         CompraDetalle " + Environment.NewLine + "where IDCompra = " + IdCompraWEB + " " + Environment.NewLine + " ";
                        dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "CheckSeries", connWEB);
                        sQuery = "";
                        sQueryInsert = "";
                        if (dt.Rows.Count > 0)
                        {

                            //mensaje = "Operación realizada con Éxito.";
                            //InsertaLog(Tipos.CO, IdCompraWEB, Session("IDSTORE"), Session("WHSID"));
                        }
                    }
                    var idCompra = IdCompraWEB;
                    int IDPRINTCOMPRA = Convert.ToInt32(idCompra);
                    int IDSTORE = Convert.ToInt32(AddCompra.Idstore);
                    var WHSID = AddCompra.WhsID;
                    returncompra infoCompra = new returncompra(IDPRINTCOMPRA, IDSTORE, WHSID);
                    infocompra = infoCompra;

                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [System.Web.Http.Route("api2/GetReportsReimpNomina")]
        [System.Web.Http.HttpPost]
        public List<ReimpresionComprasView> GetReportsReimpNomina(ParametersReqReimpCO ObjCom)
        {
            DataSet daDepostis = null/* TODO Change to default(_) if this is not a reference type */;
            string squery = "";
            string strWhere = "";
            string strWhere2 = "";
            Utilities.admin Master = new Utilities.admin();
            try
            {
                if (ObjCom.IdStore == "-1")
                {
                    strWhere2 = " and cd.IDTIENDA IN  " + Environment.NewLine + "   (SELECT ADS.AdminStoreID " + Constants.vbCrLf + "   FROM AdminStore ADS " + Environment.NewLine + "   LEFT JOIN UserStores US ON US.AdminStoreID = ADS.AdminStoreID AND US.AdminUserID = " + ObjCom.AdminUserID + Constants.vbCrLf + "   WHERE DefaultCustomer='" + ObjCom.SesionFRCARDCODE + "') ";
                }
                else
                {
                    strWhere2 = " and cd.idTienda = '" + ObjCom.IdStore + "'";
                }

                if (!string.IsNullOrEmpty(ObjCom.FechaIN) && !string.IsNullOrEmpty(ObjCom.FechaFin))
                {
                    strWhere = " and cast(ce.FechaComWeb as date) between '" + Convert.ToDateTime(ObjCom.FechaIN).ToString("yyyyMMdd") + "' and '" + Convert.ToDateTime(ObjCom.FechaFin).ToString("yyyyMMdd") + "' ";
                }
                else if (!string.IsNullOrEmpty(ObjCom.FechaIN) && string.IsNullOrEmpty(ObjCom.FechaFin))//Session("RTVTFI").ToString <> "" And Session("RTVTFF").ToString = ""
                {
                    strWhere = " and cast(ce.FechaComWeb as date) between '" + Convert.ToDateTime(ObjCom.FechaIN) + "' and '" + Convert.ToDateTime(ObjCom.FechaIN) + "' ";
                }
                else if (string.IsNullOrEmpty(ObjCom.FechaIN) && !string.IsNullOrEmpty(ObjCom.FechaFin))
                {
                    strWhere = " and cast(ce.FechaComWeb as date) between '" + Convert.ToDateTime(ObjCom.FechaFin).ToString("yyyyMMdd") + "' and '" + Convert.ToDateTime(ObjCom.FechaFin).ToString("yyyyMMdd") + "' ";
                }
                squery = "set dateformat dmy  " + Environment.NewLine + "SELECT     EntradaOCDet.IDEntrada as [Idinterno]," + Environment.NewLine + "(SELECT DocNum FROM  " + ObjCom.SesionSAPDB + ".OINV AS OINV WHERE (DocEntry = EntradaOCDet.NoDocSBO)) AS [Documento SAP]," + Environment.NewLine + "(SELECT numatcard FROM  " + ObjCom.SesionSAPDB + ".OINV AS OINV WHERE (DocEntry = EntradaOCDet.NoDocSBO)) AS [Referencia SAP]," + Environment.NewLine + "(SELECT ItemName FROM " + ObjCom.SesionSAPDB + ".OITM AS OITM WHERE (ItemCode = Articulos.ArticuloSBO)) AS Artículo, " + Environment.NewLine + "EntradaOCDet.Cantidad, EntradaOCDet.LineaSBO as [Linea SAP], cast(EntradasOC.FechaSBO as date) as [Fecha SAP], cast(EntradasOC.FechaEntWeb as date) as [Fecha Web], EntradasOC.SNSBO as [Socio de negocios],  " + Environment.NewLine + "(AdminUser.FirstName + ' ' + AdminUser.LastName) as Usuario, AdminStore.StoreName AS Tienda" + Environment.NewLine + ",inv1.U_FOLPOS as 'Folio POS' " + Environment.NewLine + "FROM         EntradaOCDet INNER JOIN" + Environment.NewLine + "                      EntradasOC ON EntradaOCDet.IDEntrada = EntradasOC.IDEntrada INNER JOIN" + Environment.NewLine + "                      Articulos ON EntradaOCDet.Articulo = Articulos.IDArticulo INNER JOIN" + Environment.NewLine + "                      AdminUser ON EntradasOC.IDUsuario = AdminUser.AdminUserID INNER JOIN" + Environment.NewLine + "                      AdminStore ON EntradaOCDet.idTienda = AdminStore.AdminStoreID " + Environment.NewLine + "		INNER JOIN     " + ObjCom.SesionSAPDB + ".inv1 on EntradaOCDet.nodocsbo = inv1.docentry    and inv1.linenum = EntradaOCDet.lineasbo" + Environment.NewLine + "WHERE 1 = 1 " + strWhere + strWhere2 + Environment.NewLine + "and storetypeid <> 3 and inv1.U_FOLPOS like '%OCPOS%'" + Environment.NewLine + "ORDER BY EntradaOCDet.IDEntrada, EntradaOCDet.LineaSBO" + Environment.NewLine + "" + Environment.NewLine + "" + Environment.NewLine + "" + Environment.NewLine + "";


                squery = "";
                squery = squery + " ce.IDCompra as [Idinterno]";
                squery = squery + " ,isnull((select cast(DocNum as varchar) from " + ObjCom.SesionSAPDB + ".inv1," + ObjCom.SesionSAPDB + ".oinv where oinv.DocEntry = inv1.DocEntry and U_DOCPOS = 'Compra' and U_IDDOCPOS=ce.idcompra and U_LINPOS=cd.Linea),'No Facturado') AS [Documento SAP]";
                squery = squery + " ,isnull((select cast(numatcard as varchar) from " + ObjCom.SesionSAPDB + ".inv1," + ObjCom.SesionSAPDB + ".oinv where oinv.DocEntry = inv1.DocEntry and U_DOCPOS = 'Compra' and U_IDDOCPOS=ce.idcompra and U_LINPOS=cd.Linea),'No Facturado') AS [Referencia SAP]";
                squery = squery + " ,(SELECT ItemName FROM " + ObjCom.SesionSAPDB + ".OITM AS OITM WHERE (ItemCode = (select ArticuloSBO from Articulos where IDArticulo=cd.Articulo))) AS Artículo";
                squery = squery + " ,cd.Cantidad";
                squery = squery + " ,isnull((select cast(LineNum + 1 as varchar) from " + ObjCom.SesionSAPDB + ".inv1 where U_DOCPOS = 'Compra' and U_IDDOCPOS=ce.idcompra and U_LINPOS=cd.Linea),'No Facturado') AS [Linea SAP]";
                squery = squery + " ,(select oinv.DocDate from " + ObjCom.SesionSAPDB + ".inv1," + ObjCom.SesionSAPDB + ".oinv where oinv.DocEntry = inv1.DocEntry and U_DOCPOS = 'Compra' and U_IDDOCPOS=ce.idcompra and U_LINPOS=cd.Linea) AS [Fecha SAP]";
                squery = squery + " ,ce.FechaComWeb as [Fecha Web]";
                squery = squery + " ,(select oinv.CardCode from " + ObjCom.SesionSAPDB + ".inv1," + ObjCom.SesionSAPDB + ".oinv where oinv.DocEntry = inv1.DocEntry and U_DOCPOS = 'Compra' and U_IDDOCPOS=ce.idcompra and U_LINPOS=cd.Linea) AS [Socio de negocios]";
                squery = squery + " ,(select AdminUser.FirstName + ' ' + AdminUser.LastName from AdminUser where AdminUserID=ce.IDUsuario) as Usuario";
                squery = squery + " ,(select StoreName from AdminStore where AdminStoreID=cd.IDTienda) as Tienda";
                squery = squery + " ,(select oinv.U_FOLPOS from " + ObjCom.SesionSAPDB + ".inv1," + ObjCom.SesionSAPDB + ".oinv where oinv.DocEntry = inv1.DocEntry and U_DOCPOS = 'Compra' and U_IDDOCPOS=ce.idcompra and U_LINPOS=cd.Linea) as [Folio POS]";
                squery = squery + " from";
                squery = squery + " CompraEncabezado Ce, CompraDetalle cd";
                squery = squery + " where ce.IDCompra = cd.IDCompra " + strWhere + strWhere2;

                squery = "" + "drop table #tempPUR" + Environment.NewLine + "SELECT " + Environment.NewLine + " ce.IDCompra as [Idinterno] " + Environment.NewLine + " ,cast(DocNum as varchar) AS [Documento SAP] " + Environment.NewLine + " ,cast(numatcard as varchar) AS [Referencia SAP] " + Environment.NewLine + " ,OITM.ItemName AS Artículo" + Environment.NewLine + " ,CD.Cantidad" + Environment.NewLine + " ,CAST(INV1.LineNum + 1 as varchar) AS [Linea SAP] " + Environment.NewLine + " ,OINV.DocDate AS [Fecha SAP]" + Environment.NewLine + " ,CE.FechaComWeb as [Fecha Web] " + Environment.NewLine + " ,OINV.CardCode AS [Socio de negocios]" + Environment.NewLine + " ,(ADU.FirstName + ' ' + ADU.LastName) AS Usuario" + Environment.NewLine + " ,ADS.StoreName AS Tienda" + Environment.NewLine + " ,OINV.U_FOLPOS  " + Environment.NewLine + " into #tempPUR" + Environment.NewLine + "FROM " + Environment.NewLine + "DORMIMUNDO_PRODUCTIVA.DBO.OINV OINV INNER JOIN" + Environment.NewLine + "DORMIMUNDO_PRODUCTIVA.DBO.INV1 INV1 ON INV1.DocEntry=OINV.DocEntry AND INV1.U_DOCPOS='Compra' INNER JOIN" + Environment.NewLine + "CompraDetalle CD ON CD.IDCompra=INV1.U_IDDOCPOS AND CD.Linea=INV1.U_LINPOS INNER JOIN" + Environment.NewLine + "CompraEncabezado CE ON CE.IDCompra=CD.IDCompra INNER JOIN" + Environment.NewLine + "DORMIMUNDO_PRODUCTIVA.DBO.OITM OITM ON OITM.ItemCode=INV1.ItemCode INNER JOIN" + Environment.NewLine + "AdminUser ADU ON ADU.AdminUserID=CE.IDUsuario INNER JOIN" + Environment.NewLine + "AdminStore ADS ON ADS.AdminStoreID=CD.IDTienda " + Environment.NewLine + "WHERE" + Environment.NewLine + "1 = 1  " + strWhere + strWhere2 + Environment.NewLine + "" + Environment.NewLine + "SELECT * FROM #tempPUR" + Environment.NewLine + "UNION" + Environment.NewLine + "select  " + Environment.NewLine + " CE.IDCompra as [Idinterno] " + Environment.NewLine + " ,'No Facturado' AS [Documento SAP] " + Environment.NewLine + " ,'No Facturado' AS [Referencia SAP] " + Environment.NewLine + " ,OITM.ItemName  AS Artículo " + Environment.NewLine + " ,CD.Cantidad " + Environment.NewLine + " ,0 AS [Linea SAP] " + Environment.NewLine + " ,GETDATE() AS [Fecha SAP] " + Environment.NewLine + " ,CE.FechaComWeb as [Fecha Web] " + Environment.NewLine + " ,ADS.DefaultCustomer AS [Socio de negocios] " + Environment.NewLine + " ,(ADU.FirstName + ' ' + ADU.LastName) AS Usuario" + Environment.NewLine + " ,ADS.StoreName AS Tienda" + Environment.NewLine + " ,'OCPOS' + CAST(CE.IDCompra AS VARCHAR)  as [Folio POS]  " + Environment.NewLine + "FROM " + Environment.NewLine + "CompraEncabezado CE INNER JOIN" + Environment.NewLine + "CompraDetalle CD ON CD.IDCompra=CE.IDCompra INNER JOIN" + Environment.NewLine + "AdminUser ADU ON ADU.AdminUserID=CE.IDUsuario INNER JOIN" + Environment.NewLine + "AdminStore ADS ON ADS.AdminStoreID=CD.IDTienda INNER JOIN " + Environment.NewLine + "Articulos ART ON ART.IDArticulo=CD.Articulo INNER JOIN" + Environment.NewLine + "DORMIMUNDO_PRODUCTIVA.DBO.OITM OITM ON OITM.ItemCode=ART.ArticuloSBO" + Environment.NewLine + "WHERE " + Environment.NewLine + "CE.IDCompra NOT IN (SELECT Idinterno  FROM #tempPUR)" + Environment.NewLine + strWhere + strWhere2 + Environment.NewLine + "";

                daDepostis = Master.DBConn.GetQuerydts(squery);
                daDepostis = Master.DBConn.GetQuerydts(squery);

                if (daDepostis != null)
                {

                    var ls = (from DataRow rows in daDepostis.Tables[0].Rows
                              select new ReimpresionComprasView
                              {
                                  IdInterno = rows["IdInterno"] is DBNull ? 0 : (int)rows["IdInterno"],
                                  DocumentoSAP = rows["Documento SAP"] is DBNull ? "" : (string)(rows["Documento SAP"]),
                                  ReferenciaSAP = rows["Referencia SAP"] is DBNull ? "" : (string)rows["Referencia SAP"],
                                  Articulo = rows["Artículo"] is DBNull ? "" : (string)rows["Artículo"],
                                  Cantidad = rows["Cantidad"] is DBNull ? 0 : (Decimal)(rows["Cantidad"]),
                                  LineaSAP = rows["Linea SAP"] is DBNull ? 0 : Convert.ToInt32(rows["Linea SAP"]),
                                  FechaSAP = rows["Fecha SAP"] is DBNull ? DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") : Convert.ToDateTime(rows["Fecha SAP"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                  FechaWEB = rows["Fecha WEB"] is DBNull ? DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") : Convert.ToDateTime(rows["Fecha WEB"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                  Socio = rows["Socio de negocios"] is DBNull ? "" : (string)rows["Socio de negocios"],
                                  Usuario = rows["Usuario"] is DBNull ? "" : (string)rows["Usuario"],
                                  Tienda = rows["Tienda"] is DBNull ? "" : (string)rows["Tienda"],
                                  U_FOLPOS = rows["U_FOLPOS"] is DBNull ? "" : (string)rows["U_FOLPOS"],
                                  btnDownloadcompra = "<button type='button' onclick='GetReimprimeCOmpras("+ (rows["IdInterno"] is DBNull ? 0 : (int)rows["IdInterno"]) + ")' name='Reimprimir' class='btn btn-info btn-sm '>" +
                                            "<span class='fa fa-download'></span> Reimprimir</button>"
                              }).ToList();

                    return (ls);
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