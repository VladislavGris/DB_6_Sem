using System.IO;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void WriteProcedure (string filename, string data)
    {
        if (!File.Exists(filename))
        {
            using (StreamWriter sw = File.CreateText(filename))
            {
                sw.WriteLine(data);
            }
            return;
        }
        using (StreamWriter sw = File.AppendText(filename))
        {
            sw.WriteLine(data);
        }
    }
}
