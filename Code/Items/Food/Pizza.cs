using PizzaWorld.Code.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items.Food;

public class BasicPizza : ModItem
{
    protected virtual int WellFedDuration =>
        Time.Seconds(30);

    protected virtual int Value =>
        Price.Silver(2);
    
    public override void SetDefaults()
    {
        Item.DefaultToFood(16, 16, BuffID.WellFed3, WellFedDuration);
        Item.value = Value;
    }
}

public class Champignon : BasicPizza
{
    protected override int WellFedDuration =>
        Time.Minutes(1);

    protected override int Value => Price.Silver(25);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(TileID.Dirt, 20);
    }
}

public class FourCheeses : BasicPizza
{
    protected override int Value => Price.Silver(50);
    
    protected override int WellFedDuration =>
        Time.Minutes(1) + Time.Seconds(30);
}

public class Napoletana : BasicPizza
{
    protected override int Value => Price.Silver(75);
    
    protected override int WellFedDuration =>
        Time.Minutes(2);
}

public class Barbeque : BasicPizza
{
    protected override int Value => Price.Gold(1);
    
    protected override int WellFedDuration =>
        Time.Minutes(3);
}

public class Margherita : BasicPizza
{
    protected override int WellFedDuration =>
        Time.Minutes(5);
}
