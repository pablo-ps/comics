using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Site.Areas.Admin.Models
{
	public class User
	{
		public int Id { get; protected set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public User CheckLogin(string email, string password)
		{
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email", "Parameter cannot be null");

			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password", "Parameter cannot be null");

			var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ToString());
			conn.Open();

			SqlCommand cmd = new SqlCommand("select t.id, t.name, t.email, t.password from users t where t.email=@email and t.password=@password", conn);
			cmd.Parameters.Add(new SqlParameter("@email", email) { DbType = DbType.String });
			cmd.Parameters.Add(new SqlParameter("@password", MD5Hash(password)) { DbType = DbType.String });

			User obj = null;

			using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
				if (reader.Read())
				{
					obj = new User();
					obj.Id = reader.GetInt32(0);
					obj.Name = reader.GetString(1);
					obj.Email = reader.GetString(2);
					obj.Password = reader.GetString(3);
				}

			conn.Close();

			return obj;
		}

		public bool ChangePassword(string email, string currentPassword, string newPassword, string confirmNewPassword)
		{
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email", "Parameter cannot be null");

			if (string.IsNullOrEmpty(currentPassword))
				throw new ArgumentNullException("currentPassword", "Parameter cannot be null");

			if (string.IsNullOrEmpty(newPassword))
				throw new ArgumentNullException("newPassword", "Parameter cannot be null");

			if (string.IsNullOrEmpty(confirmNewPassword))
				throw new ArgumentNullException("confirmNewPassword", "Parameter cannot be null");

			if (newPassword != confirmNewPassword)
				throw new Exception("Senhas não conferem!");

			var us = CheckLogin(email, currentPassword);

			if (us == null)
				throw new Exception("Senha atual errada!");

			var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ToString());
			conn.Open();

			SqlCommand cmd = new SqlCommand("update users set password=@password where id=@id", conn);
			cmd.Parameters.Add(new SqlParameter("@password", MD5Hash(newPassword)) { DbType = DbType.String });
			cmd.Parameters.Add(new SqlParameter("@id", us.Id) { DbType = DbType.Int32 });

			var rows = cmd.ExecuteNonQuery();

			conn.Close();

			return rows == 1;
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