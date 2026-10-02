using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Sigf.Content.Items;

namespace Sigf.Content;

public class SigfMod : ModSystem
{
    // Ground spot dx tiles to the left (-) or right (+) of the stream player.
    static Vector2 At(float dx)
    {
        Vector2 c = Mix.Host.Center;
        int x = (int)(c.X / 16f + dx);
        for (int y = (int)(c.Y / 16f) - 25; y < (int)(c.Y / 16f) + 40; y++)
            if (WorldGen.InWorld(x, y, 5) && Main.tile[x, y].HasUnactuatedTile && Main.tileSolid[Main.tile[x, y].TileType])
                return new Vector2(x * 16f + 8, y * 16f);
        return c;
    }
    static int side = 1;
    static void Wave(string title, string sub, (int type, float dx)[] w)
    {
        Mix.Title(title, sub, 3);
        foreach (var (type, dx) in w) Mix.Spawn(type, At(dx));
    }

    public override void OnWorldLoad()
    {
        Mix.Every(4, () =>
        {
            if (Mix.Hostiles(Mix.Host.Center, 40).Count >= 4) return;
            side = -side;
            int[] t = { ModContent.NPCType<RedScout>(), ModContent.NPCType<RedSoldier>(), ModContent.NPCType<RedPyro>(), ModContent.NPCType<BluHeavy>() };
            Mix.Spawn(t[Main.rand.Next(t.Length)], At(side * Main.rand.Next(22, 34)));
        });
        int sc = ModContent.NPCType<RedScout>(), so = ModContent.NPCType<RedSoldier>(), py = ModContent.NPCType<RedPyro>(), hv = ModContent.NPCType<BluHeavy>();
        var waves = new (string, string, (int, float)[])[]
        {
            ("TEAM FORTRESS TAKEOVER", "RED vs BLU: the forest is a battlefield", new[] { (sc, -24f), (hv, 30f), (so, 38f), (py, -34f) }),
            ("SCOUT RUSH", "Fast and annoying", new[] { (sc, 22f), (sc, -26f), (py, 34f) }),
            ("PYRO AMBUSH", "Mind the flames!", new[] { (py, -22f), (py, 30f), (sc, -34f) }),
            ("HEAVY RUSH", "BLU brings the big guns", new[] { (hv, 24f), (hv, -34f), (so, 40f) }),
            ("ROCKET BARRAGE", "Soldiers incoming", new[] { (so, -22f), (so, 28f), (sc, 36f) }),
        };
        int wi = 0;
        Mix.After(1, () => { Mix.Arm<RocketLauncher>(); Mix.Autopilot = true; Mix.AutoAttack = true; });
        Mix.After(2, () => { var w = waves[wi++ % waves.Length]; Wave(w.Item1, w.Item2, w.Item3); });
        Mix.Every(9, () =>
        {
            Mix.Arm<RocketLauncher>();
            var w = waves[wi++ % waves.Length]; Wave(w.Item1, w.Item2, w.Item3);
        });
    }
}
