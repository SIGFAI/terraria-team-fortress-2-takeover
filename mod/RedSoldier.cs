using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class RedSoldier : MercBase
{
    protected override int FireEvery => 150;
    protected override float Chance => 0.25f;
    public override void SetDefaults() { Common(44, 80, 140, 18, 4); }
    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        base.ModifyNPCLoot(npcLoot);
        npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(ModContent.ItemType<Sigf.Content.Items.RocketLauncher>(), 4));
    }
    protected override void Fire(Player t)
    {
        Vector2 from = Muzzle(44, -18);
        Mix.Shoot<RocketShot>(from, Mix.Aim(from, t.Center, 7f), 25, 4f, true);
        Mix.Sound(SoundID.Item92, NPC.Center);
        Flash(from);
    }
}
