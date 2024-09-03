using System;

public class Chicken
{
    private string _name;
    private int _age;
    private const int MinAge = 0;
    private const int MaxAge = 15;
    public Chicken(string name, int age)
    {
        Name = name;  
        Age = age; 
    }
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Contains(" "))
                throw new ArgumentException("Ім'я не може бути порожнім.");
            _name = value;
        }
    }
    public int Age
    {
        get => _age;
        private set
        {
            if (value < MinAge || value > MaxAge)
                throw new ArgumentException("Вік повинен бути в межах від 0 до 15.");
            _age = value;
        }
    }

    public int ProductPerDay => CalculateProductPerDay();

    public override string ToString()
    {
        return $"{Name} (age {Age}) can produce {ProductPerDay} eggs per day.";
    }

    private int CalculateProductPerDay()
    {
        if (Age < 1)
            return 0;
        if (Age < 3)
            return 1;
        if (Age < 7)
            return 2;
        return 3;
    }
}

public class Program
{
    public static void Main()
    {
        try
        {
            Console.WriteLine("Enter the name of the chicken:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter the age of the chicken:");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Invalid age input.");
                return;
            }

            Chicken chicken = new Chicken(name, age);

            Console.WriteLine(chicken);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
