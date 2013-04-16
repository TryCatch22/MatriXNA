using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MatriXNA
{
	class Util
	{
		public static readonly Random Random = new Random();

		public static Color HSVToColor(Vector3 hsv)
		{
			if (hsv.X == 0 && hsv.Y == 0)
				return new Color(hsv.Z, hsv.Z, hsv.Z);

			float c = hsv.Y * hsv.Z;
			float x = c * (1 - Math.Abs(hsv.X % 2 - 1));
			float m = hsv.Z - c;

			if (hsv.X < 1) return new Color(c + m, x + m, m);
			else if (hsv.X < 2) return new Color(x + m, c + m, m);
			else if (hsv.X < 3) return new Color(m, c + m, x + m);
			else if (hsv.X < 4) return new Color(m, x + m, c + m);
			else if (hsv.X < 5) return new Color(x + m, m, c + m);
			else return new Color(c + m, m, x + m);
		}

		public static Vector3 ColorToHSV(Color color)
		{
			Vector3 c = color.ToVector3();
			float v = Math.Max(c.X, Math.Max(c.Y, c.Z));
			float chroma = v - Math.Min(c.X, Math.Min(c.Y, c.Z));

			if (chroma == 0f)
				return new Vector3(0, 0, v);

			float s = chroma / v;

			if (c.X >= c.Y && c.Y >= c.Z)
			{
				float h = (c.Y - c.Z) / chroma;
				if (h < 0)
					h += 6;
				return new Vector3(h, s, v);
			}
			else if (c.Y >= c.Z && c.Y >= c.X)
				return new Vector3((c.Z - c.X) / chroma + 2, s, v);
			else
				return new Vector3((c.X - c.Y) / chroma + 4, s, v);

		}
	}
}
