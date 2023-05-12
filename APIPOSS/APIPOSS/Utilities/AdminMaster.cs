using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
//using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic
namespace APIPOSS.Utilities
{
    public class admin
    {
        public admin()
        {
             Page_Init(null,null);
            //this.Load += Page_Load;
            //this.PreRender += Page_PreRender;
            //Page_Unload(null,null);
            
        }

        private clsDBMgr p_DBConn;
        private clsDBMgrSAP p_DBConnSAP;
        // private clsDBSAPSMU p_DBConnSAPSIESTA;

        public clsDBMgr DBConn
        {
            get
            {
                return p_DBConn;
            }
        }

        public clsDBMgrSAP DBConnSAP
        {
            get
            {
                return p_DBConnSAP;
            }
        }

        //public clsDBSAPSMU DBConnSAPSIESTA
        //{
        //    get
        //    {
        //        return p_DBConnSAPSIESTA;
        //    }
        //}

        //private clsGlobalLib p_GLib = new clsGlobalLib();

        //public clsGlobalLib GLib
        //{
        //    get
        //    {
        //        return p_GLib;
        //    }
        //}

        protected void Page_Init(object sender, EventArgs e)
        {
            // Get DB Conection
            try
            {
                p_DBConn = new clsDBMgr();
                p_DBConnSAP = new clsDBMgrSAP();
                //  p_DBConnSAPSIESTA = new clsDBSAPSMU();
            }
            catch (Exception ex)
            {
            }
        }



        protected void Page_Unload(object sender, EventArgs e)
        {
            // Close DB Conection
            try
            {
                p_DBConn.DBConn.Close();
                p_DBConn = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}