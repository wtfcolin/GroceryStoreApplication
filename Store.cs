using System;
using System.Collections.Generic;
using System.IO;

public class Store {
    private int storeID; // ID of the store
    private string storeName; // Name of the store
    private string storeAddress; // Address of the store
    private double storeBalance; // Balance of the store
    private double taxRate; // Tax rate for checkout functions and pricing calculations
    private readonly List<Item> inventory; // Inventory for the stores with 'Item' objects
    
    // Constructor for 'Store' object
    public Store(int storeID, string storeName, string storeAddress, double storeBalance, double taxRate, List<Item> inventory) {
        StoreID = storeID;
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
        return $"--------------[ Store Information ]---------------\nName: {StoreName}\nBalance: {StoreBalance:$#,##0.00}\n--------------------------------------------------";
    }

    // Function that allows the user to view the store's inventory (can be seen with 'look' command)
    public void ViewStore() {
        List<string> categories = new();
        Console.WriteLine("===============[ Store Inventory ]================");

        foreach (var storeItem in Inventory) {
            if (categories.Contains(storeItem.Category) == false) {
                categories.Add(storeItem.Category);
                Console.WriteLine($"\n---[ {storeItem.Category} ]---");
                Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
            } else {
                Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    // Function that allows the user to look up a specific item based on the item's name (can be seen with 'search [item]' command)
    public void SearchStore(string item) {
        Console.WriteLine("================[ Search Results ]================");
        Console.WriteLine($"Search Term... {item}\n");

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
        Console.WriteLine($"\tYou are shopping at...  {StoreName}\n");

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
        Random randomNumber = new();
        int storeID = randomNumber.Next(10000000);
        string path = $"reciept-{randomNumber.Next(1000000)}.txt";
        using StreamWriter writer = new(path);

        Console.WriteLine($"===========[ Reciept For {storeName} ]============");
        Console.WriteLine($"\t{StoreName.ToUpper()}");
        Console.WriteLine($"\tSTORE ID # {StoreID}\n");
        Console.WriteLine($"\t{StoreAddress.ToUpper()}");
        Console.WriteLine($"\tTEL: +{randomNumber.Next(1000)}-{randomNumber.Next(1000)}-{randomNumber.Next(10000)}");
        Console.WriteLine($"--------------------------------------------------");
        Console.WriteLine($"CASHIER:\t\t\t#{randomNumber.Next(10)}");
        Console.WriteLine($"MANAGER:\t\t\tJOHN JAMESON");
        Console.WriteLine($"--------------------------------------------------\n");
        Console.WriteLine($"NAME\t\t\tQTY\tPRICE\n");
        writer.WriteLine("===========[ Transaction Information ]============");
        writer.WriteLine($"\t{StoreName.ToUpper()}");
        writer.WriteLine($"\tSTORE ID # {StoreID}\n");
        writer.WriteLine($"\t{StoreAddress.ToUpper()}");
        writer.WriteLine($"\tTEL: +{randomNumber.Next(1000)}-{randomNumber.Next(1000)}-{randomNumber.Next(10000)}");
        writer.WriteLine($"--------------------------------------------------");
        writer.WriteLine($"CASHIER:\t\t\t#{randomNumber.Next(10)}");
        writer.WriteLine($"MANAGER:\t\t\tJOHN JAMESON");
        writer.WriteLine($"--------------------------------------------------\n");
        writer.WriteLine($"NAME\t\tQTY\tPRICE\n");

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
    
    public int StoreID {
        get => storeID;
        set {
            storeID = value;
        }
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
            // Check for type: double
            if (value.GetType() != typeof(double)) {
                Console.WriteLine("[!] Invalid balance! Balance was set to '0' instead.");
                storeBalance = 0;
                // Check for range: balance < 0
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