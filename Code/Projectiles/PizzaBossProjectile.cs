using System;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.NPCs.Bosses;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Projectiles;

public class PizzaBossProjectile : ModProjectile
{
    private Player _currentTarget;

    private float _projectileLifetime = 300;
    
    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 26;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.ignoreWater = true;

        Projectile.tileCollide = false;
        
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        Projectile.damage = 20;

        _currentTarget = PizzaBoss.Instance.CurrentBossAI.CurrentProjectileTarget;
    }

    public override void AI()
    {
        Projectile.ai[0]++;

        if (Projectile.ai[0] > _projectileLifetime)
        {
            Projectile.Kill();
            return;
        }
         
        if(_currentTarget == null)
            return;
        
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        MoveTowardsProjectile(Projectile, _currentTarget.Center, 13);
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        if (target == _currentTarget)
            Projectile.Kill();
    }

    private void MoveTowardsProjectile(Projectile projectile ,Vector2 targetCenter, float speed)
    {
        Vector2 direction = targetCenter - projectile.Center;

        float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

        if (magnitude > speed)
        {
            direction *= speed / magnitude;
        }
            
        float turnResistance = 10f;
            
        direction = (projectile.velocity * turnResistance + direction) / (turnResistance + 1f);
        magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            
        if(magnitude > speed) 
            direction *= speed / magnitude;
            
        projectile.velocity = direction;
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
