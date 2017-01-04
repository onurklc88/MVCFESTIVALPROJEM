using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace FestivalWeb.SystemHelper
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str == "" ? true : false;
        }
        public static bool IsMail(this string mail)
        {
            try
            {
                MailAddress m = new MailAddress(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}