using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaGuitar : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.CarbonGuitar);
        Item.UseSound = SoundID.GuitarAm;
    }
    
}