using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class MushroomBug : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Buggy];
        Main.npcCatchable[Type] = true;
        
        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Buggy);
        NPC.width = 16;
        NPC.height = 12;
        AnimationType = NPCID.Buggy;
    }
}