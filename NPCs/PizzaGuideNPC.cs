using System.Linq;
using ChubK.Utilities;
using log4net.Repository.Hierarchy;
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
    }
    
    #if false
    // 🤔🤔🤔
    public void SpawnChubK(Player invoker, bool syncData = false, int syncID = 0)
    {
        Debug.Log(invoker.name);
        
        /*if(Main.netMode == NetmodeID.MultiplayerClient)
            return;*/
        
        int x = (int)invoker.position.X;
        int y = (int)invoker.position.Y;
        
        int index = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), x, y, ModContent.NPCType<PizzaGuideNPC>());
        
        if (syncID < 0) 
            Main.npc[index].SetDefaults(syncID);
        
        NPC.netUpdate = true;
        NetMessage.SendData(MessageID.SyncNPC, number: Main.npc[index].type);

        
        /*if (Main.netMode == NetmodeID.Server) {
            
        }*/
    }
    #endif

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
    
    public static void SpawnNPC()
    {
        ModPacket packet = PizzaWorld.Instance.GetPacket();
        ModNPC npc = ModContent.GetModNPC(ModContent.NPCType<PizzaGuideNPC>());
        packet.Write(0);
        packet.Write(npc.Type);
        packet.Write(npc.NPC.netID);
        packet.Send();
    }
}