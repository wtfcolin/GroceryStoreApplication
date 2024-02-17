using System;
using System.Collections.Generic;
using System.IO;

public class User {
    private string name;
    private bool admin;
    private int age;
    private double balance;
    private List<Food> cart;
    private List<Food> groceryList;

    public User(string name, int age, double balance, List<Food> cart, bool admin, List<Food> groceryList) {
        Name = name;
        Age = age;
        Balance = balance;
        Cart = cart;
        Admin = admin;
        GroceryList = groceryList;
    }

    public override string ToString() {
        return $"---[ User Information ]---\nAge: {Age}\nBalance: {Balance:$#,##0.00}\nAdmin: {Admin}";
    }

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

    public void ViewList() {
        Console.WriteLine("==============[ Your Grocery List ]===============");

        if (GroceryList.Count == 0) {
            Console.WriteLine("There are currently 0 items on your grocery list...");
        } else {
            foreach (var listItem in GroceryList) {
                Console.WriteLine($"- {listItem.Quantity} {listItem.Name}");
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    public void AddItem(string item, int amount, Store store, int userAge) {
        bool check = true;

        foreach (var storeItem in store.Inventory) {
            // For each food in inventory, check if the store has the item and its not out of stock
            if (item.ToLower() == storeItem.Name.ToLower() && storeItem.Quantity > 0) {
                // For each food in inventory, check if the amount that was requested is less than the current stock
                check = false;
                if (userAge < 21 && storeItem.Category == "Alcohol") {
                    Console.WriteLine("[!] This store does not sell alcohol to people younger than 21!");
                    return;
                }

                if (storeItem.Quantity - amount >= 0) {
                    if (Cart.Count > 0) {
                        foreach (var cartItem in Cart) {
                            // For each item in the cart, check to see if the item already exists in the cart and if they have the same name
                            if (cartItem.Name.ToLower() == item.ToLower()) {
                                // Add 1 to quantity to existing item
                                cartItem.Quantity += amount;
                                storeItem.Quantity -= amount;
                            }
                        }
                    } else {
                        // Create a new food object and set the quantity to 1 while copying the other properties from the store
                        Food newItem = new(storeItem.Name, amount, storeItem.Category, storeItem.Price, storeItem.Calories);
                        storeItem.Quantity -= amount;
                        Cart.Add(newItem);
                    }
                    // Tell the user that the item was added to their cart
                    Console.WriteLine($"{amount} {storeItem.Name} was added to your cart!");
                } else {
                    // If the amount is more than the store's quantity, give an error
                    Console.WriteLine($"[!] There is not enough '{item}' in stock!");
                }
            }
        }
        // If there are no matching items in the store or if there is 0 stock, give an error
        if (check) {
            Console.WriteLine($"[!] This store does not have any '{item}'!");
        }

        return;
    }

    public void RemoveItem(string item, int amount, Store store) {
        bool check = true;

        foreach (var cartItem in Cart) {
            // For each item in the cart, find item with matching name and quantity is greater than 0
            if (item.ToLower() == cartItem.Name.ToLower() && cartItem.Quantity > 0) {
                check = false;
                foreach (var storeItem in store.Inventory) {
                    // For each item in the store inventory, if item matches & current cart quantity is either = or > 0, then proceed with transaction
                    if (storeItem.Name.ToLower() == item.ToLower()) {
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
                            Console.WriteLine($"[!] You do not have that many '{item}' in your cart!");
                        }
                    }
                }
            }
        }

        if (check) {
            Console.WriteLine($"[!] There is currently no '{item}' in your cart!");
        }

        return;
    }

    public string Name {
        get => name;
        set {
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
                age = 0;
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

    public List<Food> Cart {
        get => cart;
        set {
            cart = value;
        }
    }

    public List<Food> GroceryList {
        get => groceryList;
        set {
            groceryList = value;
        }
    }

    public bool Admin {
        get => admin;
        set {
            admin = value;
        }
    }
}