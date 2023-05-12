using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebPOS.Utilities
{
    public class DBMaster
    {
        private string sServer;
        private string sDB;
        private string sUser;
        private string sPass;
        private bool bTrusted;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private SqlConnection BD_SQL;
        private SqlCommand cmd_SQL;
        private SqlDataAdapter dta_SQL;
        private DataSet dts_SQL;
        public DataTableCollection[] dtc_SQL;
        public int int_SQL;

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public object Server
        {
            get
            {
                object ServerRet = default;
                ServerRet = sServer;
                return ServerRet;
            }

            set
            {
                sServer = value.ToString();
            }
        }

        public object DB
        {
            get
            {
                object DBRet = default;
                DBRet = sDB;
                return DBRet;
            }

            set
            {
                sDB = value.ToString();
            }
        }

        public object User
        {
            get
            {
                object UserRet = default;
                UserRet = sUser;
                return UserRet;
            }

            set
            {
                sUser = value.ToString();
            }
        }

        public object Pass
        {
            get
            {
                object PassRet = default;
                PassRet = sPass;
                return PassRet;
            }

            set
            {
                sPass = value.ToString();
            }
        }

        public Boolean TrustedConection
        {
            get
            {
                return bTrusted;
            }

            set
            {
                bTrusted = value;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public enum Visible
        {
            NO = 0,
            SI = 1
        }

        public enum Editable
        {
            NO = 0,
            SI = 1
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        public string GetConnString()
        {
            string GetConnStringRet = string.Empty;
            // ******************************************************************************
            // Crear cadena de Conexión
            // ******************************************************************************
            try
            {
                if (TrustedConection == false)
                {
                    GetConnStringRet = "Data Source=" + sServer.Trim().ToString() + ";Initial Catalog=" + sDB.Trim().ToString() + ";" + "User ID=" + sUser.Trim().ToString() + ";Password=" + sPass.Trim().ToString() + ";" + "Persist Security Info=True; Packet Size=4096";

                }
                else
                {
                    GetConnStringRet = "Data Source=" + sServer.Trim().ToString() + ";Initial Catalog=" + sDB.Trim().ToString() + ";Integrated Security=True";
                }
            }
            catch (Exception ex)
            {
            }

            return GetConnStringRet;
        }
        public Boolean ConectaDBConnString(string conn)
        {
            bool ConectaDBConnStringRet = true;

            // ******************************************************************************
            // Función para realizar conexiones a BD SQL
            // ******************************************************************************

            string strConexion = "";
            // ******************************************************************************
            // Crear cadena de Conexión
            // ******************************************************************************
            strConexion = conn;
            // ******************************************************************************
            // Crea una nueva conexión
            // ******************************************************************************
            BD_SQL = new SqlConnection();
            try
            {
                // ******************************************************************************
                // Establece la ruta de la BD
                // ******************************************************************************
                BD_SQL.ConnectionString = strConexion;

                // ******************************************************************************
                // Abre la conexión
                // ******************************************************************************
                BD_SQL.Open();
                DB = BD_SQL.Database;


                // ******************************************************************************
                // Verificar si se realizo la conexion  
                // ******************************************************************************
                if (BD_SQL.State == ConnectionState.Closed)
                {
                    throw new Exception("No se realizo la conexión a la BD");
                }

                char[] str = { ';' };

                //Server = BD_SQL.ConnectionString.Split(";")(0).Split("=")(1);
                Server = BD_SQL.ConnectionString.Split(str);

                return true;
            }
            catch (Exception ex)
            {
                ConectaDBConnStringRet = false;
                //if (Information.Err().Number.ToString() != "-2147217843")
                //{
                //    throw new Exception("Error de conexión a la BD." + Constants.vbCrLf + "Error: " + Information.Err().Number.ToString() + " " + Information.Err().Description);
                //}
                //else if (Conversions.ToDouble(Information.Err().Number.ToString()) == 5d)
                //{
                //    throw new Exception("Error la contraseña a expirado." + Constants.vbCrLf + "Error: " + Information.Err().Number.ToString() + " " + Information.Err().Description);
                //}
            }

            ConectaDBConnStringRet = true;
            return ConectaDBConnStringRet;
        }
        public DataTable EjecutaQry_Tabla(string strSentencia, CommandType sTipoAccion, string sNmbTabla, string conn)
        {
            DataTable EjecutaQry_TablaRet = default;
            // ******************************************************************************
            // Ejecuta Sentencias BD SQL que Extraen Datos
            // ******************************************************************************

            EjecutaQry_TablaRet = new DataTable();
            try
            {
                // ******************************************************************************
                // Conexion a BD
                // ******************************************************************************

                string sErr = "";
                ConectaDBConnString(conn);
                if (!string.IsNullOrEmpty(sErr))
                {
                    //MsgBox(sErr);
                }

                if (BD_SQL.State != ConnectionState.Open)
                    return EjecutaQry_TablaRet;

                // ******************************************************************************
                // Inicializa el comando
                // ******************************************************************************
                cmd_SQL = new SqlCommand();

                // ******************************************************************************
                // Indicar conexión, Nmb SP e Indicar q se Ejecutará un SP
                // ******************************************************************************
                cmd_SQL.CommandText = strSentencia;
                cmd_SQL.CommandType = sTipoAccion;
                cmd_SQL.Connection = BD_SQL;
                cmd_SQL.CommandTimeout = 0;

                // **************************************************************************************
                // Se recuperan los datos del SP de 1 DataAdapter a DataSet
                // **************************************************************************************
                dta_SQL = new SqlDataAdapter(cmd_SQL);
                dts_SQL = new DataSet();
                dta_SQL.Fill(dts_SQL, sNmbTabla);
                EjecutaQry_TablaRet = dts_SQL.Tables[sNmbTabla];
                EjecutaQry_TablaRet.TableName = sNmbTabla;
            }
            catch (Exception ex)
            {
            }

            cmd_SQL.Dispose();
            CierraBD();
            return EjecutaQry_TablaRet;
        }

        public DataTable EjecutaQry_Tabla(string strSentencia, List<SqlParameter> paramerers, CommandType sTipoAccion, string sNmbTabla, string conn)
        {
            DataTable EjecutaQry_TablaRet = default;
            // ******************************************************************************
            // Ejecuta Sentencias BD SQL que Extraen Datos
            // ******************************************************************************

            EjecutaQry_TablaRet = new DataTable();
            try
            {
                // ******************************************************************************
                // Conexion a BD
                // ******************************************************************************

                string sErr = "";
                ConectaDBConnString(conn);
                if (!string.IsNullOrEmpty(sErr))
                {
                    //MsgBox(sErr);
                }

                if (BD_SQL.State != ConnectionState.Open)
                    return EjecutaQry_TablaRet;

                // ******************************************************************************
                // Inicializa el comando
                // ******************************************************************************
                cmd_SQL = new SqlCommand();

                // ******************************************************************************
                // Indicar conexión, Nmb SP e Indicar q se Ejecutará un SP
                // ******************************************************************************
                cmd_SQL.CommandText = strSentencia;
                cmd_SQL.CommandType = sTipoAccion;
                cmd_SQL.Parameters.AddRange(paramerers.ToArray());
                cmd_SQL.Connection = BD_SQL;
                cmd_SQL.CommandTimeout = 0;

                // **************************************************************************************
                // Se recuperan los datos del SP de 1 DataAdapter a DataSet
                // **************************************************************************************
                dta_SQL = new SqlDataAdapter(cmd_SQL);
                dts_SQL = new DataSet();
                dta_SQL.Fill(dts_SQL, sNmbTabla);
                EjecutaQry_TablaRet = dts_SQL.Tables[sNmbTabla];
                EjecutaQry_TablaRet.TableName = sNmbTabla;
            }
            catch (Exception ex)
            {
            }

            cmd_SQL.Dispose();
            CierraBD();
            return EjecutaQry_TablaRet;
        }

        public DataTable EjecutaQry_Tabla(string strSentencia, List<SqlParameter> paramerers, CommandType sTipoAccion, string conn)
        {
            DataTable EjecutaQry_TablaRet = default;
            // ******************************************************************************
            // Ejecuta Sentencias BD SQL que Extraen Datos
            // ******************************************************************************

            EjecutaQry_TablaRet = new DataTable();
            try
            {
                // ******************************************************************************
                // Conexion a BD
                // ******************************************************************************

                string sErr = "";
                ConectaDBConnString(conn);
                if (!string.IsNullOrEmpty(sErr))
                {
                    //MsgBox(sErr);
                }

                if (BD_SQL.State != ConnectionState.Open)
                    return EjecutaQry_TablaRet;

                // ******************************************************************************
                // Inicializa el comando
                // ******************************************************************************
                cmd_SQL = new SqlCommand();

                // ******************************************************************************
                // Indicar conexión, Nmb SP e Indicar q se Ejecutará un SP
                // ******************************************************************************
                cmd_SQL.CommandText = strSentencia;
                cmd_SQL.CommandType = sTipoAccion;
                if (paramerers != null)
                    cmd_SQL.Parameters.AddRange(paramerers.ToArray());
                cmd_SQL.Connection = BD_SQL;
                cmd_SQL.CommandTimeout = 0;

                // **************************************************************************************
                // Se recuperan los datos del SP de 1 DataAdapter a DataSet
                // **************************************************************************************
                dta_SQL = new SqlDataAdapter(cmd_SQL);
                dts_SQL = new DataSet();
                dta_SQL.Fill(dts_SQL);
                EjecutaQry_TablaRet = dts_SQL.Tables[0];
            }
            catch (Exception ex)
            {
            }

            cmd_SQL.Dispose();
            CierraBD();
            return EjecutaQry_TablaRet;
        }
        public int EjecutaQry_Tabl(string strSentencia, string returnValue, List<SqlParameter> paramerers, CommandType sTipoAccion, string conn)
        {
            DataTable EjecutaQry_TablaRet = default;
            // ******************************************************************************
            // Ejecuta Sentencias BD SQL que Extraen Datos
            // ******************************************************************************
            int EjecutaQry = 0;
            int rowsAffected;
            EjecutaQry_TablaRet = new DataTable();
            try
            {
                // ******************************************************************************
                // Conexion a BD
                // ******************************************************************************

                string sErr = "";
                ConectaDBConnString(conn);
                if (!string.IsNullOrEmpty(sErr))
                {
                    //MsgBox(sErr);
                }

                if (BD_SQL.State != ConnectionState.Open)
                    return EjecutaQry = 2;

                // ******************************************************************************
                // Inicializa el comando
                // ******************************************************************************
                cmd_SQL = new SqlCommand();

                // ******************************************************************************
                // Indicar conexión, Nmb SP e Indicar q se Ejecutará un SP
                // ******************************************************************************
                cmd_SQL.CommandText = strSentencia;
                cmd_SQL.CommandType = sTipoAccion;
                cmd_SQL.Parameters.AddRange(paramerers.ToArray());
                cmd_SQL.Connection = BD_SQL;
                cmd_SQL.CommandTimeout = 0;

                if (returnValue != "")
                {

                    cmd_SQL.Parameters.Add(returnValue, SqlDbType.Int).Direction = ParameterDirection.Output;
                    rowsAffected = cmd_SQL.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string sqlIdentity = "SELECT @@IDENTITY";
                        SqlCommand cmdIdentity = new SqlCommand(sqlIdentity, BD_SQL);

                        return Convert.ToInt32(cmdIdentity.ExecuteScalar());
                    }
                    else
                        return -1;
                }
                else
                {
                    return rowsAffected = cmd_SQL.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return EjecutaQry = -1;
            }

            cmd_SQL.Dispose();
            CierraBD();
            return EjecutaQry;
        }
        public int EjecutaQry(string stNmbSentencia, CommandType sTipoAccion, string sconn, string sDError)
        {
            sDError = "";
            int EjecutaQry;
            // ******************************************************************************
            // Ejecuta Sentencias BD SQL que NO Extraen Datos
            // ******************************************************************************
            EjecutaQry = 0;
            try
            {
                // ******************************************************************************
                // Conexion a BD
                // ******************************************************************************
                string sErr = "";

                ConectaDBConnString(sconn);

                if (BD_SQL.State != ConnectionState.Open)
                    return EjecutaQry;

                // ******************************************************************************
                // Inicializa comando
                // ******************************************************************************
                cmd_SQL = new SqlCommand();

                cmd_SQL.CommandTimeout = 0;
                cmd_SQL.CommandText = stNmbSentencia;
                cmd_SQL.CommandType = sTipoAccion;
                cmd_SQL.Connection = BD_SQL;

                // ******************************************************************************
                // Si el resultado pasa al Recorset
                // ******************************************************************************
                cmd_SQL.ExecuteNonQuery();

                // ******************************************************************************
                // Quita Obj de Memoria
                // ******************************************************************************
                cmd_SQL.Dispose();
                CierraBD();

                return EjecutaQry = 1;
            }
            catch (Exception ex)
            {
                sDError = ex.Message.ToString();
                return EjecutaQry = 2;
            }
        }
        public bool CierraBD()
        {
            bool CierraBDRet = default;
            // ******************************************************************************
            // Función para desconexion de BD SQL
            // ******************************************************************************
            // Cierra la conexión
            if (BD_SQL.State == ConnectionState.Closed)
            {
                BD_SQL.Close();
                BD_SQL.Dispose();
                BD_SQL = null;
                GC.Collect();
            }

            CierraBDRet = true;
            return CierraBDRet;
        }

        public int EjecutaQry_T(string strSentencia, string returnValue, SqlParameter paramerers, CommandType sTipoAccion, string conn)
        {
            DataTable EjecutaQry_TablaRet = default;
            // ******************************************************************************
            // Ejecuta Sentencias BD SQL que Extraen Datos 
            // ******************************************************************************
            int EjecutaQry = 0;
            int rowsAffected;
            EjecutaQry_TablaRet = new DataTable();
            try
            {
                // ******************************************************************************
                // Conexion a BD
                // ******************************************************************************

                string sErr = "";
                ConectaDBConnString(conn);
                if (!string.IsNullOrEmpty(sErr))
                {
                    //MsgBox(sErr);
                }

                if (BD_SQL.State != ConnectionState.Open)
                    return EjecutaQry = 2;

                // ******************************************************************************
                // Inicializa el comando
                // ******************************************************************************
                cmd_SQL = new SqlCommand();

                // ******************************************************************************
                // Indicar conexión, Nmb SP e Indicar q se Ejecutará un SP
                // ******************************************************************************
                cmd_SQL.CommandText = strSentencia;
                cmd_SQL.CommandType = sTipoAccion;
                cmd_SQL.Parameters.Add(paramerers);
                cmd_SQL.Connection = BD_SQL;
                cmd_SQL.CommandTimeout = 0;

                if (returnValue != "")
                {

                    cmd_SQL.Parameters.Add(returnValue, SqlDbType.Int).Direction = ParameterDirection.Output;
                    rowsAffected = cmd_SQL.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string sqlIdentity = "SELECT @@IDENTITY";
                        SqlCommand cmdIdentity = new SqlCommand(sqlIdentity, BD_SQL);

                        return Convert.ToInt32(cmdIdentity.ExecuteScalar());
                    }
                    else
                        return -1;
                }
                else
                {
                    return rowsAffected = cmd_SQL.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return EjecutaQry = -1;
            }

            cmd_SQL.Dispose();
            CierraBD();
            return EjecutaQry;
        }

    }
}