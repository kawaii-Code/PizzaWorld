using System.Collections.Generic;
using PizzaWorld.Code.Items;
using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs;

[AutoloadHead]
public class PizzaDeliveryGuy : ModNPC
{
    public static bool WasPizzaBossKilled;
    private Profiles.StackedNPCProfile _profile;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];
        NPCID.Sets.ActsLikeTownNPC[Type] = true;
        NPCID.Sets.ExtraFramesCount[Type] = 9;
        NPCID.Sets.AttackFrameCount[Type] = 4;
        NPCID.Sets.DangerDetectRange[Type] = 60;
        NPCID.Sets.AttackType[Type] = 3;
        NPCID.Sets.AttackTime[Type] = 12;
        NPCID.Sets.AttackAverageChance[Type] = 1;
        NPCID.Sets.HatOffsetY[Type] = 4;
        NPCID.Sets.ShimmerTownTransform[Type] = true;
        NPCID.Sets.FaceEmote[Type] = EmoteID.Hungry;

        _profile = new Profiles.StackedNPCProfile(
            new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
            new Profiles.DefaultNPCProfile(Texture + "_Shimmer", 0)
        );
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Guide);
        NPC.aiStyle = 7;
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.width = 18;
        NPC.height = 40;

        AnimationType = NPCID.Guide;
        AIType = NPCID.Guide;

        NPC.Happiness
            .SetBiomeAffection<PizzaBiome>(AffectionLevel.Love)
            .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
            .SetBiomeAffection<SnowBiome>(AffectionLevel.Hate)
            .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
            .SetNPCAffection<PizzaGuide>(AffectionLevel.Hate);

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

    public override ITownNPCProfile TownNPCProfile()
    {
        return _profile;
    }

    public override bool UsesPartyHat()
    {
        return false;
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
            .Add<BasicPizza>()
            .Add<Champignon>()
            .Add<FourCheeses>()
            .Add<Napoletana>()
            .Add<Barbeque>()
            .Add<PizzaWing>();
        shop.FinishSetup();
        shop.Register();
    }

    public override bool CanTownNPCSpawn(int numTownNPCs)
    {
        return WasPizzaBossKilled;
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
