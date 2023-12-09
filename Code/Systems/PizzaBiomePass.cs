using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
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

        { // Decorations
            int shroomCount = 15;
            int shroomLeft = x - 200;
            int shroomRight = x + 200;
            for (int i = 0; i < shroomCount; i++)
            {
                PlaceOnPizzaTile<MushroomDecoration>(100, 6, pizzaTile, shroomLeft, shroomRight);
            }
            
            int cucumberCount = 10;
            int cucumberLeft = x - 200;
            int cucumberRight = x + 200;
            for (int i = 0; i < cucumberCount; i++)
            {
                PlaceOnPizzaTile<CucumberDecoration>(100, 1, pizzaTile, cucumberLeft, cucumberRight);
            }
            
            int logCount = 2;
            int logLeft = x - 200;
            int logRight = x + 200;
            for (int i = 0; i < logCount; i++)
            {
                PlaceOnPizzaTile<LogDecoration>(100, 1, pizzaTile, logLeft, logRight);
            }
        }

        bool PlaceOnPizzaTile<T>(int maxAttempts, int styleCount, ushort pizzaTileType, int left = 0, int right = -1)
            where T : ModTile
        {
            if (right == -1)
            {
                right = Main.maxTilesX;
            }
            
            bool spawned = false;
            int attempts = 0;
            while (!spawned && attempts <= maxAttempts)
            {
                attempts++;
                    
                int tileX = WorldGen.genRand.Next(left, right);
                int tileY = FindSurface(tileX);
                if (Main.tile[tileX, tileY].TileType != pizzaTileType)
                {
                    continue;
                }
                    
                int type = ModContent.TileType<T>();
                int style = WorldGen.genRand.Next(styleCount);
                WorldGen.PlaceTile(tileX, tileY - 1, type, mute: true, style: style);
                spawned = true;
            }

            return spawned;
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