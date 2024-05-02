using System;
using System.Numerics;
using DrawingColor = System.Drawing.Color;

namespace SilkDemo.WinUI.Common;

public struct SilkColor
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }

    //
    // 摘要:
    //     Zero color.
    public static readonly SilkColor Zero = FromBgra(0);

    //
    // 摘要:
    //     Transparent color.
    public static readonly SilkColor Transparent = FromBgra(0);

    //
    // 摘要:
    //     AliceBlue color.
    public static readonly SilkColor AliceBlue = FromBgra(4293982463u);

    //
    // 摘要:
    //     AntiqueWhite color.
    public static readonly SilkColor AntiqueWhite = FromBgra(4294634455u);

    //
    // 摘要:
    //     Aqua color.
    public static readonly SilkColor Aqua = FromBgra(4278255615u);

    //
    // 摘要:
    //     Aquamarine color.
    public static readonly SilkColor Aquamarine = FromBgra(4286578644u);

    //
    // 摘要:
    //     Azure color.
    public static readonly SilkColor Azure = FromBgra(4293984255u);

    //
    // 摘要:
    //     Beige color.
    public static readonly SilkColor Beige = FromBgra(4294309340u);

    //
    // 摘要:
    //     Bisque color.
    public static readonly SilkColor Bisque = FromBgra(4294960324u);

    //
    // 摘要:
    //     Black color.
    public static readonly SilkColor Black = FromBgra(4278190080u);

    //
    // 摘要:
    //     BlanchedAlmond color.
    public static readonly SilkColor BlanchedAlmond = FromBgra(4294962125u);

    //
    // 摘要:
    //     Blue color.
    public static readonly SilkColor Blue = FromBgra(4278190335u);

    //
    // 摘要:
    //     BlueViolet color.
    public static readonly SilkColor BlueViolet = FromBgra(4287245282u);

    //
    // 摘要:
    //     Brown color.
    public static readonly SilkColor Brown = FromBgra(4289014314u);

    //
    // 摘要:
    //     BurlyWood color.
    public static readonly SilkColor BurlyWood = FromBgra(4292786311u);

    //
    // 摘要:
    //     CadetBlue color.
    public static readonly SilkColor CadetBlue = FromBgra(4284456608u);

    //
    // 摘要:
    //     Chartreuse color.
    public static readonly SilkColor Chartreuse = FromBgra(4286578432u);

    //
    // 摘要:
    //     Chocolate color.
    public static readonly SilkColor Chocolate = FromBgra(4291979550u);

    //
    // 摘要:
    //     Coral color.
    public static readonly SilkColor Coral = FromBgra(4294934352u);

    //
    // 摘要:
    //     CornflowerBlue color.
    public static readonly SilkColor CornflowerBlue = FromBgra(4284782061u);

    //
    // 摘要:
    //     Cornsilk color.
    public static readonly SilkColor Cornsilk = FromBgra(4294965468u);

    //
    // 摘要:
    //     Crimson color.
    public static readonly SilkColor Crimson = FromBgra(4292613180u);

    //
    // 摘要:
    //     Cyan color.
    public static readonly SilkColor Cyan = FromBgra(4278255615u);

    //
    // 摘要:
    //     DarkBlue color.
    public static readonly SilkColor DarkBlue = FromBgra(4278190219u);

    //
    // 摘要:
    //     DarkCyan color.
    public static readonly SilkColor DarkCyan = FromBgra(4278225803u);

    //
    // 摘要:
    //     DarkGoldenrod color.
    public static readonly SilkColor DarkGoldenrod = FromBgra(4290283019u);

    //
    // 摘要:
    //     DarkGray color.
    public static readonly SilkColor DarkGray = FromBgra(4289309097u);

    //
    // 摘要:
    //     DarkGreen color.
    public static readonly SilkColor DarkGreen = FromBgra(4278215680u);

    //
    // 摘要:
    //     DarkKhaki color.
    public static readonly SilkColor DarkKhaki = FromBgra(4290623339u);

    //
    // 摘要:
    //     DarkMagenta color.
    public static readonly SilkColor DarkMagenta = FromBgra(4287299723u);

    //
    // 摘要:
    //     DarkOliveGreen color.
    public static readonly SilkColor DarkOliveGreen = FromBgra(4283788079u);

    //
    // 摘要:
    //     DarkOrange color.
    public static readonly SilkColor DarkOrange = FromBgra(4294937600u);

    //
    // 摘要:
    //     DarkOrchid color.
    public static readonly SilkColor DarkOrchid = FromBgra(4288230092u);

    //
    // 摘要:
    //     DarkRed color.
    public static readonly SilkColor DarkRed = FromBgra(4287299584u);

    //
    // 摘要:
    //     DarkSalmon color.
    public static readonly SilkColor DarkSalmon = FromBgra(4293498490u);

    //
    // 摘要:
    //     DarkSeaGreen color.
    public static readonly SilkColor DarkSeaGreen = FromBgra(4287609995u);

    //
    // 摘要:
    //     DarkSlateBlue color.
    public static readonly SilkColor DarkSlateBlue = FromBgra(4282924427u);

    //
    // 摘要:
    //     DarkSlateGray color.
    public static readonly SilkColor DarkSlateGray = FromBgra(4281290575u);

    //
    // 摘要:
    //     DarkTurquoise color.
    public static readonly SilkColor DarkTurquoise = FromBgra(4278243025u);

    //
    // 摘要:
    //     DarkViolet color.
    public static readonly SilkColor DarkViolet = FromBgra(4287889619u);

    //
    // 摘要:
    //     DeepPink color.
    public static readonly SilkColor DeepPink = FromBgra(4294907027u);

    //
    // 摘要:
    //     DeepSkyBlue color.
    public static readonly SilkColor DeepSkyBlue = FromBgra(4278239231u);

    //
    // 摘要:
    //     DimGray color.
    public static readonly SilkColor DimGray = FromBgra(4285098345u);

    //
    // 摘要:
    //     DodgerBlue color.
    public static readonly SilkColor DodgerBlue = FromBgra(4280193279u);

    //
    // 摘要:
    //     Firebrick color.
    public static readonly SilkColor Firebrick = FromBgra(4289864226u);

    //
    // 摘要:
    //     FloralWhite color.
    public static readonly SilkColor FloralWhite = FromBgra(4294966000u);

    //
    // 摘要:
    //     ForestGreen color.
    public static readonly SilkColor ForestGreen = FromBgra(4280453922u);

    //
    // 摘要:
    //     Fuchsia color.
    public static readonly SilkColor Fuchsia = FromBgra(4294902015u);

    //
    // 摘要:
    //     Gainsboro color.
    public static readonly SilkColor Gainsboro = FromBgra(4292664540u);

    //
    // 摘要:
    //     GhostWhite color.
    public static readonly SilkColor GhostWhite = FromBgra(4294506751u);

    //
    // 摘要:
    //     Gold color.
    public static readonly SilkColor Gold = FromBgra(4294956800u);

    //
    // 摘要:
    //     Goldenrod color.
    public static readonly SilkColor Goldenrod = FromBgra(4292519200u);

    //
    // 摘要:
    //     Gray color.
    public static readonly SilkColor Gray = FromBgra(4286611584u);

    //
    // 摘要:
    //     Green color.
    public static readonly SilkColor Green = FromBgra(4278222848u);

    //
    // 摘要:
    //     GreenYellow color.
    public static readonly SilkColor GreenYellow = FromBgra(4289593135u);

    //
    // 摘要:
    //     Honeydew color.
    public static readonly SilkColor Honeydew = FromBgra(4293984240u);

    //
    // 摘要:
    //     HotPink color.
    public static readonly SilkColor HotPink = FromBgra(4294928820u);

    //
    // 摘要:
    //     IndianRed color.
    public static readonly SilkColor IndianRed = FromBgra(4291648604u);

    //
    // 摘要:
    //     Indigo color.
    public static readonly SilkColor Indigo = FromBgra(4283105410u);

    //
    // 摘要:
    //     Ivory color.
    public static readonly SilkColor Ivory = FromBgra(4294967280u);

    //
    // 摘要:
    //     Khaki color.
    public static readonly SilkColor Khaki = FromBgra(4293977740u);

    //
    // 摘要:
    //     Lavender color.
    public static readonly SilkColor Lavender = FromBgra(4293322490u);

    //
    // 摘要:
    //     LavenderBlush color.
    public static readonly SilkColor LavenderBlush = FromBgra(4294963445u);

    //
    // 摘要:
    //     LawnGreen color.
    public static readonly SilkColor LawnGreen = FromBgra(4286381056u);

    //
    // 摘要:
    //     LemonChiffon color.
    public static readonly SilkColor LemonChiffon = FromBgra(4294965965u);

    //
    // 摘要:
    //     LightBlue color.
    public static readonly SilkColor LightBlue = FromBgra(4289583334u);

    //
    // 摘要:
    //     LightCoral color.
    public static readonly SilkColor LightCoral = FromBgra(4293951616u);

    //
    // 摘要:
    //     LightCyan color.
    public static readonly SilkColor LightCyan = FromBgra(4292935679u);

    //
    // 摘要:
    //     LightGoldenrodYellow color.
    public static readonly SilkColor LightGoldenrodYellow = FromBgra(4294638290u);

    //
    // 摘要:
    //     LightGray color.
    public static readonly SilkColor LightGray = FromBgra(4292072403u);

    //
    // 摘要:
    //     LightGreen color.
    public static readonly SilkColor LightGreen = FromBgra(4287688336u);

    //
    // 摘要:
    //     LightPink color.
    public static readonly SilkColor LightPink = FromBgra(4294948545u);

    //
    // 摘要:
    //     LightSalmon color.
    public static readonly SilkColor LightSalmon = FromBgra(4294942842u);

    //
    // 摘要:
    //     LightSeaGreen color.
    public static readonly SilkColor LightSeaGreen = FromBgra(4280332970u);

    //
    // 摘要:
    //     LightSkyBlue color.
    public static readonly SilkColor LightSkyBlue = FromBgra(4287090426u);

    //
    // 摘要:
    //     LightSlateGray color.
    public static readonly SilkColor LightSlateGray = FromBgra(4286023833u);

    //
    // 摘要:
    //     LightSteelBlue color.
    public static readonly SilkColor LightSteelBlue = FromBgra(4289774814u);

    //
    // 摘要:
    //     LightYellow color.
    public static readonly SilkColor LightYellow = FromBgra(4294967264u);

    //
    // 摘要:
    //     Lime color.
    public static readonly SilkColor Lime = FromBgra(4278255360u);

    //
    // 摘要:
    //     LimeGreen color.
    public static readonly SilkColor LimeGreen = FromBgra(4281519410u);

    //
    // 摘要:
    //     Linen color.
    public static readonly SilkColor Linen = FromBgra(4294635750u);

    //
    // 摘要:
    //     Magenta color.
    public static readonly SilkColor Magenta = FromBgra(4294902015u);

    //
    // 摘要:
    //     Maroon color.
    public static readonly SilkColor Maroon = FromBgra(4286578688u);

    //
    // 摘要:
    //     MediumAquamarine color.
    public static readonly SilkColor MediumAquamarine = FromBgra(4284927402u);

    //
    // 摘要:
    //     MediumBlue color.
    public static readonly SilkColor MediumBlue = FromBgra(4278190285u);

    //
    // 摘要:
    //     MediumOrchid color.
    public static readonly SilkColor MediumOrchid = FromBgra(4290401747u);

    //
    // 摘要:
    //     MediumPurple color.
    public static readonly SilkColor MediumPurple = FromBgra(4287852763u);

    //
    // 摘要:
    //     MediumSeaGreen color.
    public static readonly SilkColor MediumSeaGreen = FromBgra(4282168177u);

    //
    // 摘要:
    //     MediumSlateBlue color.
    public static readonly SilkColor MediumSlateBlue = FromBgra(4286277870u);

    //
    // 摘要:
    //     MediumSpringGreen color.
    public static readonly SilkColor MediumSpringGreen = FromBgra(4278254234u);

    //
    // 摘要:
    //     MediumTurquoise color.
    public static readonly SilkColor MediumTurquoise = FromBgra(4282962380u);

    //
    // 摘要:
    //     MediumVioletRed color.
    public static readonly SilkColor MediumVioletRed = FromBgra(4291237253u);

    //
    // 摘要:
    //     MidnightBlue color.
    public static readonly SilkColor MidnightBlue = FromBgra(4279834992u);

    //
    // 摘要:
    //     MintCream color.
    public static readonly SilkColor MintCream = FromBgra(4294311930u);

    //
    // 摘要:
    //     MistyRose color.
    public static readonly SilkColor MistyRose = FromBgra(4294960353u);

    //
    // 摘要:
    //     Moccasin color.
    public static readonly SilkColor Moccasin = FromBgra(4294960309u);

    //
    // 摘要:
    //     NavajoWhite color.
    public static readonly SilkColor NavajoWhite = FromBgra(4294958765u);

    //
    // 摘要:
    //     Navy color.
    public static readonly SilkColor Navy = FromBgra(4278190208u);

    //
    // 摘要:
    //     OldLace color.
    public static readonly SilkColor OldLace = FromBgra(4294833638u);

    //
    // 摘要:
    //     Olive color.
    public static readonly SilkColor Olive = FromBgra(4286611456u);

    //
    // 摘要:
    //     OliveDrab color.
    public static readonly SilkColor OliveDrab = FromBgra(4285238819u);

    //
    // 摘要:
    //     Orange color.
    public static readonly SilkColor Orange = FromBgra(4294944000u);

    //
    // 摘要:
    //     OrangeRed color.
    public static readonly SilkColor OrangeRed = FromBgra(4294919424u);

    //
    // 摘要:
    //     Orchid color.
    public static readonly SilkColor Orchid = FromBgra(4292505814u);

    //
    // 摘要:
    //     PaleGoldenrod color.
    public static readonly SilkColor PaleGoldenrod = FromBgra(4293847210u);

    //
    // 摘要:
    //     PaleGreen color.
    public static readonly SilkColor PaleGreen = FromBgra(4288215960u);

    //
    // 摘要:
    //     PaleTurquoise color.
    public static readonly SilkColor PaleTurquoise = FromBgra(4289720046u);

    //
    // 摘要:
    //     PaleVioletRed color.
    public static readonly SilkColor PaleVioletRed = FromBgra(4292571283u);

    //
    // 摘要:
    //     PapayaWhip color.
    public static readonly SilkColor PapayaWhip = FromBgra(4294963157u);

    //
    // 摘要:
    //     PeachPuff color.
    public static readonly SilkColor PeachPuff = FromBgra(4294957753u);

    //
    // 摘要:
    //     Peru color.
    public static readonly SilkColor Peru = FromBgra(4291659071u);

    //
    // 摘要:
    //     Pink color.
    public static readonly SilkColor Pink = FromBgra(4294951115u);

    //
    // 摘要:
    //     Plum color.
    public static readonly SilkColor Plum = FromBgra(4292714717u);

    //
    // 摘要:
    //     PowderBlue color.
    public static readonly SilkColor PowderBlue = FromBgra(4289781990u);

    //
    // 摘要:
    //     Purple color.
    public static readonly SilkColor Purple = FromBgra(4286578816u);

    //
    // 摘要:
    //     Red color.
    public static readonly SilkColor Red = FromBgra(4294901760u);

    //
    // 摘要:
    //     RosyBrown color.
    public static readonly SilkColor RosyBrown = FromBgra(4290547599u);

    //
    // 摘要:
    //     RoyalBlue color.
    public static readonly SilkColor RoyalBlue = FromBgra(4282477025u);

    //
    // 摘要:
    //     SaddleBrown color.
    public static readonly SilkColor SaddleBrown = FromBgra(4287317267u);

    //
    // 摘要:
    //     Salmon color.
    public static readonly SilkColor Salmon = FromBgra(4294606962u);

    //
    // 摘要:
    //     SandyBrown color.
    public static readonly SilkColor SandyBrown = FromBgra(4294222944u);

    //
    // 摘要:
    //     SeaGreen color.
    public static readonly SilkColor SeaGreen = FromBgra(4281240407u);

    //
    // 摘要:
    //     SeaShell color.
    public static readonly SilkColor SeaShell = FromBgra(4294964718u);

    //
    // 摘要:
    //     Sienna color.
    public static readonly SilkColor Sienna = FromBgra(4288696877u);

    //
    // 摘要:
    //     Silver color.
    public static readonly SilkColor Silver = FromBgra(4290822336u);

    //
    // 摘要:
    //     SkyBlue color.
    public static readonly SilkColor SkyBlue = FromBgra(4287090411u);

    //
    // 摘要:
    //     SlateBlue color.
    public static readonly SilkColor SlateBlue = FromBgra(4285160141u);

    //
    // 摘要:
    //     SlateGray color.
    public static readonly SilkColor SlateGray = FromBgra(4285563024u);

    //
    // 摘要:
    //     Snow color.
    public static readonly SilkColor Snow = FromBgra(4294966010u);

    //
    // 摘要:
    //     SpringGreen color.
    public static readonly SilkColor SpringGreen = FromBgra(4278255487u);

    //
    // 摘要:
    //     SteelBlue color.
    public static readonly SilkColor SteelBlue = FromBgra(4282811060u);

    //
    // 摘要:
    //     Tan color.
    public static readonly SilkColor Tan = FromBgra(4291998860u);

    //
    // 摘要:
    //     Teal color.
    public static readonly SilkColor Teal = FromBgra(4278222976u);

    //
    // 摘要:
    //     Thistle color.
    public static readonly SilkColor Thistle = FromBgra(4292394968u);

    //
    // 摘要:
    //     Tomato color.
    public static readonly SilkColor Tomato = FromBgra(4294927175u);

    //
    // 摘要:
    //     Turquoise color.
    public static readonly SilkColor Turquoise = FromBgra(4282441936u);

    //
    // 摘要:
    //     Violet color.
    public static readonly SilkColor Violet = FromBgra(4293821166u);

    //
    // 摘要:
    //     Wheat color.
    public static readonly SilkColor Wheat = FromBgra(4294303411u);

    //
    // 摘要:
    //     White color.
    public static readonly SilkColor White = FromBgra(uint.MaxValue);

    //
    // 摘要:
    //     WhiteSmoke color.
    public static readonly SilkColor WhiteSmoke = FromBgra(4294309365u);

    //
    // 摘要:
    //     Yellow color.
    public static readonly SilkColor Yellow = FromBgra(4294967040u);

    //
    // 摘要:
    //     YellowGreen color.
    public static readonly SilkColor YellowGreen = FromBgra(4288335154u);

    public SilkColor(byte red, byte green, byte blue, byte alpha = 255)
    {
        R = red;
        G = green;
        B = blue;
        A = alpha;
    }
    public SilkColor(float red, float green, float blue, float alpha = 1.0f)
    {
        R = ToByte(red);
        G = ToByte(green);
        B = ToByte(blue);
        A = ToByte(alpha);
    }
    public SilkColor(int rgba)
    {
        A = (byte)((uint)(rgba >> 24) & 0xFFu);
        B = (byte)((uint)(rgba >> 16) & 0xFFu);
        G = (byte)((uint)(rgba >> 8) & 0xFFu);
        R = (byte)((uint)rgba & 0xFFu);
    }
    public SilkColor(uint rgba)
    {
        A = (byte)((rgba >> 24) & 0xFFu);
        B = (byte)((rgba >> 16) & 0xFFu);
        G = (byte)((rgba >> 8) & 0xFFu);
        R = (byte)(rgba & 0xFFu);
    }

    public readonly int ToBgra() => B | (G << 8) | (R << 16) | (A << 24);
    public readonly int ToRgba() => R | (G << 8) | (B << 16) | (A << 24);
    public readonly int ToAbgr() => A | (B << 8) | (G << 16) | (R << 24);

    public static SilkColor FromBgra(int color)
    {
        return new SilkColor((byte)((uint)(color >> 16) & 0xFFu), (byte)((uint)(color >> 8) & 0xFFu), (byte)((uint)color & 0xFFu), (byte)((uint)(color >> 24) & 0xFFu));
    }
    public static SilkColor FromBgra(uint color)
    {
        return FromBgra((int)color);
    }

    public static SilkColor FromRgba(int color)
    {
        return new SilkColor(color);
    }
    public static SilkColor FromRgba(uint color)
    {
        return new SilkColor(color);
    }

    public static SilkColor FromAbgr(int color)
    {
        return new SilkColor((byte)(color >> 24), (byte)(color >> 16), (byte)(color >> 8), (byte)color);
    }
    public static SilkColor FromAbgr(uint color)
    {
        return FromAbgr((int)color);
    }

    public static SilkColor FromHsv(Vector4 hsv)
    {
        float num = hsv.X * 360f;
        float y = hsv.Y;
        float z = hsv.Z;
        float num2 = z * y;
        float num3 = num / 60f;
        float num4 = num2 * (1f - Math.Abs(num3 % 2f - 1f));
        float num5;
        float num6;
        float num7;
        if (num3 >= 0f && num3 < 1f)
        {
            num5 = num2;
            num6 = num4;
            num7 = 0f;
        }
        else if (num3 >= 1f && num3 < 2f)
        {
            num5 = num4;
            num6 = num2;
            num7 = 0f;
        }
        else if (num3 >= 2f && num3 < 3f)
        {
            num5 = 0f;
            num6 = num2;
            num7 = num4;
        }
        else if (num3 >= 3f && num3 < 4f)
        {
            num5 = 0f;
            num6 = num4;
            num7 = num2;
        }
        else if (num3 >= 4f && num3 < 5f)
        {
            num5 = num4;
            num6 = 0f;
            num7 = num2;
        }
        else if (num3 >= 5f && num3 < 6f)
        {
            num5 = num2;
            num6 = 0f;
            num7 = num4;
        }
        else
        {
            num5 = 0f;
            num6 = 0f;
            num7 = 0f;
        }

        float num8 = z - num2;
        return new SilkColor(hsv.W, num5 + num8, num6 + num8, num7 + num8);
    }

    public static DrawingColor ByDrawingColor(SilkColor color)
    {
        return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
    }

    private static byte ToByte(float component)
    {
        return ToByte((int)(component * 255f));
    }
    public static byte ToByte(int value)
    {
        return (byte)((value >= 0) ? ((value > 255) ? 255u : ((uint)value)) : 0u);
    }
}
