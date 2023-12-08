using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace PizzaWorld.Code.Systems;

public class PizzaBiomePass : GenPass
{
    public PizzaBiomePass(string name, double loadWeight) : base(name, loadWeight)
    {
    }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            return;
        }
        
        progress.Message = Localized("PizzaBiomePassMessage");
        bool toTheRight = WorldGen.genRand.NextBool();
        int x = 0;
        int attempt = 1;
        int offsetFromSpawn = 100;
        while (attempt < 1000)
        {
            if (toTheRight)
            {
                x = WorldGen.genRand.Next(Main.maxTilesX / 3, Main.maxTilesX / 2 - offsetFromSpawn);
            }
            else
            {
                x = WorldGen.genRand.Next(Main.maxTilesX / 2 + offsetFromSpawn, 2 * Main.maxTilesX / 3);
            }

            if (IsSuitableTile(WorldGen.TileType(x, FindSurface(x))))
            {
                break;
            }
            attempt++;
        }

        int plotchCount = 20;
        int offset = 10;
        int depth = 5;
        List<Point> points = new();
        for (int i = 0; i < plotchCount; i++)
        {
            int tx = x + (i - plotchCount / 2) * offset;
            int ty = FindSurface(tx) + depth;
            points.Add(new Point(tx, ty));
        }

        ushort pizzaTile = (ushort)ModContent.TileType<PizzaTile>();
        foreach (Point point in points)
        {
            WorldUtils.Gen(point, new Shapes.Circle(offset, depth), new Actions.SetTile(pizzaTile));
        }
        
        bool IsSuitableTile(int tile)
        {
            return tile is TileID.Dirt or TileID.Stone or TileID.Sand;
        }

        int FindSurface(int x)
        {
            int y = 1;
            while (y < Main.worldSurface && !WorldGen.SolidTile(x, ++y))
                ;
            return y;
        }
    }

    private string Localized(string key)
    {
        const string prefix = "Mods.PizzaWorld.";
        return Language.GetText(prefix + key).Value;
    }
}