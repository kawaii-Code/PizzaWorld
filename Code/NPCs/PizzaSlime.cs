using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Systems;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaSlime : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 2;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.LavaSlime);
        AnimationType = NPCID.BlueSlime;
        AIType = NPCID.BlueSlime;

        NPC.DeathSound = SoundID.NPCDeath64;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CoinsRule(Price.Copper(50)));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<Napoletana>(), 4));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return spawnInfo.Player.InModBiome<PizzaBiome>() ? 0.5f : 0.0f;
    }
}