using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MatriXNA
{
	public class MatriXNA : Microsoft.Xna.Framework.Game
	{
		public const int Fps = 60;
		public const float ColorRotationRate = 0.001f;

		public static readonly List<Color> AllowedColors = new List<Color>()
			{
				new Color(0, 255, 0),
				new Color(0, 255, 127),
				new Color(0, 255, 255),
				new Color(0, 127, 255),
				new Color(0, 0, 255),
				new Color(127, 0, 255),
				new Color(255, 0, 255),
				new Color(255, 0, 127),
				new Color(255, 0, 0),
				new Color(255, 127, 0),
				new Color(255, 255, 0),
				new Color(127, 255, 0),
				new Color(255, 255, 255),
			};

		public static readonly Vector2 ScreenSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

		public static SpriteFont Font;

		private List<Trail> trails;
		private Mode mode;
		private Vector3 currentColorHSV;
		private int currentColorIndex;

		private KeyboardState keyboardState, prevKeyboardState;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		public MatriXNA()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromSeconds(1.0f / Fps);

			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
			graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
			
			//IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();

			Font = Content.Load<SpriteFont>(@"Fonts\Font");

			mode = Mode.Static;
			currentColorIndex = 0;
			currentColorHSV = new Vector3(1.0f, 1.0f, 1.0f);
			ResetTrails();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}
		protected override void Update(GameTime gameTime)
		{
			keyboardState = Keyboard.GetState();

			if (keyboardState.IsKeyDown(Keys.Escape))
				this.Exit();

			if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
				ResetTrails();

			if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up))
			{
				mode = Mode.Rainbow;
				currentColorHSV = Util.ColorToHSV(trails[0].CurrentColor);
			}
			else if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down))
			{
				mode = Mode.Static;
			}

			if (mode == Mode.Static)
			{
				if (keyboardState.IsKeyDown(Keys.Right) && prevKeyboardState.IsKeyUp(Keys.Right))
				{
					currentColorIndex++;
					ApplyNewColor();
				}

				if (keyboardState.IsKeyDown(Keys.Left) && prevKeyboardState.IsKeyUp(Keys.Left))
				{
					currentColorIndex--;
					ApplyNewColor();
				}
			}

			foreach (Trail trail in trails)
				trail.Update(gameTime);

			if (mode == Mode.Rainbow)
			{
				currentColorHSV.X += ColorRotationRate;
				if (currentColorHSV.X > 6.0f)
					currentColorHSV.X = 0.0f;

				ApplyNewColor();
			}

			prevKeyboardState = keyboardState;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

			foreach (Trail trail in trails)
				trail.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}

		private void ResetTrails()
		{
			trails = new List<Trail>();

			for (int i = 0; i < ScreenSize.X; i += Character.Width)
				trails.Add(new Trail(i, GetCurrentColor()));
		}

		private void ApplyNewColor()
		{
			currentColorIndex = Modulo(currentColorIndex, AllowedColors.Count);

			foreach (Trail trail in trails)
				trail.CurrentColor = GetCurrentColor();
		}

		private Color GetCurrentColor()
		{
			Color color = new Color();

			if (mode == Mode.Static)
				color = AllowedColors[currentColorIndex];
			else if (mode == Mode.Rainbow)
				color = Util.HSVToColor(currentColorHSV);

			return color;
		}

		private int Modulo(int x, int m)
		{
			return (x % m + m) % m;
		}
	}
}
