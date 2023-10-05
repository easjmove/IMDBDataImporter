using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class NormalInserter : IInserter
    {
        public void InsertData(SqlConnection sqlConn, List<Title> titles)
        {
            foreach (Title title in titles)
            {
                SqlCommand sqlComm = new SqlCommand("" +
                        "INSERT INTO [dbo].[Titles]" +
                        "([tconst],[titleType],[primaryTitle],[originalTitle]" +
                        ",[isAdult],[startYear],[endYear],[runTimeMinutes])" +
                        "VALUES " +
                        $"('{title.tconst}'," +
                        $"'{title.titleType}'," +
                        $"'{title.primaryTitle.Replace("'", "''")}'," +
                        $"'{title.originalTitle.Replace("'", "''")}'," +
                        $"'{title.isAdult}'," +
                        $"{CheckIntForNull(title.startYear)}," +
                        $"{CheckIntForNull(title.endYear)}," +
                        $"{CheckIntForNull(title.runtimeMinutes)})"
                        , sqlConn);
                try
                {
                    sqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(sqlComm.CommandText);
                }
            }
        }

        public string CheckIntForNull(int? input)
        {
            if (input == null)
            {
                return "NULL";
            }
            else
            {
                return "" + input;
            }
        }
    }
}
