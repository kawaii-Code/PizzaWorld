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
        NPC.life += 100;
        NPC.lifeMax += 100;
        NPC.damage += 15;
        NPC.defense += 8;
        AnimationType = NPCID.Zombie;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath13;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CoinsRule(Price.Silver(1), true));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<BasicPizza>(), 6));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<Barbeque>(), 2));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return !Main.dayTime && spawnInfo.Player.InModBiome<PizzaBiome>() ? 0.45f : 0.0f;
    }
}
