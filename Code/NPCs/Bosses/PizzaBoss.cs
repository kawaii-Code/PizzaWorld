using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Systems;
using Terraria;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses;

[AutoloadBossHead]
public class PizzaBoss : ModNPC
{
    private bool _stunned;
    private int _stunnedTimer;
    private int _ai;
    private int _frame;
    
    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Skeleton];
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
        
        AnimationType = NPCID.Goldfish;
        AIType = NPCID.Goldfish;
    }

    public override void AI()
    {
        NPC.TargetClosest(true);
        Player player = Main.player[NPC.target];
        
        Vector2 targetPosition = NPC.HasPlayerTarget ? player.Center : Main.npc[NPC.target].Center;

        NPC.netAlways = true;

        if (NPC.life > NPC.lifeMax)
            NPC.life = NPC.lifeMax;

        if (NPC.target < 0 || NPC.target == 255 || player.dead || player.active == false)
        {
            NPC.TargetClosest(false);
            NPC.direction = 1;
            NPC.velocity.Y = 0;

            if (NPC.timeLeft > 20)
            {
                NPC.timeLeft = 20;
                return;
            }
        }

        if (_stunned)
        {
            NPC.velocity = Vector2.Zero;
            
            _stunnedTimer++;

            if (_stunnedTimer >= 100)
            {
                _stunned = false;
                _stunnedTimer = 0;
            }
        }

        _ai++;

        int distance = (int)Vector2.Distance(targetPosition, NPC.Center);

        NPC.netUpdate = true;
        
        if ((double)NPC.ai[0] < 300)
        {
        }
    }
}