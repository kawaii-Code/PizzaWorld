using Microsoft.Xna.Framework;
using PizzaWorld.Code.Data;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaDeliveryGuy : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
      
        NPC.Happiness
            .SetBiomeAffection<PizzaBiome>(AffectionLevel.Love)
            .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
            .SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
            .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
            .SetNPCAffection<PizzaGuide>(AffectionLevel.Hate);
    }
   
    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.friendly = false;
        NPC.damage = 20;
        
        AnimationType = NPCID.Guide;
        AIType = NPCID.Guide;
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
}