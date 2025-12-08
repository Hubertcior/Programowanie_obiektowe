using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;
using System.Globalization;


public class Student
{
    public string Imie { get; set; } 
    public string Nazwisko { get; set; } 
    public List<int> Oceny { get; set; } 
 
    public Student() { }

    public Student(string imie, string nazwisko, List<int> oceny)
    {
        Imie = imie;
        Nazwisko = nazwisko;
        Oceny = oceny;
    }

    public override string ToString()
    {
        return $"{Imie} {Nazwisko}, Oceny: {string.Join(", ", Oceny)}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Katalog roboczy programu: " + Directory.GetCurrentDirectory());
        Console.WriteLine("----------------------------------------------------------");
        Console.WriteLine("--- ZADANIE 2: Zapis tekstu ---");
        Zadanie2_ZapisTekstu("dane.txt");

        Console.WriteLine("\n--- ZADANIE 3: Odczyt tekstu ---");
        Zadanie3_OdczytTekstu("dane.txt");

        Console.WriteLine("\n--- ZADANIE 4: Dopisywanie tekstu ---");
        Zadanie4_DopisywanieTekstu("dane.txt");
        Console.WriteLine("Treść po dopisaniu:");
        Zadanie3_OdczytTekstu("dane.txt");

        List<Student> studenci = new List<Student>
        {
            new Student("Jan", "Kowalski", new List<int> { 3, 4, 5 }),
            new Student("Anna", "Nowak", new List<int> { 5, 5, 4 }),
            new Student("Piotr", "Wiśniewski", new List<int> { 2, 3, 3 })
        };

        Console.WriteLine("\n--- ZADANIE 6 i 7: JSON ---");
        Zadanie6_JSON_Zapis(studenci, "studenci.json");
        Zadanie7_JSON_Odczyt("studenci.json");

        Console.WriteLine("\n--- ZADANIE 8 i 9: XML ---");
        Zadanie8_XML_Zapis(studenci, "studenci.xml");
        Zadanie9_XML_Odczyt("studenci.xml");

        if (File.Exists("Iris.csv"))
        {
            Console.WriteLine("\n--- ZADANIE 10 i 11: CSV Statystyki ---");
            Zadanie10_11_CSV_OdczytStatystyki("Iris.csv");

            Console.WriteLine("\n--- ZADANIE 12: CSV Filtrowanie ---");
            Zadanie12_CSV_Filtrowanie("Iris.csv", "iris_filtered.csv");
        }
        else
        {
            Console.WriteLine("\nBrak pliku Iris.csv - pominięto zadania 10-12.");
        }

        Console.WriteLine("\nKoniec programu. Naciśnij klawisz...");
        Console.ReadKey();
    }


    static void Zadanie2_ZapisTekstu(string sciezka)
    {
        using (StreamWriter sw = new StreamWriter(sciezka))
        {
            Console.WriteLine("Wpisz 3 linie tekstu:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Linia {i + 1}: ");
                string tekst = Console.ReadLine();
                sw.WriteLine(tekst);
            }
        }
        Console.WriteLine("Dane zapisano.");
    }

    static void Zadanie3_OdczytTekstu(string sciezka)
    {
        if (File.Exists(sciezka))
        {
            string[] linie = File.ReadAllLines(sciezka);
            foreach (var linia in linie)
            {
                Console.WriteLine(linia);
            }
        }
        else
        {
            Console.WriteLine("Plik nie istnieje.");
        }
    }


    static void Zadanie4_DopisywanieTekstu(string sciezka)
    {
        // true w konstruktorze StreamWriter oznacza tryb 'append' (dopisywanie)
        using (StreamWriter sw = new StreamWriter(sciezka, true))
        {
            Console.WriteLine("Dopisz jedną linię tekstu:");
            string tekst = Console.ReadLine();
            sw.WriteLine(tekst);
        }
        Console.WriteLine("Dopisano linię.");
    }


    static void Zadanie6_JSON_Zapis(List<Student> lista, string sciezka)
    {
        string jsonString = JsonSerializer.Serialize(lista, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(sciezka, jsonString);
        Console.WriteLine("Zapisano studentów do JSON.");
    }


    static void Zadanie7_JSON_Odczyt(string sciezka)
    {
        if (!File.Exists(sciezka)) return;

        string jsonString = File.ReadAllText(sciezka);
        List<Student> wczytani = JsonSerializer.Deserialize<List<Student>>(jsonString);

        Console.WriteLine("Odczytano z JSON:");
        foreach (var s in wczytani)
        {
            Console.WriteLine(s.ToString());
        }
    }


    static void Zadanie8_XML_Zapis(List<Student> lista, string sciezka)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
        using (TextWriter writer = new StreamWriter(sciezka))
        {
            serializer.Serialize(writer, lista);
        }
        Console.WriteLine("Zapisano studentów do XML.");
    }


    static void Zadanie9_XML_Odczyt(string sciezka)
    {
        if (!File.Exists(sciezka)) return;

        XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
        using (FileStream fs = new FileStream(sciezka, FileMode.Open))
        {
            List<Student> wczytani = (List<Student>)serializer.Deserialize(fs);
            Console.WriteLine("Odczytano z XML:");
            foreach (var s in wczytani)
            {
                Console.WriteLine(s.ToString());
            }
        }
    }


    static void Zadanie10_11_CSV_OdczytStatystyki(string sciezka)
    {
        var linie = File.ReadAllLines(sciezka);

        var daneNumeryczne = new List<double[]>();

        Console.WriteLine("Pierwsze 5 wierszy pliku CSV:");
        for (int i = 0; i < Math.Min(linie.Length, 6); i++)
        {
            Console.WriteLine(linie[i]);
            if (i > 0) 
            {
                var wartosci = linie[i].Split(',');

                if (wartosci.Length >= 4)
                {
                    double[] wiersz = new double[4];
                    for (int j = 0; j < 4; j++)
                    {

                        if (double.TryParse(wartosci[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        {
                            wiersz[j] = val;
                        }
                    }
                    daneNumeryczne.Add(wiersz);
                }
            }
        }


        Console.WriteLine("\nŚrednie wartości kolumn numerycznych:");
        string[] nazwyKolumn = { "Sepal Length", "Sepal Width", "Petal Length", "Petal Width" };

        for (int kol = 0; kol < 4; kol++)
        {
            if (daneNumeryczne.Count > 0)
            {
                double suma = daneNumeryczne.Sum(row => row[kol]);
                double srednia = suma / daneNumeryczne.Count;
                Console.WriteLine($"{nazwyKolumn[kol]}: {srednia:F2}");
            }
        }
    }


    static void Zadanie12_CSV_Filtrowanie(string wejscie, string wyjscie)
    {
        var linie = File.ReadAllLines(wejscie);
        var wynikoweLinie = new List<string>();

        wynikoweLinie.Add("sepal length,sepal width,class");

        for (int i = 1; i < linie.Length; i++)
        {
            var cols = linie[i].Split(',');
            if (cols.Length >= 5)
            {
                if (double.TryParse(cols[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double sepalLength))
                {
                    if (sepalLength < 5.0)
                    {
                        string nowaLinia = $"{cols[0]},{cols[1]},{cols[4]}";
                        wynikoweLinie.Add(nowaLinia);
                    }
                }
            }
        }

        File.WriteAllLines(wyjscie, wynikoweLinie);
        Console.WriteLine($"\nWyfiltrowane dane zapisano do pliku {wyjscie} (Znaleziono: {wynikoweLinie.Count - 1} rekordów)");
    }
}