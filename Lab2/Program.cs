public class Zwierze
{
    private protected string _name; 
    public string Name { get { return _name; } set { _name = value; } }
    
    public Zwierze(string name)
    {
        this.Name = name;
    }

    public virtual void daj_glos()
    {
        Console.WriteLine("Daje głos");
    }

}

public class Pies : Zwierze
{
    public Pies(string name) : base(name) { }
    public override void daj_glos()
    {
        Console.WriteLine("Woof Woof!");
    }
}

public class Kot : Zwierze
{
    public Kot(string name) : base(name) { }
    public override void daj_glos() 
    {
        Console.WriteLine("Miau Miau!");
    }
}

public class Waz : Zwierze
{
    public Waz(string name) : base(name) { }
    public override void daj_glos()
    {
        Console.WriteLine("SSSSS");
    }
}

public abstract class Pracownik
{
    public abstract void Pracuj();
}

public class Piekarz : Pracownik
{
    public override void Pracuj()
    {
        Console.WriteLine("Trwa pieczenie...");
    }
}

public class A
{
    public A()
    {
        Console.WriteLine("To jest konstruktor A");
    }
}

public class B : A
{
    public B() : base()
    {
        Console.WriteLine("To jest konstruktor B");
    }
}

public class C : B
{
    public C() : base()
    {
        Console.WriteLine("To jest konstruktor C");
    }
}



class Program
{
    public static void powiedz_cos(Zwierze jakiesZwierze)
    {
        Console.WriteLine("Typ Klasy Zwierze: " + jakiesZwierze.GetType());
        jakiesZwierze.daj_glos();
    }

    public static void Main(string[] args)
    {
        Zwierze zwierze = new Zwierze("Ptak");
        Pies pies = new Pies("Piesek");
        Kot kot = new Kot("Kotek");
        Waz waz = new Waz("Wezyk");

        Piekarz piekarz = new Piekarz();

        A a = new A();
        B b = new B();
        C c = new C();

        Console.WriteLine("Dzialam xD");

        powiedz_cos(waz);
        piekarz.Pracuj();
    }
}