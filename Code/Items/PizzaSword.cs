using PizzaWorld.Code.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaSword : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.damage = 80;
    }
    
    public override void SetDefaults()
    {
        Item.damage = 50;
        Item.DamageType = DamageClass.Melee;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 6;
        Item.value = 10000;
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        ItemID.Sets.CatchingTool[Type] = true;
    }

    public override void OnCatchNPC(NPC npc, Player player, bool failed)
    {
        PizzaWorld.SpawnNPC<PizzaGuideNPC>();
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.DirtBlock, 10);
        recipe.AddTile(TileID.WorkBenches);
        recipe.Register();
    }
}