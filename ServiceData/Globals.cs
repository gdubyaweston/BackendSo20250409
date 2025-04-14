using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServiceData
{
    public static class Globals
    {
        public static string ReadDataFromFile(string path)
        {
            var s = "";

            if (!File.Exists(path))
                return "";

            try
            {
                StreamReader sr = File.OpenText(path);
                s = sr.ReadToEnd();
                sr.Close();

                return s;
            }
            catch (Exception ex)
            {
                return "";
                throw;
            }
        }

        public static bool SendEmail(string subject, string body, string userEmail, bool isBodyHtml, string smtpServer, string smtpUser, string smtpPassword)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(smtpUser);
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = subject;// "Two Factor Code";
            mailMessage.IsBodyHtml = isBodyHtml;
            mailMessage.Body = body; // code;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
            client.Host = smtpServer;
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(mailMessage);
                // TDOD: log email sent
                return true;
            }
            catch (Exception ex)
            {
                // log exception
                string msg = ex.Message;
            }
            return false;
        }

        public static bool IsItTrue()
        {
            return true;
        }

        // this is temp for testing
        public static string MyAspNetEmail()
        {
            return "gweston@aictitle.com";
            //return "test@aictitle.com";
        }

        // this is temp for testing
        public static string MyAspNetUserId()
        {
            return "e14d8d0c-7781-49a6-b833-ec828a50a09e";
        }

        // this is temp for testing
        public static string GetHostUrl(bool incluseProtocolScheme = true)
        {
            return "test.aic.com";
        }

        // this is temp for testing
        public static int VectorCustomerID()
        {
            return 128;
        }

        // this is temp for testing
        public static string UriHost()
        {
            //return "myclosingroom";
            return "myaircraftclosingroom";
        }

        // this is temp for testing
        public static bool IsTextronURL()
        {
            return UriHost().Contains("textron");
        }

        // this is temp for testing
        public static bool IsAicURL()
        {
            return UriHost().Contains("aicvirtual");
        }

        // this is temp for testing
        public static bool IsACRurl()
        {
            //return false;
            return UriHost().Contains("aircraft");
        }

        // this is temp for testing
        public static bool IsAicGlobalURL()
        {
            return UriHost().Contains("aicglobal");
        }

        public static bool IsMyLocalhostDevURL()
        {
            switch (UriHost())
            {
                case "myclosingroom":
                case "myaircraftclosingroom":
                case "myaicglobal":
                case "myaicclosingroom":
                case "mytextronaircraftclosingroom":

                    return true;

                // setup the "my..." hosts in the C:\Windows\System32\drivers\etc\hosts file as
                // 127.0.0.1  myavsure
                // 127.0.0.1  myaicglobal
                // 127.0.0.1  mytextronclosingroom

                default:
                    return false;
            }
        }

        public static string GCRAgreementType(string playerRole)
        {
            var agreementType = "";

            switch (RoleReplace(playerRole))
            {
                case "Closing Agent":
                case "Manager":
                    // no role necessary since no need to sign
                    break;
                case "Buyer":
                case "Purchaser":
                case "Trustor (P)":
                case "Trustor (B)":
                    // agreementType = "Commercial Agent";
                    agreementType = "Virtual Closing Solutions";
                    break;
                default:
                    // agreementType = "Transaction Counterparty";
                    agreementType = "Virtual Closing Solutions";
                    break;
            }

            return agreementType;
        }

        public static string RoleReplace(string role)
        {
            if (role == null)
                return "";

            return role.Replace("Buyer", "Purchaser").Replace("(B)", "(P)").Replace("(LB)", "(LP)");
        }

        /*
        public static async void AddErrorLogAsync(AIC_DBContext db, string ClosingRoomUserID, string ExMsg, string ExMsg2, string URL, MethodBase mbase)
        {
            using (db)
            {
                var tE = new tblExceptionError
                {
                    Date = DateTime.Now,
                    DeclaringType = GetHostUrl(true),
                    ClosingRoomUserID = ClosingRoomUserID,
                    CallingMethod = mbase.Name,
                    ExMessage = ExMsg,
                    ExMessageToString = ExMsg2,
                    NotesMax = ExMsg2
                };
                db.tblExceptionErrors.Add(tE);
                // await db.SaveChangesAsync();
                //SaveDBChanges(db, MethodBase.GetCurrentMethod());
            }
        }
        */

        /*
        public static async Task<bool> SaveDBChangesAsync(AIC_DBContext db, MethodBase mbase, string vars = "")
        {
            var msg = new StringBuilder();
            msg.AppendLine("");
            if (!string.IsNullOrWhiteSpace(vars))
            {
                msg.AppendLine("<br/><br/>");
                msg.AppendLine("vars:<br/>" + vars);
            }

            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbu)
            {
                msg.AppendLine("");
                msg.AppendLine("Message:");
                msg.AppendLine(dbu.Message);

                msg.AppendLine("");
                msg.AppendLine("dbu.StackTrace:");
                msg.AppendLine(dbu.StackTrace?.ToString());

                if (dbu.InnerException != null)
                {
                    msg.Append("");
                    msg.Append("dbu.InnerException.Message:");
                    msg.Append(dbu.InnerException?.Message.ToString());
                }

                if (dbu.InnerException != null && dbu.InnerException.InnerException != null)
                {
                    msg.Append("");
                    msg.Append("dbu.InnerException.InnerException.Message:");
                    msg.Append(dbu.InnerException?.InnerException?.Message.ToString());
                }

                foreach (var vr in dbu.Entries)
                {
                    msg.Append("");
                    msg.Append("ChangeTrackinEntity:");
                    msg.Append(vr.Entity.ToString());
                }

                Debug.WriteLine(msg.ToString());
                AddErrorLogAsync(db, MyAspNetUserId(), dbu.Message, msg.ToString(), "", mbase);

                return false;

            }
            catch (Exception ex)
            {
                msg.AppendLine("");
                msg.AppendLine("Message:");
                msg.AppendLine(ex.Message);

                msg.AppendLine("");
                msg.AppendLine("ex.StackTrace:");
                msg.AppendLine(ex.StackTrace?.ToString());

                if (ex.InnerException != null)
                {
                    msg.Append("");
                    msg.Append("ex.InnerException.Message:");
                    msg.Append(ex.InnerException?.Message.ToString());
                }

                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                {
                    msg.Append("");
                    msg.Append("ex.InnerException.InnerException.Message:");
                    msg.Append(ex.InnerException?.InnerException?.Message.ToString());
                }

                Debug.WriteLine(msg.ToString());
                AddErrorLogAsync(db, MyAspNetUserId(), ex.Message, msg.ToString(), "", mbase);
                return false;
            }
        }
        */

        public static bool IsRole(string role)
        {
            //HttpContext.
            //var currenrContext = HttpContext.Current;

            return false;
        }

        public static string GetPortalDocsFolder(bool returnDefault = false, string targetEnvironment = "")
        {
            var DefaultFolder = @"\\aicserver1\AIC Data\Escrow\";
            var parentFolder = @"\\aicserver1\AIC Data\Closing Room";

            // Uri uriBrowserUrl = Request.Url;
            // ERROR: the name request does not exist in the current context
            // https://stackoverflow.com/questions/10439709/why-the-name-request-does-not-exist-when-writing-in-a-class-cs-file

            //if (HttpContext.Current == null)
            //    return DefaultFolder;

            var subfolder1 = "AIC";
            var subfolder2 = "Dev";

            switch (UriHost())
            {
                // AIC
                case "aircraftclosingroom.com":
                case "prodtest.aircraftclosingroom.com":
                case "dev.aircraftclosingroom.com":
                case "demo.aircraftclosingroom.com":
                case "sandbox.aircraftclosingroom.com":
                case "aicvirtualclosings.com":
                case "prodtest.aicvirtualclosings.com":
                case "dev.aicvirtualclosings.com":
                case "demo.aicvirtualclosings.com":
                case "sandbox.aicvirtualclosings.com":
                case "myaircraftclosingroom": // newer
                case "bwaircraftclosingroom": // newer
                case "myclosingroom": // to be deprecated
                case "bwclosingroom": // to be deprecated
                case "closingroom.aictitle.com": // old
                case "closingroomdev.aictitle.com": // old
                case "closingroomsandbox.aictitle.com": // old
                    subfolder1 = "AIC";
                    break;
                // Textron
                case "textron.aircraftclosingroom.com":
                case "textronprodtest.aircraftclosingroom.com":
                case "textronsandbox.aircraftclosingroom.com":
                case "mytextronaircraftclosingroom": // newer
                case "bwtextronaircraftclosingroom": // newer
                case "mytextronclosingroom": // to be deprecated
                case "bwtextronclosingroom": // to be deprecated
                    subfolder1 = "Textron";
                    break;
                // Global
                case "aicglobal.com":
                case "prodtest.aicglobal.com":
                case "dev.aicglobal.com":
                case "demo.aicglobal.com":
                case "myaicglobal":
                case "bwaicglobal":
                case "sandbox.aicglobal.com":
                case "global.aircraftclosingroom.com": // old
                case "globalsandbox.aircraftclosingroom.com": // old
                    subfolder1 = "Global";
                    break;
                case "funnelsandbox.aircraftclosingroom.com":
                case "myfunnelclosingroom":
                case "bwfunnelclosingroom":
                    subfolder1 = "Funnel";
                    break;
                default:
                    break;
            }


            switch (UriHost())
            {
                // AIC
                case "aircraftclosingroom.com":
                case "textron.aircraftclosingroom.com":
                case "aicglobal.com":
                case "aicvirtualclosings.com":
                    subfolder2 = "Prod";
                    break;
                case "dev.aircraftclosingroom.com":
                case "sandbox.aircraftclosingroom.com":
                case "textronsandbox.aircraftclosingroom.com":
                case "dev.aicglobal.com":
                case "sandbox.aicglobal.com":
                case "myaircraftclosingroom":
                case "bwaircraftclosingroom":
                case "mytextronaircraftclosingroom":
                case "bwtextronaircraftclosingroom":
                case "myaicglobal":
                case "bwaicglobal":
                case "dev.aicvirtualclosings.com":
                case "sandbox.aicvirtualclosings.com":
                case "myclosingroom": // to be deprecated
                case "mytextronclosingroom": // to be deprecated
                case "bwtextronclosingroom": // to be deprecated
                    subfolder2 = "Dev";
                    break;
                default:
                    break;
            }
            if (UriHost().Contains("prodtest"))
                subfolder2 = "Prod";
            if (UriHost().Contains("sandbox"))
                subfolder2 = "Sandbox";
            if (UriHost().Contains("demo") || targetEnvironment.Contains("demo"))
                subfolder2 = "Demo";

            var folder = parentFolder + @"\" + subfolder1 + @"\" + subfolder2 + @"\";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (returnDefault)
                return DefaultFolder;

            return folder.Trim();
        }

        public static string RandomString(int length, string validchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz")
        {
            Thread.Sleep(17);
            var stringChars = new char[length];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = validchars[random.Next(validchars.Length)];
            }

            string finalString = new string(stringChars);
            return finalString;
        }

        /*
        public static string GetClient_UserHostName(IHttpContextAccessor ctx)
        {
            // Uri uriBrowserUrl = Request.Url;
            // ERROR: the name request does not exist in the current context
            // https://stackoverflow.com/questions/10439709/why-the-name-request-does-not-exist-when-writing-in-a-class-cs-file

            //if (HttpContext.Current == null)
            //    return "";

            //var request = HttpContext.Current.Request;

            if (ctx.HttpContext == null)
                return "";

            if (ctx.HttpContext.Features.Get<IHttpConnectionFeature>() == null)
                return "";

            var hostName = ctx.HttpContext.Features.Get<IHttpConnectionFeature>();
            if (hostName == null)
                return "";

            var rr = hostName.LocalIpAddress;
            if (rr == null)
                return "";

            return rr.ToString();
        }
        */

        public static string GetRootUriHost()
        {
            if (UriHost().Contains("www.")) return UriHost().Replace("www.", "");
            return UriHost();
        }

        public static string GetPortalTitle(bool Fullname = false, bool AppendTM = true, bool ForBrowserTab = false)
        {
            // Uri uriBrowserUrl = Request.Url;
            // ERROR: the name request does not exist in the current context
            // https://stackoverflow.com/questions/10439709/why-the-name-request-does-not-exist-when-writing-in-a-class-cs-file

            // need to replace
            //if (HttpContext.Current == null)
            //    return "";

            var _name = "";

            switch (GetRootUriHost())
            {
                case "textron.aircraftclosingroom.com":
                case "textronprodtest.aircraftclosingroom.com":
                    _name = "TEXTRON AVIATION";
                    break;
                case "textronsandbox.aircraftclosingroom.com":
                    _name = "TEXTRON AVIATION SANDBOX";
                    break;
                case "funnelsandbox.aircraftclosingroom.com":
                    _name = "FUNNEL SANDBOX";
                    break;
                case "dev.aicglobal.com":
                    _name = "DEV AIC GLOBAL SOLUTIONS";
                    Fullname = false;
                    break;
                case "demo.aicglobal.com":
                    _name = "DEMO AIC GLOBAL SOLUTIONS";
                    Fullname = false;
                    break;
                case "globalsandbox.aircraftclosingroom.com":
                case "sandbox.aicglobal.com":
                    _name = "SANDBOX AIC GLOBAL SOLUTIONS";
                    Fullname = false;
                    break;
                case "aicglobal.com":
                case "global.aircraftclosingroom.com":
                case "prodtest.aicglobal.com":
                    _name = "AIC GLOBAL SOLUTIONS";
                    Fullname = false;
                    break;
                case "closingroomdev.aictitle.com":
                case "dev.aircraftclosingroom.com":
                    _name = "DEV";
                    break;
                case "demo.aircraftclosingroom.com":
                    _name = "DEMO";
                    break;
                case "closingroomsandbox.aictitle.com":
                case "sandbox.aircraftclosingroom.com":
                    _name = "SANDBOX";
                    break;
                case "aircraftclosingroom.com":
                case "prodtest.aircraftclosingroom.com":
                    break;
                case "myaircraftclosingroom": // newer
                case "myclosingroom": // to be deprecated
                    _name = "MY LOCALHOST";
                    break;
                case "myaicglobal":
                    _name = "MY LOCALHOST GLOBAL";
                    Fullname = false;
                    break;
                case "myaicvirtualclosings":
                    _name = "MY LOCALHOST AIC";
                    Fullname = false;
                    break;
                case "aicvirtualclosings.com":
                case "prodtest.aicvirtualclosings.com":
                    _name = "AIC Virtual Closings";
                    //Fullname = false;
                    break;
                case "dev.aicvirtualclosings.com":
                    _name = "DEV AIC";
                    Fullname = false;
                    break;
                case "demo.aicvirtualclosings.com":
                    _name = "DEMO AIC";
                    Fullname = false;
                    break;
                case "mytextronaircraftclosingroom": // newer
                case "mytextronclosingroom": // to be deprecated
                    _name = "MY LOCALHOST TEXTRON";
                    Fullname = false;
                    break;
                case "bwaircraftclosingroom":
                    _name = "bw LOCALHOST";
                    break;
                case "bwaicglobal":
                    _name = "bw LOCALHOST GLOBAL";
                    Fullname = false;
                    break;
                case "bwtextronaircraftclosingroom":
                    _name = "bw LOCALHOST TEXTRON";
                    Fullname = false;
                    break;
            }

            if (Fullname)
            {
                if (AppendTM)
                {
                    if (IsAicURL())
                    {
                        if (ForBrowserTab)
                        {
                            _name += " &trade;";
                        }
                        else
                        {
                            _name += " <sup>&trade;</sup>";
                        }
                    }
                    else
                    {
                        if (ForBrowserTab)
                        {
                            _name += " AIRCRAFT CLOSING ROOM&trade;";
                        }
                        else
                        {
                            _name += " AIRCRAFT CLOSING ROOM<sup>&trade;</sup>";
                        }
                    }
                }
                else
                {
                    if (!IsAicURL())
                        _name += " AIRCRAFT CLOSING ROOM";
                }
            }
            else
            {
                if (!_name.Contains("CLOSING ROOM") && !IsAicURL() && !IsTextronURL() && !IsAicGlobalURL())
                    _name += " VIRTUAL CLOSING";
            }

            return _name.Trim();
        }

    }
}
