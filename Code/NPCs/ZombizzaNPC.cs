using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using PizzaWorld.Code.Utilities;

namespace PizzaWorld.Code.NPCs;

public class ZombizzaNPC : ModNPC
{
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Zombie);
        NPC.lifeMax = 50;
        NPC.damage = 64;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ItemID.Pizza, 20));
    }
    
    public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
    {
        Debug.Log($"{NPC.FullName}: {target.name}, ну ты и лох");
    }
}