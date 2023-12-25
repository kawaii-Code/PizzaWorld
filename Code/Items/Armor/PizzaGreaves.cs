using PizzaWorld.Code.Items.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items.Armor;

[AutoloadEquip(EquipType.Legs)]
public class PizzaGreaves : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;

        Item.value = Item.buyPrice(platinum:1, gold: 1);
        Item.rare = ItemRarityID.Blue;

        Item.defense = 6;
    }
    
    public override void UpdateEquip(Player player)
    {
        player.AddBuff(BuffID.Gravitation, 60 * 99999 , quiet: true);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 3)
            .AddIngredient<Margherita>()
            .AddTile(TileID.Furnaces);
    }
}