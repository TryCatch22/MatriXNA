using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MatriXNA
{
	class Character
	{
		public const int Width = 12;
		public const int Height = 12;

		public Color CurrentColor { get; set; }

		public char Value { get; private set; }
		public Vector2 Position { get; private set; }
		public float Opacity { get; private set; }

		private const float DecayConstant = 0.97f;

		public Character(Vector2 position, Color color)
		{
			Position = position;
			Opacity = 1.0f;

			Value = MatriXNA.Font.Characters[Util.Random.Next(MatriXNA.Font.Characters.Count)];
			CurrentColor = color;
		}

		public void Update()
		{
			Opacity *= DecayConstant;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(MatriXNA.Font, Value.ToString(), Position, CurrentColor * Opacity);
		}
	}
}
