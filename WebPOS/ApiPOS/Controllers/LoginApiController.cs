using Entities.Models.Franquicias;
using Entities.viewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiPOS.Controllers
{
    public class LoginApiController : ApiController
    {
        // GET: api/LoginApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LoginApi/5
        public IHttpActionResult Get(Users users)
        {
            UsersView usersView = new UsersView();
            
            if (usersView!=null)
            {
                return Ok(usersView);
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: api/LoginApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LoginApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LoginApi/5
        public void Delete(int id)
        {
        }
    }
}
