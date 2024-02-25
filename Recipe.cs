using System;
using System.Collections.Generic;

public class Recipe {
    private string recipeName;
    private List<Item> recipeIngredients;
    
    public Recipe(string recipeName, List<Item> ingredients) {
        RecipeName = recipeName;
        RecipeIngredients = ingredients;
    }

    public override string ToString() {
        string ingredientsList = "";

        foreach (Item ingredient in RecipeIngredients) {
            ingredientsList += $"* {ingredient.Quantity} {ingredient.Name}\n";    
        }

        return $"==============[ Recipe Information ]==============\nName:\t{RecipeName}\nIngredients:\n{ingredientsList}\n==================================================";
    }

    /*  Recipe properties
     */
    public string RecipeName {
        get => recipeName;
        set {
            // Set the name to 'NONE' if the input is empty/null
            if (string.IsNullOrWhiteSpace(value)) {
                Console.WriteLine("[!] Invalid name! Name was set to 'NONE'.");
                recipeName = "NONE";
            }

            recipeName = value;
        }
    }
    public List<Item> RecipeIngredients {
        get => recipeIngredients;
        set {
            recipeIngredients = value;
        }
    }
}