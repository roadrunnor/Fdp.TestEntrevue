using System.Text;
namespace Fdp.TestEntrevue;
/// <summary>
/// Ce programme lit un fichier texte, applique un filtre sur les lignes et affiche le nombre de lignes correspondantes.
/// Ce code a été revu pour respecter les meilleures pratiques de développement en mode Azure DevOps.
/// </summary>
internal class Program
{
    /// <summary>
    /// Point d'entrée principal de l'application.
    /// </summary>
    /// <param name="args">Arguments de la ligne de commande : chemin du fichier et critère de filtre.</param>
    private static void Main(string[] args)
    {
        // Vérification des arguments : Vérification du nombre d'arguments pr s'assurer de la présence des valeurs. 
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Fdp.TestEntrevue.exe <C:\\Employee.txt> <Pascal>");
            return;
        }

        var filePath = args[0];
        var filter = args[1];

        try
        {
            /** 
             * Pourquoi renommer x ?
             * 
             * Un bon nom de variable améliore la lisibilité du code. (X) est un nom générique et ne donne aucune autre 
             * indication sur ce qu’il représente. 
             * 
             * Renommer en (filteredLines), ce nouveau nom est immédiat et qu'il s'agit des lignes du fichier qui correspondent au filtre appliqué.
             */

            //var x = GetData(ref filePath, ref filter);
            /**
             * Pourquoi supprimer ref ?
             * 
             * ref est utilisé pour passer un argument par référence, ce qui signifie que la méthode peut modifier
             * directement la valeur de cette variable. 
             * 
             * Dans ce ici-haut, la méthode GetData ne modifie ni filePath et ni filter, donc il n'est pas nécessaire de 
             * passer ces variables par référence. 
             * 
             * Cela simplifie l'API de la méthode et évite donc des modifications potentielles inattendues.
             * 
             * Ces petits changements améliorent la lisibilité et la maintenabilité du code, ce qui est particulièrement 
             * important dans un environnement de collaboration comme Azure DevOps.             
             */

            // Vérifier si le fichier est vide avant de le lire.
            if (new FileInfo(filePath).Length == 0)
            {
                Console.WriteLine($"Erreur : Le fichier {filePath} est vide.");
                return;
            }

            // Appel de la méthode GetData avec les arguments fournis.
            var filteredLines = GetData(filePath, filter);

            // Gestion du cas où aucune ligne ne correspond au filtre.
            if (filteredLines.Count == 0)
            {
                Console.WriteLine($"Aucune ligne trouvée dans le fichier {filePath} avec le filtre '{filter}'.");
            }
            else
            {
                // Affichage du nombre de lignes trouvées
                Console.WriteLine($"{filteredLines.Count} ligne(s) trouvée(s) dans le fichier {filePath} avec le filtre '{filter}'");

                // Affichage des lignes trouvées contenant "Pascal"
                Console.WriteLine("Voici les lignes correspondantes :");
                foreach (var line in filteredLines)
                {
                    Console.WriteLine(line);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            // Gestion spécifique pour les fichiers non trouvés.
            Console.WriteLine($"Erreur : Fichier non trouvé. Détails : {ex.Message}");
        }
        catch (UnauthorizedAccessException ex)
        {
            // Gestion spécifique pour les erreurs d'accès.
            Console.WriteLine($"Erreur : Accès refusé. Détails : {ex.Message}");
        }
        catch (Exception ex)
        {
            // Gestion générique pour toute autre exception.
            Console.WriteLine($"Une erreur inattendue s'est produite. Détails : {ex.Message}");
        }
    }

    /// <summary>
    /// Récupère les lignes d'un fichier texte correspondant au filtre fourni (insensible à la casse).
    /// </summary>
    /// <param name="filePath">Le chemin du fichier texte.</param>
    /// <param name="filter">Le critère de filtrage (insensible à la casse).</param>
    /// <returns>Une liste de lignes qui correspondent au filtre.</returns>
    public static IList<string> GetData(string filePath, string filter)
    {
        var result = new List<string>();

        // Utilisation d'un bloc "using" pour assurer que les ressources sont libérées, même en cas d'exception.
        using (var fileStream = File.OpenRead(filePath))
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string? line;

            // Lecture, ligne par ligne 
            while ((line = streamReader.ReadLine()) != null)
            {
                // Comparaison insensible à la case
                if (line.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(line);
                }
            }
            return result;
        }
    }

    /**
     * Simplicité et Robustesse :
     * Ce code a été rendu plus robuste en ajoutant des vérifications d'arguments et une meilleure gestion des erreurs. 
     * Cela permet d’éviter des comportements inattendus et des erreurs difficiles à diagnostiquer, surtout dans des environnements 
     * automatisés comme Azure DevOps.
     * 
     * Gestion des ressources :
     * En utilisant des blocs using, le programme s'assure que toutes les ressources (comme les fichiers ouverts) sont 
     * correctement fermées, évitant ainsi des problèmes de performance et de stabilité.
     * 
     * Performance :
     * Des techniques comme l'utilisation de StringComparison.OrdinalIgnoreCase pour comparer les chaînes de caractères 
     * permettent d'optimiser les performances. C'est particulièrement utile quand on travaille sur des fichiers volumineux 
     * ou dans des systèmes où les performances sont cruciales.
     * 
     * Gestion des erreurs explicite :
     * La gestion des erreurs est un élément essentiel dans tout projet déployé en production. Dans ce code, on a ajouté 
     * des exceptions spécifiques pour pouvoir mieux comprendre et corriger les erreurs, ce qui est crucial dans les environnements 
     * CI/CD comme Azure DevOps où les tâches sont souvent exécutées sans intervention humaine directe.
     * 
     * Sylvain Breton 
     */
}