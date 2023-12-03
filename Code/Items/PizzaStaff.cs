using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaStaff : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.CactusSword);
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.damage = 20;
        Item.crit = 30;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(ItemID.Wood, 5)
            .AddIngredient(ItemID.Pizza)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (hit.Crit && Main.rand.NextBool())
        {
            target.AddBuff(BuffID.Confused, 60 * 3);
            player.AddBuff(BuffID.WellFed3, 10 * 60);
        }
    }
}
