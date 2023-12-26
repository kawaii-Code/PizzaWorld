using System.Collections.Generic;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace PizzaWorld.Code.NPCs;

[AutoloadHead]
public class PizzaGuide : ModNPC
{
    private string[] _pizzaTrivia;
    private string[] _pizzaRecipes;
    private bool _isFirstTimeTalk;

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
            Localized("Trivia.RandomPizzaTrivia1"),
            Localized("Trivia.RandomPizzaTrivia3"),
            Localized("Trivia.RandomPizzaTrivia4"),
            Localized("Trivia.RandomPizzaTrivia5"),
            Localized("Trivia.RandomPizzaTrivia6"),
            Localized("Trivia.RandomPizzaTrivia7"),
            Localized("Trivia.RandomPizzaTrivia8"),
            Localized("Trivia.RandomPizzaTrivia9"),
            Localized("Trivia.RandomPizzaTrivia10"),
        };
        _pizzaRecipes = new[]
        {
            Localized("Recipes.Recipe1"),
            Localized("Recipes.Recipe2"),
        };
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
        if (_isFirstTimeTalk)
        {
            _isFirstTimeTalk = false;
            return Localized("Dialogue.Welcome");
        }
        return Localized("Dialogue.BossHint");
    }

    public override void SetChatButtons(ref string button, ref string button2)
    {
        button = Localized("PizzaRecipes");
        button2 = Localized("PizzaFacts");
    }

    public override void OnChatButtonClicked(bool firstButton, ref string shopName)
    {
        if (firstButton)
        {
            Main.npcChatText = _pizzaRecipes[Main.rand.Next(_pizzaRecipes.Length)];
        }
        else
        {
            Main.npcChatText = _pizzaTrivia[Main.rand.Next(0, _pizzaTrivia.Length)];
        }
    }

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];
            if (!player.active)
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

    public override void SaveData(TagCompound tag)
    {
        tag[nameof(_isFirstTimeTalk)] = _isFirstTimeTalk;
    }

    public override void LoadData(TagCompound tag)
    {
        _isFirstTimeTalk = tag.GetBool(nameof(_isFirstTimeTalk));
    }

    private string Localized(string key)
    {
        const string prefix = "Mods.PizzaWorld.NPCs.PizzaGuide.";
        return Language.GetText(prefix + key).Value;
    }
}
