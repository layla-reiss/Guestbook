using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace guestbook.Models
{
    public class guestDAO
    {
        public void AddNew(guest thisGuest)
        {
            Account account = new Account(
              "Layla",
              "813475858513613",
              "2sa6usWCjTJYxRclzrRtE30ZiPA");
            Cloudinary cloudinary = new Cloudinary(account);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "CAI-5J2F063";
            builder.InitialCatalog = "LreissWex";
            builder.IntegratedSecurity = true;

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                string query = "INSERT INTO [dbo].[allguests] ([email],[name],[date],[comment],[language]) VALUES(@email,@name,@date,@comment,@language)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", thisGuest.email);
                cmd.Parameters.AddWithValue("name", thisGuest.name);
                cmd.Parameters.AddWithValue("date", thisGuest.date);
                cmd.Parameters.AddWithValue("comment", thisGuest.comment);
                cmd.Parameters.AddWithValue("language", thisGuest.language);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("@" + thisGuest.image),
                PublicId = thisGuest.email
            };
            var uploadResult = cloudinary.Upload(uploadParams);
        }
        public bool isEmailValid(string email)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "CAI-5J2F063";
            builder.InitialCatalog = "LreissWex";
            builder.IntegratedSecurity = true;

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                string query = "SELECT * FROM allguests WHERE email = @email;";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", email);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public List<guest> FetchAll()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "CAI-5J2F063";
            builder.InitialCatalog = "LreissWex";
            builder.IntegratedSecurity = true;

            List<guest> returnList = new List<guest>();


            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                string query = "SELECT * FROM allguests ORDER BY date";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        guest guest = new guest();
                        guest.email = rdr.GetString(0);
                        guest.name = rdr.GetString(1);
                        guest.date = rdr.GetString(2);
                        guest.comment = rdr.GetString(3);
                        guest.language = rdr.GetString(4);
                        returnList.Add(guest);
                    }
                }
            }
            return returnList;

        }
        public List<string> FetchLanguages()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "CAI-5J2F063";
            builder.InitialCatalog = "LreissWex";
            builder.IntegratedSecurity = true;

            List<string> returnList = new List<string>();


            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                string query = "select DISTINCT language from allguests;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        returnList.Add(rdr.GetString(0));
                    }
                }
                return returnList;
            }
        }
        public List<guest> FilterByLanguage(string language)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "CAI-5J2F063";
            builder.InitialCatalog = "LreissWex";
            builder.IntegratedSecurity = true;

            List<guest> returnList = new List<guest>();


            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                string query = "SELECT * FROM allguests WHERE language = @language";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("language", language);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        guest guest = new guest();
                        guest.email = rdr.GetString(0);
                        guest.name = rdr.GetString(1);
                        guest.date = rdr.GetString(2);
                        guest.comment = rdr.GetString(3);
                        guest.language = rdr.GetString(4);
                        returnList.Add(guest);
                    }
                }
            }
            return returnList;

        }
        public void DeleteGuest(string email)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "CAI-5J2F063";
            builder.InitialCatalog = "LreissWex";
            builder.IntegratedSecurity = true;

            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                string query = "DELETE FROM allguests WHERE email = @email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("email", email);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                conn.Close();
            }
        }
    }
}
