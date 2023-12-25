using PizzaWorld.Code.Items.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items.Armor;

[AutoloadEquip(EquipType.Body)]
public class PizzaBreastplate : ModItem
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
        player.buffImmune[BuffID.Cursed] = true;
        player.AddBuff(BuffID.Honey, 99999 * 60, quiet: true);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.IronBar, 3)
            .AddIngredient<Margherita>(2)
            .AddTile(TileID.Furnaces);
    }
}