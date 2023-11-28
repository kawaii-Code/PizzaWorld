using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaAxe : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldAxe);
        Item.axe = 65;
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