using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Pyro flame: a stream of real fire dust and embers, short range.</summary>
public class FlameShot : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 26; Projectile.height = 26; Projectile.hostile = true; Projectile.friendly = false;
        Projectile.penetrate = -1; Projectile.timeLeft = 32; Projectile.tileCollide = true; Projectile.ignoreWater = true;
    }
    public override void AI()
    {
        Projectile.velocity *= 0.985f;
        float grow = 1f + (32 - Projectile.timeLeft) / 10f;
        for (int i = 0; i < 2; i++)
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8, 8), DustID.Torch,
                Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1, 1), 100, default, 1.4f + grow * 0.5f);
            d.noGravity = true;
        }
        if (Main.rand.NextBool(2))
        {
            Dust e = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, new Vector2(Main.rand.NextFloat(-1, 1), -Main.rand.NextFloat(1, 3)), 150, Color.OrangeRed, 1f);
            e.noGravity = true;
        }
        if (Main.rand.NextBool(4))
        {
            Dust s = Dust.NewDustPerfect(Projectile.Center, DustID.YellowStarDust, new Vector2(Main.rand.NextFloat(-2, 2), -Main.rand.NextFloat(1, 3)), 0, default, 0.9f);
            s.noGravity = true;
        }
        Mix.Light(Projectile.Center, new Color(255, 140, 40));
    }
    public override bool PreDraw(ref Color lightColor) => false;
}
