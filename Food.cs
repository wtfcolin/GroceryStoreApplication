using System;

public class Food {
    private string name; // Name of food item
    private int quantity; // Quantity of the food item in the store
    private string category; // Type of food for potential categorization
    private double price; // Price for the food item (per 1)
    private int calories; // Calories for the food item (per 1)

    public Food(string name, int quantity, string category, double price, int calories) {
        Name = name;
        Quantity = quantity;
        Category = category;
        Price = price;
        Calories = calories;
    }

    public override string ToString() {
        return $"===[ {Name} ]===\nCategory: {Category}\nCalories: {Calories}\n\nPrice: ${Price}\n({Quantity}) in stock";
    }

    public string Name {
        get => name;
        set {
            // Set name to NONE if the input is empty/null.
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid Name! Name was set to 'PLACEHOLDER' instead.");
                name = "NONE";
            } else {
                name = value;
            }
        }
    }

    public int Quantity {
        get => quantity;
        set {
            // Set quantity to 0 if the input is not an integer
            if (value.GetType() != typeof(int)) {
                Console.WriteLine("[!] Invalid Quantity! Quantity was set to '0' instead.");
                quantity = 0;
                // Set quantity to 0 if the input is less than 0
            } else if (value < 0) {
                Console.WriteLine("[!] There can't be less than 0 items! Quantity was set to '0' instead.");
                quantity = 0;
            } else {
                quantity = value;
            }
        }
    }

    public string Category {
        get => category;
        set {
            // Set the category to NONE if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid Category! Category was set to '");
            } else {
                category = value;
            }
        }
    }

    public double Price {
        get => price;
        set {
            // Set the price to 0 if the input is not a double
            if (value.GetType() != typeof(double)) {
                Console.WriteLine("[!] Invalid Price! Price was set to '0.0' instead.");
                price = 0.0;
                // Set the price to 0 if the input is less than 0
            } else if (value < 0.0) {
                Console.WriteLine("[!] The price can't be less than $0! Price was set to '0.0' instead.");
                price = 0.0;
            } else {
                price = value;
            }
        }
    }

    public int Calories {
        get => calories;
        set {
            // Set the calories to 0 if the input is not an integer
            if (value.GetType() != typeof(int)) {
                Console.WriteLine("[!] Invalid Calories! Calories was set to '0' instead.");
                calories = 0;
            } else if (value < 0) {
                calories = value;
            } else {
                calories = value;
            }
        }
    }
}