using System.Data.SqlClient;

namespace IMDBDataImporter
{
    public interface IInserter
    {
        void InsertData(SqlConnection sqlConn, List<Title> titles);
    }
}