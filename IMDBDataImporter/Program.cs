using IMDBDataImporter;

List<Title> titles = new List<Title>();

foreach (string line in
    System.IO.File.ReadLines
    (@"C:\temp\title.basics.tsv\data.tsv")
    .Skip(1).Take(1000))
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

NormalInserter myInserter = new NormalInserter();
myInserter.InsertData(titles);

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