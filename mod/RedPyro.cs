using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class RedPyro : MercBase
{
    protected override int FireEvery => 4;
    protected override float Range => 260f;
    protected override float Chance => 0.2f;
    public override void SetDefaults() { Common(40, 76, 150, 20, 3); NPC.knockBackResist = 0.2f; }
    int snd;
    protected override void Fire(Player t)
    {
        Vector2 from = Muzzle(52, 4);
        Mix.Shoot<FlameShot>(from, Mix.Aim(from, t.Center, 7f).RotatedByRandom(0.1f), 14, 1f, true);
        if (snd++ % 14 == 0) Mix.Sound("flame", NPC.Center, 0.8f);
    }
}
