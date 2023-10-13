using IMDBDataImporter;
using System.Collections.Immutable;
using System.Data.SqlClient;

string ConnString = "server=localhost;database=IMDB;" +
            "user id=sa;password=imdbDaBomb;TrustServerCertificate=True";

List<Title> titles = new List<Title>();

foreach (string line in
    System.IO.File.ReadLines
    (@"C:\temp\title.basics.tsv\data.tsv")
    .Skip(1).Take(10000))
{
    string[] values = line.Split("\t");
    if (values.Length == 9)
    {
        titles.Add(new Title(
            values[0], values[1], values[2], values[3],
            ConvertToBool(values[4]), ConvertToInt(values[5]),
            ConvertToInt(values[6]), ConvertToInt(values[7]), 
            values[8]
            ));
    }
}

Console.WriteLine(titles.Count);

Console.WriteLine("Hvad vil du?");
Console.WriteLine("1 for delete all");
Console.WriteLine("2 for normal insert");
Console.WriteLine("3 for prepared insert");
Console.WriteLine("4 for bulk insert");
string input = Console.ReadLine();

DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

IInserter? myInserter = null;

switch (input)
{
    case "1":
        SqlCommand cmd = new SqlCommand(
            "DELETE FROM Titles; " +
            "DELETE FROM Genres; " +
            "DELETE FROM TitleGenres;", sqlConn);
        cmd.ExecuteNonQuery();
        break;
    case "2":
        myInserter = new NormalInserter();
        break;
    case "3":
        myInserter = new PreparedInserter();
        break;
    case "4":
        myInserter = new BulkInserter();
        break;
}

if (myInserter != null)
{
    myInserter.InsertData(sqlConn, titles);
    GenreInserter.InsertGenres(sqlConn, titles);
}

sqlConn.Close();

DateTime after = DateTime.Now;

Console.WriteLine("Tid: " + (after - before));


bool ConvertToBool(string input)
{
    if (input == "0")
    {
        return false;
    }
    else if (input == "1")
    {
        return true;
    }
    throw new ArgumentException(
        "Kolonne er ikke 0 eller 1, men " + input);
}

int? ConvertToInt(string input)
{
    if (input.ToLower() == @"\n")
    {
        return null;
    }
    else
    {
        return int.Parse(input);
    }

}