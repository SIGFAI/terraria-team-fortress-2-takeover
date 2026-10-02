using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Shared behaviour of the mercenaries: walk/attack/hurt frames, ranged fire, speed, blood, ragdoll death.</summary>
public abstract class MercBase : ModNPC
{
    protected virtual float Speed => 1f;
    protected virtual int FireEvery => 0;
    protected virtual float Chance => 0.3f;
    protected virtual float Range => 480f;
    protected virtual string Team => "RED";
    int timer, hurtT, atkT;

    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 7;

    protected void Common(int w, int h, int life, int dmg, int def, float scale = 1.4f)
    {
        NPC.scale = scale; NPC.width = (int)(w * scale); NPC.height = (int)(h * scale);
        NPC.lifeMax = life; NPC.damage = dmg; NPC.defense = def; NPC.knockBackResist = 0.35f; NPC.value = 120;
        NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie; AnimationType = -1;
        NPC.HitSound = Mix.Style("ouch", 0.8f); NPC.DeathSound = Mix.Style("ouch", 1f, -0.3f);
    }

    public override void AI()
    {
        if (Speed != 1f && NPC.velocity.Y == 0 && System.Math.Abs(NPC.velocity.X) > 0.1f)
            NPC.position.X += NPC.velocity.X * (Speed - 1f);
        NPC.rotation = 0f;
        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC o = Main.npc[i];
            if (o.active && o.whoAmI != NPC.whoAmI && o.ModNPC is MercBase && System.Math.Abs(o.Center.X - NPC.Center.X) < 44 && System.Math.Abs(o.Center.Y - NPC.Center.Y) < 60)
                NPC.velocity.X += (NPC.Center.X >= o.Center.X ? 1 : -1) * 0.25f;
        }
        if (hurtT > 0) hurtT--;
        if (atkT > 0) atkT--;
        if (FireEvery > 0)
        {
            NPC.TargetClosest(true);
            Player t = Main.player[NPC.target];
            bool see = t.active && !t.dead && Vector2.Distance(t.Center, NPC.Center) < Range
                && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, t.position, t.width, t.height);
            if (see)
            {
                NPC.direction = NPC.spriteDirection = t.Center.X < NPC.Center.X ? -1 : 1;
                if (++timer >= FireEvery) { timer = 0; atkT = 14; Fire(t); }
            }
            else timer = System.Math.Min(timer, FireEvery / 2);
        }
    }
    protected virtual void Fire(Player t) { }

    /// <summary>Position in front of the gun (sprites face left, so the front is -spriteDirection... see Muzzle).</summary>
    protected Vector2 Muzzle(float forward, float up) => NPC.Center + new Vector2(NPC.spriteDirection * forward, up);

    protected void Flash(Vector2 at)
    {
        Mix.Burst(at, DustID.Torch, 3, 4f, 3f, null, 1.4f);
        Mix.Light(at, Color.Orange);
    }

    public override void FindFrame(int frameHeight)
    {
        int f = 0;
        if (hurtT > 0) f = 6;
        else if (atkT > 0) f = 5;
        else if (NPC.velocity.Y != 0) f = 2;
        else if (System.Math.Abs(NPC.velocity.X) > 0.3f)
        {
            NPC.frameCounter += System.Math.Min(2.5, System.Math.Abs(NPC.velocity.X) * 0.6 + 0.4);
            f = 1 + (int)(NPC.frameCounter / 6) % 4;
        }
        NPC.frame.Y = f * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        hurtT = 9;
        Mix.Burst(NPC.Center, DustID.Blood, hit.Damage > 20 ? 6 : 3);
        if (Main.rand.NextBool(2)) Mix.Popup(NPC.Top + new Vector2(Main.rand.Next(-30, 30), -10), hit.Crit ? "CRIT!" : "MINI-CRIT!", hit.Crit ? Color.Gold : Color.Orange);
        if (NPC.life <= 0)
        {
            Mix.Burst(NPC.Center, DustID.Blood, 8);
            Mix.Burst(NPC.Center, DustID.Confetti, 10);
            Hud.Score(Team);
            Mix.Popup(NPC.Top, "HEADSHOT!", Color.Red);
            Mix.Say("SIGF killed " + NPC.FullName + " (" + Team + ")");
            Projectile c = Mix.Shoot<MercCorpse>(NPC.Center, new Vector2(hit.HitDirection * 3f, -7f), 0, 0f);
            if (c != null) { c.ai[0] = Type; c.ai[1] = NPC.spriteDirection; c.friendly = false; }
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
        spawnInfo.Player.ZoneForest && spawnInfo.SpawnTileY < Main.worldSurface ? Chance : 0f;

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(Terraria.GameContent.ItemDropRules.ItemDropRule.Common(ItemID.SilverCoin, 1, 5, 15));
    }
}
