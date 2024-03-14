using System;
using System.Collections.Generic;
using System.IO;
using static Functions;

public class Program {
    /* Function that gets the line count of a file located in path
    */
    private static int GetLineCount(string path) {
        // Check if path exists
        if (!File.Exists(path)) {
            Console.WriteLine($"[!] Count not read file '{path}'!");
        }

        // Read each line of the file in path
        int count = 0;
        using StreamReader reader = new(path);

        while (!reader.EndOfStream) {
            reader.ReadLine();
            count++;
        }

        return count;
    }
    
    /* Function that loads a store from 'Store.csv'
    */
    private static Store LoadStore(string path) {
        try {
            using StreamReader reader = new(path);
            int storeID = int.Parse(reader.ReadLine()); // ID of the store.
            string storeName = reader.ReadLine(); // Name of the store in the CSV file.
            string storeAddress = reader.ReadLine(); // Address of the store in the CSV file.
            double storeBalance = double.Parse(reader.ReadLine()); // Balance of the store in the CSV file.
            
            reader.ReadLine(); // Skip over the header

            int lineCount = GetLineCount(path);
            List<Item> storeInventory = new();
            
            // For each of the lines past the store information, collect each part of information that creates the item and assigns them to a list
            for (int i = 5; i < lineCount; i++) {
                string line = reader.ReadLine();
                string[] cols = line.Split(',');
                string itemName = cols[0];
                int itemQuantity = int.Parse(cols[1]);
                string itemCategory = cols[2];
                double itemPrice = double.Parse(cols[3]);
                int itemCalories = int.Parse(cols[4]);

                Item item = new(itemName, itemQuantity, itemCategory, itemPrice, itemCalories);
                storeInventory.Add(item);
            }
            
            Store store = new(storeID, storeName, storeAddress, storeBalance, 0.15, storeInventory);
            return store;
        } catch {
            ClearTerminal();
            Console.WriteLine("[!] Loading items was unsuccessful, check the syntax of the CSV file!");
            Store store = new(0, "NULL", "NULL", 0.0, 0.15, new());
            return store;
        }
    }
    
    /* Main function
    */
    private static void Main() {
        bool RUNNING = true; // Status of the program. Once it toggles false the program ends.
        string path = "Store.csv"; // Path to the file that contains 'Item' object properties in a CSV format.

        List<Item> cart = new();
        List<Item> groceryList = new();
        List<Recipe> recipeList = new();
        string name;
        int age;
        double balance;

        // Prompt the user before the program starts to enter personal information
        try {
            ClearTerminal();
            Console.WriteLine("What is your name?");
            Console.Write(">> ");
            name = Console.ReadLine();
            Console.WriteLine("\nHow old are you?");
            Console.Write(">> ");
            age = int.Parse(Console.ReadLine());
            Console.WriteLine("\nHow much money do you want?");
            Console.Write(">> ");
            balance = double.Parse(Console.ReadLine());
        } catch {
            ClearTerminal();
            Console.WriteLine("[!] Invalid syntax! Default user settings set: 'John', '18', '$250.00'");
            name = "John";
            age = 18;
            balance = 250.00;
        }

        User user = new(name, age, balance, cart, false, groceryList, recipeList);
        Store store = LoadStore(path);

        ClearTerminal();
        store.Greeting(user.Name, user.Balance, user.Age, user.Cart);

        while (RUNNING) { 
            RUNNING = CommandLine(user, store);
        }
    }
}
