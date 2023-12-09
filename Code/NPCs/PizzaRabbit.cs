using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaRabbit : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bunny];
        
        NPCID.Sets.CountsAsCritter[Type] = true;
        Main.npcCatchable[Type] = true; 
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Bunny);
        AnimationType = NPCID.Bunny;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ItemID.Pizza, 20, chanceNumerator: 2));
    }
    
    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return Main.player[Main.myPlayer].InModBiome<PizzaBiome>() ? 0.1f : 0.0f;
    }
}