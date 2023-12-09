using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaBird : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bird];
        Main.npcCatchable[Type] = true;

        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Bird);
        AnimationType = NPCID.Bird;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return Main.player[Main.myPlayer].InModBiome<PizzaBiome>() ? 0.1f : 0.0f;
    }
}