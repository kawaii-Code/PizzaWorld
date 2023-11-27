﻿using Terraria.ModLoader;

namespace PizzaWorld;

public class ModEntry : ModSystem
{
    public ModKeybind SpawnBind;
    
    public override void Load()
    {
        SpawnBind = KeybindLoader.RegisterKeybind(Mod, "Spawn Pizza NPC", "P");
    }
    
    public override void Unload()
    {
        SpawnBind = null;
    }
}