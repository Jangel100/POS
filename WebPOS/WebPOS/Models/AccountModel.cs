using System;
using System.Linq;

namespace WebPOS.Models
{
    public class AccountModel
    {
        //    private List<Account> listAccounts = new List<Account>();
        string[] Roless = { "AG", "US", "AF", "LC", "VL", "AG2", "AG3", "AG4", "TO" };


        public Boolean find(string[] Role)
        {
            bool result = false;
            foreach (var p in Role.ToArray())
            {
                result = Roless.Contains(p);

                if (result == true)
                {
                    break;
                };
            }

            return result;
        }
    }
}