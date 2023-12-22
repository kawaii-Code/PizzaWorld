using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaBroadsword : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldBroadsword);
        Item.crit = 10;
        Item.damage = 18;
        Item.UseSound = SoundID.Item177;
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (hit.Crit)
        {
            target.AddBuff(BuffID.Poisoned, Time.Seconds(3));
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 3)
            .AddIngredient<BasicPizza>(3)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}