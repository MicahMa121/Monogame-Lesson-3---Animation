using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Monogame_Lesson_3___Animation;

public class Game1 : Game
{
    // Micha

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D tribbleBrown, tribbleGrey, tribbleCream, tribbleOrange, forestBackground, quitTexture,tribbleIntroTexture;
    Rectangle tribbleBrownRect, tribbleGreyRect, tribbleCreamRect, tribbleOrangeRect, quitRect;
    Vector2 tribbleBrownSpeed, tribbleGreySpeed, tribbleCreamSpeed;
    MouseState mouseState, prevMouseState;
    Color quitColor;
    Random gen = new Random();
    SoundEffect tribbleCoo;
    enum Screen
    {
        Intro,
        TribbleYard
    }
    Screen screen;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();
        screen = Screen.Intro;
        tribbleBrownRect = new Rectangle(gen.Next(_graphics.PreferredBackBufferWidth),gen.Next(tribbleBrownRect.Height,(_graphics.PreferredBackBufferHeight-tribbleBrownRect.Height)),100,100);
        tribbleGreyRect = new Rectangle(gen.Next(tribbleGreyRect.Width,_graphics.PreferredBackBufferWidth-2*tribbleGreyRect.Width),gen.Next(_graphics.PreferredBackBufferHeight-2*tribbleGreyRect.Height),100,100);
        tribbleCreamRect = new Rectangle(gen.Next(tribbleCreamRect.Width,_graphics.PreferredBackBufferWidth-tribbleCreamRect.Width),100,50,50);
        tribbleOrangeRect = new Rectangle(300,10,100,100);
        tribbleBrownSpeed = new Vector2(2,0);
        tribbleGreySpeed = new Vector2(0,2);
        tribbleCreamSpeed = new Vector2(gen.Next(-2,2),gen.Next(-2,2));
        quitRect = new Rectangle(550,500, 200, 80);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        tribbleBrown = Content.Load<Texture2D>("tribbleBrown");
        tribbleGrey = Content.Load<Texture2D>("tribbleGrey");
        tribbleCream = Content.Load<Texture2D>("tribbleCream");
        tribbleOrange = Content.Load<Texture2D>("tribbleOrange");
        forestBackground = Content.Load<Texture2D>("forestBackground");
        quitTexture = Content.Load<Texture2D>("btnQuit");
        tribbleCoo = Content.Load<SoundEffect>("tribble_coo");
        tribbleIntroTexture = Content.Load<Texture2D>("tribbleIntro");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        prevMouseState = mouseState;
        mouseState = Mouse.GetState();
        if (screen == Screen.Intro)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
                screen = Screen.TribbleYard;

        }
        else if (screen == Screen.TribbleYard)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (quitRect.Contains(mouseState.Position))
                    Exit();
            }
            tribbleOrangeRect = new Rectangle(mouseState.Position.X,mouseState.Position.Y, 10,10);
            if (quitRect.Contains(mouseState.Position))
            {
                quitColor = Color.Green;
            }
            else
            {
                quitColor = Color.White;
            }
            tribbleCreamRect.X += (int)tribbleCreamSpeed.X;
            tribbleCreamRect.Y += (int)tribbleCreamSpeed.Y;
            tribbleCreamRect.Width += 1;
            tribbleCreamRect.Height += 1;
            tribbleGreyRect.X += (int)tribbleGreySpeed.X;
            tribbleGreyRect.Y += (int)tribbleGreySpeed.Y;
            tribbleBrownRect.X += (int)tribbleBrownSpeed.X;
            tribbleBrownRect.Y = tribbleBrownRect.Y + (int)tribbleBrownSpeed.Y;
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (tribbleBrownRect.Contains(mouseState.Position))
                {
                    tribbleBrownSpeed = new Vector2(0, 0);
                    tribbleBrownRect = new Rectangle(-10000, -10000, 0, 0);
                }

            }
            else if (tribbleBrownRect.Left>=_graphics.PreferredBackBufferWidth)
            {
                tribbleCoo.Play();
                tribbleBrownRect.X = 0-tribbleBrownRect.Width;
                tribbleBrownRect.Y = gen.Next(tribbleBrownRect.Height,(_graphics.PreferredBackBufferHeight-tribbleBrownRect.Height));
            }
            if ((tribbleGreyRect.Bottom>=_graphics.PreferredBackBufferHeight)||(tribbleGreyRect.Top<=0))
            {
                tribbleGreySpeed.Y *= -1;
                tribbleCoo.Play();
            }
            if ((tribbleCreamRect.Bottom<=0)||(tribbleCreamRect.Top>=_graphics.PreferredBackBufferHeight)||(tribbleCreamRect.Left>=_graphics.PreferredBackBufferWidth)||(tribbleCreamRect.Right<=0)||(tribbleCreamRect.Width>=300))
            {
                tribbleCoo.Play();
                tribbleCreamSpeed = new Vector2(gen.Next(-2,2),gen.Next(-2,2));//gen.Next(-1,2),gen.Next(-1,2)
                tribbleCreamRect = new Rectangle(0,0,50,50);
                tribbleCreamRect = new Rectangle(gen.Next(tribbleCreamRect.Width,_graphics.PreferredBackBufferWidth-tribbleCreamRect.Width),gen.Next(tribbleCreamRect.Height,(_graphics.PreferredBackBufferHeight-tribbleCreamRect.Height)),50,50);
            }
        }
        
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        if (screen == Screen.Intro)
        {
            _spriteBatch.Draw(tribbleIntroTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) , Color.White);
        }
        else if (screen == Screen.TribbleYard)
        {
            _spriteBatch.Draw(forestBackground, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.Draw(tribbleBrown, tribbleBrownRect, Color.White);
            _spriteBatch.Draw(tribbleGrey, tribbleGreyRect, Color.White);
            _spriteBatch.Draw(tribbleCream, tribbleCreamRect, Color.White);
            _spriteBatch.Draw(quitTexture, quitRect, quitColor);
            _spriteBatch.Draw(tribbleOrange, tribbleOrangeRect, Color.White);
        }
         
        _spriteBatch.End();

        base.Draw(gameTime);
        
    }
}
