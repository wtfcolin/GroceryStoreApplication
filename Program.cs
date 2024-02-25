using System;
using System.Collections.Generic;
using System.IO;
using static Commands;

public class Program {
    // Function that gets the line count of a file located in path
    private static int GetLineCount(string path) {
        if (!File.Exists(path)) {
            Console.WriteLine($"[!] Count not read file '{path}'!");
        }

        int count = 0;
        using StreamReader reader = new StreamReader(path);

        while (!reader.EndOfStream) {
            reader.ReadLine();
            count++;
        }

        return count;
    }
    // Function that loads a store from 'Store.csv'
    private static Store LoadStore(string path) {
        try {
            using StreamReader reader = new StreamReader(path);
            int storeID = 80342271; // ID of the store.
            string storeName = reader.ReadLine(); // Name of the store in the CSV file.
            string storeAddress = reader.ReadLine(); // Address of the store in the CSV file.
            double storeBalance = double.Parse(reader.ReadLine()); // Balance of the store in the CSV file.
            
            reader.ReadLine(); // Skip over the header

            int lineCount = GetLineCount(path);
            List<Item> inventory = new();
            
            // For each of the lines past the store information, collect each part of information that creates the item and assigns them to a list
            for (int i = 4; i < lineCount; i++) {
                string line = reader.ReadLine();
                string[] cols = line.Split(',');
                string name = cols[0];
                int quantity = int.Parse(cols[1]);
                string category = cols[2];
                double price = double.Parse(cols[3]);
                int calories = int.Parse(cols[4]);

                Item item = new(name, quantity, category, price, calories);
                inventory.Add(item);
            }
            
            Store store = new(storeID, storeName, storeAddress, storeBalance, 0.15, inventory);
            return store;
        } catch {
            ClearTerminal();
            Console.WriteLine("[!] Loading items was unsuccessful, check the syntax of the CSV file!");
            Store store = new(0, "NULL", "NULL", 0.0, 0.15, new());
            return store;
        }
    }
    // Main function
    private static void Main() {
        bool RUNNING = true; // Status of the program. Once it toggles false the program ends.
        string path = "Store.csv"; // Path to the file that contains 'Food' object properties in a CSV format.

        List<Item> cart = new();
        List<Item> groceryList = new();
        List<Recipe> recipeList = new();

        // Prompt the user before the program starts to enter personal information
        /*
        ClearTerminal();
        Console.WriteLine("What is your name?");
        Console.Write(">> ");
        string name = Console.ReadLine();
        Console.WriteLine("How old are you?");
        Console.Write(">> ");
        int age = int.Parse(Console.ReadLine());
        Console.WriteLine("How much money do you want? (0.00 format)");
        Console.Write(">> ");
        double balance = double.Parse(Console.ReadLine());
        User user = new User(name, age, balance, cart, false, groceryList);
        */

        User user = new("Colin", 22, 1000.0, cart, false, groceryList, recipeList);
        Store store = LoadStore(path);

        ClearTerminal();
        store.Greeting(user.Name, user.UserBalance, user.Age, user.Cart);

        while (RUNNING) { 
            RUNNING = CommandLine(user, store);
        }
    }
}
