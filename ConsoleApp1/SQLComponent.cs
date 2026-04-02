using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class SQLComponent
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection connessione = new SqlConnection(connectionString);
                return connessione;
            } catch (Exception)
            {
                return null;
            }
        }

        public static bool TestConnection()
        {
            var conn = SQLComponent.GetConnection();
            try
            {
                conn.Open();
                conn.Close();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static List<FileEntry> GetAll()
        {
            var rList = new List<FileEntry>();

            using (var conn = SQLComponent.GetConnection())
            {
                string strSQL = "select * from [dbo].[FileEntries]";

                try
                {
                    using (var cmd = new SqlCommand(strSQL, conn))
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader();

                        FileEntry file;
                        while (reader.Read())
                        {
                            file = new FileEntry();
                            file.Id = (int)reader["Id"];
                            file.Nome = reader["Nome"] == DBNull.Value ? "" : reader["Nome"].ToString();
                            file.Dimensione = reader["Dimensione"] == DBNull.Value ? 0 : (int)reader["Dimensione"];

                            rList.Add(file);
                        }

                        conn.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return rList;
        }

        public static FileEntry Get(int id)
        {
            FileEntry file = new FileEntry();

            using (var conn = SQLComponent.GetConnection())
            {
                string strSQL = "select * from [dbo].[FileEntries] where id=@id";

                try
                {
                    using (var cmd = new SqlCommand(strSQL, conn))
                    {
                        conn.Open();
                        var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                        cmd.Parameters.Add(new SqlParameter("@id", DbType.Int32));
                        cmd.Parameters["id"].Value = id;

                        reader.Read();

                        file = new FileEntry();
                        file.Id = (int)reader["Id"];
                        file.Nome = reader["Nome"] == DBNull.Value ? "" : reader["Nome"].ToString();
                        file.Dimensione = reader["Dimensione"] == DBNull.Value ? 0 : (int)reader["Dimensione"];

                        conn.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return file;
        }


        public static bool Insert(FileEntry f)
        {

            using (var conn = SQLComponent.GetConnection())
            {

                const string query = "insert into [dbo].[FileEntries] (Nome, Dimensione) values (@Nome, @Dimensione)";
                try
                {

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        cmd.Parameters.Add("@Nome", SqlDbType.NVarChar).Value = f.Nome;
                        cmd.Parameters.Add("@Dimensione", SqlDbType.Int).Value = f.Dimensione;

                        cmd.ExecuteNonQuery();

                        conn.Close();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }
    }
}
