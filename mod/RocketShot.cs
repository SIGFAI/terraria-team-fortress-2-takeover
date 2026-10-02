using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class RocketShot : ModProjectile
{
    bool boom;
    public override void SetDefaults()
    {
        Projectile.width = 20; Projectile.height = 14; Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged; Projectile.penetrate = -1; Projectile.timeLeft = 200;
        Projectile.usesLocalNPCImmunity = true; Projectile.localNPCHitCooldown = -1;
    }
    public override bool PreAI()
    {
        if (boom)
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.alpha = 255;
            return false;
        }
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
        Projectile.velocity *= 1.012f;
        Mix.Burst(Projectile.Center - Projectile.velocity, DustID.Torch, 2);
        Mix.Burst(Projectile.Center, DustID.Smoke, 1);
        Mix.Light(Projectile.Center, Color.OrangeRed);
        return false;
    }
    void Explode()
    {
        if (boom) return;
        boom = true;
        Projectile.tileCollide = false;
        Vector2 c = Projectile.Center;
        Projectile.Resize(150, 150);
        Projectile.Center = c;
        Projectile.timeLeft = 3;
        Projectile.knockBack = 9f;
        Mix.Burst(Projectile.Center, DustID.Torch, 40, 30f, 5f, null, 2f);
        Mix.Burst(Projectile.Center, DustID.Smoke, 10);
        Mix.Sound("boom", Projectile.Center);
        Mix.Shake(9, 0.35);
    }
    public override bool OnTileCollide(Vector2 oldVelocity) { Explode(); return false; }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => Explode();
    public override void OnHitPlayer(Player target, Player.HurtInfo info) => Explode();
    public override void OnKill(int timeLeft) { if (!boom) { boom = true; Mix.Burst(Projectile.Center, DustID.Torch, 20); Mix.Sound("boom", Projectile.Center, 0.7f); } }
}
