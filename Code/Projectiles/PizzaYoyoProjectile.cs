using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Projectiles;

public class PizzaYoyoProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = 6f;
        ProjectileID.Sets.YoyosMaximumRange[Type] = 200f;
        ProjectileID.Sets.YoyosTopSpeed[Type] = 13f;
    }

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.aiStyle = ProjAIStyleID.Yoyo;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.penetrate = -1;
    }
}