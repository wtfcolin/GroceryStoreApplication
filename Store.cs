using System;
using System.Collections.Generic;
using System.IO;

public class Store {
    private string name; // Name of the store
    private string address; // Address of the store
    private double balance; // Balance of the store
 
    public Store(int id, string name, string address, double balance, double taxRate, List<Item> inventory) {
        ID = id;
        Name = name;
        Address = address;
        Balance = balance;
        TaxRate = taxRate;
        Inventory = inventory;
    }

    /* Override function that displays information about the store (can be seen with 'store' command)
     */
    public override string ToString() {
        return $"==============[ Store Information ]===============\nName:\t\t{Name}\nBalance:\t{Balance:$#,##0.00}\n==================================================";
    }

    /*  Function that allows the user to view the store's inventory (can be seen with 'look' command)
     */
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

    /*  Function that allows the user to look up a specific item based on the item's name (can be seen with 'search [item]' command)
     */
    public void SearchStore(string item, bool searchCategory) {
        int count = 0;
        Console.WriteLine("================[ Search Results ]================");

        foreach (var storeItem in Inventory) {
            bool matchedItem = storeItem.Name.ToLower().Contains(item.Replace("_", " ").ToLower());
            bool matchedCategory = storeItem.Category.ToLower().Contains(item.Replace("_", " ").ToLower());
            
            if (searchCategory) {
                if (matchedCategory) {
                    Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
                    count++;
                }
            } else {
                if (matchedItem) {
                    Console.WriteLine($"- {storeItem.Name} ({storeItem.Quantity}) | {storeItem.Category} | {storeItem.Price:$#,##0.00}/item | {storeItem.Calories} calories");
                    count++;
                }
            }
        }

        if (count > 0) {
            if (searchCategory) {
                Console.WriteLine($"\nReturned {count} results for category '{item}'...");
            } else {
                Console.WriteLine($"\nReturned {count} results for '{item}'...");
            }
            Console.WriteLine("==================================================");
        } else {
            Console.WriteLine($"There were 0 results for '{item}'...");
            Console.WriteLine("==================================================");
        }

        return;
    }

    /*  Function that greets the user once they first open the program after short questioning
     */
    public void Greeting(string userName, double userBalance, int userAge, List<Item> Cart) {
        Console.WriteLine("=========[ Grocery Shopping Application ]=========\n");
        Console.WriteLine($"\tYou are shopping at...  {name}\n");

        double subtotal = 0.0;
        foreach (var cartItem in Cart) {
            subtotal += cartItem.Price;
        }

        Console.WriteLine($"\tItems in cart: {Cart.Count}  ({subtotal:$#,##0.00})");
        Console.WriteLine($"\tRemaining Balance: {userBalance - subtotal:$#,##0.00}\n");
        Console.WriteLine($"\t> Name: {userName}");
        Console.WriteLine($"\t> Age: {userAge}");
        Console.WriteLine($"\t> Balance: {userBalance:$#,##0.00}\n");
        Console.WriteLine("==================================================");
        return;
    }

    /*  Function that handles the printing of the receipt and calculating total cost
     */
    public bool CheckOut(User user) {
        // Check if all grocery list items are in cart
        foreach (var listItem in user.GroceryList) {
            bool listCheck = false;
            foreach (var cartItem in user.Cart) {
                if (listItem.Name == cartItem.Name && cartItem.Quantity >= listItem.Quantity) {
                    listCheck = true;
                }
            }

            if (!listCheck) {
                Console.WriteLine("[!] You still have things on your grocery list to pick up!");
                return true;
            }
        }

        // Sum the subtotal of cart items
        double cartTotal = 0;
        foreach (var cartItem in user.Cart) {
            cartTotal += cartItem.Price * cartItem.Quantity + (cartItem.Price * cartItem.Quantity * TaxRate);
        }
        
        // Check if the user has enough money
        if (user.Balance - cartTotal < 0) {
            Console.WriteLine("[!] Invalid balance! You do not have enough money to checkout!");
            return true;
        }
        
        // Generate reciept
        Random randomNumber = new();

        int recieptID = randomNumber.Next(10000000);
        string path = $"reciept-{recieptID}.txt";
        using StreamWriter writer = new(path);

        Console.WriteLine($"===========[ Reciept For {Name} ]============");
        Console.WriteLine($"\t{Name.ToUpper()}");
        Console.WriteLine($"\tSTORE ID # {ID}");
        Console.WriteLine($"\tRECIEPT ID # {recieptID}");
        Console.WriteLine($"\t{Address.ToUpper()}");
        Console.WriteLine($"\tTEL: +{randomNumber.Next(1000)}-{randomNumber.Next(1000)}-{randomNumber.Next(10000)}");
        Console.WriteLine($"--------------------------------------------------");
        Console.WriteLine($"CASHIER:\t\t\t#{randomNumber.Next(10)}");
        Console.WriteLine($"MANAGER:\t\t\tJOHN JAMESON");
        Console.WriteLine($"--------------------------------------------------\n");
        Console.WriteLine($"NAME\t\t\tQTY\tPRICE\n");
        
        writer.WriteLine("===========[ Transaction Information ]============");
        writer.WriteLine($"\t{Name.ToUpper()}");
        writer.WriteLine($"\tSTORE ID # {ID}\n");
        writer.WriteLine($"\t{Address.ToUpper()}");
        writer.WriteLine($"\tTEL: +{randomNumber.Next(1000)}-{randomNumber.Next(1000)}-{randomNumber.Next(10000)}");
        writer.WriteLine($"--------------------------------------------------");
        writer.WriteLine($"CASHIER:\t\t\t#{randomNumber.Next(10)}");
        writer.WriteLine($"MANAGER:\t\t\tJOHN JAMESON");
        writer.WriteLine($"--------------------------------------------------\n");
        writer.WriteLine($"NAME\t\tQTY\tPRICE\n");

        // If a cart item name is longer than 8 characters, add an extra tab (FORMATTING)
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

        user.Balance -= (subtotal + (subtotal * TaxRate));
        Balance += (subtotal + (subtotal * TaxRate));

        Console.WriteLine($"--------------------------------------------------\n");
        Console.WriteLine($"SUBTOTAL\t\t{subtotal:$#,##0.00}");
        Console.WriteLine($"TAX ({TaxRate:0%})\t\t{(subtotal * TaxRate):$#,##0.00}");
        Console.WriteLine($"TOTAL\t\t\t{(subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        Console.WriteLine($"CASH\t\t\t{Math.Ceiling(subtotal + (subtotal * TaxRate)):$#,##0.00}");
        Console.WriteLine($"CHANGE\t\t\t{(Math.Ceiling(subtotal + (subtotal * TaxRate))) - (subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        Console.WriteLine($"\t\t# ITEMS SOLD {user.Cart.Count}");
        Console.WriteLine($"\t\t{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}");
        Console.WriteLine("\n\t\tTHANK YOU!\n");
        Console.WriteLine("\t\tGlad to see you again!");
        Console.WriteLine("==================================================");

        writer.WriteLine($"--------------------------------------------------\n");
        writer.WriteLine($"SUBTOTAL\t\t\t{subtotal:$#,##0.00}");
        writer.WriteLine($"TAX ({TaxRate:0%})\t\t{(subtotal * TaxRate):$#,##0.00}");
        writer.WriteLine($"TOTAL\t\t\t{(subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        writer.WriteLine($"CASH\t\t\t{Math.Ceiling(subtotal + (subtotal * TaxRate)):$#,##0.00}");
        writer.WriteLine($"CHANGE\t\t\t{(Math.Ceiling(subtotal + (subtotal * TaxRate))) - (subtotal + (subtotal * TaxRate)):$#,##0.00}\n");
        writer.WriteLine($"\t\t# ITEMS SOLD {user.Cart.Count}");
        writer.WriteLine($"\t\t{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}   {DateTime.Now.ToUniversalTime()}");
        writer.WriteLine("\n\t\tThank You!\n");
        writer.WriteLine("\t\tGlad to see you again!");
        writer.WriteLine("==================================================");

        return false;
    }
    
    /*  Store properties
     */
    public int ID { get; set; }
    public string Name {
        get => name;
        set {
            // Set the store name to 'NONE' if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid store name! Store name was set to 'NONE' instead.");
                name = "NONE";
            }

            name = value;
        }
    }
    public string Address {
        get => address;
        set {
            // Set the store address to 'NONE' if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid store address! Store address was set to 'NONE' instead.");
                address = "NONE";
            }

            address = value;
        }
    }
    public double Balance {
        get => balance;
        set {
            if (value.GetType() != typeof(double)) {
                Console.WriteLine("[!] Invalid balance! Balance was set to '0' instead.");
                balance = 0;
            } else if (value < 0) {
                Console.WriteLine("[!] The balance can't be less than $0! Balance was set to '0.0' instead.");
                balance = 0;
            }

            balance = value;
        }
    }
    public double TaxRate { get; set; }
    public List<Item> Inventory { get; }
}