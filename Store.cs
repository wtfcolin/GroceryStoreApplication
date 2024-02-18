using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Store {
    private string storeName; // Name of the store
    private string storeAddress; // Address of the store
    private double storeBalance; // Balance of the store
    private double taxRate; // Tax rate for checkout functions and pricing calculations
    private readonly List<Item> inventory; // Inventory for the stores with 'Item' objects
    /*
     * Constructor for 'Store' object
     * - storeName (string): Name of the store
     * - storeAddress (string): Name of the street the store is located [*street name*, *state abriviation*, *zipcode*]
     * - balance (double): Amount of money that the store currently has
     * - taxRate (double): The percentage of money that get's applied to the subtotal
     * - inventory (List<Food>): List of items that the store currently has. Objects are loaded from a .csv file
     */
    public Store(string storeName, string storeAddress, double storeBalance, double taxRate, List<Item> inventory) {
        StoreName = storeName;
        StoreAddress = storeAddress;
        StoreBalance = storeBalance;
        TaxRate = taxRate;
        this.inventory = inventory;
    }
    // Overflow constructor incase some values are null in objects from the file
    public Store(string storeName, double storeBalance) {
        StoreName = storeName;
        StoreBalance = storeBalance;
    }
    // Override function that displays information about the store (can be seen with 'store' command)
    public override string ToString() {
        return $"---[ Store Information ]---\nName: {StoreName}\nBalance: {StoreBalance:$#,##0.00}";
    }
    // Function that allows the user to view the store's inventory (can be seen with 'look' command)
    public void ViewStore() {
        List<string> categories = new();
        Console.WriteLine("===============[ Store Inventory ]================");

        // Create a new list that will hold unique categories obtained through Food.Category. Seperate
        // the printing of the items by category to format the output cleaner.
        foreach (var storeItem in Inventory) {
            if (categories.Contains(storeItem.Category) == false) {
                categories.Add(storeItem.Category);
                Console.WriteLine($"\n---[ {storeItem.Category} ]---");
                Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Category} | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
            } else {
                Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Category} | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
            }
        }

        Console.WriteLine("==================================================");
        return;
    }
    // Function that allows the user to look up a specific item based on the item's name (can be seen with 'search [item]' command)
    public void SearchStore(string item) {
        Console.WriteLine($"===========[ Items matching '{item}' ]============");

        foreach (var storeItem in Inventory) {
            if (storeItem.Name.ToLower() == item.Replace("_", " ").ToLower()) {
                Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Category} | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
                Console.WriteLine("==================================================");
                return;
            }
        }
        Console.WriteLine($"There were 0 results for '{item}'...");
        Console.WriteLine("==================================================");
        return;
    }
    // Function that greets the user once they first open the program after short questioning
    public void Greeting(string userName, double userBalance, int userAge, List<Item> Cart) {
        Console.WriteLine("=========[ Grocery Shopping Application ]=========\n");
        Console.WriteLine($"\tYou are shopping at...\t{StoreName}\n");

        double subtotal = 0.0;
        foreach (var cartItem in Cart) {
            subtotal += cartItem.Price;
        }

        Console.WriteLine($"\tItems in cart: {Cart.Count}  ({subtotal:$#,##0.00})");
        Console.WriteLine($"\tRemaining Balance: {userBalance - subtotal:$#,##0.00}\n");
        Console.WriteLine($"> Name: {userName}");
        Console.WriteLine($"> Age: {userAge}");
        Console.WriteLine($"> Balance: {userBalance:$#,##0.00}\n");
        Console.WriteLine("==================================================");
        return;
    }
    // Function that handles the printing of the receipt and calculating total cost
    public void CheckOut(User user) {
        // Generates a unique store ID by getting a hash value from the store name
        // Credit: https://stackoverflow.com/questions/26870267/generate-integer-based-on-any-given-string-without-gethashcode
        MD5 md5Hasher = MD5.Create();
        var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(StoreName));
        int storeID = Math.Abs(BitConverter.ToInt32(hashed, 0));

        // Implement a random number generator that output's a different receipt file name
        Random randomNumber = new();
        string path = $"reciept-{randomNumber.Next(1000000)}.txt";
        using StreamWriter writer = new(path);

        Console.WriteLine($"===========[ Reciept For {storeName} ]============");
        Console.WriteLine($"\t{StoreName.ToUpper()}");
        Console.WriteLine($"\tSTORE ID # {storeID}\n");
        Console.WriteLine($"\t{StoreAddress.ToUpper()}");
        Console.WriteLine($"\tTEL: +{randomNumber.Next(1000)}-{randomNumber.Next(1000)}-{randomNumber.Next(10000)}");
        Console.WriteLine($"--------------------------------------------------");
        Console.WriteLine($"CASHIER:\t\t\t#{randomNumber.Next(10)}");
        Console.WriteLine($"MANAGER:\t\t\tJOHN JAMESON");
        Console.WriteLine($"--------------------------------------------------\n");
        Console.WriteLine($"NAME\t\t\tQTY\tPRICE\n");
        writer.WriteLine("===========[ Transaction Information ]============");
        writer.WriteLine($"\t{StoreName.ToUpper()}");
        writer.WriteLine($"\tSTORE ID # {storeID}\n");
        writer.WriteLine($"\t{StoreAddress.ToUpper()}");
        writer.WriteLine($"\tTEL: +{randomNumber.Next(1000)}-{randomNumber.Next(1000)}-{randomNumber.Next(10000)}");
        writer.WriteLine($"--------------------------------------------------");
        writer.WriteLine($"CASHIER:\t\t\t#{randomNumber.Next(10)}");
        writer.WriteLine($"MANAGER:\t\t\tJOHN JAMESON");
        writer.WriteLine($"--------------------------------------------------\n");
        writer.WriteLine($"NAME\t\tQTY\tPRICE\n");

        // Collect a running subtotal of all the items in the user's cart and print the item's name and price
        double subtotal = 0.00;
        foreach (Item cartItem in user.Cart) {
            if (cartItem.Name.Length >= 8) {
                Console.WriteLine($"{cartItem.Name.ToUpper()}\t\t{cartItem.Quantity}\t{cartItem.Quantity * cartItem.Price:$#,##0.00}");
                writer.WriteLine($"{cartItem.Name.ToUpper()}\t\t{cartItem.Quantity}\t{cartItem.Quantity * cartItem.Price:$#,##0.00}");
            } else {
                Console.WriteLine($"{cartItem.Name.ToUpper()}\t\t\t{cartItem.Quantity}\t{cartItem.Quantity * cartItem.Price:$#,##0.00}");
                writer.WriteLine($"{cartItem.Name.ToUpper()}\t\t\t{cartItem.Quantity}\t{cartItem.Quantity * cartItem.Price:$#,##0.00}");
            }

            subtotal += (cartItem.Price * cartItem.Quantity);
        }
        Console.WriteLine(" ");

        // Subtract the total from the user's account and add the result to the store's account
        user.UserBalance -= (subtotal + (subtotal * TaxRate));
        StoreBalance += (subtotal + (subtotal * TaxRate));

        Console.WriteLine($"--------------------------------------------------\n");
        Console.WriteLine($"SUBTOTAL\t\t{subtotal:$#,##0.00}");
        Console.WriteLine($"TAX ({TaxRate:0%})\t\t{(subtotal * TaxRate):$#,##0.00}");
        Console.WriteLine($"TOTAL\t\t\t{(subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        Console.WriteLine($"CASH\t\t\t{Math.Ceiling(subtotal + (subtotal * TaxRate)):$#,##0.00}");
        Console.WriteLine($"CHANGE\t\t\t{(Math.Ceiling(subtotal + (subtotal * TaxRate))) - (subtotal + (subtotal * TaxRate))}\n");
        Console.WriteLine($"\t# ITEMS SOLD {user.Cart.Count}");
        Console.WriteLine($"\t{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}\n");
        Console.WriteLine("\t\tTHANK YOU!\n");
        Console.WriteLine("\t\tGlad to see you again!");
        Console.WriteLine("==================================================");
        writer.WriteLine($"--------------------------------------------------\n");
        writer.WriteLine($"SUBTOTAL\t\t\t{subtotal:$#,##0.00}");
        writer.WriteLine($"TAX ({TaxRate:0%})\t\t{(subtotal * TaxRate):$#,##0.00}");
        writer.WriteLine($"TOTAL\t\t\t{(subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        writer.WriteLine($"CASH\t\t\t{Math.Ceiling(subtotal + (subtotal * TaxRate)):$#,##0.00}");
        writer.WriteLine($"CHANGE\t\t\t{(Math.Ceiling(subtotal + (subtotal * TaxRate))) - (subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        writer.WriteLine($"\t# ITEMS SOLD {user.Cart.Count}");
        writer.WriteLine($"\t{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}   {DateTime.Now.ToUniversalTime()}");
        writer.WriteLine("\n\t\tThank You!\n");
        writer.WriteLine("Glad to see you again!");
        writer.WriteLine("==================================================");
        return;
    }
    public string StoreName {
        get => storeName;
        set {
            // Set the store name to 'NONE' if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid store name! Store name was set to 'NONE' instead.");
                storeName = "NONE";
            }

            storeName = value;
        }
    }
    public string StoreAddress {
        get => storeAddress;
        set {
            // Set the store address to 'NONE' if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid store address! Store address was set to 'NONE' instead.");
                storeAddress = "NONE";
            }

            storeAddress = value;
        }
    }
    public double StoreBalance {
        get => storeBalance;
        set {
            // Set the balance to '0' if the input is empty/null.
            if (value.GetType() != typeof(double)) {
                Console.WriteLine("[!] Invalid balance! Balance was set to '0' instead.");
                storeBalance = 0;
                // Set the balance to '0' if the balance is less than 0.
            } else if (value < 0) {
                Console.WriteLine("[!] The balance can't be less than $0! Balance was set to '0.0' instead.");
                storeBalance = 0;
            }

            storeBalance = value;
        }
    }
    public double TaxRate {
        get => taxRate;
        set {
            taxRate = value;
        }
    }
    public List<Item> Inventory {
        get => inventory;
    }
}