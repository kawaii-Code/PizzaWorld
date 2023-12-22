using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld.Code.NPCs.Bosses;

public class PizzaBossMusic : ModSceneEffect
{
    public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/PizzaBoss");
    public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;

    public override float GetWeight(Player player)
    {
        return 1.0f;
    }

    public override bool IsSceneEffectActive(Player player)
    {
        return NPC.AnyNPCs(ModContent.NPCType<PizzaBoss>());
    }
}