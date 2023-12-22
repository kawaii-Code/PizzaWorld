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
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CoinsRule(Price.Silver(1), true));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<BasicPizza>(), 50))
            .OnFailedRoll(new CommonDrop(ModContent.ItemType<Champignon>(), 35))
            .OnFailedRoll(new CommonDrop(ModContent.ItemType<FourCheeses>(), 25))
            .OnFailedRoll(new CommonDrop(ModContent.ItemType<Napoletana>(), 15))
            .OnFailedRoll(new CommonDrop(ModContent.ItemType<Barbeque>(), 5));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return Main.player[Main.myPlayer].InModBiome<PizzaBiome>() ? 0.2f : 0.0f;
    }
}