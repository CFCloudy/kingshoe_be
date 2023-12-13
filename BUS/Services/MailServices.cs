using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services
{
    public class MailServices
    {
        public bool SendMail(string target, string subject, string body)
        {
            try
            {
                string EmailAddress = "clone9291@gmail.com";
                string Password = "nyztldajpihdiqnu";
                MailMessage message = new MailMessage(EmailAddress, target.Replace(" ", ""));
                message.Subject = subject;
                message.Body = Template(body);
                //message.Body = "s0d0sd";
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                string server;
                int port;
                if (target.Contains("@gmail"))
                {
                    server = "smtp.gmail.com";
                    port = 587;
                }
                else if (target.Contains("@fpt"))
                {
                    server = "omail.edu.fpt.vn";
                    port = 587;
                }
                else
                {
                    server = "smtp.live.com";
                    port = 587;
                }
                using (SmtpClient smtp = new SmtpClient(server, port))
                //using (SmtpClient smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 2525))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(EmailAddress, Password);
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Template(string mess)
        {
            var temp = @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
<html style=""-webkit-text-size-adjust: none;-ms-text-size-adjust: none;"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
    <title>King Shoes | {EMAIL_TITLE}</title>
</head>
<body style=""padding: 0px; margin: 0px;"">
    <div id=""mailsub"" class=""notification"" align=""center"">
        <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
            <tr>
                <td align=""center"" bgcolor=""#eff3f8"">

                    <!--[if gte mso 10]>
                    <table width=""680"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                    <tr><td>
                    <![endif]-->
                    <table border=""0"" cellspacing=""0"" cellpadding=""0"" class=""table_width_100"" width=""100%"" style=""max-width: 880px;"">
                        <!--header -->
                        <tr>
                            <td align=""center"" bgcolor=""#eff3f8"">
                                <!-- padding --><div style=""height: 20px; line-height: 20px; font-size: 10px;"">&nbsp;</div>
                                <table width=""96%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                    <tr>
                                        <td align=""left"">
                                            <!--Item -->
                                            <div class=""mob_center_bl"" style=""float: left; display: inline-block; width: 115px;"">
                                                <table class=""mob_center"" width=""115"" border=""0"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""border-collapse: collapse;"">
                                                    <tr>
                                                        <td align=""left"" valign=""middle"">
                                                            <!-- padding --><div style=""height: 20px; line-height: 20px; font-size: 10px;"">&nbsp;</div>
                                                            <table width=""115"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                                                <tr>
                                                                    <td align=""left"" valign=""top"" class=""mob_center"">
                                                                        <a href=""#"" target=""_blank"" style=""color: #596167; font-family: Arial, Helvetica, sans-serif; font-size: 13px;"">
                                                                            <font face=""Arial, Helvetica, sans-seri; font-size: 13px;"" size=""3"" color=""#596167"">
                                                                                <img src="" width=""168"" alt=""King Shoes"" border=""0"" style=""display: block;"">
                                                                            </font>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div><!-- Item END-->
                                            <!--[if gte mso 10]>
                                                </td>
                                                <td align=""right"">
                                            <![endif]-->
                                            <!--

                                            Item -->
                                            <div class=""mob_center_bl"" style=""float: right; display: inline-block; width: 88px;"">
                                                <table width=""88"" border=""0"" cellspacing=""0"" cellpadding=""0"" align=""right"" style=""border-collapse: collapse;"">
                                                    <tr>
                                                        <td align=""right"" valign=""middle"">
                                                            <!-- padding --><div style=""height: 20px; line-height: 20px; font-size: 10px;"">&nbsp;</div>
                                                            <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                                                <tr>
                                                                    <td align=""right"">
                                                                        <!--social -->
                                                                        <!--<div class=""mob_center_bl"" style=""width: 88px;"">
                                                                            <table border=""0"" cellspacing=""0"" cellpadding=""0"">
                                                                                <tr>
                                                                                    <td width=""39"" align=""center"" style=""line-height: 19px;"">
                                                                                        <a href=""https://twitter.com/aspnetzero"" target=""_blank"" style=""color: #596167; font-family: Arial, Helvetica, sans-serif; font-size: 12px;"">
                                                                                            <font face=""Arial, Helvetica, sans-serif"" size=""2"" color=""#596167"">
                                                                                                <img src=""data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAABmElEQVR4Ae3OvUvVcRjG4ScTo0WSloiQIoLEoGhyDRTX6GWJEImWQKIQGrIhoheioNaGKGhpiKCh/yADKUJqcJLGoMheDkJx1KdrjDie8/tyzhkCb7ju+RMb21gnd/LZUj+nucNtTtFPANBLEOH+NUyU4gzfSAC+MEGwgynurRdwmGVGiAIXyBYWWKPGvvUCpkl+MkpUMESdrOAzE5xlvFHALZKkzgw9RBN3yYpWSF6wuVHAeRKAdxwh1vGaLPCUPqJRwG5WyQbmmGSA+Ms8WdEim4jGAfCYbKLOWx4wwwJZ0QeiWcBBdjJHdsFsq4Cr1HhDdsGTVgFjZBddbBXQwzzZJYeaB8AwX8kOWyRaB8B+XpEddKUkYIRpXrJGtukH20sCdrFMdshlonoATLJGtuk9feUBcJxPZDH4zhBRGgCwhaM8Jwv8YoxoN2AbN/hNVlRjnCgJ6CXYyh6O8ZAaWWCeA0RpwCCPWCVLscQl+ojSAIC9XOMj2cIKs5yjn4D2AgAGOcE0N7nPdaYYZYAA2g8o998HbAT8AXjZKQTgI9L4AAAAAElFTkSuQmCC"" width=""19"" height=""16"" alt=""Twitter"" border=""0"" style=""display: block;"">
                                                                                            </font>
                                                                                        </a>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>-->
                                                                        <!--social END-->
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div><!-- Item END-->
                                        </td>
                                    </tr>
                                </table>
                                <!-- padding --><div style=""height: 30px; line-height: 30px; font-size: 10px;"">&nbsp;</div>
                            </td>
                        </tr>
                        <!--header END-->
                        <!--content 1 -->
                        <tr>
                            <td align=""center"" bgcolor=""#ffffff"">
                                <table width=""90%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                    <tr>
                                        <td align=""left"">
                                            <table width=""80%"" align=""left"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                                <tr>
                                                    <td align=""left"">
                                                        <div style=""line-height: 24px;"">
                                                            <font face=""Arial, Helvetica, sans-serif"" size=""4"" color=""#57697e"" style=""font-size: 16px;"">
                                                                <span style=""font-family: Arial, Helvetica, sans-serif; font-size: 15px; color: #57697e;"">
                                                                    {EMAIL_BODY}
                                                                </span>
                                                            </font>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <!-- padding --><div style=""height: 45px; line-height: 45px; font-size: 10px;"">&nbsp;</div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <!--content 1 END-->
                        <!--footer -->
                        <tr>
                            <td class=""iage_footer"" align=""center"" bgcolor=""#eff3f8"">
                                <!-- padding --><div style=""height: 40px; line-height: 40px; font-size: 10px;"">&nbsp;</div>

                                <table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                    <tr>
                                        <td align=""center"">
                                            <font face=""Arial, Helvetica, sans-serif"" size=""3"" color=""#96a5b5"" style=""font-size: 13px;"">
                                                <span style=""font-family: Arial, Helvetica, sans-serif; font-size: 13px; color: #96a5b5;"">
                                                    2023 &copy; King Shoes.
                                                </span>
                                            </font>
                                        </td>
                                    </tr>
                                </table>

                                <!-- padding --><div style=""height: 50px; line-height: 50px; font-size: 10px;"">&nbsp;</div>
                            </td>
                        </tr>
                        <!--footer END-->
                    </table>
                    <!--[if gte mso 10]>
                    </td></tr>
                    </table>
                    <![endif]-->

                </td>
            </tr>
        </table>

    </div>
</body>
</html>
";

            temp = temp.Replace("{EMAIL_BODY}", mess);
            return temp;
        }
    }
}
