using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaHammer : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldHammer);
        Item.hammer = 65;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 3)
            .AddIngredient(ItemID.Pizza, 3)
            .AddTile(TileID.Anvils)
            .Register();
    }
}