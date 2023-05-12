using System.Web;

namespace WebPOS.Security
{
    public class SessionPersister
    {
        static string usernameSessionVar = "TypeRole";

        public static string TypeRole
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var sessionVar = HttpContext.Current.Session[usernameSessionVar];
                if (sessionVar != null)
                    return sessionVar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionVar] = value;
            }
        }
    }
}