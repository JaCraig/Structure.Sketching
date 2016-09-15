using System;

namespace Structure.Sketching.Colors
{
    /// <summary>
    /// Color definitions
    /// </summary>
    /// <seealso cref="IEquatable{Color}" />
    public partial struct Color : IEquatable<Color>
    {
        /// <summary>
        /// The alice blue
        /// </summary>
        public static Color AliceBlue = "#F0F8FF";

        /// <summary>
        /// The antique white
        /// </summary>
        public static Color AntiqueWhite = "#FAEBD7";

        /// <summary>
        /// The aqua
        /// </summary>
        public static Color Aqua = "#00FFFF";

        /// <summary>
        /// The aquamarine
        /// </summary>
        public static Color Aquamarine = "#7FFFD4";

        /// <summary>
        /// The azure
        /// </summary>
        public static Color Azure = "#F0FFFF";

        /// <summary>
        /// The beige
        /// </summary>
        public static Color Beige = "#F5F5DC";

        /// <summary>
        /// The bisque
        /// </summary>
        public static Color Bisque = "#FFE4C4";

        /// <summary>
        /// The black
        /// </summary>
        public static Color Black = "#000000";

        /// <summary>
        /// The blanched almond
        /// </summary>
        public static Color BlanchedAlmond = "#FFEBCD";

        /// <summary>
        /// The blue color
        /// </summary>
        public static Color BlueColor = "#0000FF";

        /// <summary>
        /// The blue violet
        /// </summary>
        public static Color BlueViolet = "#8A2BE2";

        /// <summary>
        /// The brown
        /// </summary>
        public static Color Brown = "#A52A2A";

        /// <summary>
        /// The burly wood
        /// </summary>
        public static Color BurlyWood = "#DEB887";

        /// <summary>
        /// The cadet blue
        /// </summary>
        public static Color CadetBlue = "#5F9EA0";

        /// <summary>
        /// The chartreuse
        /// </summary>
        public static Color Chartreuse = "#7FFF00";

        /// <summary>
        /// The chocolate
        /// </summary>
        public static Color Chocolate = "#D2691E";

        /// <summary>
        /// The coral
        /// </summary>
        public static Color Coral = "#FF7F50";

        /// <summary>
        /// The cornflower blue
        /// </summary>
        public static Color CornflowerBlue = "#6495ED";

        /// <summary>
        /// The cornsilk
        /// </summary>
        public static Color Cornsilk = "#FFF8DC";

        /// <summary>
        /// The crimson
        /// </summary>
        public static Color Crimson = "#DC143C";

        /// <summary>
        /// The cyan
        /// </summary>
        public static Color Cyan = "#00FFFF";

        /// <summary>
        /// The dark blue
        /// </summary>
        public static Color DarkBlue = "#00008B";

        /// <summary>
        /// The dark cyan
        /// </summary>
        public static Color DarkCyan = "#008B8B";

        /// <summary>
        /// The dark golden rod
        /// </summary>
        public static Color DarkGoldenRod = "#B8860B";

        /// <summary>
        /// The dark green
        /// </summary>
        public static Color DarkGreen = "#006400";

        /// <summary>
        /// The dark grey
        /// </summary>
        public static Color DarkGrey = "#A9A9A9";

        /// <summary>
        /// The dark khaki
        /// </summary>
        public static Color DarkKhaki = "#BDB76B";

        /// <summary>
        /// The dark magenta
        /// </summary>
        public static Color DarkMagenta = "#8B008B";

        /// <summary>
        /// The dark olive green
        /// </summary>
        public static Color DarkOliveGreen = "#556B2F";

        /// <summary>
        /// The dark orange
        /// </summary>
        public static Color DarkOrange = "#FF8C00";

        /// <summary>
        /// The dark orchid
        /// </summary>
        public static Color DarkOrchid = "#9932CC";

        /// <summary>
        /// The dark red
        /// </summary>
        public static Color DarkRed = "#8B0000";

        /// <summary>
        /// The dark salmon
        /// </summary>
        public static Color DarkSalmon = "#E9967A";

        /// <summary>
        /// The dark sea green
        /// </summary>
        public static Color DarkSeaGreen = "#8FBC8F";

        /// <summary>
        /// The dark slate blue
        /// </summary>
        public static Color DarkSlateBlue = "#483D8B";

        /// <summary>
        /// The dark slate grey
        /// </summary>
        public static Color DarkSlateGrey = "#2F4F4F";

        /// <summary>
        /// The dark turquoise
        /// </summary>
        public static Color DarkTurquoise = "#00CED1";

        /// <summary>
        /// The dark violet
        /// </summary>
        public static Color DarkViolet = "#9400D3";

        /// <summary>
        /// The deep pink
        /// </summary>
        public static Color DeepPink = "#FF1493";

        /// <summary>
        /// The deep sky blue
        /// </summary>
        public static Color DeepSkyBlue = "#00BFFF";

        /// <summary>
        /// The dim grey
        /// </summary>
        public static Color DimGrey = "#696969";

        /// <summary>
        /// The dodger blue
        /// </summary>
        public static Color DodgerBlue = "#1E90FF";

        /// <summary>
        /// The fire brick
        /// </summary>
        public static Color FireBrick = "#B22222";

        /// <summary>
        /// The floral white
        /// </summary>
        public static Color FloralWhite = "#FFFAF0";

        /// <summary>
        /// The forest green
        /// </summary>
        public static Color ForestGreen = "#228B22";

        /// <summary>
        /// The fuchsia
        /// </summary>
        public static Color Fuchsia = "#FF00FF";

        /// <summary>
        /// The gainsboro
        /// </summary>
        public static Color Gainsboro = "#DCDCDC";

        /// <summary>
        /// The ghost white
        /// </summary>
        public static Color GhostWhite = "#F8F8FF";

        /// <summary>
        /// The gold
        /// </summary>
        public static Color Gold = "#FFD700";

        /// <summary>
        /// The golden rod
        /// </summary>
        public static Color GoldenRod = "#DAA520";

        /// <summary>
        /// The green color
        /// </summary>
        public static Color GreenColor = "#008000";

        /// <summary>
        /// The green yellow
        /// </summary>
        public static Color GreenYellow = "#ADFF2F";

        /// <summary>
        /// The grey
        /// </summary>
        public static Color Grey = "#808080";

        /// <summary>
        /// The honey dew
        /// </summary>
        public static Color HoneyDew = "#F0FFF0";

        /// <summary>
        /// The hot pink
        /// </summary>
        public static Color HotPink = "#FF69B4";

        /// <summary>
        /// The indian red
        /// </summary>
        public static Color IndianRed = "#CD5C5C";

        /// <summary>
        /// The indigo
        /// </summary>
        public static Color Indigo = "#4B0082";

        /// <summary>
        /// The ivory
        /// </summary>
        public static Color Ivory = "#FFFFF0";

        /// <summary>
        /// The khaki
        /// </summary>
        public static Color Khaki = "#F0E68C";

        /// <summary>
        /// The lavender
        /// </summary>
        public static Color Lavender = "#E6E6FA";

        /// <summary>
        /// The lavender blush
        /// </summary>
        public static Color LavenderBlush = "#FFF0F5";

        /// <summary>
        /// The lawn green
        /// </summary>
        public static Color LawnGreen = "#7CFC00";

        /// <summary>
        /// The lemon chiffon
        /// </summary>
        public static Color LemonChiffon = "#FFFACD";

        /// <summary>
        /// The light blue
        /// </summary>
        public static Color LightBlue = "#ADD8E6";

        /// <summary>
        /// The light coral
        /// </summary>
        public static Color LightCoral = "#F08080";

        /// <summary>
        /// The light cyan
        /// </summary>
        public static Color LightCyan = "#E0FFFF";

        /// <summary>
        /// The light golden rod yellow
        /// </summary>
        public static Color LightGoldenRodYellow = "#FAFAD2";

        /// <summary>
        /// The light green
        /// </summary>
        public static Color LightGreen = "#90EE90";

        /// <summary>
        /// The light grey
        /// </summary>
        public static Color LightGrey = "#D3D3D3";

        /// <summary>
        /// The light pink
        /// </summary>
        public static Color LightPink = "#FFB6C1";

        /// <summary>
        /// The light salmon
        /// </summary>
        public static Color LightSalmon = "#FFA07A";

        /// <summary>
        /// The light sea green
        /// </summary>
        public static Color LightSeaGreen = "#20B2AA";

        /// <summary>
        /// The light sky blue
        /// </summary>
        public static Color LightSkyBlue = "#87CEFA";

        /// <summary>
        /// The light slate grey
        /// </summary>
        public static Color LightSlateGrey = "#778899";

        /// <summary>
        /// The light steel blue
        /// </summary>
        public static Color LightSteelBlue = "#B0C4DE";

        /// <summary>
        /// The light yellow
        /// </summary>
        public static Color LightYellow = "#FFFFE0";

        /// <summary>
        /// The lime
        /// </summary>
        public static Color Lime = "#00FF00";

        /// <summary>
        /// The lime green
        /// </summary>
        public static Color LimeGreen = "#32CD32";

        /// <summary>
        /// The linen
        /// </summary>
        public static Color Linen = "#FAF0E6";

        /// <summary>
        /// The magenta
        /// </summary>
        public static Color Magenta = "#FF00FF";

        /// <summary>
        /// The maroon
        /// </summary>
        public static Color Maroon = "#800000";

        /// <summary>
        /// The medium aqua marine
        /// </summary>
        public static Color MediumAquaMarine = "#66CDAA";

        /// <summary>
        /// The medium blue
        /// </summary>
        public static Color MediumBlue = "#0000CD";

        /// <summary>
        /// The medium orchid
        /// </summary>
        public static Color MediumOrchid = "#BA55D3";

        /// <summary>
        /// The medium purple
        /// </summary>
        public static Color MediumPurple = "#9370D8";

        /// <summary>
        /// The medium sea green
        /// </summary>
        public static Color MediumSeaGreen = "#3CB371";

        /// <summary>
        /// The medium slate blue
        /// </summary>
        public static Color MediumSlateBlue = "#7B68EE";

        /// <summary>
        /// The medium spring green
        /// </summary>
        public static Color MediumSpringGreen = "#00FA9A";

        /// <summary>
        /// The medium turquoise
        /// </summary>
        public static Color MediumTurquoise = "#48D1CC";

        /// <summary>
        /// The medium violet red
        /// </summary>
        public static Color MediumVioletRed = "#C71585";

        /// <summary>
        /// The midnight blue
        /// </summary>
        public static Color MidnightBlue = "#191970";

        /// <summary>
        /// The mint cream
        /// </summary>
        public static Color MintCream = "#F5FFFA";

        /// <summary>
        /// The misty rose
        /// </summary>
        public static Color MistyRose = "#FFE4E1";

        /// <summary>
        /// The moccasin
        /// </summary>
        public static Color Moccasin = "#FFE4B5";

        /// <summary>
        /// The navajo white
        /// </summary>
        public static Color NavajoWhite = "#FFDEAD";

        /// <summary>
        /// The navy
        /// </summary>
        public static Color Navy = "#000080";

        /// <summary>
        /// The old lace
        /// </summary>
        public static Color OldLace = "#FDF5E6";

        /// <summary>
        /// The olive
        /// </summary>
        public static Color Olive = "#808000";

        /// <summary>
        /// The olive drab
        /// </summary>
        public static Color OliveDrab = "#6B8E23";

        /// <summary>
        /// The orange
        /// </summary>
        public static Color Orange = "#FFA500";

        /// <summary>
        /// The orange red
        /// </summary>
        public static Color OrangeRed = "#FF4500";

        /// <summary>
        /// The orchid
        /// </summary>
        public static Color Orchid = "#DA70D6";

        /// <summary>
        /// The pale golden rod
        /// </summary>
        public static Color PaleGoldenRod = "#EEE8AA";

        /// <summary>
        /// The pale green
        /// </summary>
        public static Color PaleGreen = "#98FB98";

        /// <summary>
        /// The pale turquoise
        /// </summary>
        public static Color PaleTurquoise = "#AFEEEE";

        /// <summary>
        /// The pale violet red
        /// </summary>
        public static Color PaleVioletRed = "#D87093";

        /// <summary>
        /// The papaya whip
        /// </summary>
        public static Color PapayaWhip = "#FFEFD5";

        /// <summary>
        /// The peach puff
        /// </summary>
        public static Color PeachPuff = "#FFDAB9";

        /// <summary>
        /// The peru
        /// </summary>
        public static Color Peru = "#CD853F";

        /// <summary>
        /// The pink
        /// </summary>
        public static Color Pink = "#FFC0CB";

        /// <summary>
        /// The plum
        /// </summary>
        public static Color Plum = "#DDA0DD";

        /// <summary>
        /// The powder blue
        /// </summary>
        public static Color PowderBlue = "#B0E0E6";

        /// <summary>
        /// The purple
        /// </summary>
        public static Color Purple = "#800080";

        /// <summary>
        /// The red color
        /// </summary>
        public static Color RedColor = "#FF0000";

        /// <summary>
        /// The rosy brown
        /// </summary>
        public static Color RosyBrown = "#BC8F8F";

        /// <summary>
        /// The royal blue
        /// </summary>
        public static Color RoyalBlue = "#4169E1";

        /// <summary>
        /// The saddle brown
        /// </summary>
        public static Color SaddleBrown = "#8B4513";

        /// <summary>
        /// The salmon
        /// </summary>
        public static Color Salmon = "#FA8072";

        /// <summary>
        /// The sandy brown
        /// </summary>
        public static Color SandyBrown = "#F4A460";

        /// <summary>
        /// The sea green
        /// </summary>
        public static Color SeaGreen = "#2E8B57";

        /// <summary>
        /// The sea shell
        /// </summary>
        public static Color SeaShell = "#FFF5EE";

        /// <summary>
        /// The sienna
        /// </summary>
        public static Color Sienna = "#A0522D";

        /// <summary>
        /// The silver
        /// </summary>
        public static Color Silver = "#C0C0C0";

        /// <summary>
        /// The sky blue
        /// </summary>
        public static Color SkyBlue = "#87CEEB";

        /// <summary>
        /// The slate blue
        /// </summary>
        public static Color SlateBlue = "#6A5ACD";

        /// <summary>
        /// The slate grey
        /// </summary>
        public static Color SlateGrey = "#708090";

        /// <summary>
        /// The snow
        /// </summary>
        public static Color Snow = "#FFFAFA";

        /// <summary>
        /// The spring green
        /// </summary>
        public static Color SpringGreen = "#00FF7F";

        /// <summary>
        /// The steel blue
        /// </summary>
        public static Color SteelBlue = "#4682B4";

        /// <summary>
        /// The tan
        /// </summary>
        public static Color Tan = "#D2B48C";

        /// <summary>
        /// The teal
        /// </summary>
        public static Color Teal = "#008080";

        /// <summary>
        /// The thistle
        /// </summary>
        public static Color Thistle = "#D8BFD8";

        /// <summary>
        /// The tomato
        /// </summary>
        public static Color Tomato = "#FF6347";

        /// <summary>
        /// The turquoise
        /// </summary>
        public static Color Turquoise = "#40E0D0";

        /// <summary>
        /// The violet
        /// </summary>
        public static Color Violet = "#EE82EE";

        /// <summary>
        /// The wheat
        /// </summary>
        public static Color Wheat = "#F5DEB3";

        /// <summary>
        /// The white
        /// </summary>
        public static Color White = "#FFFFFF";

        /// <summary>
        /// The white smoke
        /// </summary>
        public static Color WhiteSmoke = "#F5F5F5";

        /// <summary>
        /// The yellow
        /// </summary>
        public static Color Yellow = "#FFFF00";

        /// <summary>
        /// The yellow green
        /// </summary>
        public static Color YellowGreen = "#9ACD32";
    }
}