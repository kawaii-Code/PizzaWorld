using Terraria;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaShroomBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<PizzaShroom>()] > 0)
        {
            return;
        }
        
        player.DelBuff(buffIndex);
        buffIndex--;
    }
}