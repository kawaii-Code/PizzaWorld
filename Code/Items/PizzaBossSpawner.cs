using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.NPCs.Bosses;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaBossSpawner : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 17;
        Item.height = 17;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.rare = ItemRarityID.Orange;
        Item.maxStack = 1;
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            PizzaWorld.SpawnNPC<PizzaBoss>();
        }
        return true;
    }

    public override bool CanUseItem(Player player)
    {
        bool a = player.InModBiome<PizzaBiome>() && !NPC.AnyNPCs(ModContent.NPCType<PizzaBoss>());
        return a;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<BasicPizza>()
            .AddIngredient<Champignon>()
            .AddIngredient<FourCheeses>()
            .AddIngredient<Napoletana>()
            .AddIngredient<Barbeque>()
            .AddTile(TileID.DemonAltar)
            .Register();
    }
}