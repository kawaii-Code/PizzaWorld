using PizzaWorld.Code.NPCs;
using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Systems;

public class PizzaGuideSpawn : ModSystem
{
    private bool _disabled;

    public override void PostUpdatePlayers()
    {
        if (_disabled)
        {
            return;
        }

        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player p = Main.player[i];
            if (!p.active)
            {
                continue;
            }

            if (NPC.AnyNPCs(ModContent.NPCType<PizzaGuide>()))
            {
                PizzaWorld.SpawnNPC<PizzaGuide>();
                _disabled = true;
            }
        }
    }
}
