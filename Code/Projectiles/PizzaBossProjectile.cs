using System;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.NPCs.Bosses;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Projectiles;

public class PizzaBossProjectile : ModProjectile
{
    public static int GigaCrutch = 0;
    public Player CurrentTarget;

    private float _projectileLifetime = 170;
    
    public override void SetDefaults()
    {
        Projectile.width = 34;
        Projectile.height = 26;
        Projectile.hostile = true;
        Projectile.ignoreWater = true;

        Projectile.tileCollide = false;
        
        Projectile.DamageType = DamageClass.Magic;
        Projectile.aiStyle = -1;
        Projectile.penetrate = -1;
        Projectile.damage = 13;
    }

    public override void AI()
    {
        if (CurrentTarget == null)
        {
            int playerCount = 0;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active)
                {
                    playerCount++;
                }
            }

            if (playerCount != 0)
            {
                CurrentTarget = Main.player[GigaCrutch % playerCount];
                GigaCrutch++;
            }
        }
        Projectile.ai[0]++;

        if (Projectile.ai[0] > _projectileLifetime)
        {
            Projectile.Kill();
            return;
        }

        if(CurrentTarget == null || !CurrentTarget.active ||
           CurrentTarget.dead)
            return;

        MoveProjectile();
    }

    private void MoveProjectile() => MoveTowardsProjectile(Projectile, CurrentTarget.Center, 9);

    public override void OnHitPlayer(Player target, Player.HurtInfo info) => this.Projectile.Kill();

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
