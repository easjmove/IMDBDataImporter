using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class PreparedInserter : IInserter
    {
        public void InsertData(SqlConnection sqlConn, List<Title> titles)
        {
            SqlCommand sqlComm = new SqlCommand("" +
                        "INSERT INTO [dbo].[Titles]" +
                        "([tconst],[titleType],[primaryTitle],[originalTitle]" +
                        ",[isAdult],[startYear],[endYear],[runTimeMinutes])" +
                        "VALUES " +
                        $"(@tconst," +
                        $"@titleType," +
                        $"@primaryTitle," +
                        $"@originalTitle," +
                        $"@isAdult," +
                        $"@startYear," +
                        $"@endYear," +
                        $"@runtimeMinutes)"
                        , sqlConn);
            SqlParameter tconstParameter = new SqlParameter("@tconst",
                System.Data.SqlDbType.VarChar, 10);
            sqlComm.Parameters.Add(tconstParameter);

            SqlParameter titleTypeParameter = new SqlParameter("@titleType",
                System.Data.SqlDbType.VarChar, 50);
            sqlComm.Parameters.Add(titleTypeParameter);

            SqlParameter primaryTitleParameter = new SqlParameter("@primaryTitle",
                System.Data.SqlDbType.VarChar, 8000);
            sqlComm.Parameters.Add(primaryTitleParameter);

            SqlParameter originalTitleParameter = new SqlParameter("@originalTitle",
                System.Data.SqlDbType.VarChar, 8000);
            sqlComm.Parameters.Add(originalTitleParameter);

            SqlParameter isAdultParameter = new SqlParameter("@isAdult",
                System.Data.SqlDbType.Bit);
            sqlComm.Parameters.Add(isAdultParameter);

            SqlParameter startYearParameter = new SqlParameter("@startYear",
                System.Data.SqlDbType.Int);
            sqlComm.Parameters.Add(startYearParameter);

            SqlParameter endYearParameter = new SqlParameter("@endYear",
                System.Data.SqlDbType.Int);
            sqlComm.Parameters.Add(endYearParameter);

            SqlParameter runtimeMinutesParameter = new SqlParameter("@runtimeMinutes",
                System.Data.SqlDbType.Int);
            sqlComm.Parameters.Add(runtimeMinutesParameter);

            sqlComm.Prepare();

            foreach (Title title in titles)
            {
                FillParameter(tconstParameter, title.tconst);
                FillParameter(titleTypeParameter, title.titleType);
                FillParameter(primaryTitleParameter, title.primaryTitle);
                FillParameter(originalTitleParameter, title.originalTitle);
                FillParameter(isAdultParameter, title.isAdult);
                FillParameter(startYearParameter, title.startYear);
                FillParameter(endYearParameter, title.endYear);
                FillParameter(runtimeMinutesParameter, title.runtimeMinutes);
                try
                {
                    sqlComm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(sqlComm.CommandText);
                    Console.ReadKey();
                }
            }
        }

        public void FillParameter(SqlParameter parameter, object? value)
        {
            if (value != null)
            {
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }
    }
}
