using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Utilities;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses;

public class PizzaItemNPC : ModNPC
{
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Bunny);
        NPC.DeathSound = SoundID.Item1;
        NPC.life = 1;

        this.NPC.friendly = true;
    }

    public override void AI()
    {
        NPC.ai[0]++;

        if (NPC.ai[0] >= 300)
        {
            NPC.life -= 1000;
        }
    }
    
    public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.Add(new CommonDrop(ModContent.ItemType<Margherita>(), 1, 1, 2));
}