using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaBroadsword : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldBroadsword);
        Item.crit = 10;
        Item.damage = 18;
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (hit.Crit)
        {
            target.AddBuff(BuffID.Poisoned, Seconds(3));
        }

        int Seconds(int value)
        {
            return 60 * value;
        }
    }
}