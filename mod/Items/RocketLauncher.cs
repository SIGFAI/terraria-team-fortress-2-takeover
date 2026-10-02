using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class RocketLauncher : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 48; Item.height = 24;
        Item.damage = 38; Item.DamageType = DamageClass.Ranged; Item.knockBack = 7f;
        Item.useTime = 28; Item.useAnimation = 28; Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true; Item.autoReuse = true;
        Item.UseSound = SoundID.Item92; Item.rare = ItemRarityID.Red;
        Item.shoot = ModContent.ProjectileType<RocketShot>(); Item.shootSpeed = 11f;
    }
    public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 tip = position + Vector2.Normalize(velocity) * 36f;
        Mix.Burst(tip, DustID.Torch, 4, 4f, 3f, null, 1.4f);
        Mix.Light(tip, Color.Orange);
        return true;
    }
    public override Vector2? HoldoutOffset() => new Vector2(-6, 0);
}
