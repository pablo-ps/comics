using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Site.Models
{
    public class Configs
    {
        public int Id { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        private const string SQL_BASE = "" +
           "select t.id, t.publickey, t.privatekey from dbo.configs t";

        public Configs GetConfigs()
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ToString());
            conn.Open();

            SqlCommand cmd = new SqlCommand(SQL_BASE + " where t.id=@id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", 1) { DbType = DbType.Int32 });

            Configs obj = null;

            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                if (reader.Read())
                {
                    obj = new Configs();
                    obj.Id = reader.GetInt32(0);
                    obj.PublicKey = reader.GetString(1);
                    obj.PrivateKey = reader.GetString(2);
                }

            conn.Close();

            return obj;
        }
    }
}