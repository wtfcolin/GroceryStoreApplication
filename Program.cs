using System;
using System.Collections.Generic;
using System.IO;
using static Commands;

public class Program {
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

    private static Store LoadStore(string path) {
        try {
            using StreamReader reader = new StreamReader(path);
            string storeName = reader.ReadLine(); // Name of the store in the CSV file.
            double storeBalance = double.Parse(reader.ReadLine()); // Balance of the store in the CSV file.
            
            reader.ReadLine(); // Skip over the header

            int lineCount = GetLineCount(path);
            List<Food> inventory = new();
            
            for (int i = 3; i < lineCount; i++) {
                string line = reader.ReadLine();
                string[] cols = line.Split(',');
                string name = cols[0];
                int quantity = int.Parse(cols[1]);
                string category = cols[2];
                double price = double.Parse(cols[3]);
                int calories = int.Parse(cols[4]);

                Food item = new Food(name, quantity, category, price, calories);
                inventory.Add(item);
            }

            Store store = new Store(storeName, storeBalance, inventory);
            return store;
        } catch {
            Console.WriteLine("[!] Loading food items was unsuccessful, check the syntax of the CSV file!");
            Store store = new Store("NONE", 0);
            return store;
        }
    }

    private static void Main() {
        bool RUNNING = true; // Status of the program. Once it toggles false the program ends.
        bool ADMIN = false; // Toggle for administration priviledges.
        string path = "Store.csv"; // Path to the file that contains 'Food' object properties in a CSV format.

        List<Food> cart = new();
        List<Food> groceryList = new();

        User user = new User(21, 100.0, cart, false, groceryList);
        Store store = LoadStore(path);

        ClearTerminal();
        store.Greeting(user.Balance, user.Age, store.StoreName, user.Cart.Count);

        while (RUNNING) {
            RUNNING = CommandLine(ADMIN, user, store);
        }
    }
}
