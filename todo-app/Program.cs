using System;
using System.Collections.Generic;

class TodoItem
{
    public int Id { get; }
    public string Text { get; private set; }
    public bool Done { get; private set; }

    public TodoItem(int id, string text)
    {
        Id = id;
        Text = text;
        Done = false;
    }

    public void Toggle() => Done = !Done;
    public void Edit(string text) => Text = text;
}

class Program
{
    static void Main()
    {
        var todos = new List<TodoItem>();
        int nextId = 1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("todo-app");
            Console.WriteLine($"Toplam: {todos.Count} | Bekleyen: {CountPending(todos)} | Biten: {CountDone(todos)}");
            Console.WriteLine("1) Ekle");
            Console.WriteLine("2) Tamamla / Geri al");
            Console.WriteLine("3) Duzenle");
            Console.WriteLine("4) Sil");
            Console.WriteLine("5) Tum liste");
            Console.WriteLine("6) Sadece bekleyenler");
            Console.WriteLine("7) Sadece bitenler");
            Console.WriteLine("0) Cikis");
            Console.Write("Secim: ");

            var choice = Console.ReadLine();

            if (choice == "0") break;

            if (choice == "1")
            {
                var text = ReadNonEmpty("Todo: ");
                todos.Add(new TodoItem(nextId++, text));
            }
            else if (choice == "2")
            {
                if (todos.Count == 0) { Pause("Liste bos."); continue; }
                PrintTodos(todos, 0);
                int id = ReadInt("ID: ");
                var item = FindById(todos, id);
                if (item == null) { Pause("Bulunamadi."); continue; }
                item.Toggle();
            }
            else if (choice == "3")
            {
                if (todos.Count == 0) { Pause("Liste bos."); continue; }
                PrintTodos(todos, 0);
                int id = ReadInt("ID: ");
                var item = FindById(todos, id);
                if (item == null) { Pause("Bulunamadi."); continue; }
                var text = ReadNonEmpty("Yeni metin: ");
                item.Edit(text);
            }
            else if (choice == "4")
            {
                if (todos.Count == 0) { Pause("Liste bos."); continue; }
                PrintTodos(todos, 0);
                int id = ReadInt("ID: ");
                int index = IndexOfId(todos, id);
                if (index < 0) { Pause("Bulunamadi."); continue; }
                todos.RemoveAt(index);
            }
            else if (choice == "5")
            {
                PrintTodos(todos, 0);
                Pause("");
            }
            else if (choice == "6")
            {
                PrintTodos(todos, 1);
                Pause("");
            }
            else if (choice == "7")
            {
                PrintTodos(todos, 2);
                Pause("");
            }
            else
            {
                Pause("Gecersiz secim.");
            }
        }
    }

    static int CountPending(List<TodoItem> todos)
    {
        int c = 0;
        for (int i = 0; i < todos.Count; i++)
            if (!todos[i].Done) c++;
        return c;
    }

    static int CountDone(List<TodoItem> todos)
    {
        int c = 0;
        for (int i = 0; i < todos.Count; i++)
            if (todos[i].Done) c++;
        return c;
    }

    static void PrintTodos(List<TodoItem> todos, int filter)
    {
        Console.WriteLine();
        if (todos.Count == 0) { Console.WriteLine("(bos)"); Console.WriteLine(); return; }

        for (int i = 0; i < todos.Count; i++)
        {
            var t = todos[i];
            if (filter == 1 && t.Done) continue;
            if (filter == 2 && !t.Done) continue;

            var mark = t.Done ? "x" : " ";
            Console.WriteLine($"{t.Id,3}  [{mark}]  {t.Text}");
        }
        Console.WriteLine();
    }

    static TodoItem FindById(List<TodoItem> todos, int id)
    {
        for (int i = 0; i < todos.Count; i++)
            if (todos[i].Id == id) return todos[i];
        return null;
    }

    static int IndexOfId(List<TodoItem> todos, int id)
    {
        for (int i = 0; i < todos.Count; i++)
            if (todos[i].Id == id) return i;
        return -1;
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim();
            if (int.TryParse(s, out int v) && v > 0)
                return v;
            Console.WriteLine("Gecersiz sayi.");
        }
    }

    static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim();
            if (s.Length > 0)
                return s;
            Console.WriteLine("Bos olamaz.");
        }
    }

    static void Pause(string message)
    {
        if (message.Length > 0) Console.WriteLine(message);
        Console.WriteLine("Devam icin Enter");
        Console.ReadLine();
    }
}
