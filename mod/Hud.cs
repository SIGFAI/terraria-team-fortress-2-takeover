using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>TF2-style scoreboard drawn over the game, plus a dusty orange sky.</summary>
public class Hud : ModSystem
{
    static int red, blu;
    public static void Score(string team) { if (team == "RED") blu++; else red++; }

    public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
    {
        backgroundColor = Color.Lerp(backgroundColor, new Color(255, 150, 90), 0.4f);
        tileColor = Color.Lerp(tileColor, new Color(255, 215, 170), 0.25f);
    }

    public override void PostDrawInterface(SpriteBatch sb)
    {
        var pix = TextureAssets.MagicPixel.Value;
        float cx = Main.screenWidth / 2f;
        int y = 10;
        sb.Draw(pix, new Rectangle((int)cx - 150, y, 150, 44), new Color(170, 40, 35) * 0.9f);
        sb.Draw(pix, new Rectangle((int)cx, y, 150, 44), new Color(40, 80, 170) * 0.9f);
        sb.Draw(pix, new Rectangle((int)cx - 150, y + 44, 300, 3), Color.Black * 0.8f);
        Utils.DrawBorderStringBig(sb, "RED " + blu, new Vector2(cx - 140, y + 6), Color.White, 0.7f);
        Utils.DrawBorderStringBig(sb, "BLU " + red, new Vector2(cx + 10, y + 6), Color.White, 0.7f);
        Utils.DrawBorderString(sb, "TEAM FORTRESS TAKEOVER", new Vector2(cx, y + 54), Color.Gold, 0.9f, 0.5f);
    }
}
