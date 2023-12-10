using Microsoft.Xna.Framework.Graphics;
using PizzaWorld.Code.Items;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Tiles;

public class PizzaTree : ModTree
{
    public override void SetStaticDefaults()
    {
        GrowsOnTileId = new[] { ModContent.TileType<PizzaTile>() };
    }

    public override Asset<Texture2D> GetTexture()
    {
        return ModContent.Request<Texture2D>("PizzaWorld/Code/Tiles/PizzaTree");
    }

    public override Asset<Texture2D> GetTopTextures()
    {
        return ModContent.Request<Texture2D>("PizzaWorld/Code/Tiles/PizzaTreeTops");
    }

    public override Asset<Texture2D> GetBranchTextures()
    {
        return ModContent.Request<Texture2D>("PizzaWorld/Code/Tiles/PizzaTreeBranches");
    }

    public override int DropWood()
    {
        return ModContent.ItemType<PizzaWood>();
    }

    public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth,
        ref int topTextureFrameHeight)
    {
    }

    public override TreePaintingSettings TreeShaderSettings =>
        new()
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 0.88f,
        };
}