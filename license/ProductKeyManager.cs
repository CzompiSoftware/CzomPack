using hu.czompisoftware.libraries.crypto;
using hu.czompisoftware.libraries.general;
using hu.czompisoftware.libraries.license.exception;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hu.czompisoftware.libraries.license
{
    /**
     * <summary>
     * This class add license managing system to selected code. If client entered product key once, the code will be save to license.key file, then it will load it.<br/>
     * Copyright Czompi Software 2020<br/>
     * File version <b>1.3.3</b> | Author <b>Czompi</b>
     * </summary>
     */
    public class ProductKeyManager
    {
        protected readonly List<KeyValuePair<string, string>> lines = new List<KeyValuePair<string, string>>();
        public ProductKeyManager()
        {
            if (!File.Exists("license.key"))
            {
                throw new SoftwareFreeUse("No product key found.");
            }
            try
            {
                var lk = new string(File.ReadAllLines("license.key", Encoding.UTF8).Where(x => !x.StartsWith("--#")).Aggregate((x, y) => x + "" + y).Reverse().ToArray());
                lk = lk.Contains("=") ? lk.Replace("=", "") + new string('=', lk.Count(x => x == '=')) : lk;
                lk = Base64.Decode(lk);
                //lk = RSA.Decrypt(Base64.Decode(lk), new System.Security.Cryptography.RSAParameters {  }, false);
                String[] lines_str = lk.Split('\n');
                lines.AddRange(lines_str.Select(x => new KeyValuePair<string, string>(x.Split('=')[0].Trim(), x.Split('=')[1].Trim())));
                Logger.Info("Product key loaded sucessfully!");
            }
            catch (Exception ex)
            {
                throw new SoftwareLicenseException("Licensing process ended due to invalid license file!", ex);
            }

        }
        public string GetPublicKey()
        {
            return GetValue("pkey");
        }

        public string GetCompany()
        {
            return GetValue("company") == "not-found" ? (GetValue("corporate") == "not-found" ? "" : GetValue("corporate")) : GetValue("company");
        }

        public string GetProductOwner()
        {
            var licensedto = (GetValue("user") == "not-found" ? GetValue("licensedto") : GetValue("user"));
            if (licensedto.ToLower() == "{dynamic}") licensedto = UserPrincipal.Current.DisplayName;
            return licensedto;
        }

        public string GetIssuer()
        {
            return GetValue("issuer");
        }

        protected string GetValue(string key)
        {
            try
            {
                var val = lines.Where(x => x.Key.ToLower() == key.ToLower()).ToList();
                return val.Count == 0 ? "not-found" : val.FirstOrDefault().Value;
            }
            catch (Exception)
            {
                return "not-found";
            }
        }

    }
}
