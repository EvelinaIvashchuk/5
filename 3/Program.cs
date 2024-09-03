using System;
using System.Collections.Generic;
using System.Linq;

public class Dough
{
    private string _flour_type;
    private string _baking_technique;
    private double _weight;

    private static readonly double _base_calories_per_gram = 2.0;
    private static readonly double _white_flour_modifier = 1.5;
    private static readonly double _wholegrain_flour_modifier = 1.0;
    private static readonly double _crispy_technique_modifier = 0.9;
    private static readonly double _chewy_technique_modifier = 1.1;
    private static readonly double _homemade_technique_modifier = 1.0;

    public string FlourType
    {
        get => _flour_type;
        private set
        {
            if (value != "White" && value != "Wholegrain")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            _flour_type = value;
        }
    }

    public string BakingTechnique
    {
        get => _baking_technique;
        private set
        {
            if (value != "Crispy" && value != "Chewy" && value != "Homemade")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            _baking_technique = value;
        }
    }

    public double Weight
    {
        get => _weight;
        private set
        {
            if (value < 1 || value > 200)
            {
                throw new ArgumentException("Dough weight should be in the range [1..200].");
            }
            _weight = value;
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
        double flour_modifier = FlourType == "White" ? _white_flour_modifier : _wholegrain_flour_modifier;
        double technique_modifier = BakingTechnique == "Crispy" ? _crispy_technique_modifier
                              : BakingTechnique == "Chewy" ? _chewy_technique_modifier
                              : _homemade_technique_modifier;

        return _base_calories_per_gram * flour_modifier * technique_modifier;
    }

    public double CalculateCalories()
    {
        return GetCaloriesPerGram() * Weight;
    }
}

public class Topping
{
    private string _topping_type;
    private double _weight;

    private static readonly double _base_calories_per_gram = 2.0;
    private static readonly double _meat_modifier = 1.2;
    private static readonly double _vegetables_modifier = 0.8;
    private static readonly double _cheese_modifier = 1.1;
    private static readonly double _sauce_modifier = 0.9;

    public string ToppingType
    {
        get => _topping_type;
        private set
        {
            if (value != "Meat" && value != "Vegetables" && value != "Cheese" && value != "Sauce")
            {
                throw new ArgumentException($"Cannot place {value} on top of your pizza.");
            }
            _topping_type = value;
        }
    }

    public double Weight
    {
        get => _weight;
        private set
        {
            if (value < 1 || value > 50)
            {
                throw new ArgumentException($"{ToppingType} weight should be in the range [1..50].");
            }
            _weight = value;
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
            "Meat" => _meat_modifier,
            "Vegetables" => _vegetables_modifier,
            "Cheese" => _cheese_modifier,
            "Sauce" => _sauce_modifier,
            _ => throw new ArgumentException($"Cannot place {ToppingType} on top of your pizza.")
        };

        return _base_calories_per_gram * modifier;
    }

    public double CalculateCalories()
    {
        return GetCaloriesPerGram() * Weight;
    }
}

public class Pizza
{
    private string _name;
    private Dough _dough;
    private List<Topping> _toppings = new List<Topping>();

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 15)
            {
                throw new ArgumentException("Pizza name should be between 1 and 15 characters.");
            }
            _name = value;
        }
    }

    public int ToppingCount => _toppings.Count;

    public Pizza(string name, Dough dough)
    {
        Name = name;
        _dough = dough;
    }

    public void AddTopping(Topping topping)
    {
        if (ToppingCount >= 10)
        {
            throw new InvalidOperationException("Number of toppings should be in range [0..10].");
        }
        _toppings.Add(topping);
    }

    public Dough Dough
    {
        get => _dough;
        set => _dough = value;
    }

    public double CalculateTotalCalories()
    {
        double total_calories = _dough.CalculateCalories();
        total_calories += _toppings.Sum(t => t.CalculateCalories());
        return total_calories;
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
                string pizza_name = input;

                string[] dough_parts = Console.ReadLine().Split();
                if (dough_parts.Length != 4 || dough_parts[0] != "Dough")
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                string flour_type = dough_parts[1];
                string baking_technique = dough_parts[2];
                double dough_weight;

                if (!double.TryParse(dough_parts[3], out dough_weight))
                {
                    Console.WriteLine("Invalid weight format.");
                    continue;
                }

                Dough dough = new Dough(flour_type, baking_technique, dough_weight);

                Pizza pizza = new Pizza(pizza_name, dough);

                string topping_input;
                while ((topping_input = Console.ReadLine()) != "END")
                {
                    string[] topping_parts = topping_input.Split();
                    if (topping_parts.Length != 3 || topping_parts[0] != "Topping")
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }

                    string topping_type = topping_parts[1];
                    double topping_weight;

                    if (!double.TryParse(topping_parts[2], out topping_weight))
                    {
                        Console.WriteLine("Invalid weight format.");
                        continue;
                    }

                    Topping topping = new Topping(topping_type, topping_weight);

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

