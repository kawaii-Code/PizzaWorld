using Microsoft.Xna.Framework;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace PizzaWorld.Code.NPCs.Bosses.BossStages;

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