using System;

public class Program {
    private static void Main() {
        bool RUNNING = true;
        bool ADMIN = false;
        double BALANCE = 1000.00;
        int AGE = 18;

        Console.WriteLine("=========================");
        Console.WriteLine("Grocery Store Application\n");
        Console.WriteLine("Current Balance: " + );
        Console.WriteLine("=========================");

        while (RUNNING) {
            Console.WriteLine("\n\n\n");
            Console.Write(">> ");
            string command = Console.ReadLine();

            if (command == "exit") {
                RUNNING = false;
            }
        }
    }
}
