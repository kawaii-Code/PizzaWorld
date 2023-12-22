using PizzaWorld.Code.Items.Food;
using PizzaWorld.Code.Projectiles;
using PizzaWorld.Code.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PizzaWorld.Code.Items;

public class PizzaStaff : ModItem
{
    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.DamageType = DamageClass.Magic;
        Item.UseSound = SoundID.Dolphin;

        Item.shoot = ModContent.ProjectileType<PizzaProjectile>();
        
        Item.damage = 20;
        Item.crit = 30;
        Item.noMelee = true;
        Item.mana = 8;
        Item.damage = 24;
        Item.knockBack = 3.2f;
        Item.useTime = 15;
        Item.useAnimation = 15;
        Item.shootSpeed = 1f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(ItemID.Wood, 3)
            .AddIngredient<Margherita>(2)
            .AddTile(TileID.WorkBenches)
            .Register();
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        Debug.Log("Cum");
        
        if (hit.Crit && Main.rand.NextBool())
        {
            target.AddBuff(BuffID.Confused, 60 * 3);
            player.AddBuff(BuffID.WellFed3, 10 * 60);
        }
    }
    
}
