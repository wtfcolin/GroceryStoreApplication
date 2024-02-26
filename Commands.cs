using System;
using System.Collections.Generic;

public static class Commands {
    /*  Function to handle user input
    */
    public static bool CommandLine(User user, Store store) {
        Console.WriteLine("\n__________________________________________________");
        Console.Write(">> ");
        string command = Console.ReadLine(); // Get the user input
        string[] arguments = command.Split(' '); // Split the input by spaces to get the arguments
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
        ClearTerminal();
        switch (arguments[0]) {
            case "set":
            try {
                SetValue(arguments[1], arguments[2], user, store);
            } catch {
                Console.WriteLine("[!] Invalid syntax! (set {option} {value})");
            }
            return true;

            case "help":
            PrintHelp(user.Admin);
            return true;

            case "cart":
            if (arguments.Length > 1) {
                try {
                    if (arguments[1] == "add") {
                        user.AddCartItem(arguments[2], int.Parse(arguments[3]), store);
                    } else if (arguments[1] == "remove") {
                        user.RemoveCartItem(arguments[2], int.Parse(arguments[3]), store);
                    } else {
                        Console.WriteLine("[!] Invalid syntax! (cart [add/remove {item name} {amount}])");
                    }
                } catch {
                    Console.WriteLine("[!] Invalid syntax! (cart [add/remove {item name} {amount}])");
                }
            } else {
                user.ViewCart();
            }
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
            if (arguments.Length > 1) {
                try {
                    if (arguments[1] == "add") {
                        user.AddListItem(arguments[2], int.Parse(arguments[3]), store);
                    } else if (arguments[1] == "remove") {
                        user.RemoveListItem(arguments[2], int.Parse(arguments[3]));
                    } else {
                        Console.WriteLine("[!] Invalid syntax! (cart [add/remove {item name} {amount}])");
                    }
                } catch {
                    Console.WriteLine("[!] Invalid syntax! (cart [add/remove {item name} {amount}])");
                }
            } else {
                user.ViewList();
            }
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

            case "recipe":
            try {
                if (arguments[1] == "create") {
                    CreateRecipe(store, user, true);
                } else if (arguments[1] == "remove") {
                    RemoveRecipe(user);
                }
            } catch {
                Console.WriteLine("[!] Invalid syntax! (recipe [create/remove])");
            }
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
        Console.WriteLine("\t{}: Required Args, []: Optional Args\n");

        Console.WriteLine("- help = Displays a list of commands");
        Console.WriteLine("- exit = Exits the program\n");

        Console.WriteLine("- list [add/remove {item name}] = Print, add, and remove items from your grocery list");
        Console.WriteLine("- cart [add/remove {item name}]= Print, add, and remove items from your cart");
        Console.WriteLine("- checkout = Prints a receipt of your items if all the conditions are satisfied\n");

        Console.WriteLine("- look = Lists all the items that the store has and their quantity");
        Console.WriteLine("- search {item name/category} = Prints out information about a specific item\n");

        Console.WriteLine("- recipes = Prints out your recipe list");
        Console.WriteLine("- recipe {create/remove} = Prints out recipe or create and remove recipes\n");

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
        Console.WriteLine("__________________________________________________");
        Console.Write(">> ");
        string name = Console.ReadLine();
        ClearTerminal();

        while (toggle) {
            Console.WriteLine("\nPlease enter an ingredient name and amount ({item name} {amount})");
            Console.WriteLine("When you are finished adding ingredients, type 'done'.");
            Console.WriteLine(" - 'look' and 'search {item name}' can still be used");
            Console.WriteLine("__________________________________________________");
            Console.Write(">> ");
            string command = Console.ReadLine();
            string[] arguments = command.Split(" ");

            if (arguments[0] == "search") {
                ClearTerminal();
                try {
                    int count = 0;
                    foreach (var item in store.Inventory) {
                        if (arguments[1].Replace("_", " ").ToLower() == item.Category.ToLower()) {
                            count++;
                        }
                    }

                    if (count == 0) {
                        store.SearchStore(arguments[1], false);
                    } else {
                        store.SearchStore(arguments[1], true);
                    }
                } catch {
                    Console.WriteLine("[!] Invalid syntax! (search {item name})");
                }
            } else if (arguments[0] == "look") {
                ClearTerminal();
                store.ViewStore();
            } else if (arguments[0] == "done") {
                ClearTerminal();
                Recipe recipe = new(name, ingredients);
                user.RecipeList.Add(recipe);
                Console.WriteLine($"'{recipe.RecipeName}' recipe has been created successfully!\n");
                Console.WriteLine(recipe);
                foreach (var ingredient in recipe.RecipeIngredients) {
                    user.GroceryList.Add(ingredient);
                }
                return;
            } else if (arguments.Length > 1) {
                try {
                    int storeItemCount = 0;
                    foreach (var storeItem in store.Inventory) {
                        if (arguments[0].Replace("_", " ").ToLower() == storeItem.Name.ToLower() && int.Parse(arguments[1]) > 0) {        
                            int ingredientCount = 0;
                            storeItemCount++;
                            foreach (var ingredient in ingredients) {
                                if (arguments[0].Replace("_", " ").ToLower() == ingredient.Name.ToLower()) {
                                    ingredient.Quantity += int.Parse(arguments[1]);
                                    ClearTerminal();
                                    Console.WriteLine($"Added {arguments[1]} {ingredient.Name} to the '{name}' recipe successfully! ({ingredient.Quantity} in recipe)");
                                    ingredientCount++;
                                }
                            }

                            if (ingredientCount == 0) {
                                Item newIngredient = new(storeItem.Name, int.Parse(arguments[1]), storeItem.Category, storeItem.Price, storeItem.Calories);
                                ingredients.Add(newIngredient);
                                ClearTerminal();
                                Console.WriteLine($"Added {newIngredient.Quantity} {newIngredient.Name} to the '{name}' recipe successfully!");
                            }
                        }
                    }

                    if (storeItemCount == 0) {
                        ClearTerminal();
                        Console.WriteLine($"[!] Ingredient '{arguments[0]}' does not exist!");
                    }
                } catch {
                    ClearTerminal();
                    Console.WriteLine("[!] Invalid syntax! ({item name} {quantity})");
                }
            } else {
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
        Console.WriteLine("__________________________________________________");
        Console.Write(">> ");
        string name = Console.ReadLine();

        foreach (var recipe in user.RecipeList) {
            try {
                if (name == recipe.RecipeName) {
                    user.RecipeList.Remove(recipe);
                    ClearTerminal();
                    Console.WriteLine($"Recipe '{recipe.RecipeName}' has been removed successfully!");
                    foreach (var ingredient in recipe.RecipeIngredients) {
                        if (user.GroceryList.Contains(ingredient)) {
                            user.GroceryList.Remove(ingredient);
                        }
                    }
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