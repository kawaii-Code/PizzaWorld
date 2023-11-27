using Microsoft.Xna.Framework.Input;
using PizzaWorld.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld;

public class ModEntry : ModSystem
{
    public override void PostUpdateWorld()
    {
        if(Main.keyState.IsKeyDown(Keys.G))
            PizzaGuideNPC.Instance.SpawnChubK(Main.LocalPlayer);
    }
}