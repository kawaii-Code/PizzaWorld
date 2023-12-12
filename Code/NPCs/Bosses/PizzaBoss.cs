using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Utilities;
using SteelSeries.GameSense.DeviceZone;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Animations;
using Terraria.GameContent.RGB;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses;

[AutoloadBossHead]
public class PizzaBoss : ModNPC
{
    private BossAI _currentBossAI;
    
    public enum BossStage
    {
        First,
        Second,
        Third
    }
    
    public override void SetStaticDefaults()
    {
        //Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Skeleton];
    }

    public override void SetDefaults()
    {
        //replace
        NPC.width = 2;
        NPC.height = 2;

        NPC.aiStyle = -1;
        NPC.npcSlots = 5f;

        NPC.lifeMax = 13400;
        NPC.damage = 50;
        NPC.defense = 20;
        NPC.knockBackResist = 0f;

        NPC.value = Item.buyPrice(gold: 10);

        NPC.lavaImmune = true;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        
        NPC.boss = true;
        
        //AnimationType = NPCID.Skeleton;

        //_currentBossAI = new FirstBossStageAI(NPC);
        _currentBossAI = new SecondBossStageAI(NPC);
    }

    public override void AI()
    {
        NPC.ai[0]++;
        _currentBossAI.Update();
        CheckStageTransit();
    }

    private void CheckStageTransit()
    {
        if (_currentBossAI.IsNeedTransit)
        {
            //transit to next stage
        }
    }

    internal abstract class BossAI
    {
        private float _intertia;
        private float _velocity;

        protected NPC NPC { get; }
        
        public abstract int MinLife { get; protected set; }
        public abstract bool IsNeedTransit { get; protected set; }
        
        protected abstract int Damage { get; set; }
        protected abstract float Speed { get; set; }
        protected abstract float RushSpeed { get; set; }
        
        protected bool IsStunned { get; set; }
        protected bool IsRushing { get; set; }

        public abstract void Update();

        public BossAI(NPC npc) => NPC = npc;

        protected virtual void MoveTowards(Vector2 targetCenter)
        {
            var speed = IsRushing ? RushSpeed : Speed;
            
            Vector2 direction = targetCenter - NPC.Center;

            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude > speed)
            {
                direction *= speed / magnitude;
            }
            
            float turnResistance = 10f;
            
            direction = (NPC.velocity * turnResistance + direction) / (turnResistance + 1f);
            magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            
            if(magnitude > speed) 
                direction *= speed / magnitude;
            
            NPC.velocity = direction;
        }
        
        protected virtual void MoveTowardsProjectile(Projectile projectile ,Vector2 targetCenter, float speed)
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
        
        protected virtual void OnPlayerHit()
        {
        }

        protected  void Floating()
        {
            if (NPC.ai[1] > 40)
            {
                NPC.velocity = new Vector2(Main.rand.Next(-10, 10), Main.rand.NextFloat(-4,-6));
                NPC.ai[1] = 0;
            }
        }
    }

    internal class FirstBossStageAI : BossAI
    {
        private float _idleDistance = 200f;
        private float _rushDistance = 130f;
        private float _timer;
        private float _timeToChangeState = 300;

        private Player _currentTarget;

        private bool _isIdleState = true;
        
        public override int MinLife { get; protected set; }
        public override bool IsNeedTransit { get; protected set; } = false;
        
        protected override int Damage { get; set; }
        protected override float Speed { get; set; } = 5f;
        protected override float RushSpeed { get; set; } = 12f;

        public FirstBossStageAI(NPC npc) : base(npc) {}

        public override void Update()
        {
            if (NPC.life <= MinLife)
                IsNeedTransit = true;
            
            NPC.TargetClosest();

            _currentTarget = Main.player[NPC.target];

            _timer = NPC.ai[0];

            if (_isIdleState)
                IdleState();
            else
                RushState();
            
            if (_timer > _timeToChangeState)
            {
                Debug.Log("ChangeState");
                _isIdleState = !_isIdleState;
                _timer = 0;
                NPC.ai[0] = 0;
            }
        }
        
        private void RushState()
        {
            if(_currentTarget == null)
                return;

            
            MoveTowards(_currentTarget.Center + new Vector2(_currentTarget.direction * 30, 0) );
            
            if (Vector2.Distance(NPC.Center, _currentTarget.Center) > _rushDistance)
            {
                return;   
            }

            IsRushing = true;
        }

        private void IdleState()
        {
            IsRushing = false;
            
            if (_currentTarget == null)
            {
                //do smth if null target
                return;
            }

            if (Vector2.Distance(NPC.Center, _currentTarget.Center) > _idleDistance)
                MoveTowards(_currentTarget.Center);
            else
            {
                NPC.ai[1]++;
                Floating();
            }
        }
         
    }

    internal class SecondBossStageAI : BossAI
    {
        private float _projectileReleaseDelay = 200;

        private Player _currentTarget;

        private List<Projectile> _projectiles = new();
        
        public override int MinLife { get; protected set; }
        public override bool IsNeedTransit { get; protected set; }
        
        protected override int Damage { get; set; }
        protected override float Speed { get; set; } = 5f;
        protected override float RushSpeed { get; set; }
        public SecondBossStageAI(NPC npc) : base(npc) {}

        public override void Update()
        {
            _currentTarget = Main.player[NPC.target];
            NPC.TargetClosest();
            
            NPC.ai[2]++;

            MoveProjectiles();
            
            if (Vector2.Distance(NPC.Center, _currentTarget.Center) > 200)
                MoveTowards(_currentTarget.Center);
            else
            {
                NPC.ai[1]++;
                Floating();
            }
            
            if (NPC.ai[2] > _projectileReleaseDelay)
            {
                var created = Projectile.NewProjectileDirect(new EntitySource_Film(), NPC.Center + new Vector2(NPC.direction * 20, 0),
                    new Vector2(20, 0), ProjectileID.Fireball, Damage, 20);

                _projectiles.Add(created);
                
                NPC.ai[2] = 0;
            }
        }

        private void MoveProjectiles()
        {
            if(_projectiles == null || _projectiles.Count == 0)
                return;
            
            foreach (var projectile in _projectiles)
            {
                MoveTowardsProjectile(projectile, _currentTarget.Center, 20);
                
                if (Vector2.Distance(projectile.Center, _currentTarget.Center) < 2f) 
                    projectile.Kill();
            }

            for (int i = _projectiles.Count - 1; i >= 0 ; i--)
            {
                if (_projectiles[i] == null)
                    _projectiles.Remove(_projectiles[i]);
            }
        }
    }
}