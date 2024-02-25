using System;
using System.Collections.Generic;

public static class Commands {
    /*  Function to handle user input
    */
    public static bool CommandLine(User user, Store store) {
        Console.WriteLine("\n==================================================");
        Console.Write(">> ");
        string command = Console.ReadLine(); // Get the user input
        string[] arguments = command.Split(' '); // Split the input by spaces to get the arguments
        Console.WriteLine("\n==================================================");
        ClearTerminal();
        return Command(arguments, user, store);
    }
    
    /*  Function that is used by other functions / used to clear the screen (can be seen using 'clear' command)
    */
    public static void ClearTerminal() {
        for (int i = 0; i < 30; i++) {
            Console.WriteLine("\n");
        }
    }

    /*  Internal function used to handle user input arguments
    */
    private static bool Command(string[] arguments, User user, Store store) {
        switch (arguments[0]) {
            case "set":
            try {
                SetValue(arguments[1], arguments[2], user, store);
            } catch {
                Console.WriteLine("[!] Invalid syntax! (set {option} {value})");
            }
            return true;

            case "add":
            try {
                user.AddItem(arguments[1], int.Parse(arguments[2]), store);
            } catch {
                Console.WriteLine("[!] Invalid syntax! (add {item name} {amount})");
            }
            return true;

            case "remove":
            try {
                user.RemoveItem(arguments[1], int.Parse(arguments[2]), store);
            } catch {
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
            user.ViewBalance(store.TaxRate);
            return true;

            case "search":
            try {
                int count = 0;
                foreach (var item in store.Inventory) {
                    if (arguments[1].Replace("_", " ").ToLower() == item.Category.ToLower()) {
                        store.SearchStore(arguments[1], true);
                        return true;
                    }
                }

                if (count == 0) {
                    store.SearchStore(arguments[1], false);
                }
            } catch {
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
            return store.CheckOut(user);

            case "exit":
            return false;

            case "admin123":
            if (user.Admin) {
                Console.WriteLine("ADMINISTRATOR MODE: DISABLED");
                user.Admin = false;
            } else {
                Console.WriteLine("ADMINISTRATOR MODE: ENABLED");
                user.Admin = true;
            }
            return true;

            case "recipes":
            user.ViewRecipes();
            return true;

            case "createrecipe":
            CreateRecipe(store, user, true);
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

    /*  Prints the help menu that displays a list of commands
    */
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
    
    /* Sets the values for user age, user balance, and store balance (ADMIN)
    */
    private static void SetValue(string option, string value, User user, Store store) {
        if (user.Admin) {
            if (option == "name") {
                if (!string.IsNullOrWhiteSpace(value)) {
                    Console.WriteLine($"You name was updated from '{user.Name}' to '{value}'");
                    user.Name = value;
                } else {
                    Console.WriteLine("[!] Invalid name! Name must be a non-empty string.");
                }
            } else if (option == "age") {
                const int MAXUSERAGE = 110; 
                try {
                    if (int.Parse(value) >= 1 && int.Parse(value) <= MAXUSERAGE) {
                        Console.WriteLine($"Your age was updated from '{user.Age}' to '{value}'!");
                        user.Age = int.Parse(value);
                    } else {
                        Console.WriteLine($"[!] Invalid age! Age must be between 1 - {MAXUSERAGE}.");
                        return;
                    }
                } catch {
                    Console.WriteLine("[!] Invalid age! Age must be an integer.");
                }
            } 
            else if (option == "balance") {
                const double MAXUSERBALANCE = 1000000.0;
                try {
                    if (double.Parse(value) >= 1 && double.Parse(value) <= MAXUSERBALANCE) {
                        Console.WriteLine($"Your balance was updated from '{user.UserBalance:$#,##0.00}' to '{double.Parse(value):$#,##0.00}'!");
                        user.UserBalance = double.Parse(value);
                    } else {
                        Console.WriteLine($"[!] Invalid balance! Balance must be between 1 - {MAXUSERBALANCE}.");
                        return;
                    }
                } catch {
                    Console.WriteLine("[!] Invalid balance ! Balance must be an double.");
                }
            } else if (option == "storebalance") {
                const double MAXSTOREBALANCE = 100000000.0;
                try {
                    if (double.Parse(value) >= 1 && double.Parse(value) <= MAXSTOREBALANCE) {
                        Console.WriteLine($"The store balance was updated from '{store.StoreBalance:$#,##0.00}' to '{double.Parse(value):$#,##0.00}'!");
                        store.StoreBalance = double.Parse(value);
                    } else {
                        Console.WriteLine($"[!] Invalid store balance! Store balance must be between 1 - {MAXSTOREBALANCE}.");
                        return;
                    }
                } catch {
                    Console.WriteLine("[!] Invalid store balance! Store balance must be a double.");
                }
            } else {
                Console.WriteLine("[!] Invalid option! Please provide a valid option.");
                return;
            }
        } else {
            Console.WriteLine("[!] You do not have proper permissions to access this command!");
            return;
        }
    }

    /*  Creates a recipe for the user and appends it to their personal recipe list
    */
    public static void CreateRecipe(Store store, User user, bool toggle) {
        List<Item> ingredients = new();

        ClearTerminal();
        Console.WriteLine("What is the name of your recipe?");
        Console.WriteLine("==================================================");
        Console.Write(">> ");
        string name = Console.ReadLine();
        ClearTerminal();

        while (toggle) {
            Console.WriteLine("\nPlease enter an ingredient name and amount seperated by a space (Ex: Burger_Patty 2, Apple 4, ...)");
            Console.WriteLine("When you are finished adding ingredients, type 'done'.");
            Console.WriteLine("==================================================");
            Console.Write(">> ");
            string command = Console.ReadLine();
            string[] arguments = command.Split(" ");

            if (arguments.Length > 1) {
                try {
                    int count = 0;
                    foreach (var storeItem in store.Inventory) {
                        if (arguments[0].Replace("_", " ").ToLower() == storeItem.Name.ToLower()) {
                            ClearTerminal();
                            Console.WriteLine($"Added {arguments[1]} {storeItem.Name} to your '{name}' recipe!");
                            storeItem.Quantity = int.Parse(arguments[1]);
                            ingredients.Add(storeItem);      
                            count++;
                        }
                    }

                    if (count == 0) {
                        ClearTerminal();
                        Console.WriteLine("[!] Invalid syntax! ({item name} {quantity})");
                    }
                } catch {
                    ClearTerminal();
                    Console.WriteLine("[!] Invalid syntax! ({item name} {quantity})");
                }
            } else if (arguments[0] == "look") {
                store.ViewStore();
            } else if (arguments[0] == "search" /* NEEDS FIXING */) {
                try {
                    int count = 0;
                    foreach (var item in store.Inventory) {
                        if (arguments[1].Replace("_", " ").ToLower() == item.Category.ToLower()) {
                            store.SearchStore(arguments[1], true);
                            return;
                        }
                    }

                    if (count == 0) {
                        store.SearchStore(arguments[1], false);
                    }
                } catch {
                    Console.WriteLine("[!] Invalid syntax! (search {item name})");
                }
            } else if (arguments[0] == "done") {
                ClearTerminal();
                Recipe recipe = new(name, ingredients);
                user.RecipeList.Add(recipe);
                Console.WriteLine($"'{recipe.RecipeName}' recipe has been created successfully!\n");
                Console.WriteLine(recipe);
                return;
            } 
            else {
                ClearTerminal();
                Console.WriteLine("[!] Invalid syntax! ({item name} {quantity})");
            }
        }
    }

    /*  Removes a recipe from the user's personal recipe list
    */
    public static void RemoveRecipe(User user) {
        ClearTerminal();
        Console.WriteLine("What is the name of your recipe?");
        Console.WriteLine("==================================================");
        Console.Write(">> ");
        string name = Console.ReadLine();

        foreach (var recipe in user.RecipeList) {
            try {
                if (name == recipe.RecipeName) {
                    user.RecipeList.Remove(recipe);
                    ClearTerminal();
                    Console.WriteLine($"Recipe '{recipe.RecipeName}' has been removed successfully!");
                    return;
                }
            } catch {
                ClearTerminal();
                Console.WriteLine("[!] Invalid syntax! ({recipename})");
                return;
            }
        }

        ClearTerminal();
        Console.WriteLine($"[!] There is no recipe with the name '{name}' in your list!");
        return;
    }
}