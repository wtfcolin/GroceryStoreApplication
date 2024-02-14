public class Food {
    private string name; // Name of food item
    private int quantity; // Quantity of the food item in the store
    private string foodGroup; // Type of food for potential categorization
    private double price; // Price for the food item (per 1)
    private int calories; // Calories for the food item (per 1)

    public Food(string name, int quantity, string foodGroup, double price, int calories) {
        Name = name;
        Quantity = quantity;
        FoodGroup = foodGroup;
        Price = price;
        Calories = calories;
    }

    public override string ToString() {
        return $"===[ {Name} ]===\nCategory: {FoodGroup}\nCalories: {Calories}\n\nPrice: ${Price}\n({Quantity}) in stock";
    }

    public string Name {
        get => name;
        set {

        }
    }

    public int Quantity {
        get => quantity;
        set {

        }
    }

    public string FoodGroup {
        get => foodGroup;
        set {

        }
    }

    public double Price {
        get => price;
        set {

        }
    }

    public int Calories {
        get => calories;
        set {

        }
    }
}