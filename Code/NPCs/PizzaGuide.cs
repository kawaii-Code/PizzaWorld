using System;
using System.Collections.Generic;
using PizzaWorld.Code.Items;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PizzaWorld.Code.NPCs;

public class PizzaGuide : ModNPC
{
    private bool _isFirstTimeTalk;
    private NPCShop _shop;

    private string[] _pizzaTrivia;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];

        NPCID.Sets.AttackType[Type] = 1;
        NPC.Happiness
            .SetBiomeAffection<PizzaBiome>(AffectionLevel.Love)
            .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
            .SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
            .SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Love)
            .SetNPCAffection<PizzaDeliveryGuy>(AffectionLevel.Hate);
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.width = 18;
        NPC.height = 40;
        NPC.defense = 15;
        NPC.lifeMax = 250;

        AIType = NPCID.Guide;
        AnimationType = NPCID.Guide;
        
        _pizzaTrivia = new[]
        {
            Phrase("RandomPizzaTrivia1"),
            Phrase("RandomPizzaTrivia2"),
            Phrase("RandomPizzaTrivia3"),
            Phrase("RandomPizzaTrivia4"),
            Phrase("RandomPizzaTrivia5"),
            Phrase("RandomPizzaTrivia6"),
            Phrase("RandomPizzaTrivia7"),
            Phrase("RandomPizzaTrivia8"),
            Phrase("RandomPizzaTrivia9"),
            Phrase("RandomPizzaTrivia10"),
        };
    }

    public override string GetChat()
    {
        if (_isFirstTimeTalk)
        {
            _isFirstTimeTalk = false;
            return Phrase("Welcome");
        }

        return _pizzaTrivia[Main.rand.Next(0, _pizzaTrivia.Length)];
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

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];
            if (player.active)
            {
                continue;
            }

            if (player.InModBiome<PizzaBiome>())
            {
                return true;
            }
        }
        return false;
    }
    
    public override void TownNPCAttackStrength(ref int damage, ref float knockback)
    {
        damage = 20;
        knockback = 4f;
    }

    public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
    {
        cooldown = 30;
        randExtraCooldown = 30;
    }

    public override List<string> SetNPCNameList()
    {
        return new List<string>
        {
            Localized("Name1"),
            Localized("Name2"),
            Localized("Name3"),
            Localized("Name4"),
        };
    }

    public override void SaveData(TagCompound tag)
    {
        tag[nameof(_isFirstTimeTalk)] = _isFirstTimeTalk;
    }

    public override void LoadData(TagCompound tag)
    {
        _isFirstTimeTalk = tag.GetBool(nameof(_isFirstTimeTalk));
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