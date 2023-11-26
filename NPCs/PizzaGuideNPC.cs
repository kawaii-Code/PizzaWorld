using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PizzaWorld.NPCs;

public class PizzaGuideNPC : ModNPC
{
    public override void SetStaticDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        
    }

    public override string GetChat()
    {
        var player = Main.player.FirstOrDefault(player => player.name == "Chub");

        if (player != null)
            return "I smell pizza";

        switch (Main.rand.Next(4))
        {
            case 0:
                return "Pizza";

            case 1:
                return "Chub";

            case 2:
                return "K";

            default:
                return "Chub_k";
        }
    }

    public override void OnSpawn(IEntitySource source)
    {
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{NPC.type}"), Color.Red);
    }

    /*public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        for (int i = 0; i < 255; i++)
        {
            Player player = Main.player[i];

            if (player.active == false)
                continue;
        }
    }*/
}