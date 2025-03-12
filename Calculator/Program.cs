using System;

namespace Calculator;

class Program
{
    static void Main()
    {
        var dbService = new DatabaseService();

        Console.WriteLine("Indtast et regneudtryk (f.eks. 5 + 3):");
        string expression = Console.ReadLine();

        Console.WriteLine("Indtast resultatet:");
        if (double.TryParse(Console.ReadLine(), out double result))
        {
            dbService.SaveCalculation(expression, result);
            Console.WriteLine("✅ Beregning gemt!");

            Console.WriteLine("\n🔍 Historik:");
            foreach (var entry in dbService.GetHistory())
            {
                Console.WriteLine($"{entry.Expression} = {entry.Result} (Oprettet: {entry.CreatedAt})");
            }
        }
        else
        {
            Console.WriteLine("⚠️ Ugyldigt resultat.");
        }
    }
}