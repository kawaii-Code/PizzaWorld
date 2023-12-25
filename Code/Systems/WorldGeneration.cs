using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using MonoMod.Cil;
using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.NPCs.Bosses;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using PizzaWorld.Code.Utilities;
using PizzaWorld.Code.Utilities;

namespace PizzaWorld.Code.Systems;

public class WorldGeneration : ModSystem
{
    private int _timer;
    
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
        if (KeyDown(Keys.N))
        {
            PizzaWorld.SpawnItem<Napoletana>((int)(Main.player[Main.myPlayer].Center.X + 10f) ,(int)(Main.player[Main.myPlayer].Center.Y ));
        }
    }

    private bool KeyDown(Keys keys)
    {
        return Main.keyState.IsKeyDown(keys) && Main.oldKeyState.IsKeyUp(keys);
    }
}
