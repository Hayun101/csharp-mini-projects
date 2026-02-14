using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "data.txt";
        var data = new List<string>();

        if (File.Exists(path))
        {
            using (var reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    data.Add(line);
            }
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("file-io");
            Console.WriteLine("1) Ekle");
            Console.WriteLine("2) Listele");
            Console.WriteLine("0) Cikis");
            Console.Write("Secim: ");

            var c = Console.ReadLine();

            if (c == "0")
                break;

            if (c == "1")
            {
                Console.Write("Metin: ");
                var text = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(text))
                    data.Add(text);
            }
            else if (c == "2")
            {
                Console.WriteLine();
                for (int i = 0; i < data.Count; i++)
                    Console.WriteLine($"{i + 1}. {data[i]}");

                Console.WriteLine();
                Console.WriteLine("Enter");
                Console.ReadLine();
            }
        }

        using (var writer = new StreamWriter(path))
        {
            for (int i = 0; i < data.Count; i++)
                writer.WriteLine(data[i]);
        }
    }
}
