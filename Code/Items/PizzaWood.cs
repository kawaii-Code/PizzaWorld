using PizzaWorld.Code.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaWood : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Wood);
        Item.createTile = ModContent.TileType<PizzaWoodTile>();
    }
}