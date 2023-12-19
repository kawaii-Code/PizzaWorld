using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Projectiles;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses;

[AutoloadBossHead]
public class PizzaBoss : ModNPC
{
    private BossAI _currentBossAI;
    
    public BossAI CurrentBossAI => _currentBossAI;
    public static PizzaBoss Instance { get; private set; }

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
        Instance = this;
        
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

        _currentBossAI = new FirstBossStageAI(NPC);
        _currentBossAI.Start();
    }

    public override void AI()
    {
        if(Main.netMode == NetmodeID.MultiplayerClient)
            return;
        
        NPC.ai[0]++;
        _currentBossAI.Update();
        CheckStageTransit();
    }

    private void CheckStageTransit()
    {
        if (this.NPC.life < 7000 && NPC.life > 3000)
        {
            if(_currentBossAI.StageID == 2)
                return;
            
            _currentBossAI = new SecondBossStageAI(NPC);
            Debug.Log("Boss : Нет! Тебе меня не победить АХАХАХА (лох)", Color.Purple);
        }
        else if (NPC.life < 3000)
        {
            if(_currentBossAI.StageID == 3)
                return;
            
            _currentBossAI = new ThirdBossStageAI(NPC);
            this.NPC.AddBuff(BuffID.Ironskin, 200000);
            this.NPC.AddBuff(BuffID.Regeneration, 200000);
            this.NPC.AddBuff(BuffID.Honey, 200000);
            this.NPC.AddBuff(BuffID.Honey, 200000);
            Debug.Log("Boss : Твоя взяля. Я сдаюсь. Не убивай меня... Возьми лучше это", Color.Aqua);
        }
    }

    public abstract class BossAI
    {
        private float _intertia;
        private float _velocity;

        protected NPC NPC { get; }
        
        public abstract int MinLife { get; protected set; }
        public abstract bool IsNeedTransit { get; protected set; }
        
        public abstract int StageID { get; protected set; }
        
        protected abstract int Damage { get; set; }
        protected abstract float Speed { get; set; }
        protected abstract float RushSpeed { get; set; }
        
        protected bool IsRushing { get; set; }

        public abstract void Update();

        public Player CurrentProjectileTarget { get; protected set; }
        
        public BossAI(NPC npc) => NPC = npc;

        public virtual void Start() {}
        
        public virtual void OnTransit() {}
        
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
        public override int StageID { get; protected set; } = 1;

        protected override int Damage { get; set; }
        protected override float Speed { get; set; } = 9f;
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
        private float _projectileReleaseDelay = 100;

        private Player _currentTarget;

        private int _projectileTargetIndex = 0;

        private List<Player> _players = new();
          
        public override int MinLife { get; protected set; }
        public override bool IsNeedTransit { get; protected set; }
        public override int StageID { get; protected set; } = 2;

        protected override int Damage { get; set; } = 20;
        protected override float Speed { get; set; } = 9f;
        protected override float RushSpeed { get; set; }
        public SecondBossStageAI(NPC npc) : base(npc) {}
        
        public override void Start()
        {
            foreach (var player in Main.player)
            {
                if (player.active)
                    _players.Add(player);
            }
        }

        public override void Update()
        {
            _currentTarget = Main.player[NPC.target];
            NPC.TargetClosest();

            if (Vector2.Distance(NPC.Center, _currentTarget.Center) > 300)
                MoveTowards(_currentTarget.Center);
            else
            {
                NPC.ai[1]++;
                Floating();
            }

            NPC.ai[2]++;
            if (NPC.ai[2] > _projectileReleaseDelay)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (!player.active)
                    {
                        continue;
                    }

                    CurrentProjectileTarget = player;
                    
                    int projectileId = Projectile.NewProjectile(new EntitySource_BossSpawn(Main.player[NPC.target]), NPC.Center + new Vector2(NPC.direction * 20, 0),
                        Vector2.Zero, ModContent.ProjectileType<PizzaBossProjectile>(), Damage, 20);

                    Projectile created = Main.projectile[projectileId];
                    
                    created.tileCollide = false;
                    created.friendly = false;
                    created.damage = 20;
                    created.hostile = true;
                
                    NPC.ai[2] = 0;
                }
            }
        }
    }
    
    internal class ThirdBossStageAI : BossAI
    {
        private float _bombReleaseDelay = 40;
        
        private Player _currentTarget;

        private int _pizzaDropCounter;
        
        public override int MinLife { get; protected set; }
        public override bool IsNeedTransit { get; protected set; }
        public override int StageID { get; protected set; } = 3;

        protected override int Damage { get; set; }
        protected override float Speed { get; set; } = 50f;
        protected override float RushSpeed { get; set; }
        
        public ThirdBossStageAI(NPC npc) : base(npc)
        {
        }

        public override void OnTransit()
        {
            NPC.ai[2] = 0;
            NPC.friendly = true;
        }

        public override void Update()
        {
            NPC.TargetClosest();
            _currentTarget = Main.player[NPC.target];

            MoveTowards(_currentTarget.Center + new Vector2(0, -250));

            NPC.ai[2]++;

            if (NPC.ai[2] > _bombReleaseDelay && _pizzaDropCounter <= 15)
            {
                _pizzaDropCounter++;
                Debug.Log("Pizza Drop");

                //Item.NewItem(new EntitySource_Loot(Main.item[ItemID.Pizza]),(int)NPC.Center.X , (int)NPC.Center.Y , 32, 16, ItemID.Pizza, 1);
                
                //NPC.DropItemInstanced(NPC.Center + new Vector2(0, 30), new Vector2(20, 20), ItemID.Pizza);
                
                NPC.ai[2] = 0;
                return;
            }

            if(_pizzaDropCounter < 10)
                return;

            _pizzaDropCounter = 100;
            
            if (NPC.ai[2] > _bombReleaseDelay)
            {
                var created = Projectile.NewProjectileDirect(new EntitySource_BossSpawn(Main.player[NPC.target]),
                    NPC.position + new Vector2(0, 30), Vector2.Zero, ProjectileID.Boulder, 200, 20);
                
                NPC.ai[2] = 0;
            }
        }
    }
}