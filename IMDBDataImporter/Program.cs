using IMDBDataImporter;
using System.Collections.Immutable;
using System.Data.SqlClient;

string ConnString = "server=localhost;database=IMDB;" +
            "user id=sa;password=imdbDaBomb;TrustServerCertificate=True";

List<Title> titles = new List<Title>();

foreach (string line in
    System.IO.File.ReadLines
    (@"C:\temp\title.basics.tsv\data.tsv")
    .Skip(1).Take(50000))
{
    string[] values = line.Split("\t");
    if (values.Length == 9)
    {
        titles.Add(new Title(
            values[0], values[1], values[2], values[3],
            ConvertToBool(values[4]), ConvertToInt(values[5]),
            ConvertToInt(values[6]), ConvertToInt(values[7])
            ));
    }
}

Console.WriteLine(titles.Count);

DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

IInserter myInserter = new PreparedInserter();
myInserter.InsertData(sqlConn, titles);

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