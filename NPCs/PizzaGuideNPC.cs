using System.Diagnostics;
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
using PizzaWorld.Data;

namespace PizzaWorld.NPCs;

public class PizzaGuideNPC : ModNPC
{
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.friendly = false;
        NPC.damage = 20;
        
    }

    public override string GetChat()
    {
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

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = "Get pizza recipe";
        button2 = "Get advice";
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
    {
        if (firstButton)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    Main.npcChatText = PizzaRecipes.PepperoniRecipe;
                    break;
                
                case 1: 
                    Main.npcChatText = PizzaRecipes.FourCheeseRecipe;
                    break;
                
                default:
                    Main.npcChatText = "Idi v zad";
                    break;
            }
            return;
        }

        Main.npcChatText = "Кто прочитал тот лох";
    }

    public override void OnSpawn(IEntitySource source)
    {
        base.OnSpawn(source);
        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{NPC.type} spawned"), Color.Blue);
    }
    
    public static void SendSpawnNPCRequest()
    {
        if(Main.netMode == NetmodeID.SinglePlayer)
            return;
        
        ModPacket packet = PizzaWorld.Instance.GetPacket();
        ModNPC npc = ModContent.GetModNPC(ModContent.NPCType<PizzaGuideNPC>());
        packet.Write(0);
        packet.Write(npc.Type);
        packet.Write(npc.NPC.netID);
        packet.Send();
    }
}