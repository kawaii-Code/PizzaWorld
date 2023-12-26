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

        NPC.lifeMax = 6000;
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
        if(Main.netMode == NetmodeID.MultiplayerClient)
            return;

        NPC.ai[0]++;
        _currentBossAI.Update();
        CheckStageTransit();
    }

    /*public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ModContent.ItemType<Margherita>(), 1, 10, 15));
        if (!NPC.AnyNPCs(ModContent.NPCType<PizzaDeliveryGuy>()))
        {
            Debug.Log("Fuck you");
            Point playerPosition = Main.player[Main.myPlayer].position.ToTileCoordinates();
            PizzaWorld.SpawnNPC<PizzaDeliveryGuy>(playerPosition.X, playerPosition.Y);
        }
    }*/

    private void CheckStageTransit()
    {
        if (this.NPC.life < 4500 && NPC.life > 1700)
        {
            if(_currentBossAI.StageID == 2)
                return;

            _currentBossAI = new SecondBossStageAI(NPC);
            Debug.Log("Boss : Нет! Тебе меня не победить АХАХАХА (лох)", Color.Purple);
        }
        else if (NPC.life <= 1700)
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
