using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaBat : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.JungleBat];
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.JungleBat);
        AnimationType = NPCID.JungleBat;
        AIType = NPCID.JungleBat;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return spawnInfo.Player.InModBiome<PizzaBiome>() ? 0.25f : 0.0f;
    }
}