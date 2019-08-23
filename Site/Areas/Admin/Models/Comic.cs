using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Site.Areas.Admin.Models
{
    public class Comic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Likes { get; set; }
        public decimal PorcentLikes { get; set; }

        private const string SQL_BASE = "" +
            "select t.id, t.name, t.likes from dbo.comics t where t.removed=0";

        public bool Save(int id = 0, string name = null, int likes = 1)
        {
            if (id == 0)
                throw new ArgumentNullException("id", "Parameter cannot be null or empty");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name", "Parameter cannot be null or empty");

            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString());
            conn.Open();

            var cmd = new SqlCommand();

            cmd = new SqlCommand("insert into dbo.comics (id, name, likes) values (@id, @name, @likes)", conn);

            cmd.Parameters.Add(new SqlParameter("@id", id) { DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter("@name", name) { DbType = DbType.String });
            cmd.Parameters.Add(new SqlParameter("@likes", likes) { DbType = DbType.Int32 });
            var rows = cmd.ExecuteNonQuery();

            conn.Close();

            return rows > 0;
        }
        public List<Comic> GetList()
        {
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString());
            conn.Open();

            var cmd = new SqlCommand(SQL_BASE, conn);

            SqlDataReader dt = cmd.ExecuteReader();

            var lst = new List<Comic>();

            if (dt.HasRows)
                while (dt.Read())
                {
                    var obj = new Comic();
                    obj.Id = dt.GetInt32(0);
                    if (!dt.IsDBNull(1))
                        obj.Name = dt.GetString(1);
                    if (!dt.IsDBNull(2))
                        obj.Likes = dt.GetInt32(2);
                    lst.Add(obj);
                }

            conn.Close();

            return lst;
        }
        public Comic GetObjectById(int id)
        {
            if (id < 1)
                throw new ArgumentException("Parameter cannot be zero or negative", "id");

            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ToString());
            conn.Open();

            SqlCommand cmd = new SqlCommand(SQL_BASE + " and t.id=@id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", id) { DbType = DbType.Int32 });

            Comic obj = null;

            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                if (reader.Read())
                {
                    obj = new Comic();
                    obj.Id = reader.GetInt32(0);
                    obj.Name = reader.GetString(1);
                    obj.Likes = reader.GetInt32(2);
                }

            conn.Close();

            return obj;
        }
        public bool Remove(int id)
        {
            if (id < 1)
                throw new ArgumentException("Parameter cannot be zero or negative", "id");

            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString());
            conn.Open();

            var cmd = new SqlCommand("update dbo.comics set removed=1 where id=@id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", id) { DbType = DbType.Int32 });

            var rows = cmd.ExecuteNonQuery();

            conn.Close();

            return rows > 0;
        }
        public bool Like(int id, string name)
        {
            if (id < 1)
                throw new ArgumentException("Parameter cannot be zero or negative", "id");

            var obj = GetObjectById(id);

            if (obj == null)
               return Save(id, name);

            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString.ToString());
            conn.Open();

            var cmd = new SqlCommand("update dbo.comics set likes=@likes where id=@id", conn);
            cmd.Parameters.Add(new SqlParameter("@id", id) { DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter("@likes", obj.Likes + 1) { DbType = DbType.Int32 });

            var rows = cmd.ExecuteNonQuery();

            conn.Close();

            return rows > 0;
        }
        public List<Comic> GetListPorcentLike()
        {
            var lst = GetList();
            var totalLikes = 0;

            lst.ForEach(o => totalLikes += o.Likes);
            lst.ForEach(o => o.PorcentLikes = Math.Round(o.Likes * 100m / totalLikes, 1));

            return lst.OrderByDescending(o => o.PorcentLikes).ToList();
        }
    }
}