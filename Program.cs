using System;
using System.Collections.Generic;
using System.IO;

public class Program {
    private static bool Command(string[] arguments, bool ADMIN) {
        // -- Exception Handling --
        if (arguments[0].Length == 0) {
            Console.WriteLine("[!] Invalid Command! Try using 'help' for a list of commands.");
            return true;
        }
        // -- Admin Commands --
        // If there are no arguments in the input, encourage the user to look for commands

        // If the argument is 'exit', end the program by toggling returning false
        if (arguments[0].ToLower() == "exit") {
            return false;
        } else if (arguments[0].ToLower() == "help") {
            Console.WriteLine("===[ List Of Commands ]===");
            Console.WriteLine("- help = Displays a list of commands");
            Console.WriteLine("- exit = Exits the program");
            Console.WriteLine("- view store = Lists all the items that the store has an their quantity");
            return true;
        }

        // If the arguments did not match any of the conditions, go back to input prompt
        Console.WriteLine("[!] Invalid Command! Try using 'help' for a list of commands.");
        return true;
    }

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

    private static void Main() {
        bool RUNNING = true; // Status of the program. Once it toggles false the program ends.
        bool ADMIN = false; // Toggle for administration priviledges.
        double balance = 1000.00; // Current balance for the user (set to any double).
        int age = 18; // Current age for the user (set to any int)
        string path = "Store.csv";

        var cart = new List<Food>();
        Store store = new Store();

        if (!File.Exists(path)) {
            Console.WriteLine("[!] Could not load store correctly!");
            return;
        } else {
            Console.WriteLine("{GSA} >> Store has successfully loaded!");
        }

        int lineCount = GetLineCount(path);

        Console.WriteLine("============================");
        Console.WriteLine("Grocery Shopping Application");
        Console.WriteLine("----------------------------");
        Console.WriteLine($"> Balance: ${balance}");
        Console.WriteLine($"> Age: {age}\n");
        Console.WriteLine($"> Items in cart: {cart.Count}");
        Console.WriteLine("============================");

        while (RUNNING) {
            Console.WriteLine("\n----------------------------");
            Console.Write(">> ");
            string command = Console.ReadLine(); // Get the user input
            string[] arguments = command.Split(' '); // Split the input by spaces to get the arguments

            Console.WriteLine("----------------------------");
            RUNNING = Command(arguments, ADMIN);
        }
    }
}
