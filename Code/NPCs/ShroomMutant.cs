using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class ShroomMutant : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.ZombieMushroom];
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.ZombieMushroom);
        NPC.life = NPC.lifeMax = 120;
        NPC.damage = 25;
        NPC.defense = 8;
        AIType = NPCID.ZombieMushroom;
        AnimationType = NPCID.ZombieMushroom;
        
        NPC.HitSound = SoundID.NPCHit9;
        NPC.DeathSound = SoundID.NPCDeath21;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ModContent.ItemType<BasicPizza>(), 6));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<Champignon>(), 4));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return spawnInfo.Player.InModBiome<PizzaBiome>() ? 0.33f : 0.0f;
    }
}
