using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class BluHeavy : MercBase
{
    protected override string Team => "BLU";
    protected override float Speed => 0.6f;
    protected override int FireEvery => 20;
    protected override float Chance => 0.25f;
    int shots;
    public override void SetDefaults() { Common(62, 96, 220, 24, 4, 1.35f); NPC.knockBackResist = 0.1f; NPC.value = 400; }
    protected override void Fire(Player t)
    {
        Vector2 from = Muzzle(62, 6);
        Vector2 v = Mix.Aim(from, t.Center, 9f).RotatedByRandom(0.12f);
        Mix.Shoot(ProjectileID.Bullet, from, v, 10, 1f, true);
        if (shots++ % 4 == 0) Mix.Sound("minigun", NPC.Center, 0.6f);
        Flash(from);
    }
}
