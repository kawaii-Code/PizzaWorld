using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class Zombizza : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];
        Main.npcCatchable[Type] = true;

        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Zombie);
        AnimationType = NPCID.Zombie;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return Main.player[Main.myPlayer].InModBiome<PizzaBiome>() ? 0.1f : 0.0f;
    }
}