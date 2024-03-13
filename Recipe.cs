using System;
using System.Collections.Generic;

public class Recipe {
    private string name;
    
    public Recipe(string name, List<Item> ingredients) {
        Name = name;
        Ingredients = ingredients;
    }

    public override string ToString() {
        string ingredientsList = "";
        int totalCalories = 0;

        foreach (Item ingredient in Ingredients) {
            ingredientsList += $"\t\t* {ingredient.Quantity} {ingredient.Name}\n";
            totalCalories += ingredient.Calories * ingredient.Quantity;
        }

        return $"==============[ Recipe Information ]==============\nName:\t\t{Name}\nCalories:\t{totalCalories}\nIngredients:\n{ingredientsList}\n==================================================";
    }

    /*  Recipe properties
     */
    public string Name {
        get => name;
        set {
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid name! Name was set to 'NONE'.");
                name = "NONE";
            }

            name = value;
        }
    }
    public List<Item> Ingredients { get; set; }
}