using PizzaWorld.Code.Items.Food;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaAxe : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldAxe);
        Item.axe = 65;
        Item.UseSound = SoundID.Item111;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 3)
            .AddIngredient(ModContent.ItemType<BasicPizza>(), 2)
            .AddTile(TileID.Anvils)
            .Register();
    }
}