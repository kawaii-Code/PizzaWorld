using Microsoft.Xna.Framework;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaShroom : ModProjectile
{
    private const int TicksPerFrame = 5;
    
    private const float AggroRange = 600.0f;
    private const float Reach = 40.0f;
    
    private const float TeleportDistance = 2000.0f;
    private const float SpeedupToPlayerDistance = 450.0f;
    private const float FastReturnToPlayerSpeed = 12f;
    private const float FastReturnToPlayerInertia = 60f;
    private const float ReturnToPlayerSpeed = 4f;
    private const float ReturnToPlayerInertia = 20f;
    private const float ChaseSpeed = 8f;
    private const float ChaseInertia = 20f;

    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 4;
        Main.projPet[Type] = true;

        ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        ProjectileID.Sets.MinionSacrificable[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 12;
        
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.minion = true;
        Projectile.DamageType = DamageClass.Summon;
        Projectile.minionSlots = 1.0f;
        Projectile.penetrate = -1;
    }

    public override bool? CanCutTiles()
    {
        return false;
    }

    public override bool MinionContactDamage()
    {
        return true;
    }

    public override void AI()
    {
        Player owner = Main.player[Projectile.owner];
        if (owner.dead || !owner.active)
        {
            owner.ClearBuff(ModContent.BuffType<PizzaShroomBuff>());
        }
        
        if (owner.HasBuff(ModContent.BuffType<PizzaShroomBuff>()))
        {
            Projectile.timeLeft = 2;
        }

        NPC target = null;
        if (owner.HasMinionAttackTargetNPC)
        {
            NPC npc = Main.npc[owner.MinionAttackTargetNPC];
            if (IsInAggroRange(npc))
            {
                target = npc;
            }
        }
        else
        {
            target = FindClosestNpc();
        }

        if (target != null)
        {
            if (!IsInReach(target))
            {
                FlyTowards(target.Center, ChaseSpeed, ChaseInertia);
            }
        }
        else
        {
            Vector2 idlePosition = GetIdlePosition(owner);
            Vector2 directionToIdle = idlePosition - Projectile.Center;
            float distanceToIdle = directionToIdle.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdle > TeleportDistance)
            {
                TeleportTo(idlePosition);
            }
            else
            {
                float speed, inertia;
                if (distanceToIdle > SpeedupToPlayerDistance)
                {
                    speed = FastReturnToPlayerSpeed;
                    inertia = FastReturnToPlayerInertia;
                }
                else
                {
                    speed = ReturnToPlayerSpeed;
                    inertia = ReturnToPlayerInertia;
                }

                Debug.Log($"{speed} {inertia}");
                directionToIdle.Normalize();
                Fly(directionToIdle, speed, inertia);

                if (Projectile.velocity == Vector2.Zero)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
        }

        Projectile.rotation = Projectile.velocity.X * 0.05f;
        Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.5f);
        
        AnimationTick();
    }

    private void AnimationTick()
    {
        Projectile.frameCounter++;
        if (Projectile.frameCounter >= TicksPerFrame)
        {
            Projectile.frameCounter = 0;

            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.frame = 0;
            }
        }
    }

    private NPC FindClosestNpc()
    {
        NPC result = null;
        float minDistance = AggroRange;
        
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC npc = Main.npc[i];
            if (!npc.CanBeChasedBy())
                continue;

            float distance = Vector2.Distance(Projectile.Center, npc.Center);
            if (distance < minDistance && (HasLineOfSightTo(npc) || distance < 100.0f))
            {
                minDistance = distance;
                result = npc;
            }
        }

        return result;
    }

    private bool HasLineOfSightTo(NPC npc)
    {
        return Collision.CanHitLine(
            Projectile.Center, Projectile.width, Projectile.height,
            npc.Center, npc.width, npc.height);
    }

    private bool IsInReach(NPC target)
    {
        float distance = Vector2.Distance(Projectile.Center, target.Center);
        return distance < Reach;
    }

    private bool IsInAggroRange(NPC npc)
    {
        float distance = Vector2.Distance(Projectile.Center, npc.Center);
        return distance < AggroRange;
    }

    private void TeleportTo(Vector2 position)
    {
        Projectile.position = position;
        Projectile.velocity *= 0.5f;
        Projectile.netUpdate = true;
    }

    private void FlyTowards(Vector2 targetPosition, float speed, float inertia)
    {
        Vector2 directionToTarget = targetPosition - Projectile.Center;
        directionToTarget.Normalize();
        Fly(directionToTarget, speed, inertia);
    }

    private void Fly(Vector2 direction, float speed, float inertia)
    {
        Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction * speed) / inertia;
    }

    private Vector2 GetIdlePosition(Player player)
    {
        return player.Center - Vector2.UnitY * 48 + Vector2.UnitX * (10 + 40 * Projectile.minionPos) * -player.direction;
    }
}