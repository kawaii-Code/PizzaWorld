using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items.PizzaAxe;

public class PizzaAxe : ModItem
{
    public override void SetDefaults()
    {
        Item.axe = 20;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.UseSound = SoundID.Dig;
    }

    public override void AddRecipes()
    {
        
    }
}