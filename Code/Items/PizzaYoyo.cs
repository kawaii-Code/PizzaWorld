using PizzaWorld.Code.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaYoyo : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.Yoyo[Type] = true;
        ItemID.Sets.GamepadExtraRange[Type] = 10;
        ItemID.Sets.GamepadSmartQuickReach[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Rally);
        Item.shoot = ModContent.ProjectileType<PizzaYoyoProjectile>();
        Item.damage = 16;
    }
}