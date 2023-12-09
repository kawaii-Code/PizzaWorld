using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace PizzaWorld.Code.Tiles;

public abstract class PizzaBiomeDecoration : ModTile
{
    protected virtual TileObjectData TileStyle => TileObjectData.Style1x1;
    protected virtual int Dust => DustID.Grass;
    protected virtual int DrawYOffset => 2;
    
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoFail[Type] = true;
        Main.tileObsidianKill[Type] = true;
        
        TileObjectData.newTile.CopyFrom(TileStyle);
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.DrawYOffset = DrawYOffset;
        TileObjectData.addTile(Type);

        DustType = Dust;
        AddMapEntry(new Color(255, 0, 0));
    }
}