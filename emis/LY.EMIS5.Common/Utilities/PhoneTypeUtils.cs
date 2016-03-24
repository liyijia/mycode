using LY.EMIS5.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Utilities
{
     public class PhoneTypeUtils
     {
         private static Regex _cmpp_reg = null;
         private static Regex _sgip_reg = null;
         private static Regex _smgp_reg = null;

         private static Regex CMPP_REG
         {
             get
             {
                 if (_cmpp_reg == null)
                 {
                     string reg = System.Configuration.ConfigurationManager.AppSettings["phonetype.cmpp"];
                     if (reg == null)
                         _cmpp_reg = new Regex(@"^(13[4-9]\d{8})|(15[0-2,7-9]\d{8})|187\d{8}|180\d{8}|182\d{8}|184\d{8}|147\d{8}|183\d{8}$");
                     else
                         _cmpp_reg = new Regex(reg);
                 }
                 return _cmpp_reg;
             }
         }

         private static Regex SGIP_REG
         {
             get
             {
                 if (_sgip_reg == null)
                 {
                     string reg = System.Configuration.ConfigurationManager.AppSettings["phonetype.sgip"];
                     if (reg == null)
                         _sgip_reg = new Regex(@"^(13[0-2]\d{8})|(15[5,6]\d{8})|185\d{8}|186\d{8}$");
                     else
                         _sgip_reg = new Regex(reg);
                 }
                 return _sgip_reg;
             }
         }

         private static Regex SMGP_REG
         {
             get
             {
                 if (_smgp_reg == null)
                 {
                     string reg = System.Configuration.ConfigurationManager.AppSettings["phonetype.smgp"];
                     if (reg == null)
                         _smgp_reg = new Regex(@"^(0\d{10,11})|(18[7-9]\d{8})|(1[3,5]3\d{8})|(181\d{8})$");
                     else
                         _smgp_reg = new Regex(reg);
                 }
                 return _smgp_reg;
             }
         }

         public static PhoneType GetPhoneType(ref string phoneNumber)
         {
             if (phoneNumber == null || phoneNumber.Length < 11)
                 return PhoneType.Unknown;

             if (phoneNumber.Length >= 11)
                 phoneNumber = phoneNumber.Trim();
             if (phoneNumber.StartsWith("+"))
                 phoneNumber = phoneNumber.Remove(0, 1);
             if (phoneNumber.StartsWith("86"))
                 phoneNumber = phoneNumber.Remove(0, 2);
             if (phoneNumber.StartsWith("106"))
                 phoneNumber = phoneNumber.Remove(0, 3);

             if (CMPP_REG.IsMatch(phoneNumber))
                 return PhoneType.CMPP;
             if (SGIP_REG.IsMatch(phoneNumber))
                 return PhoneType.SGIP;
             if (SMGP_REG.IsMatch(phoneNumber))
                 return PhoneType.SMGP;

             return PhoneType.Unknown;
         }
     }
}
