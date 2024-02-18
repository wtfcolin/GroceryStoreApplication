using System;
using System.Collections.Generic;

public class Recipe {
    private string name;
    private List<Item> ingredient;
    /*
     * Constructor for 'Recipe' object
     * - name (string): Name of the recipe
     * - ingredient (List<Item>): List of item objects that can be chosen from the list of items in the store inventory
     */
    // Override function that displays information about the store (can be seen with 'store' command)
    public override string ToString() {
        string ingredientsList = "";

        foreach (var item in Ingredient) {
            ingredientsList += $"- {item.Quantity} {item.Name}\n";    
        }

        return $"---[ Recipe Information ]---\nName: {Name}\nIngredients:\n{ingredientsList}";
    }
    public void CreateRecipe(Store store) {
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
            
            Console.WriteLine("Please enter an ingredient name and amount seperated by a space (Ex: Burger_Patty 2)");
            Console.WriteLine("--------------------------------------------------");
            Console.Write(">> ");
            string input = Console.ReadLine();
            string[] args = input.Split(" ");
            
            if (args[1].GetType() == typeof(string) && args[2].GetType() == typeof(int)) {
                foreach (var storeItem in store.Inventory) {
                    if (args[1].Replace("_", " ").ToLower() == storeItem.Name.ToLower()) {

                    } else {

                    }
                }
            }
        }
    }
    public Recipe(string name, List<Item> ingredient) {
        Name = name;
        Ingredient = ingredient;
    }
    public string Name {
        get => name;
        set {
            // Set the name to 'NONE' if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid name! Name was set to 'NONE'.");
                name = "NONE";
            }

            name = value;
        }
    }
    public List<Item> Ingredient {
        get => ingredient;
        set {
            ingredient = value;
        }
    }
}