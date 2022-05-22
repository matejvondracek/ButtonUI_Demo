using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonUI;

namespace ButtonUI_Use_Example
{
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        readonly UIGroup group = new UIGroup();
        readonly int windowWidth = 800, windowHeight = 500;
        Button button;
        SpriteFont font;
        Color background = Color.CornflowerBlue;
        int count = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
           
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            renderTarget = new RenderTarget2D(GraphicsDevice, 800, 500);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Arial");
            
            Texture2D[] textures = new Texture2D[2];
            textures[0] = Content.Load<Texture2D>("button1");
            textures[1] = Content.Load<Texture2D>("button2");

            Texture2D[] sliderTextures = new Texture2D[3];
            sliderTextures[0] = Content.Load<Texture2D>("slider_slot");
            sliderTextures[1] = Content.Load<Texture2D>("slider_selected");
            sliderTextures[2] = Content.Load<Texture2D>("slider_line");

            //button
            button = new Button(new Rectangle(50, 100, 200, 100), textures);
            button.AddText("button", font, 10, Color.Red);
            group.Add(button);

            //label
            group.Add(new Label(new Rectangle(200, 0, 400, 100), "DEMO", font, Color.White));
            group.Add(new Label(new Rectangle(50, 50, 150, 50), "Clicked 0 times", font, Color.White), "counter");
            group.Add(new Label(new Rectangle(500, 330, 150, 30), "Value: ", font, Color.White), "value");

            //textBox
            TextBox textBox = new TextBox(new Rectangle(50, 210, 200, 100), textures, font, 10, Color.Green)
            {
                text = "WRITE IN ME"
            };
            group.Add(textBox);

            //valueBox
            group.Add(new ValueBox(new Rectangle(300, 100, 200, 100), 1 / 5f, 10, 0, 10, font, Color.Blue, textures));

            //toggleButton
            ToggleButton toggleButton = new ToggleButton(new Rectangle(300, 210, 200, 100), textures, true);
            toggleButton.DefineText("ON", "OFF", font, 10, Color.Magenta);
            group.Add(toggleButton, "toggleB");

            //slider
            group.Add(new Slider(new Rectangle(50, 330, 450, 30), 0, 100, 21, sliderTextures), "slider");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
            group.Update(mouse, keyboard);

            if (button.IsPressed())
            {
                count++;
            }

            Label label1 = group.GetObject<Label>("counter");
            label1.text = "Clicked " + count + " times.";
            base.Update(gameTime);

            ToggleButton toggleButton = group.GetObject<ToggleButton>("toggleB");
            if (toggleButton.on)
            {
                background = Color.Red;
            }
            else background = Color.Blue;

            Label label2 = group.GetObject<Label>("value");
            Slider slider = group.GetObject<Slider>("slider");
            label2.text = "Value: " + slider.value;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(background);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            group.Draw(spriteBatch);

            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
