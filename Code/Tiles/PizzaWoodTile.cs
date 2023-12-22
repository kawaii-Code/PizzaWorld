using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Tiles;

public class PizzaWoodTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Color pizzaWoodColor = new(0, 200, 200);

        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;

        DustType = DustID.PalmWood;
        AddMapEntry(pizzaWoodColor);
    }
}