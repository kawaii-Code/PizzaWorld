using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Projectiles;
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
        
        _currentBossAI.Start();
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

        public virtual void Start()
        {
        }
        
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

        private int _projectileTargetIndex = 0;

        private Player _projectileTarget;

        private List<Player> _players = new();
        
        private float _projectileLifetime = 100;
        
        private List<ProjectileInfo> _projectiles = new();
        
        public override int MinLife { get; protected set; }
        public override bool IsNeedTransit { get; protected set; }

        protected override int Damage { get; set; } = 20;
        protected override float Speed { get; set; } = 5f;
        protected override float RushSpeed { get; set; }
        public SecondBossStageAI(NPC npc) : base(npc) {}

        public override void Start()
        {
            foreach (var plr in Main.player)
            {
                if (plr.active)
                    _players.Add(plr);
                
            }
            
            Debug.Log("Count " + _players.Count);
        }

        public override void Update()
        {
            _currentTarget = Main.player[NPC.target];
            NPC.TargetClosest();
            
            NPC.ai[2]++;

            HandleProjectileLifetime();
            MoveProjectiles();
            
            if (Vector2.Distance(NPC.Center, _currentTarget.Center) > 300)
                MoveTowards(_currentTarget.Center);
            else
            {
                NPC.ai[1]++;
                Floating();
            }
            
            if (NPC.ai[2] > _projectileReleaseDelay)
            {
                if (_projectileTargetIndex < _players.Count)
                {
                    _projectileTarget = _players[_projectileTargetIndex];
                    Debug.Log(_projectileTarget);
                }
                else
                    _projectileTargetIndex = 0;
 
                _projectileTargetIndex++;
                
                Debug.Log(_projectileTarget.name);
                
                var created = Projectile.NewProjectileDirect(new EntitySource_BossSpawn(Main.player[NPC.target]), NPC.Center + new Vector2(NPC.direction * 20, 0),
                    new Vector2(20, 0), ModContent.ProjectileType<PizzaProjectile>(), Damage, 20);

                created.tileCollide = false;
                created.friendly = false;
                created.damage = 20;
                created.hostile = true;
                
                _projectiles.Add(new ProjectileInfo{ Projectile = created, StartTime = created.ai[0], KillTime = 150});
                
                NPC.ai[2] = 0;
            }
        }

        private void MoveProjectiles()
        {
            if(_projectiles == null || _projectiles.Count == 0)
                return;
            
            foreach (var projectile in _projectiles)
            {
                if (projectile.Projectile == null)
                    continue;

                if (_projectileTarget == null)
                    _projectileTarget = Main.player[NPC.target];
                
                MoveTowardsProjectile(projectile.Projectile, _projectileTarget.Center, 13);
                
                if (Vector2.Distance(projectile.Projectile.Center, _currentTarget.Center) < 2f) 
                    projectile.Projectile.Kill();
            }

            for (int i = _projectiles.Count - 1; i >= 0 ; i--)
            {
                if (_projectiles[i] == null)
                    _projectiles.Remove(_projectiles[i]);
            }
        }

        private void HandleProjectileLifetime()
        {
            foreach (var projectile in _projectiles)
            {
                if (projectile.Projectile.ai[0] - projectile.StartTime >= projectile.KillTime)
                {
                    projectile.Projectile.Kill();
                    projectile.Killed = true;
                    Debug.Log("Projectile killed");
                }
            }

            for (int i = _projectiles.Count - 1; i >= 0 ; i--)
            {
                if (_projectiles[i].Killed == true) 
                    _projectiles.Remove(_projectiles[i]);
            }   
        }

        class ProjectileInfo
        {
            public Projectile Projectile;
            public float StartTime;
            public float KillTime;
            public bool Killed;
        }
    }
}