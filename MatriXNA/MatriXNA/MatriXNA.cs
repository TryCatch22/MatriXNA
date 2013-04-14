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

namespace Matrix
{
	public class Matrix : Microsoft.Xna.Framework.Game
	{
		public const int Fps = 60;

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
		public static readonly Random Random = new Random();

		public static SpriteFont Font;

		private List<Trail> trails;
		private int currentColorIndex;

		private KeyboardState keyboardState, prevKeyboardState;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		public Matrix()
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

			currentColorIndex = 0;
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

			foreach (Trail trail in trails)
				trail.Update(gameTime);

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
				trails.Add(new Trail(i, AllowedColors[currentColorIndex]));
		}

		private void ApplyNewColor()
		{
			currentColorIndex = mod(currentColorIndex, AllowedColors.Count);
			foreach (Trail trail in trails)
				trail.CurrentColor = AllowedColors[currentColorIndex];
		}

		private int mod(int x, int m)
		{
			return (x % m + m) % m;
		}
	}
}
