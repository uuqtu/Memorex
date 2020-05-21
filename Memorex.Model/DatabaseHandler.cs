using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorex.Model
{
    public class DatabaseHandler
    {
        private static readonly Lazy<DatabaseHandler> lazy = new Lazy<DatabaseHandler>(() => new DatabaseHandler());
        private const string DbName = "Memorex.sqlite";

        private SQLiteConnection sql_con;


        private DatabaseHandler()
        {
            if (!File.Exists(DbName))
                SQLiteConnection.CreateFile(DbName);

            sql_con = new SQLiteConnection($"Data Source={DbName};Version=3;");
            try
            {
                sql_con.Open();
                string sql = "CREATE TABLE IF NOT EXISTS ENTRIES (" +
                                                                "ID TEXT PRIMARY KEY, " +
                                                                "LINK TEXT, " +
                                                                "SEARCHWORDS TEXT, " +
                                                                "CATEGORY TEXT)";
                SQLiteCommand command = new SQLiteCommand(sql, sql_con);
                command.ExecuteNonQuery();
                //Enter Category Table
                sql = "CREATE TABLE IF NOT EXISTS CATEGORIES (" +
                                                "ID TEXT PRIMARY KEY, " +
                                                "CATEGORY TEXT)";
                command = new SQLiteCommand(sql, sql_con);
                command.ExecuteNonQuery();
            }
            finally
            {
                sql_con.Close();
            }
        }


        public static DatabaseHandler Instance
        {
            get { return lazy.Value; }
        }

        public string AddEntry(string link, string category, params string[] searchwords)
        {
            searchwords = searchwords.Select(s => s.ToLowerInvariant()).ToArray();
            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO ENTRIES (ID, LINK, SEARCHWORDS, CATEGORY) VALUES (@param0, @param1, @param2, @param3)", sql_con);
            insertSQL.CommandType = CommandType.Text;
            string guid = Guid.NewGuid().ToString();
            insertSQL.Parameters.Add(new SQLiteParameter("@param0", guid));
            insertSQL.Parameters.Add(new SQLiteParameter("@param1", link));
            insertSQL.Parameters.Add(new SQLiteParameter("@param2", string.Join(" ", searchwords)));
            insertSQL.Parameters.Add(new SQLiteParameter("@param3", category));
            try
            {
                sql_con.Open();
                insertSQL.ExecuteNonQuery();
                return guid;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                sql_con.Close();
            }
        }

        public void AddCategoryIfNotExist(string category)
        {
            var cats = GetCategories().Select(s => s.ToLowerInvariant()).ToArray();
            if (cats.Contains(category.ToLowerInvariant()))
                return;

            SQLiteCommand insertSQL = new SQLiteCommand("INSERT INTO CATEGORIES (ID, CATEGORY) VALUES (@param0, @param1)", sql_con);
            insertSQL.CommandType = CommandType.Text;
            string guid = Guid.NewGuid().ToString();
            insertSQL.Parameters.Add(new SQLiteParameter("@param0", guid));
            insertSQL.Parameters.Add(new SQLiteParameter("@param1", category));
            try
            {
                sql_con.Open();
                insertSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                sql_con.Close();
            }
        }

        public void DeleteEntry(string guid)
        {
            SQLiteCommand insertSQL = new SQLiteCommand($"DELETE FROM ENTRIES WHERE ID LIKE '{guid}' ", sql_con);
            try
            {
                sql_con.Open();
                insertSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sql_con.Close();
            }
        }

        public List<KnoledgeElement> SearchEntry(string searchString)
        {
            searchString = searchString.ToLowerInvariant();
            //filterWords = filterWords.Select(s => s.ToLowerInvariant()).ToArray();
            var informationItems = SelectEntries(searchString);
            return informationItems;
        }

        private List<KnoledgeElement> SelectEntries(string search)
        {
            List<KnoledgeElement> info = new List<KnoledgeElement>();
            try
            {
                sql_con.Open();
                using (SQLiteCommand fmd = sql_con.CreateCommand())
                {
                    //https://www.sqlitetutorial.net/sqlite-full-text-search/
                    //SQLiteCommand sqlComm = new SQLiteCommand($@"SELECT* FROM MAIN WHERE VALUE like {filter}", sql_con);
                    SQLiteCommand sqlComm = new SQLiteCommand($@"SELECT * FROM ENTRIES", sql_con);
                    SQLiteDataReader r = sqlComm.ExecuteReader();
                    while (r.Read())
                    {
                        string id = (string)r["ID"];
                        string searchwords = (string)r["SEARCHWORDS"];
                        string link = (string)r["LINK"];
                        string category = (string)r["CATEGORY"];

                        var tmp = new KnoledgeElement(link, searchwords, id, category);
                        tmp.CalculateMatchingScore(search);

                        info.Add(tmp);
                    }
                }
            }
            finally
            {
                sql_con.Close();
            }

            return info;
        }

        public List<string> GetCategories()
        {
            List<string> info = new List<string>();
            try
            {
                sql_con.Open();
                using (SQLiteCommand fmd = sql_con.CreateCommand())
                {
                    //https://www.sqlitetutorial.net/sqlite-full-text-search/
                    //SQLiteCommand sqlComm = new SQLiteCommand($@"SELECT* FROM MAIN WHERE VALUE like {filter}", sql_con);
                    SQLiteCommand sqlComm = new SQLiteCommand($@"SELECT DISTINCT CATEGORY FROM CATEGORIES", sql_con);
                    SQLiteDataReader r = sqlComm.ExecuteReader();
                    while (r.Read())
                    {
                        string category = (string)r["CATEGORY"];
                        info.Add(category);
                    }
                }
            }
            finally
            {
                sql_con.Close();
            }

            return info;
        }

    }
}
