using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOWEN.Models
{
    public class CheckLogin
    {
       public static bool CheckFloat(string Check)
        {
            if (Check != "" || Check != null)
            {
                return true;
            }
            else
                return false;
        }
    }
 
}