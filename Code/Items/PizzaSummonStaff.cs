using Microsoft.Xna.Framework;
using PizzaWorld.Code.Buffs;
using PizzaWorld.Code.NPCs;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaSummonStaff : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        
        Item.damage = 12;
        Item.crit = 0;
        Item.DamageType = DamageClass.Summon;
        Item.shoot = ModContent.ProjectileType<PizzaShroom>();
        Item.buffType = ModContent.BuffType<PizzaShroomBuff>();
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 28;
        Item.useAnimation = 28;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type,
        int damage, float knockback)
    {
        player.AddBuff(ModContent.BuffType<PizzaShroomBuff>(), Timings.Seconds(6969));
        return true;
    }
}