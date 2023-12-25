using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Systems;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath13;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CoinsRule(Price.Silver(1), true));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<BasicPizza>(), 3));
        npcLoot.Add(new SequentialRulesRule(4, 
            new CommonDrop(ModContent.ItemType<BasicPizza>(), 4),
            new CommonDrop(ModContent.ItemType<Barbeque>(), 6),
            new CommonDrop(ModContent.ItemType<Napoletana>(), 8)
        ));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return !Main.dayTime && Main.player[Main.myPlayer].InModBiome<PizzaBiome>() ? 0.45f : 0.0f;
    }
}