using System;
using System.Collections.Generic;

public class Store {
    private string storeName;
    private double balance;
    private readonly List<Food> inventory;

    public Store(string storeName, double balance, List<Food> inventory) {
        StoreName = storeName;
        Balance = balance;
        this.inventory = inventory;
    }

    public Store(string storeName, double balance) {
        StoreName = storeName;
        Balance = balance;
    }

    public override string ToString() {
        return $"Store Name: {storeName}\n{Inventory}";
    }

    // If the user wants to view all the avaliable foods
    public void ListFoods() {
        List<string> categories = new();
        Console.WriteLine("===============[ Store Inventory ]================");

        foreach (var food in Inventory) {
            if (categories.Contains(food.Category) == false) {
                categories.Add(food.Category);
                Console.WriteLine($"\n---[ {food.Category} ]---");
                Console.WriteLine($"{food}");
            } else {
                Console.WriteLine($"{food}");
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    public void Greeting(double balance, int age, string storeName, int cartItems) {
        Console.WriteLine("=========[ Grocery Shopping Application ]=========");
        Console.WriteLine($"\tYou are shopping at...\t{storeName}\n");
        Console.WriteLine($"> Items in cart: {cartItems}");
        Console.WriteLine($"> Balance: {balance:$#,##0.00}");
        Console.WriteLine($"> Age: {age}");
        Console.WriteLine("==================================================");
    }

    public void PrintReciept(User user) {

    }
    
    public string StoreName {
        get => storeName;
        set {
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid store name! Store name was set to 'NONE' instead.");
                storeName = "NONE";
            }

            storeName = value;
        }
    }

    public double Balance {
        get => balance;
        set {
            // Set the balance to '0' if the input is empty/null.
            if (value.GetType() != typeof(double)) {
                Console.WriteLine("[!] Invalid balance! Balance was set to '0' instead.");
                balance = 0;
                // Set the balance to '0' if the balance is less than 0.
            } else if (value < 0) {
                Console.WriteLine("[!] The balance can't be less than $0! Balance was set to '0.0' instead.");
                balance = 0;
            }

            balance = value;
        }
    }

    public List<Food> Inventory {
        get => inventory;
    }
}