using System;
using System.Collections.Generic;
using System.Linq;

public class Dough
{
    private string flourType;
    private string bakingTechnique;
    private double weight;

    private static readonly double BaseCaloriesPerGram = 2.0;
    private static readonly double WhiteFlourModifier = 1.5;
    private static readonly double WholegrainFlourModifier = 1.0;
    private static readonly double CrispyTechniqueModifier = 0.9;
    private static readonly double ChewyTechniqueModifier = 1.1;
    private static readonly double HomemadeTechniqueModifier = 1.0;

    public string FlourType
    {
        get => flourType;
        private set
        {
            if (value != "White" && value != "Wholegrain")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            flourType = value;
        }
    }

    public string BakingTechnique
    {
        get => bakingTechnique;
        private set
        {
            if (value != "Crispy" && value != "Chewy" && value != "Homemade")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            bakingTechnique = value;
        }
    }

    public double Weight
    {
        get => weight;
        private set
        {
            if (value < 1 || value > 200)
            {
                throw new ArgumentException("Dough weight should be in the range [1..200].");
            }
            weight = value;
        }
    }

    public Dough(string flourType, string bakingTechnique, double weight)
    {
        FlourType = flourType;
        BakingTechnique = bakingTechnique;
        Weight = weight;
    }

    private double GetCaloriesPerGram()
    {
        double flourModifier = FlourType == "White" ? WhiteFlourModifier : WholegrainFlourModifier;
        double techniqueModifier = BakingTechnique == "Crispy" ? CrispyTechniqueModifier
                              : BakingTechnique == "Chewy" ? ChewyTechniqueModifier
                              : HomemadeTechniqueModifier;

        return BaseCaloriesPerGram * flourModifier * techniqueModifier;
    }

    public double CalculateCalories()
    {
        return GetCaloriesPerGram() * Weight;
    }
}

public class Topping
{
    private string toppingType;
    private double weight;

    private static readonly double BaseCaloriesPerGram = 2.0;
    private static readonly double MeatModifier = 1.2;
    private static readonly double VegetablesModifier = 0.8;
    private static readonly double CheeseModifier = 1.1;
    private static readonly double SauceModifier = 0.9;

    public string ToppingType
    {
        get => toppingType;
        private set
        {
            if (value != "Meat" && value != "Vegetables" && value != "Cheese" && value != "Sauce")
            {
                throw new ArgumentException($"Cannot place {value} on top of your pizza.");
            }
            toppingType = value;
        }
    }

    public double Weight
    {
        get => weight;
        private set
        {
            if (value < 1 || value > 50)
            {
                throw new ArgumentException($"{ToppingType} weight should be in the range [1..50].");
            }
            weight = value;
        }
    }

    public Topping(string toppingType, double weight)
    {
        ToppingType = toppingType;
        Weight = weight;
    }

    private double GetCaloriesPerGram()
    {
        double modifier = ToppingType switch
        {
            "Meat" => MeatModifier,
            "Vegetables" => VegetablesModifier,
            "Cheese" => CheeseModifier,
            "Sauce" => SauceModifier,
            _ => throw new ArgumentException($"Cannot place {ToppingType} on top of your pizza.")
        };

        return BaseCaloriesPerGram * modifier;
    }

    public double CalculateCalories()
    {
        return GetCaloriesPerGram() * Weight;
    }
}

public class Pizza
{
    private string name;
    private Dough dough;
    private List<Topping> toppings = new List<Topping>();

    public string Name
    {
        get => name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 15)
            {
                throw new ArgumentException("Pizza name should be between 1 and 15 characters.");
            }
            name = value;
        }
    }

    public int ToppingCount => toppings.Count;

    public Pizza(string name, Dough dough)
    {
        Name = name;
        this.dough = dough;
    }

    public void AddTopping(Topping topping)
    {
        if (ToppingCount >= 10)
        {
            throw new InvalidOperationException("Number of toppings should be in range [0..10].");
        }
        toppings.Add(topping);
    }

    public Dough Dough
    {
        get => dough;
        set => dough = value;
    }

    public double CalculateTotalCalories()
    {
        double totalCalories = dough.CalculateCalories();
        totalCalories += toppings.Sum(t => t.CalculateCalories());
        return totalCalories;
    }
}

public class Program
{
    public static void Main()
    {
        string input;
        while ((input = Console.ReadLine()) != "END")
        {
            try
            {
                string pizzaName = input;

                string[] doughParts = Console.ReadLine().Split();
                if (doughParts.Length != 4 || doughParts[0] != "Dough")
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                string flourType = doughParts[1];
                string bakingTechnique = doughParts[2];
                double doughWeight;

                if (!double.TryParse(doughParts[3], out doughWeight))
                {
                    Console.WriteLine("Invalid weight format.");
                    continue;
                }

                Dough dough = new Dough(flourType, bakingTechnique, doughWeight);

                Pizza pizza = new Pizza(pizzaName, dough);

                string toppingInput;
                while ((toppingInput = Console.ReadLine()) != "END")
                {
                    string[] toppingParts = toppingInput.Split();
                    if (toppingParts.Length != 3 || toppingParts[0] != "Topping")
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }

                    string toppingType = toppingParts[1];
                    double toppingWeight;

                    if (!double.TryParse(toppingParts[2], out toppingWeight))
                    {
                        Console.WriteLine("Invalid weight format.");
                        continue;
                    }

                    Topping topping = new Topping(toppingType, toppingWeight);

                    pizza.AddTopping(topping);
                }

                Console.WriteLine($"{pizza.Name} - {pizza.CalculateTotalCalories():F2} Calories.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

