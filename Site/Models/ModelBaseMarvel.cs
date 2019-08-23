using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace Site.Models
{

    public class ModelBaseMarvel<t>
    {
        public dynamic GetAll(string urlParameters)
        {
            var objConfig = new Configs().GetConfigs();
            var ts = DateTime.Now.ToString();
            var apiKey = objConfig.PublicKey;
            var hash = MD5Hash(ts + objConfig.PrivateKey + apiKey);

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://gateway.marvel.com:443/v1/public/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var response = client.GetAsync(urlParameters + "ts=" + ts + "&apikey=" + apiKey + "&hash=" + hash).Result;

            if (!response.IsSuccessStatusCode)
                return null;

            var result = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<dynamic>(result);

            return data.data.results;
        }

        private string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                strBuilder.Append(result[i].ToString("x2"));
            return strBuilder.ToString();
        }
    }
}