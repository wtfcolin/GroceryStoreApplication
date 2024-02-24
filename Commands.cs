using System;
using System.Collections.Generic;

public static class Commands {
    // Function to handle user input
    public static bool CommandLine(User user, Store store) {
        Console.WriteLine("\n==================================================");
        Console.Write(">> ");
        string command = Console.ReadLine(); // Get the user input
        string[] arguments = command.Split(' '); // Split the input by spaces to get the arguments
        Console.WriteLine("\n==================================================");
        ClearTerminal();
        return Command(arguments, user, store);
    }
    // Function that is used by other functions / used to clear the screen (can be seen using 'clear' command)
    public static void ClearTerminal() {
        for (int i = 0; i < 30; i++) {
            Console.WriteLine("\n");
        }
    }

    // Internal function used to handle user input arguments
    private static bool Command(string[] arguments, User user, Store store) {
        switch (arguments[0]) {
            case "set":
            if (arguments.Length > 1) {
                if (arguments[1] == "name") {
                    SetValue(arguments[1], arguments[2], user, store);
                } else if ((arguments[1] == "age" && arguments[2].GetType() == typeof(int))|| (arguments[1] == "balance" && arguments[2].GetType() == typeof(int))) {
                    SetValue(arguments[1], double.Parse(arguments[2]), user, store);
                } else {
                    Console.WriteLine("[!] Invalid syntax! (set {option} {value})");
                }
            } else {
                Console.WriteLine("[!] Invalid syntax! (set {option} {value})");
            }
            return true;

            case "add":
            if (arguments.Length > 1) {
                user.AddItem(arguments[1], int.Parse(arguments[2]), store);
            } else {
                Console.WriteLine("[!] Invalid syntax! (add {item name} {amount})");
            }
            return true;

            case "remove":
            if (arguments.Length > 1) {
                if (arguments[2].GetType() == typeof(int)) {
                    user.RemoveItem(arguments[1], int.Parse(arguments[2]), store);
                } else {
                    Console.WriteLine("[!] Invalid syntax! (remove {item name} {amount}");
                }
            } else {
                Console.WriteLine("[!] Invalid syntax! (remove {item name} {amount}");
            }
            return true;

            case "help":
            PrintHelp(user.Admin);
            return true;

            case "cart":
            user.ViewCart();
            return true;

            case "look":
            store.ViewStore();
            return true;

            case "balance":
            user.ViewBalance();
            return true;

            case "search":
            if (arguments.Length > 1) {
                store.SearchStore(arguments[1]);
            } else {
                Console.WriteLine("[!] Invalid syntax! (search {item name})");
            }
            return true;

            case "list":
            user.ViewList();
            return true;

            case "clear":
            ClearTerminal();
            return true;

            case "checkout":
            store.CheckOut(user);
            return false;

            case "exit":
            return false;

            case "admin123":
            if (user.Admin) {
                user.Admin = false;
            } else {
                user.Admin = true;
            }
            return true;

            case "createrecipe":
            CreateRecipe(store, user);
            return true;

            case "removerecipe":
            RemoveRecipe(user);
            return true;

            case "me":
            Console.WriteLine(user);
            return true;

            case "store":
            Console.WriteLine(store);
            return true;

            case "home":
            store.Greeting(user.Name, user.UserBalance, user.Age, user.Cart);
            return true;

            default:
            Console.WriteLine("[!] Invalid command! Try typing 'help' for a list of commands.");
            return true;
        }
    }

    // Prints the help menu that displays a list of commands
    private static void PrintHelp(bool ADMIN) {
        Console.WriteLine("===============[ List Of Commands ]===============");
        Console.WriteLine("- help = Displays a list of commands");
        Console.WriteLine("- exit = Exits the program\n");

        Console.WriteLine("- list = Prints out your grocery list");
        Console.WriteLine("- cart = Prints out the items in your cart");
        Console.WriteLine("- add [item] [amount] = Adds a food item to your cart");
        Console.WriteLine("- remove [item] [amount] = Removes a food item from your cart");
        Console.WriteLine("- checkout = Prints a receipt of your items if all the conditions are satisfied\n");

        Console.WriteLine("- look = Lists all the items that the store has and their quantity");
        Console.WriteLine("- search [item] = Prints out information about a specific item\n");

        Console.WriteLine("- recipes = Prints out your recipe list");
        Console.WriteLine("- createrecipe = Asks you questions to create a recipe");
        Console.WriteLine("- removerecipe = Removes a recipe from your personal recipe list\n");

        Console.WriteLine("- balance = Prints out your current balance");
        Console.WriteLine("- store = Prints out information about the current store");
        Console.WriteLine("- me = Prints out information about the current user");

        if (ADMIN) {
            Console.WriteLine("\n- set age [age] = Sets the age of the current user (ADMIN)");
            Console.WriteLine("- set balance [amount] = Sets the balance of the current user (ADMIN)");
            Console.WriteLine("- set storebalance [amount] = Sets the balance of the store (ADMIN)");
            Console.WriteLine("==================================================");
        } else {
            Console.WriteLine("==================================================");
        }

        return;
    }
    
    // Sets the values for user age, user balance, and store balance (ADMIN)
    private static void SetValue(string option, double amount, User user, Store store) {
        if (string.IsNullOrEmpty(option)) {
            Console.WriteLine("[!] Invalid option! Please provide a valid option.");
            return;
        } else if (amount.GetType() != typeof(double)) {
            Console.WriteLine("[!] Invalid amount! Please provide a valid amount.");
            return;
        }

        if (user.Admin == true) {
            if (option == "age") {
                user.Age = (int)amount;
            } else if (option == "balance") {
                user.UserBalance = amount;
            } else if (option == "storebalance") {
                store.StoreBalance = amount;
            }
        } else {
            Console.WriteLine("[!] You do not have proper permissions to access this command!");
            return;
        }
    }
    
    // Sets the values for user name (ADMIN)
    private static void SetValue(string option, string value, User user, Store store) {
        if (string.IsNullOrEmpty(option)) {
            Console.WriteLine("[!] Invalid option! Please provide a valid option.");
            return;
        } else if (value.GetType() != typeof(double)) {
            Console.WriteLine("[!] Invalid amount! Please provide a valid amount.");
            return;
        }

        if (user.Admin == true) {
            if (option == "name") {
                user.Name = value;
            } 
        } else {
            Console.WriteLine("[!] You do not have proper permissions to access this command!");
            return;
        }
    }

    // Creates a recipe for the user and appends it to their personal recipe list
    public static void CreateRecipe(Store store, User user) {
        for (int i = 0; i < 30; i++) {
            Console.WriteLine("\n");
        }

        Console.WriteLine("What is the name of your recipe?");
        Console.WriteLine("--------------------------------------------------");
        Console.Write(">> ");
        string name = Console.ReadLine();
        bool toggle = true;

        while (toggle) {
            for (int i = 0; i < 30; i++) {
                Console.WriteLine("\n");
            }

            Console.WriteLine("Please enter an ingredient name and amount seperated by a space (Ex: Burger_Patty 2, Apple 4, ...)");
            Console.WriteLine("When you are finished adding ingredients, type 'done'.");
            Console.WriteLine("--------------------------------------------------");
            Console.Write(">> ");
            string command = Console.ReadLine();
            string[] arguments = command.Split(" ");
            List<Item> ingredients = new();

            if (arguments[0].GetType() == typeof(string) && arguments[1].GetType() == typeof(int)) {
                foreach (var storeItem in store.Inventory) {
                    if (arguments[0].Replace("_", " ").ToLower() == storeItem.Name.ToLower() && storeItem.Quantity >= int.Parse(arguments[1])) {
                        storeItem.Quantity = int.Parse(arguments[2]);
                        ingredients.Add(storeItem);
                    }
                }
            } else if (arguments[0] == "done") {
                Recipe recipe = new Recipe(name, ingredients);
                user.RecipeList.Add(recipe);
                return;
            } else {
                Console.WriteLine("[!] Invalid syntax! You must provide a name and amount seperated by a space (Ex: Burger_Patty 2, Apple 4, ...");
            }
        }
    }

    // Removes a recipe from the user's personal recipe list
    public static void RemoveRecipe(User user) {
        for (int i = 0; i < 30; i++) {
            Console.WriteLine("\n");
        }

        Console.WriteLine("What is the name of your recipe?");
        Console.WriteLine("==================================================");
        Console.Write(">> ");
        string name = Console.ReadLine();

        foreach (var recipe in user.RecipeList) {
            if (name.Replace("_"," ").ToLower() == recipe.RecipeName.ToLower()) {
                user.RecipeList.Remove(recipe);
                return;
            }

            Console.WriteLine($"[!] There is no recipe with the name '{name}'!");
            return;
        }
    }
}