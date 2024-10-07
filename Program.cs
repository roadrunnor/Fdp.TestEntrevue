using System.Text;
namespace Fdp.TestEntrevue;
/// <summary>
/// Il n'y a pas de bogue fonctionnel dans ce code.
/// Tu dois faire une révision de code et nous présenter le tout à notre prochaine rencontre.
/// </summary>
internal class Program
{
    // Fdp.TestEntrevue.exe C:\Employee.txt Pascal
    private static void Main(string[] args)
    {
        var filePath = args[0];
        var filtre = args[1];
        var x = GetData(ref filePath, ref filtre);
        Console.WriteLine($"{x.Count} ligne(s) trouvée(s) dans le fichier {filePath} avec le filtre {filtre}");
    }

    public static IList<string> GetData(ref string filePath, ref string
    filtre)
    {
        var result = new List<string>();
        const int BufferSize = 128;
        var fileStream = File.OpenRead(filePath);
        try
        {
            var streamReader = new StreamReader(fileStream, Encoding.UTF8,
            true, BufferSize);
            string? line;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.ToUpper().Contains(filtre.ToUpper()))
                {
                    result.Add(line);
                }
            }
        } catch
        {
            throw;
        }

        return result.ToList();
    }
}