using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items.Armor;

[AutoloadEquip(EquipType.Head)]
public class PizzaHelmet : ModItem
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

    public override void UpdateArmorSet(Player player)
    {
        player.AddBuff(BuffID.WellFed3, 60 * 99999, quiet: true);
        player.buffImmune[BuffID.Invisibility] = true;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return (head.type == ModContent.ItemType<PizzaHelmet>() && legs.type == ModContent.ItemType<PizzaGreaves>() &&
                body.type == ModContent.ItemType<PizzaBreastplate>());
    }
}