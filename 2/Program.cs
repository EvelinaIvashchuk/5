using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
    public string Name { get; }
    public decimal Price { get; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

public class Person
{
    private string _name;
    private decimal _money;
    private List<Product> _bag;

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Ім'я не може бути порожнім");
            _name = value;
        }
    }

    public decimal Money
    {
        get => _money;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Гроші не можуть бути від'ємними");
            _money = value;
        }
    }

    public List<Product> Bag => _bag;

    public Person(string name, decimal money)
    {
        Name = name;
        Money = money;
        _bag = new List<Product>();
    }

    public void Buy_Product(Product product)
    {
        if (Money >= product.Price)
        {
            Money -= product.Price;
            _bag.Add(product);
            Console.WriteLine($"{Name} bought {product.Name}");
        }
        else
        {
            Console.WriteLine($"{Name} can't afford {product.Name}");
        }
    }
}

public class Program
{
    public static void Main()
    {
        try
        {
            var people = new Dictionary<string, Person>();
            var products = new Dictionary<string, Product>();

            string[] person_Inputs = Console.ReadLine().Split(';');
            foreach (var input in person_Inputs)
            {
                if (string.IsNullOrWhiteSpace(input)) continue;
                string[] parts = input.Split('=');
                string name = parts[0];
                decimal money = decimal.Parse(parts[1]);
                var person = new Person(name, money);
                people[name] = person;
            }

            string[] product_Inputs = Console.ReadLine().Split(';');
            foreach (var input in product_Inputs)
            {
                if (string.IsNullOrWhiteSpace(input)) continue;
                string[] parts = input.Split('=');
                string name = parts[0];
                decimal price = decimal.Parse(parts[1]);
                var product = new Product(name, price);
                products[name] = product;
            }

            string command;
            while ((command = Console.ReadLine()) != "END")
            {
                string[] command_Parts = command.Split(' ');
                string person_Name = command_Parts[0];
                string product_Name = command_Parts[1];

                if (people.ContainsKey(person_Name) && products.ContainsKey(product_Name))
                {
                    Person person = people[person_Name];
                    Product product = products[product_Name];
                    person.Buy_Product(product);
                }
            }

            foreach (var person in people.Values)
            {
                if (person.Bag.Count > 0)
                {
                    Console.WriteLine($"{person.Name} - {string.Join(", ", person.Bag.Select(p => p.Name))}");
                }
                else
                {
                    Console.WriteLine($"{person.Name} – Nothing bought");
                }
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
