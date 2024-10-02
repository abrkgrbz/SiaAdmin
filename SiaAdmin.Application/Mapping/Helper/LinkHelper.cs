using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiaAdmin.Application.Mapping.Helper
{
    public static class LinkHelper
    {
        public static string ModifyQueryStringManually(string url, Dictionary<string, string> queryParams)
        {
            var uri = new Uri(url);
            var existingQuery = uri.Query.TrimStart('?');
            var existingParams = existingQuery
                .Split('&')
                .Select(param => param.Split('='))
                .ToDictionary(pair => pair[0], pair => pair.Length > 1 ? pair[1] : null);
            foreach (var kvp in queryParams)
            {
                existingParams[kvp.Key] = kvp.Value;
            }
            var newQuery = string.Join("&", existingParams.
                Select(kvp => kvp.Value != null
                    ? $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}" : HttpUtility.UrlEncode(kvp.Key)));
            var newUrl = $"{uri.GetLeftPart(UriPartial.Path)}?{newQuery}{uri.Fragment}";
            return newUrl;
        }

        public static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
            }
        }

        public static string DoTheChecksum(string text)
        {
            string retVal = "";

            try
            {
                string aaa = "";
                aaa = GetStringSha256Hash(text);
                aaa = GetStringSha256Hash(aaa);
                aaa = aaa.Replace("0", "o");
                aaa = aaa.Replace("1", "l");
                aaa = aaa.Replace("6", "u");
                aaa = aaa.Replace("7", "t");
                retVal = aaa;
            }
            catch (Exception)
            {
                //throw;
            }

            return retVal;
        }

    }
}
