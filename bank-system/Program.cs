using System;
using System.Collections.Generic;

class Account
{
    public int Id { get; }
    public string Owner { get; }
    public decimal Balance { get; private set; }

    public Account(int id, string owner)
    {
        Id = id;
        Owner = owner;
        Balance = 0m;
    }

    public bool Deposit(decimal amount)
    {
        if (amount <= 0) return false;
        Balance += amount;
        return true;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0) return false;
        if (amount > Balance) return false;
        Balance -= amount;
        return true;
    }
}

class Bank
{
    private readonly List<Account> _accounts;
    private int _nextId;

    public Bank()
    {
        _accounts = new List<Account>();
        _nextId = 1;
    }

    public Account CreateAccount(string owner)
    {
        var acc = new Account(_nextId++, owner);
        _accounts.Add(acc);
        return acc;
    }

    public Account Find(int id)
    {
        for (int i = 0; i < _accounts.Count; i++)
            if (_accounts[i].Id == id) return _accounts[i];
        return null;
    }

    public List<Account> All() => _accounts;

    public bool Transfer(int fromId, int toId, decimal amount)
    {
        if (amount <= 0) return false;
        if (fromId == toId) return false;

        var from = Find(fromId);
        var to = Find(toId);

        if (from == null || to == null) return false;
        if (!from.Withdraw(amount)) return false;

        to.Deposit(amount);
        return true;
    }
}

class Program
{
    static void Main()
    {
        var bank = new Bank();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("bank-system");
            Console.WriteLine("1) Hesap ac");
            Console.WriteLine("2) Para yatir");
            Console.WriteLine("3) Para cek");
            Console.WriteLine("4) Transfer");
            Console.WriteLine("5) Hesaplari listele");
            Console.WriteLine("0) Cikis");
            Console.Write("Secim: ");

            var choice = Console.ReadLine();

            if (choice == "0")
                break;

            if (choice == "1")
            {
                var owner = ReadNonEmpty("Hesap sahibi: ");
                var acc = bank.CreateAccount(owner);
                Pause($"Hesap acildi. ID: {acc.Id}");
            }
            else if (choice == "2")
            {
                if (bank.All().Count == 0) { Pause("Hesap yok."); continue; }

                ListAccounts(bank);
                int id = ReadInt("Hesap ID: ");
                var acc = bank.Find(id);
                if (acc == null) { Pause("Bulunamadi."); continue; }

                decimal amount = ReadDecimal("Miktar: ");
                if (!acc.Deposit(amount)) { Pause("Gecersiz miktar."); continue; }

                Pause("Para yatirildi.");
            }
            else if (choice == "3")
            {
                if (bank.All().Count == 0) { Pause("Hesap yok."); continue; }

                ListAccounts(bank);
                int id = ReadInt("Hesap ID: ");
                var acc = bank.Find(id);
                if (acc == null) { Pause("Bulunamadi."); continue; }

                decimal amount = ReadDecimal("Miktar: ");
                if (!acc.Withdraw(amount)) { Pause("Islem basarisiz (miktar/bakiye)."); continue; }

                Pause("Para cekildi.");
            }
            else if (choice == "4")
            {
                if (bank.All().Count < 2) { Pause("Transfer icin en az 2 hesap lazim."); continue; }

                ListAccounts(bank);
                int fromId = ReadInt("Gonderen ID: ");
                int toId = ReadInt("Alici ID: ");
                decimal amount = ReadDecimal("Miktar: ");

                if (!bank.Transfer(fromId, toId, amount))
                {
                    Pause("Transfer basarisiz.");
                    continue;
                }

                Pause("Transfer tamam.");
            }
            else if (choice == "5")
            {
                ListAccounts(bank);
                Pause("");
            }
            else
            {
                Pause("Gecersiz secim.");
            }
        }
    }

    static void ListAccounts(Bank bank)
    {
        Console.WriteLine();
        var list = bank.All();
        if (list.Count == 0)
        {
            Console.WriteLine("(bos)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            var a = list[i];
            Console.WriteLine($"{a.Id,3}  {a.Owner,-15}  Bakiye: {a.Balance:0.00}");
        }
        Console.WriteLine();
    }

    static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim();
            if (s.Length > 0) return s;
            Console.WriteLine("Bos olamaz.");
        }
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim();
            if (int.TryParse(s, out int v) && v > 0) return v;
            Console.WriteLine("Gecersiz sayi.");
        }
    }

    static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
            if (decimal.TryParse(s, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out decimal v) && v > 0)
                return v;
            Console.WriteLine("Gecersiz miktar.");
        }
    }

    static void Pause(string message)
    {
        if (message.Length > 0) Console.WriteLine(message);
        Console.WriteLine("Devam icin Enter");
        Console.ReadLine();
    }
}
