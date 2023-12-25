using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses.BossStages;

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