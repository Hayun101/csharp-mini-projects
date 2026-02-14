using System;
using System.Collections.Generic;

class Student
{
    public int Id { get; }
    public string Name { get; private set; }
    public List<int> Grades { get; }

    public Student(int id, string name)
    {
        Id = id;
        Name = name;
        Grades = new List<int>();
    }

    public void AddGrade(int grade)
    {
        Grades.Add(grade);
    }

    public double Average()
    {
        if (Grades.Count == 0) return 0;
        int sum = 0;
        for (int i = 0; i < Grades.Count; i++)
            sum += Grades[i];
        return (double)sum / Grades.Count;
    }
}

class Program
{
    static void Main()
    {
        var students = new List<Student>();
        int nextId = 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("student-oop");
            Console.WriteLine("1) Ogrenci ekle");
            Console.WriteLine("2) Not ekle");
            Console.WriteLine("3) Ogrencileri listele");
            Console.WriteLine("0) Cikis");
            Console.Write("Secim: ");

            var choice = Console.ReadLine();

            if (choice == "0")
                break;

            if (choice == "1")
            {
                Console.Write("Isim: ");
                var name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                    students.Add(new Student(nextId++, name.Trim()));
            }
            else if (choice == "2")
            {
                if (students.Count == 0)
                {
                    Pause("Ogrenci yok.");
                    continue;
                }

                ListStudents(students);
                int id = ReadInt("Ogrenci ID: ");
                var student = FindById(students, id);
                if (student == null)
                {
                    Pause("Bulunamadi.");
                    continue;
                }

                int grade = ReadInt("Not (0-100): ", 0, 100);
                student.AddGrade(grade);
            }
            else if (choice == "3")
            {
                ListStudents(students);
                Pause("");
            }
        }
    }

    static void ListStudents(List<Student> students)
    {
        Console.WriteLine();
        for (int i = 0; i < students.Count; i++)
        {
            var s = students[i];
            Console.WriteLine($"{s.Id,3}  {s.Name,-15}  Ortalama: {s.Average():0.00}");
        }
        Console.WriteLine();
    }

    static Student FindById(List<Student> students, int id)
    {
        for (int i = 0; i < students.Count; i++)
            if (students[i].Id == id) return students[i];
        return null;
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int v) && v > 0)
                return v;
            Console.WriteLine("Gecersiz sayi.");
        }
    }

    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max)
                return v;
            Console.WriteLine("Gecersiz sayi.");
        }
    }

    static void Pause(string message)
    {
        if (message.Length > 0)
            Console.WriteLine(message);
        Console.WriteLine("Devam icin Enter");
        Console.ReadLine();
    }
}
