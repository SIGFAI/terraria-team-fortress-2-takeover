using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class RedScout : MercBase
{
    protected override float Speed => 1.7f;
    public override void SetDefaults() { Common(40, 70, 80, 14, 2, 1.7f); NPC.knockBackResist = 0.6f; }
}
