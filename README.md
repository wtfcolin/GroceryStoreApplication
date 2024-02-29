GROCERY STORE APPLICATION

Overview:
  This application is a terminal where the user can
  give inputs and simulate buying items from a grocery store.

Commands ({}: Required Arguments, []: Optional Args):
  - home = Prints the start menu
  - help = Prints the help menu
  - exit = Exits the application
  - list [add/remove {item name}] = Print, add, and remove items from your grocery list
  - cart [add/remove {item name}] = Print, add, and remove items from your cart
  - recipe {create/remove} = Create/remove recipes in your recipe list
  - reicpes = Prints out recipes in your recipe list
  - look = Lists all the items that are currently in the store
  - search {string} = Searches store inventory for items that contain item name
  - balance = Prints out information about your balance
  - store = Prints out information about the store
  - me = Prints out information about you

  Admin Commmands:
	- set age {years} = Sets your age
	- set balance {amount} = Sets your balance
	- set storebalance {amount} = Sets the store's balance

Walkthrough:
  1) When you first start the program, you are given a 'Cart (cart)',
  'Grocery List (list)', and 'Recipe List (recipes)' which prints
  information about each respectively.
		* Use 'cart' to view your current cart items.
		* Use 'list' to view your current grocery list items.
		* Use 'recipes' to view your current recipes.
  2) Let's look around the store for food items!
		* Use 'look' to view all the store items.
		* Use 'search {string}' to search for any items that match the
			substring {string}.
  3) Time to grab some items!
		* Use 'cart add {item name} {quantity}' to add food items to 
			your cart.
		* Use 'list add {item name} {quantity}' to add food items to
			your grocery list.
			!) NOTE: Any items you add to your grocery list MUST be
				added to your cart before you are able to checkout.
  4) Time to checkout!
		* Use 'checkout' to pay for the items in your cart.
