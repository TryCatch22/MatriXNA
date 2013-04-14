using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Matrix
{
	class Trail
	{
		public Color CurrentColor
		{
			get { return currentColor; }
			set
			{
				currentColor = value;
				foreach (Character character in Characters)
					character.CurrentColor = value;
			}
		}
		private Color currentColor;

		public Vector2 Position { get; private set; }
		public List<Character> Characters { get; private set; }

		private const float MovePeriod = 0.0333f;
		private const float OpacityThreshold = 0.01f;

		private TimeSpan moveTimer;
		private int endHeight;

		public Trail(int positionX, Color color)
		{
			Characters = new List<Character>();
			moveTimer = TimeSpan.FromSeconds(0.0f);

			Reset(positionX);
			CurrentColor = color;
		}

		public void Update(GameTime gameTime)
		{
			foreach (Character character in Characters)
				character.Update();

			Characters = Characters.Where(x => (x.Opacity > OpacityThreshold)).ToList();

			moveTimer += gameTime.ElapsedGameTime;

			if (moveTimer.TotalSeconds > MovePeriod)
			{
				Characters.Add(new Character(Position, CurrentColor));
				Position += new Vector2(0, Character.Height);

				moveTimer = TimeSpan.FromSeconds(0.0f);
			}

			if (Position.Y > endHeight)
				Reset((int)Position.X);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Character character in Characters)
				character.Draw(spriteBatch);
		}

		private void Reset(int positionX)
		{
			Position = new Vector2(positionX, 0);

			int maxOffsetAbove = (int)Matrix.ScreenSize.Y / 4;
			int maxOffsetBelow = (int)Matrix.ScreenSize.Y / 3;
			endHeight = Matrix.Random.Next((int)Matrix.ScreenSize.Y - maxOffsetAbove, (int)Matrix.ScreenSize.Y + maxOffsetBelow);
		}
	}
}
