using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoBrJozik
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Engine.Game _game;
        private MonoDrawer _drawer;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 564;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 394;   // set this value to the desired height of your window
            graphics.ApplyChanges();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var texture = Content.Load<Texture2D>("brush icon");
            _drawer = new MonoDrawer(spriteBatch, texture);
            _game = new Engine.Game(_drawer, (uint)graphics.PreferredBackBufferWidth, (uint)graphics.PreferredBackBufferHeight);


            // TODO: use this.Content to load your game content here
        }

        private void LoadTextures()
        {
            var textureDict = new Dictionary<uint, Texture2D>();
            Content.Load<Texture2D>("apple tree icon");
   /*         _appletreeImage = CreateBitmapImage(@"apple tree icon.png");
            _appletree1Image = CreateBitmapImage(@"apple-tree1 icon.png");

            _appletree2Image = CreateBitmapImage(@"apple-tree2 icon.png");
            _plantImage = CreateBitmapImage(@"plant icon.png");
            _dryplantImage = CreateBitmapImage(@"dry plant icon.png");
            _growingPlantImage = CreateBitmapImage(@"growing plant icon.png");

            _rockImage = CreateBitmapImage(@"rock icon2.png");
            _fireImage = CreateBitmapImage(@"fire icon.png");
            _appleImage = CreateBitmapImage(@"apple icon.png");
            _branchImage = CreateBitmapImage(@"branch icon.png");
            _bushImage = CreateBitmapImage(@"brush icon.png");
            _raspberryImage = CreateBitmapImage(@"Raspberry icon.png");
            _stoneAxeImage = CreateBitmapImage(@"Stone axe icon.png");
            _logImage = CreateBitmapImage(@"Log icon.png");
            _attenuatingFireImage = CreateBitmapImage(@"attenuating fire small.png");
            _spruceTreeImage = CreateBitmapImage(@"spruce tree.png");
            _coneImage = CreateBitmapImage(@"cone small.png");

            _dikabrozikImage = CreateBitmapImage(@"dikabroyozik small.png");
            _dikabrozikWithBundleImage = CreateBitmapImage(@"dikabroyozik with bundle small.png");

            _mushroomImage = CreateBitmapImage(@"mushroom small.png");
            _growingMushroomImage = CreateBitmapImage(@"mushroom growing small.png");

            _roastedMushroomImage = CreateBitmapImage(@"roasted mushroom small.png");
            _roastedAppleImage = CreateBitmapImage(@"roasted apple icon.png");

            _twigImage = CreateBitmapImage(@"twig icon.png");

            _grassBed0 = CreateBitmapImage(@"grassbed0.png");
            _grassBed1 = CreateBitmapImage(@"grassbed1.png");
            _grassBed2 = CreateBitmapImage(@"grassbed2.png");
            _grassBed3 = CreateBitmapImage(@"grassbed3.png");

            _diggingStickImage = CreateBitmapImage(@"digging stick icon.png");
            _sharpStoneImage = CreateBitmapImage(@"sharp stone icon.png");

            _rootImage = CreateBitmapImage(@"root icon.png");

            _nutTreeImage = CreateBitmapImage(@"nut tree icon.png");
            _nutImage = CreateBitmapImage(@"nut.png");

            _wickiupImage0 = CreateBitmapImage(@"Wickiup0 icon.png");
            _wickiupImage1 = CreateBitmapImage(@"Wickiup1 icon.png");
            _wickiupImage2 = CreateBitmapImage(@"Wickiup2 icon.png");
            _wickiupImage3 = CreateBitmapImage(@"Wickiup3 icon.png");
            _wickiupImage4 = CreateBitmapImage(@"Wickiup4 icon.png");
            _wickiupImage5 = CreateBitmapImage(@"Wickiup5 icon.png");
            _wickiupImage6 = CreateBitmapImage(@"Wickiup6 icon.png");*/
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            _game.DrawChanges();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
