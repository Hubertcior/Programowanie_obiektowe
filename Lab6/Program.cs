using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class Student
{
    public int student_id { get; set; }
    public string imie { get; set; } = "";
    public string nazwisko { get; set; } = "";
    public List<Ocena> Oceny { get; set; } = new();

    public override string ToString()
    {
        return $"{student_id}: {imie} {nazwisko}";
    }
}

public class Ocena
{
    public int ocena_id { get; set; }
    public double Wartosc { get; set; }
    public string Przedmiot { get; set; } = "";
    public int student_id { get; set; }
}

public class Program
{
    static string connectionString = "Data Source=10.200.2.28;" +
                                     "Initial Catalog=studenci_71461;" +
                                     "Integrated Security=True;" +
                                     "Encrypt=True;" +
                                     "TrustServerCertificate=True";

    public static void Main()
    {
        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            Console.WriteLine("Połączono z bazą.\n");

            

            
            Console.WriteLine("--- Zad 4: Wszyscy studenci ---");
            WyswietlWszystkichStudentow(connection);

           
            Console.WriteLine("\n--- Zad 5: Student o ID 1 ---");
            WypiszStudentaPoId(connection, 1);

            Console.WriteLine("\n--- Zad 7: Dodawanie studenta ---");
            Student nowyStudent = new Student { imie = "Jan", nazwisko = "Kowalski" };
            DodajStudenta(connection, nowyStudent);

            Console.WriteLine("\n--- Zad 8: Dodawanie oceny ---");
            DodajOcene(connection, new Ocena { Wartosc = 2.5, Przedmiot = "Matematyka", student_id = 1 });
            DodajOcene(connection, new Ocena { Wartosc = 4.5, Przedmiot = "Geografia", student_id = 1 });

            Console.WriteLine("\n--- Zad 10: Aktualizacja oceny ---");
            ZaktualizujOcene(connection, 1, 5.0); 

            Console.WriteLine("\n--- Zad 9: Usuwanie Geografii ---");
            UsunOcenyZGeografii(connection);

            Console.WriteLine("\n--- Zad 6: Lista studentów z ocenami ---");
            List<Student> studenci = PobierzStudentowZOcenami(connection);
            foreach (var s in studenci)
            {
                Console.WriteLine($"Student: {s.imie} {s.nazwisko} (ID: {s.student_id})");
                if (s.Oceny.Count > 0)
                {
                    foreach (var o in s.Oceny)
                    {
                        Console.WriteLine($"   - {o.Przedmiot}: {o.Wartosc}");
                    }
                }
                else
                {
                    Console.WriteLine("   - Brak ocen");
                }
            }

        }
        catch (Exception exc)
        {
            Console.WriteLine("\nWystąpił błąd krytyczny: " + exc.Message);
        }
    }

    public static void WyswietlWszystkichStudentow(SqlConnection connection)
    {
        string sql = "SELECT student_id, imie, nazwisko FROM Student";
        using SqlCommand command = new SqlCommand(sql, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["student_id"]} | {reader["imie"]} | {reader["nazwisko"]}");
        }
    }

    public static void WypiszStudentaPoId(SqlConnection connection, int id)
    {
        string sql = "SELECT imie, nazwisko FROM Student WHERE student_id = @id";
        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);

        using SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Console.WriteLine($"Znaleziono: {reader["imie"]} {reader["nazwisko"]}");
        }
        else
        {
            Console.WriteLine($"Nie znaleziono studenta o ID: {id}");
        }
    }

    public static List<Student> PobierzStudentowZOcenami(SqlConnection connection)
    {
        List<Student> listaStudentow = new List<Student>();

        string sqlStudenci = "SELECT student_id, imie, nazwisko FROM Student";
        using (SqlCommand cmdStu = new SqlCommand(sqlStudenci, connection))
        using (SqlDataReader rdrStu = cmdStu.ExecuteReader())
        {
            while (rdrStu.Read())
            {
                listaStudentow.Add(new Student
                {
                    student_id = (int)rdrStu["student_id"],
                    imie = rdrStu["imie"].ToString(),
                    nazwisko = rdrStu["nazwisko"].ToString()
                });
            }
        } 

        foreach (var s in listaStudentow)
        {
            string sqlOceny = "SELECT ocena_id, Wartosc, Przedmiot FROM Ocena WHERE student_id = @sid";
            using SqlCommand cmdOceny = new SqlCommand(sqlOceny, connection);
            cmdOceny.Parameters.AddWithValue("@sid", s.student_id);

            using SqlDataReader rdrOceny = cmdOceny.ExecuteReader();
            while (rdrOceny.Read())
            {
                s.Oceny.Add(new Ocena
                {
                    ocena_id = (int)rdrOceny["ocena_id"],
                    Wartosc = Convert.ToDouble(rdrOceny["Wartosc"]),
                    Przedmiot = rdrOceny["Przedmiot"].ToString(),
                    student_id = s.student_id
                });
            }
        }

        return listaStudentow;
    }

    public static void DodajStudenta(SqlConnection connection, Student student)
    {
        string sql = "INSERT INTO Student (imie, nazwisko) VALUES (@imie, @nazwisko)";
        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@imie", student.imie);
        command.Parameters.AddWithValue("@nazwisko", student.nazwisko);

        int rows = command.ExecuteNonQuery();
        Console.WriteLine($"Dodano studenta. Zmieniono wierszy: {rows}");
    }

    private static bool CzyOcenaPoprawna(double wartosc)
    {
   
        if (wartosc < 2.0 || wartosc > 5.0) return false;

        if (wartosc % 0.5 != 0) return false;

        if (wartosc == 2.5) return false;

        return true;
    }

    public static void DodajOcene(SqlConnection connection, Ocena ocena)
    {
        if (!CzyOcenaPoprawna(ocena.Wartosc))
        {
            Console.WriteLine($"BŁĄD: Ocena {ocena.Wartosc} jest niepoprawna! Nie dodano do bazy.");
            return;
        }

        string sql = "INSERT INTO Ocena (Wartosc, Przedmiot, student_id) VALUES (@val, @przedm, @sid)";
        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@val", ocena.Wartosc);
        command.Parameters.AddWithValue("@przedm", ocena.Przedmiot);
        command.Parameters.AddWithValue("@sid", ocena.student_id);

        try
        {
            int rows = command.ExecuteNonQuery();
            Console.WriteLine($"Dodano ocenę {ocena.Wartosc} z {ocena.Przedmiot}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd SQL przy dodawaniu oceny: " + ex.Message);
        }
    }


    public static void UsunOcenyZGeografii(SqlConnection connection)
    {
        string sql = "DELETE FROM Ocena WHERE Przedmiot = 'Geografia'";
        using SqlCommand command = new SqlCommand(sql, connection);

        int rows = command.ExecuteNonQuery();
        Console.WriteLine($"Usunięto oceny z Geografii. Liczba usuniętych ocen: {rows}");
    }

    public static void ZaktualizujOcene(SqlConnection connection, int ocena_id, double nowaWartosc)
    {
        if (!CzyOcenaPoprawna(nowaWartosc))
        {
            Console.WriteLine($"BŁĄD: Nowa wartość {nowaWartosc} jest niepoprawna! Anulowano edycję.");
            return;
        }

        string sql = "UPDATE Ocena SET Wartosc = @val WHERE ocena_id = @id";
        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@val", nowaWartosc);
        command.Parameters.AddWithValue("@id", ocena_id);

        int rows = command.ExecuteNonQuery();
        if (rows > 0)
            Console.WriteLine($"Zaktualizowano ocenę o ID {ocena_id} na {nowaWartosc}.");
        else
            Console.WriteLine($"Nie znaleziono oceny o ID {ocena_id}.");
    }
}