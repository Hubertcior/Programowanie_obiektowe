class Program
{
    public static void Main()
    {
        Console.WriteLine("To jest cwiczenie 1");

        Zwierze[] tablicaZwierzat = new Zwierze[4];
        
        for(int i = 0; i <= 2; i++)
        {
            Console.WriteLine("Podaj nazwę Zwierzęcia");
            string nazwa = Console.ReadLine();
            Console.WriteLine("Podaj gatunek Zwierzęcia");
            string gatunek = Console.ReadLine();
            Console.WriteLine("Podaj wiek Zwierzęcia");
            int wiek = int.Parse(Console.ReadLine());

            tablicaZwierzat[i] = new Zwierze(nazwa, gatunek, wiek); 
        }

        tablicaZwierzat[3] = new Zwierze(tablicaZwierzat[2]);

        tablicaZwierzat[3].Name = "Krowaa";

        for (int i = 0; i <= tablicaZwierzat.Length - 1; i++) {
            Console.WriteLine("Nazwa :" + tablicaZwierzat[i].Name);
            Console.WriteLine("Gatunek: " + tablicaZwierzat[i].Species);
            Console.WriteLine("Wiek: " + tablicaZwierzat[i].Age);

            tablicaZwierzat[i].daj_glos();
        }

        Zwierze.liczbaZwierzat();
    }
}

class Zwierze
{
    private string name;
    public string Name {get { return name; } set { name = value; }}

    private string species;
    public string Species { get { return species; } set { species = value;}}

    private int age;
    public int Age { get { return age; } set { age = value; }}

    private static int animalCount = 0;
    public int AnimalCount { get { return animalCount; } 
    }

    //konstruktor
    public Zwierze(string name, string species, int age)
    {
        this.Name = name;
        this.Species = species;
        this.Age = age;

        animalCount++;
    }

    //konstruktor domyślny
    public Zwierze() : this("Rex", "Pies", 4) 
    { 
    
    }

    //kontruktor kopiujący
    public Zwierze(Zwierze original) : this(original.Name, original.Species, original.Age)
    {

    }
    //metoda zwracająca aktualną liczbę zwierząt
    public static int liczbaZwierzat()
    {
        return animalCount;
    }
    
    public void daj_glos()
    {
        if(species == "Pies")
        {
            Console.WriteLine("Wof Wof!");
        }
        else if (species == "Kot")
        {
            Console.WriteLine("Miau Miau!");
        }
        else if (species == "Krowa")
        {
            Console.WriteLine("Muuuu!");
        }
        else
        {
            Console.WriteLine("IDK");
        }
    }
}