using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Projectiles;

public class PizzaProjectile : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 26;
        Projectile.friendly = true;
        Projectile.ignoreWater = true;

        Projectile.tileCollide = false;
        
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        this.Projectile.damage = 20;
    }

    public override void AI()
    {
        Projectile.ai[0]++;
        if(Projectile.ai[0] < 60f)
        {
            Projectile.velocity *= 1.05f;
            if(Projectile.ai[0] >= 180)
            {
                Projectile.Kill();
            }
        }
        
        Projectile.rotation += 0.2f * Projectile.direction;
        
        Lighting.AddLight(Projectile.Center, 0.75f, 0.75f, 0.75f);

        if(Main.rand.NextBool(2))
        {
            int numToSpawn = Main.rand.Next(3);
            for(int i = 0; i < numToSpawn; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PizzaDust>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f,
                    0, default(Color), 1f);
            }
        }
    }

    
    
    public override void OnKill(int timeLeft)
    {
        int numToSpawn = Main.rand.Next(25);
        
        for(int i = 0; i < numToSpawn; i++)
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<PizzaDust>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f,
                0, default(Color), 1f);
        }
    }
}
