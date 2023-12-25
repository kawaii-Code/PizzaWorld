using Microsoft.Xna.Framework;
using Terraria;

namespace PizzaWorld.Code.NPCs.Bosses.BossStages;

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