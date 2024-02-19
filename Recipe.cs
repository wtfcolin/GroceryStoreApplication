using System;
using System.Collections.Generic;

public class Recipe {
    private string name;
    private List<Item> ingredient;
    /*
     * Constructor for 'Recipe' object
     * 
     */
    public Recipe(string name, List<Item> ingredient) {
        Name = name;
        Ingredient = ingredient;
    }

    public override string ToString() {
        string ingredientsList = "";

        foreach (var item in Ingredient) {
            ingredientsList += $"- {item.Quantity} {item.Name}\n";    
        }

        return $"---[ Recipe Information ]---\nName: {Name}\nIngredients:\n{ingredientsList}";
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