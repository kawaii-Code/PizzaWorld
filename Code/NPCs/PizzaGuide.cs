using System;
using PizzaWorld.Code.Items;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

public class PizzaGuide : ModNPC
{
    private bool _isFirstTimeTalk;
    private NPCShop _shop;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.friendly = true;
        _isFirstTimeTalk = true;

        AnimationType = NPCID.Guide;
        AIType = NPCID.Guide;
    }

    public override string GetChat()
    {
        if (_isFirstTimeTalk)
        {
            _isFirstTimeTalk = false;
            return Phrase("Welcome");
        }

        switch (Main.rand.Next(0, 4))
        {
            case 0: return Phrase("RandomPizzaTrivia1");
            case 1: return Phrase("RandomPizzaTrivia2");
            case 2: return Phrase("RandomPizzaTrivia3");
            case 3: return Phrase("RandomPizzaTrivia4");
            default: throw new Exception();
        }
    }

    public override void AddShops()
    {
        base.AddShops();
        _shop = new NPCShop(Type, "Pizza shop");
        _shop.Add<PizzaAxe>().Add<PizzaPickaxe>().Add<PizzaYoyo>().FinishSetup();
        _shop.Register();
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

    private string Phrase(string key)
    {
        return Localized("Dialogue." + key);
    }

    private string Localized(string key)
    {
        const string prefix = "Mods.PizzaWorld.NPCs.PizzaGuide.";
        return Language.GetText(prefix + key).Value;
    }
}