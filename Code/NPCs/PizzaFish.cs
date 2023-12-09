using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaFish : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Goldfish];
        Main.npcCatchable[Type] = true;
        
        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Goldfish);
        AnimationType = NPCID.Goldfish;
        AIType = NPCID.Goldfish;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return Main.player[Main.myPlayer].InModBiome<PizzaBiome>() ? 0.1f : 0.0f;
    }
}