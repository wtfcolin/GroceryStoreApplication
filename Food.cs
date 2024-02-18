using System;

public class Item {
    private string name; // Name of item item
    private int quantity; // Quantity of the item item in the store
    private string category; // Type of item for potential categorization
    private double price; // Price for the item item (per 1)
    private int calories; // Calories for the item item (per 1)
    /*
     * Constructor for the 'Item' object
     * - name (string): Name of the item
     * - quantity (int): Amount of the item
     * - category (string): Type of item that object belongs to
     * - price (double): Price per item 
     * - calories (int): Claories per item
     */
    public Item(string name, int quantity, string category, double price, int calories) {
        Name = name;
        Quantity = quantity;
        Category = category;
        Price = price;
        Calories = calories;
    }
    // Override function that shows information about the item
    public override string ToString() {
        if (Name.Length > 16) {
            return $"- {Name} ({Quantity})\t\t\t|\t Price: {Price:$#,##0.00}\t|\tCategory: {Category}\t|\tCalories: {Calories}";
        } else if (Name.Length > 8) {
            return $"- {Name} ({Quantity})\t\t\t|\t Price: {Price:$#,##0.00}\t|\tCategory: {Category}\t|\tCalories: {Calories}";
        } else {
            return $"- {Name} ({Quantity})\t|\t Price: {Price:$#,##0.00}\t|\tCategory: {Category}\t|\tCalories: {Calories}";
        }
    }
    public string Name {
        get => name;
        set {
            // Set name to NONE if the input is empty/null.
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid Name! Name was set to 'PLACEHOLDER' instead.");
                name = "NONE";
            }
            
            name = value;
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
            }
            
            quantity = value;
        }
    }
    public string Category {
        get => category;
        set {
            // Set the category to NONE if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid Category! Category was set to 'NONE' instead.");
                category = "NONE";
            }
            
            category = value;
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
            }
            
            price = value;
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
                Console.WriteLine("[!] The calories can't be less than 0! Calories was set to '0' instead.");
                calories = 0;
            }
            
            calories = value;
        }
    }
}