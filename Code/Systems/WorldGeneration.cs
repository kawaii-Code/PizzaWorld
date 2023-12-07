using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using PizzaWorld.Code.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace PizzaWorld.Code.Systems;

public class WorldGeneration : ModSystem
{
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int insertIndex = tasks.FindIndex(genpass => genpass.Name == "Full Desert");
        if (insertIndex != -1)
        {
            tasks.Insert(insertIndex + 1, new PizzaBiomePass("Pizza Biome", 100.0f));
        }
    }

    public override void PostUpdateInput()
    {
        if (KeyDown(Keys.P))
        {
            PizzaBiomePass.SpawnPizzaTiles();
        }
    }

    private bool KeyDown(Keys keys)
    {
        return Main.keyState.IsKeyDown(keys) && Main.oldKeyState.IsKeyUp(keys);
    }
}