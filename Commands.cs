using System;

public static class Commands {
    public static bool Command(string[] arguments, User user, Store store) {
        switch (arguments[0]) {
            case "set":
            if (arguments[1] == "name") {
                SetValue(arguments[1], arguments[2], user, store);
            } else {
                SetValue(arguments[1], double.Parse(arguments[2]), user, store);
            }
            return true;

            case "add":
            user.AddItem(arguments[1], int.Parse(arguments[2]), store, user.Age);
            return true;

            case "remove":
            user.RemoveItem(arguments[1], int.Parse(arguments[2]), store);
            return true;

            case "help":
            PrintHelp(user.Admin);
            return true;

            case "cart":
            user.ViewCart();
            return true;

            case "options":
            store.ViewStore();
            return true;

            case "balance":
            Console.WriteLine(user.UserBalance);
            return true;

            case "bal":
            Console.WriteLine(user.UserBalance);
            return true;

            case "search":
            // Search function
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

    public static void PrintHelp(bool ADMIN) {
        Console.WriteLine("=========================[ List Of Commands ]=========================");
        Console.WriteLine("- help = Displays a list of commands");
        Console.WriteLine("- exit = Exits the program");
        Console.WriteLine("- add [item] [amount] = Adds a food item to your cart");
        Console.WriteLine("- remove [item] [amount] = Removes a food item from your cart");
        Console.WriteLine("- cart = Lists all the items that are in your cart currently");
        Console.WriteLine("- checkout = Prints a receipt of your items if all the conditions are satisfied");
        Console.WriteLine("- options = Lists all the items that the store has and their quantity");
        Console.WriteLine("- list = Prints out your grocery list");
        Console.WriteLine("- balance; bal = Prints out your balance");
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

    public static void SetValue(string option, double amount, User user, Store store) {
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

    public static void SetValue(string option, string value, User user, Store store) {
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

    public static void ClearTerminal() {
        for (int i = 0; i < 30; i++) {
            Console.WriteLine("\n");
        }
    }

    public static bool CommandLine (User user, Store store) {
        Console.WriteLine("\n--------------------------------------------------");
        Console.Write(">> ");
        string command = Console.ReadLine(); // Get the user input
        string[] arguments = command.Split(' '); // Split the input by spaces to get the arguments
        Console.WriteLine("--------------------------------------------------");

        ClearTerminal();

        return Command(arguments, user, store);
    } 
}