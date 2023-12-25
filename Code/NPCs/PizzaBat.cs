using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Systems;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.GameContent.ItemDropRules;
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

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CoinsRule(Price.Copper(50), true));
        npcLoot.Add(new CommonDrop(ModContent.ItemType<FourCheeses>(), 4));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        return spawnInfo.Player.InModBiome<PizzaBiome>() ? 0.25f : 0.0f;
    }
}