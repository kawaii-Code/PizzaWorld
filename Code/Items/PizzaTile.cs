using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaTile : ModItem
{
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PizzaTile>());
        Item.width = 16;
        Item.height = 16;
    }
}