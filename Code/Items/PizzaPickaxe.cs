using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaPickaxe : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.MoltenPickaxe);
        Item.damage = 10;
        Item.crit = 5;
        Item.knockBack = 3.0f;
        Item.pick = 85;
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