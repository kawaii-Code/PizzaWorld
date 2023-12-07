using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Tiles;

public class PizzaTile : ModTile
{
    public override void SetStaticDefaults()
    {
        Color pizzaColor = new(200, 200, 200);

        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;

        DustType = DustID.DirtSpray;
        AddMapEntry(pizzaColor);
    }

    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        num = 5;
    }
}