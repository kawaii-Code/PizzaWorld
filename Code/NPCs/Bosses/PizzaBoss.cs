using System;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Utilities;
using SteelSeries.GameSense.DeviceZone;
using Terraria;
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

        _currentBossAI = new FirstBossStageAI(NPC);
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
        
        protected virtual void OnPlayerHit()
        {
        } 
    }

    internal class FirstBossStageAI : BossAI
    {
        private float _idleDistance = 150f;
        private float _rushDistance = 130f;
        private float _timer;
        private float _timeToChangeState = 300;

        private Player _currentTarget;

        private bool _isIdleState = true;
        
        public override int MinLife { get; protected set; }
        public override bool IsNeedTransit { get; protected set; } = false;
        
        protected override int Damage { get; set; }
        protected override float Speed { get; set; } = 5f;
        protected override float RushSpeed { get; set; } = 7f;

        public FirstBossStageAI(NPC npc) : base(npc) {}

        public override void Update()
        {
            if (NPC.life <= MinLife)
                IsNeedTransit = true;
            
            NPC.TargetClosest();

            _currentTarget = Main.player[NPC.target];

            _timer = NPC.ai[0];
            
            if(_isIdleState)
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

            MoveTowards(_currentTarget.Center);
            
            if (Vector2.Distance(NPC.Center, _currentTarget.Center) < _rushDistance)
            {
                return;   
            }

            IsRushing = true;
            
            Vector2.SmoothStep(NPC.velocity, Vector2.Zero, 20);
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
                if (NPC.velocity.Length() > 0.1f)
                    NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.1f);
                else
                    NPC.velocity = Vector2.Zero;
            }
        }
    }
}