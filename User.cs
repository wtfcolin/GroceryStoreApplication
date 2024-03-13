using System;
using System.Collections.Generic;

public class User {
    private string name; // Name of the user
    private int age; // Age of the user
    private double balance; // Balance of the user

    public User(string name, int age, double balance, List<Item> cart, bool admin, List<Item> groceryList, List<Recipe> recipeList) {
        Name = name;
        Age = age;
        Balance = balance;
        Cart = cart;
        Admin = admin;
        GroceryList = groceryList;
        RecipeList = recipeList;
    }

    /* Override function that displays information about the user (can be seen using 'me' command)
     */
    public override string ToString() {
        return $"===============[ Your Information ]===============\nAge:\t\t{Age}\nBalance:\t{Balance:$#,##0.00}\nAdmin:\t\t{Admin}\n==================================================";
    }

    /* Function that allows the user to view their current balance subtracted by the current items in their cart
     */
    public void ViewBalance(double taxRate) {
        Console.WriteLine("=================[ Your Balance ]=================");
        Console.WriteLine($"Current Balance:\t{Balance:$#,##0.00}");

        double subtotal = 0.00;
        foreach (var item in Cart) {
            subtotal += item.Price * item.Quantity;
        }

        double tax = subtotal * taxRate;
        double newBalance = subtotal + tax;
        
        Console.WriteLine($"\nCart Subtotal:\t\t{subtotal:$#,##0.00}");
        Console.WriteLine($"Tax:\t\t\t{tax:$#,##0.00}");
        Console.WriteLine($"\t\t      + __________");
        Console.WriteLine($"Current Total:\t\t{newBalance:$#,##0.00}");
        Console.WriteLine($"\nNew Balance:\t\t{Balance - newBalance:$#,##0.00} (-{newBalance:$#,##0.00})");
        Console.WriteLine("==================================================");
    }

    /* Function that allows the user to view the current Food objects inside the cart list
     */
    public void ViewCart() {
        int counter = 0;
        Console.WriteLine("==================[ Your Cart ]===================");

        if (Cart.Count == 0) {
            Console.WriteLine("There are currently 0 items in your cart...");
        } else {
            foreach (var cartItem in Cart) {
                counter++;
                Console.WriteLine($"{counter}) {cartItem.Quantity} {cartItem.Name} [{cartItem.Quantity * cartItem.Price:$#,##0.00}]");
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    /* Function that allows the user to view their current recipes
     */
    public void ViewRecipes() {
        Console.WriteLine("===============[ Your Recipe List ]===============");

        if (RecipeList.Count == 0) {
            Console.WriteLine("There are currently 0 items on your recipe list...");
        } else {
            int totalCalories = 0;
            foreach (var recipe in RecipeList) {
                Console.WriteLine($"\n---[ {recipe.Name} ]---");
                foreach (var ingredient in recipe.Ingredients) {
                    totalCalories += ingredient.Calories * ingredient.Quantity;
                }

                Console.WriteLine($"Calories: {totalCalories}");
                foreach (var ingredient in recipe.Ingredients) {
                    Console.WriteLine($"* {ingredient.Quantity} {ingredient.Name}");
                }
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    /* Function that allows the user to view the current Food objects inside the groceryStore list
     */
    public void ViewList() {
        Console.WriteLine("==============[ Your Grocery List ]===============");

        if (GroceryList.Count == 0) {
            Console.WriteLine("There are currently 0 items on your grocery list...");
        } else {
            foreach (var listItem in GroceryList) {
                bool listItemFulfilled = false;
                foreach (var cartItem in Cart) {
                    if (listItem.Name == cartItem.Name && cartItem.Quantity >= listItem.Quantity) {
                        listItemFulfilled = true;
                    }
                }

                if (!listItemFulfilled) {
                    Console.WriteLine($"[ ] {listItem.Quantity} {listItem.Name}");
                } else {
                    Console.WriteLine($"[X] {listItem.Quantity} {listItem.Name}");
                }
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    /* Function that allows the user to add items into their grocery list
     */
    public void AddListItem(string itemName, int amount, Store store) {
        bool storeItemExists = false;

        foreach (var storeItem in store.Inventory) {
            if (itemName.Replace("_", " ").Equals(storeItem.Name, StringComparison.CurrentCultureIgnoreCase)) {
                storeItemExists = true;
                bool listItemExists = false;
                foreach (var listItem in GroceryList) {
                    if (itemName.Replace("_", " ").Equals(listItem.Name, StringComparison.CurrentCultureIgnoreCase)) {
                        listItemExists = true;
                        listItem.Quantity += amount;
                        break;
                    }
                }

                if (!listItemExists) {
                    Item newListItem = new(storeItem.Name, amount, storeItem.Category, storeItem.Price, storeItem.Calories);
                    GroceryList.Add(newListItem);
                }

                Console.WriteLine($"{amount} {storeItem.Name} was added to your grocery list!");
            }
        }

        if (!storeItemExists) {
            Console.WriteLine($"[!] This store does not have any '{itemName.Replace("_", " ")}'!");
        }

        return;
    }

    /* Function that allows the user to add items into their grocery list
     */
    public void RemoveListItem(string itemName, int amount) {
        bool listItemExists = false;

        foreach (var listItem in GroceryList) {
            if (itemName.Replace("_", " ").Equals(listItem.Name, StringComparison.CurrentCultureIgnoreCase)) {
                listItemExists = true;
                if (amount > listItem.Quantity) {
                    GroceryList.Remove(listItem);
                    Console.WriteLine($"You only had {listItem.Quantity} '{listItem.Name}' on your list...");
                    Console.WriteLine($"'{listItem.Name}' was removed from your grocery list!");
                } else {
                    listItem.Quantity -= amount;
                }
            }
        }

        if (!listItemExists) {
            Console.WriteLine($"[!] Your grocery list does not have any '{itemName.Replace("_", " ")}'!");
        }

        return;
    }

    /* Function that allows the user to add items into their cart
     */
    public void AddCartItem(string itemName, int amount, Store store) {
        bool storeItemExists = false;

        foreach (var storeItem in store.Inventory) {
            if (itemName.ToLower().Replace("_", " ").Equals(storeItem.Name, StringComparison.CurrentCultureIgnoreCase) && storeItem.Quantity > 0) {
                storeItemExists = true;
                if (Age < 21 && storeItem.Category == "Alcohol") {
                    Console.WriteLine("[!] This store does not sell alcohol to people younger than 21!");
                    return;
                }

                if (storeItem.Quantity >= amount) {
                    bool cartItemExists = false;
                    foreach (var cartItem in Cart) {
                        if (cartItem.Name.ToLower().Replace("_", " ").ToLower() == itemName.ToLower()) {
                            cartItem.Quantity += amount;
                            storeItem.Quantity -= amount;
                            cartItemExists = true;
                            break;
                        }
                    }

                    if (!cartItemExists) {
                        Item newItem = new(storeItem.Name, amount, storeItem.Category, storeItem.Price, storeItem.Calories);
                        storeItem.Quantity -= amount;
                        Cart.Add(newItem);
                    }

                    Console.WriteLine($"{amount} {storeItem.Name} was added to your cart!");
                } else {
                    Console.WriteLine($"[!] There is not enough '{itemName.Replace("_", " ")}' in stock!");
                }
                break;
            }
        }

        if (!storeItemExists) {
            Console.WriteLine($"[!] This store does not have any '{itemName.Replace("_", " ")}'!");
        }

        return;
    }
    
    /* Function that allows the user to remove items from their cart
     */
    public void RemoveCartItem(string itemName, int amount, Store store) {
        bool storeItemExists = false;

        foreach (var cartItem in Cart) {
            // For each item in the cart, find item with matching name and quantity is greater than 0
            if (itemName.Replace("_", " ").ToLower() == cartItem.Name.ToLower() && amount > 0) {
                storeItemExists = true;
                foreach (var storeItem in store.Inventory) {
                    // For each item in the store inventory, if item matches & current cart quantity is either = or > 0, then proceed with transaction
                    if (storeItem.Name.ToLower() == itemName.Replace("_", " ").ToLower()) {
                        // If the amount being removed is = or > 0, remove the items from cart and add to store inventory
                        if (cartItem.Quantity - amount == 0) {
                            Cart.Remove(cartItem);
                            storeItem.Quantity += amount;
                            Console.WriteLine($"{amount} {storeItem.Name} was removed from your cart!");
                        } else if (cartItem.Quantity - amount > 0) {
                            cartItem.Quantity -= amount;
                            storeItem.Quantity += amount;
                            Console.WriteLine($"{amount} {storeItem.Name} was removed from your cart!");
                        // If the amount is more than what is in the cart, give an error
                        } else {
                            Console.WriteLine($"[!] You do not have that many '{itemName.Replace("_", " ")}' in your cart!");
                        }
                    }
                }
            }
        }
        // If there are no matching items in the cart, give an error
        if (!storeItemExists) {
            Console.WriteLine($"[!] There is currently no '{itemName.Replace("_", " ")}' in your cart!");
        }

        return;
    }

    /*  User properties
     */
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
    public int Age {
        get => age;
        set {
            if (value.GetType() != typeof(int)) {
                Console.WriteLine("[!] Invalid age! Age was set to '18' instead.");
                age = 18;
            } else if (value < 0) {
                Console.WriteLine("[!] The age can't be less than 0! Age was set to '18' instead.");
                age = 18;
            }

            age = value;
        }
    }
    public double Balance {
        get => balance;
        set {
            if (value.GetType() != typeof(double)) {
                Console.WriteLine("[!] Invalid balance! Balance was set to '0' instead.");
                balance = 0;
            } else if (value < 0) {
                Console.WriteLine("[!] The balance can't be less than $0! Balance was set to '0.0' instead.");
                balance = 0;
            }

            balance = value;
        }
    }
    public List<Item> Cart { get; set; }
    public List<Item> GroceryList { get; set; }
    public List<Recipe> RecipeList { get; set; }
    public bool Admin { get; set; }
}