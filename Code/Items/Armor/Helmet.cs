using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

#if false

namespace PizzaWorld.Code.Items.Armor;

[AutoloadEquip(EquipType.Head)]
public class Helmet : ModItem
{
    public override void SetStaticDefaults()
    {
        
    }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;

        Item.value = Terraria.Item.buyPrice(platinum:1, gold: 1);
        Item.rare = ItemRarityID.Blue;

        Item.defense = 6;
    }

    public override void UpdateEquip(Player player)
    {
        player.buffImmune[BuffID.Cursed] = true;
    }
}

public class Breastplate : ModItem
{
    
}
#endif