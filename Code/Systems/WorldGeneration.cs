using Microsoft.Xna.Framework.Input;
using PizzaWorld.Code.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Systems;

public class WorldGeneration : ModSystem
{
    public override void PostUpdateInput()
    {
        if (KeyDown(Keys.P))
        {
            PlacePizzaTiles((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
        }
    }

    private void PlacePizzaTiles(int x, int y)
    {
        WorldGen.TileRunner(x, y, 8, 8, ModContent.TileType<PizzaTile>());
    }

    private bool KeyDown(Keys keys)
    {
        return Main.keyState.IsKeyDown(keys) && Main.oldKeyState.IsKeyUp(keys);
    }
}