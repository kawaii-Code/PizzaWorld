using Microsoft.Xna.Framework;
using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses.BossStages;

internal class ThirdBossStageAI : BossAI
{
    private float _bombReleaseDelay = 40;
    
    private Player _currentTarget;

    private int _pizzaDropCounter;
    private int _bombDropCounter;
    
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

            SoundEngine.PlaySound(SoundID.NPCDeath64);
            Item.NewItem(
                new EntitySource_Loot(Main.npc[NPC.whoAmI]),(int)NPC.Center.X , (int)NPC.Center.Y,
                32, 16,
                ModContent.ItemType<Margherita>(), 1);

            NPC.ai[2] = 0;
            return;
        }

        if(_pizzaDropCounter < 10)
            return;

        _pizzaDropCounter = 100;
        
        if (NPC.ai[2] > _bombReleaseDelay && _bombDropCounter <= 7)
        {
            NPC.dontTakeDamage = true;
            _bombDropCounter++;
            
            var created = Projectile.NewProjectileDirect(new EntitySource_BossSpawn(Main.player[NPC.target]),
                new Vector2(Main.player[NPC.target].position.X, NPC.position.Y - 30), Vector2.Zero, ProjectileID.Boulder, 100, 20);
            
            NPC.ai[2] = 0;
            return;
        }
        
        if(NPC.ai[2] > 100)
            this.NPC.dontTakeDamage = false;
    }
}
