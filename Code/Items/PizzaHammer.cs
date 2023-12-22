using PizzaWorld.Code.Items.Food;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaHammer : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldHammer);
        Item.hammer = 65;
        Item.UseSound = SoundID.Item3;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 3)
            .AddIngredient<BasicPizza>(3)
            .AddTile(TileID.Anvils)
            .Register();
    }
}