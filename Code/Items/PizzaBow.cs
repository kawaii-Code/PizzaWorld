using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaBow : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.PlatinumBow);
        Item.damage = 15;
        Item.crit = 7;
        Item.useTime = 25;
        Item.value = Silver(25);

        int Silver(int value)
        {
            return 100 * value;
        }
    }
}