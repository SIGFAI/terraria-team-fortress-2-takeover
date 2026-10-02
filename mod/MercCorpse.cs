using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Ragdoll of a dead mercenary: flung up, spinning, fading. ai[0] = npc type, ai[1] = facing.</summary>
public class MercCorpse : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 20; Projectile.height = 20; Projectile.friendly = false; Projectile.hostile = false;
        Projectile.tileCollide = true; Projectile.penetrate = -1; Projectile.timeLeft = 55;
    }
    public override void AI()
    {
        Projectile.velocity.Y += 0.35f;
        Projectile.velocity.X *= 0.99f;
        Projectile.rotation += 0.22f * (Projectile.velocity.X < 0 ? -1 : 1);
        if (Main.rand.NextBool(2)) Mix.Burst(Projectile.Center, DustID.Blood, 1, 8f, 1f);
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity.X *= 0.6f;
        if (System.Math.Abs(oldVelocity.Y) > 2f) Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
        return false;
    }
    public override void OnKill(int timeLeft)
    {
        Mix.Burst(Projectile.Center, DustID.Confetti, 20);
        Mix.Burst(Projectile.Center, DustID.Smoke, 12);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        int type = (int)Projectile.ai[0];
        if (type <= 0) return false;
        Texture2D tex = TextureAssets.Npc[type].Value;
        int fh = tex.Height / 7;
        Rectangle src = new Rectangle(0, 6 * fh, tex.Width, fh);
        float fade = Projectile.timeLeft < 20 ? Projectile.timeLeft / 20f : 1f;
        SpriteEffects fx = Projectile.ai[1] > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, src, lightColor * fade, Projectile.rotation,
            src.Size() / 2f, 1.3f, fx, 0);
        return false;
    }
}
