using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaBow : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.PlatinumBow);
        Item.damage = 15;
        Item.crit = 7;
        Item.useTime = 25;
        Item.value = Price.Silver(25);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 3)
            .AddIngredient<Margherita>(2)
            .Register();
    }
}