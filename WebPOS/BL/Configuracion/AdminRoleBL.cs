using BL.Interface;
using DAL.Configuracion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BL.Configuracion
{
    public class AdminRoleBL : IAdminRoleBL
    {
        readonly DAL.Utilities.IResponseAPI _Roles;
        public AdminRoleBL(AdminRoleDAL AdminRoleDAL)
        {
            _Roles = AdminRoleDAL;
        }

        public AdminRoleBL()
        {
            _Roles = new AdminRoleDAL();
        }

        public HttpResponseMessage GetResponseAPIRoles(string url)
        {

            try
            {
                return _Roles.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return _Roles.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
