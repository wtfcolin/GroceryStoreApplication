using System;
using System.Collections.Generic;

public class User {
    private bool admin;
    private int age;
    private double balance;
    private List<Food> cart;
    // private List<Food> list;

    public User(int age, double balance, List<Food> cart, bool admin) {
        Age = age;
        Balance = balance;
        Cart = cart;
        this.admin = admin;
    }

    public void ViewCart() {
        int counter = 0;
        Console.WriteLine("==================[ Your Cart ]===================");

        if (Cart.Count == 0) {
            Console.WriteLine("There are currently 0 items in your cart...");
        } else {
            foreach (var food in Cart) {
                counter++;
                Console.WriteLine($"{counter}) {food.Quantity} {food.Name} [{(food.Quantity * food.Price).ToString("$#,##0.00")}]");
            }
        }

        Console.WriteLine("==================================================");
        return;
    }

    public void CheckList() {
        Console.WriteLine("==============[ Your Grocery List ]===============");

        //for (int i = 0; i < list.Length; i++) {
        //    Console.WriteLine($"[{}] {}");
        //}

        Console.WriteLine("==================================================");
        return;
    }

    public void AddItem(string item, int amount, Store store) {
        bool check = true; 
        
        foreach (var storeItem in store.Inventory) {
            // For each food in inventory, check if the store has the item and its not out of stock
            if (item.ToLower() == storeItem.Name.ToLower() && storeItem.Quantity > 0) {
                // For each food in inventory, check if the amount that was requested is less than the current stock
                check = false;
                if (storeItem.Quantity - amount >= 0) {
                    if (Cart.Count > 0) {
                        foreach (var cartItem in Cart) {
                            // For each item in the cart, check to see if the item already exists in the cart and if they have the same name
                            if (cartItem.Name.ToLower() == item.ToLower()) {
                                // Add 1 to quantity to existing item
                                cartItem.Quantity += amount;
                                storeItem.Quantity -= amount;
                                Console.WriteLine("First line");
                            }
                            // Decrement 1 to the quantity of the item in the store.
                        }
                    } else {
                        // Create a new food object and set the quantity to 1 while copying the other properties from the store
                        Food newItem = new(storeItem.Name, amount, storeItem.Category, storeItem.Price, storeItem.Calories);
                        storeItem.Quantity -= amount;
                        Cart.Add(newItem);
                        Console.WriteLine("Second line");
                    }
                } else {
                    Console.WriteLine($"[!] There is not enough '{item}' in stock!");
                } 
            }
        }

        if (check) {
            Console.WriteLine($"[!] This store does not have any '{item}'!");
        }

        return;
    }

    public void RemoveItem(string item, int amount, Store store) {
        foreach (var cartItem in Cart) {

        }
        return;
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

    public bool Admin {
        get => admin;
        set {
            admin = value;
        }
    }
}