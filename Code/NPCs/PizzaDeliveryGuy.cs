using System.Collections.Generic;
using PizzaWorld.Code.Items;
using PizzaWorld.Code.Systems;
using Terraria;
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
    
    public override List<string> SetNPCNameList()
    {
        return new List<string>
        {
            Localized("Names.Name1"),
            Localized("Names.Name2"),
            Localized("Names.Name3"),
            Localized("Names.Name4"),
        };
    }

    public override string GetChat()
    {
        return Main.rand.Next(4) switch
        {
            0 => Localized("Dialogue.Phrase1"),
            1 => Localized("Dialogue.Phrase2"),
            2 => Localized("Dialogue.Phrase3"),
            _ => Localized("Dialogue.Phrase4"),
        };
    }

    public override void AddShops()
    {
        NPCShop shop = new NPCShop(Type, Localized("Shop"))
            .Add<PizzaAxe>()
            .Add<PizzaPickaxe>()
            .Add<PizzaYoyo>();
        shop.FinishSetup();
        shop.Register();
    }

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = Localized("Shop");
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
    {
        if (firstButton)
        {
            shopName = Localized("Shop");
        }
    }

    private string Localized(string key)
    {
        const string prefix = "Mods.PizzaWorld.NPCs.PizzaDeliveryGuy.";
        return Language.GetText(prefix + key).Value;
    }
}