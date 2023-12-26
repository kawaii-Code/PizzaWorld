using PizzaWorld.Code.NPCs.Bosses.BossStages;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Projectiles;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.Audio;
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

    public override void SetDefaults()
    {
        Instance = this;

        NPC.width = 65*3;
        NPC.height = 65*3;

        NPC.aiStyle = -1;
        NPC.npcSlots = 5f;

        NPC.lifeMax = 5000;
        NPC.damage = 50;
        NPC.defense = 20;
        NPC.knockBackResist = 0f;

        NPC.lavaImmune = true;
        NPC.noTileCollide = true;
        NPC.noGravity = true;

        NPC.HitSound = SoundID.NPCHit12;
        NPC.DeathSound = new SoundStyle("PizzaWorld/Sounds/HochuPizzu");

        _currentBossAI = new FirstBossStageAI(NPC);
        _currentBossAI.Start();
    }

    public override void AI()
    {
        if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
        {
            NPC.TargetClosest();
            Debug.Log(NPC.target);
        }

        Player player = Main.player[NPC.target];
        if (player.dead)
        {
            Debug.Log("Hello");
            NPC.velocity.Y -= 0.04f;
            NPC.EncourageDespawn(10);
            return;
        }

        NPC.ai[0]++;
        _currentBossAI.Update();
        CheckStageTransit();
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ModContent.ItemType<Margherita>(), 1, 10, 15));
        if (!NPC.AnyNPCs(ModContent.NPCType<PizzaDeliveryGuy>()))
        {
            Debug.Log("Fuck you");
        }
    }

    private void CheckStageTransit()
    {
        if (this.NPC.life < 3500 && NPC.life > 700)
        {
            if(_currentBossAI.StageID == 2)
                return;

            _currentBossAI = new SecondBossStageAI(NPC);
            Debug.Log("Boss : Нет! Тебе меня не победить АХАХАХА (лох)", Color.Purple);
        }
        else if (NPC.life <= 700)
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
}
