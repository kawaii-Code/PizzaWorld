using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace PizzaWorld.Code.NPCs.Bosses.BossStages;

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