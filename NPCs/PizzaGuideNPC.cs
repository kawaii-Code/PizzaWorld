using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace PizzaWorld.NPCs;

public class PizzaGuideNPC : ModNPC
{
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.friendly = true;
        Instance = this;
    }

    public static PizzaGuideNPC Instance;
    
    // 🤔🤔🤔
    private static void SpawnNPC(int type, bool syncData = false, int syncID = 0, int whoAmI = 0)
    {
        Player player;
        if (!syncData)
        {
            player = Main.LocalPlayer;
        }
        else
        {
            player = Main.player[whoAmI];
        }
        int x = (int)player.Bottom.X + player.direction * 200;
        int y = (int)player.Bottom.Y;
        int index = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x, y, type);
        if (syncID < 0)
        {
            //NPC refNPC = new NPC();
            //refNPC.netDefaults(syncID);
            Main.npc[index].SetDefaults(syncID);
        }
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
        base.OnSpawn(source);
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{NPC.type} spawned"), Color.Blue);
    }
    
    public void SpawnNPC()
    {
        NPC.NewNPC(new EntitySource_Parent(Entity), (int)Main.player[0].position.X,
            (int)Main.player[0].position.Y, ModContent.NPCType<PizzaGuideNPC>());
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