using PizzaWorld.Code.Projectiles;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PizzaWorld.Code.Tiles;

public class LogDecoration : PizzaBiomeDecoration
{
    protected override TileObjectData TileStyle => TileObjectData.Style3x2;

    protected override int Dust => ModContent.DustType<PizzaDust>();
}