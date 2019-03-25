﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoBrJozik.Controls;
using System.Globalization;
using System.Threading;
using Point = Engine.Point;
using System.IO;
using System.Linq;

namespace MonoBrJozik
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private Engine.Game _game;
        private MonoDrawer _drawer;
        private MonoMenu _menu;
        private MonoInventory _inventory;
        private MonoSwitch _pauseSwitch;
        private MonoSwitch _knowledgeSwitch;
        private MonoKnowledges _monoKnowledges;
        
        private bool _lButtonPressed = false;
        private bool _rButtonPressed = false;

        private readonly List<MonoControl> _monoControls = new List<MonoControl>();

        public Game1()
        {
            CultureInfo newCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var textures = LoadTextures();
            textures[0x00020000] = Content.Load<Texture2D>("fox anim new"); ;
            var heroTexture = Content.Load<Texture2D>("hero");
            var screenTexture = Content.Load<Texture2D>("green-paper2");
            var heroPropTextures = LoadHeroTextures();
            var font = Content.Load<SpriteFont>("Font");

            var menuTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c = new Color[1];
            c[0] = Color.White;
            menuTexture.SetData(c);

            _menu = new MonoMenu(font, Color.MintCream, MonoDrawer.ScreenWidth,
                MonoDrawer.ScreenHeight + MonoDrawer.HealthBarHeight);
            _inventory = new MonoInventory(MonoDrawer.ScreenWidth, 0, MonoDrawer.InventoryWidth,
                MonoDrawer.ScreenHeight + MonoDrawer.HealthBarHeight, font, Color.Black, menuTexture);

            
            var pauseStrLength = font.MeasureString("Pause");
            var switchTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] c1 = new Color[1];
            c[0] = Color.WhiteSmoke;
            switchTexture.SetData(c);
            var pauseInfo = new MonoItemInfo(switchTexture, null, "Pause", () => _game.SetPaused());
            _pauseSwitch = new MonoSwitch(pauseInfo, false, switchTexture, font, Color.Black, MonoDrawer.ScreenWidth - (int)pauseStrLength.X - 2, MonoDrawer.ScreenHeight);

            var knowledgesStrLength = font.MeasureString("Knowledges");
            var knowledgesInfo = new MonoItemInfo(switchTexture, null, "Knowledges", () => _game.ShowKnowledges());
            _knowledgeSwitch = new MonoSwitch(knowledgesInfo, false, switchTexture, font, Color.Black, MonoDrawer.ScreenWidth - (int)pauseStrLength.X - (int)knowledgesStrLength.X - 10, MonoDrawer.ScreenHeight);

            _monoKnowledges = new MonoKnowledges(GraphicsDevice, font);

            _drawer = new MonoDrawer(_spriteBatch, GraphicsDevice, textures, heroTexture,
                heroPropTextures, font, _menu, _inventory, _pauseSwitch, _knowledgeSwitch, _monoKnowledges);
            _game = new Engine.Game(_drawer, (uint) MonoDrawer.ScreenWidth, (uint) MonoDrawer.ScreenHeight);

            _graphics.PreferredBackBufferWidth =
                MonoDrawer.ScreenWidth +
                MonoDrawer.InventoryWidth; // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight =
                MonoDrawer.ScreenHeight +
                MonoDrawer.HealthBarHeight; // set this value to the desired height of your window
            _graphics.ApplyChanges();

            _monoControls.Add(_menu);
            _monoControls.Add(_inventory);
            _monoControls.Add(_pauseSwitch);
            _monoControls.Add(_knowledgeSwitch);
        }

        private Dictionary<uint, Texture2D> LoadTextures()
        {
            var textureDict = new Dictionary<uint, Texture2D>();

            foreach (string str in Directory.GetFiles(@"Content", "*.png", SearchOption.TopDirectoryOnly))
            {
                var textureName = Path.GetFileNameWithoutExtension(str);
                
                if (uint.TryParse(textureName, NumberStyles.AllowHexSpecifier, new NumberFormatInfo(), out var n))
                {
                    using (FileStream fs = File.OpenRead(str))
                    {
                        var t = Texture2D.FromStream(GraphicsDevice, fs);
                        textureDict[n] = t;
                    }
                }
            }

            return textureDict;
        }

/*
    private Dictionary<uint, Texture2D> LoadTextures()
        {
            var textureDict =
                new Dictionary<uint, Texture2D>
                {
                //    [0x00000100] = Content.Load<Texture2D>("apple tree icon_w"),
                 //   [0x00000200] = Content.Load<Texture2D>("apple-tree1 icon_w"),
                 //   [0x00000300] = Content.Load<Texture2D>("apple-tree2 icon_w"),
                //    [0x00001100] = Content.Load<Texture2D>("plant icon"),
                //    [0x20001100] = Content.Load<Texture2D>("dry plant icon"),
                  //  [0x10001100] = Content.Load<Texture2D>("growing plant icon"),
                   // [0x00001000] = Content.Load<Texture2D>("rock icon2"),
                  //  [0x00000600] = Content.Load<Texture2D>("fire icon"),
                 //   [0x00000700] = Content.Load<Texture2D>("apple icon"),
                  //  [0x00000800] = Content.Load<Texture2D>("branch icon"),
                   // [0x00001200] = Content.Load<Texture2D>("brush icon"),
                  //  [0x00000900] = Content.Load<Texture2D>("Raspberry icon"),
                 //   [0x00001300] = Content.Load<Texture2D>("Stone axe icon"),
                   // [0x00001400] = Content.Load<Texture2D>("Log icon"),
                   // [0x00001500] = Content.Load<Texture2D>("attenuating fire small"),
                 //   [0x00001600] = Content.Load<Texture2D>("spruce tree5 small"),//("finetree"),//("spruce tree_w"),
                  //  [0x00001700] = Content.Load<Texture2D>("cone small"),
                  //  [0x00018000] = Content.Load<Texture2D>("dikabroyozik small"),
                 //   [0x10018000] = Content.Load<Texture2D>("dikabroyozik with bundle small"),
                 //   [0x00001900] = Content.Load<Texture2D>("mushroom small"),
                 //   [0x10001900] = Content.Load<Texture2D>("mushroom growing small"),
                 //   [0x00001A00] = Content.Load<Texture2D>("roasted mushroom small"),
                 //   [0x00001B00] = Content.Load<Texture2D>("roasted apple icon"),
                //    [0x00001C00] = Content.Load<Texture2D>("twig icon"),
                 //   [0x00001D00] = Content.Load<Texture2D>("grassbed0"),
                 //   [0x00001D01] = Content.Load<Texture2D>("grassbed1"),
                 //   [0x00001D02] = Content.Load<Texture2D>("grassbed2"),
                 //   [0x00001D03] = Content.Load<Texture2D>("grassbed3"),
                    //[0x00002300] = Content.Load<Texture2D>("digging stick icon"),
                 //   [0x00002200] = Content.Load<Texture2D>("sharp stone icon"),
                 //   [0x00002400] = Content.Load<Texture2D>("root icon"),
                 //   [0x00002500] = Content.Load<Texture2D>("nut tree icon_w"),
                 //   [0x00002600] = Content.Load<Texture2D>("nut"),
                 //   [0x00001E00] = Content.Load<Texture2D>("Wickiup0 icon"),
                 //   [0x00001E01] = Content.Load<Texture2D>("Wickiup1 icon"),
                 //   [0x00001E02] = Content.Load<Texture2D>("Wickiup2 icon"),
                 //   [0x00001E03] = Content.Load<Texture2D>("Wickiup3 icon"),
                 //   [0x00001E04] = Content.Load<Texture2D>("Wickiup4 icon"),
                 //   [0x00001E05] = Content.Load<Texture2D>("Wickiup5 icon"),
                 //   [0x00001E06] = Content.Load<Texture2D>("Wickiup6 icon"),
                 //   [0x00002100] = Content.Load<Texture2D>("blue"),
                  //  [0x00002000] = Content.Load<Texture2D>("darkblue"),
                 //   [0x00002700] = Content.Load<Texture2D>("muhomor"),
                 //   [0x10002700] = Content.Load<Texture2D>("muhomor small"),
                  //  [0x00002800] = Content.Load<Texture2D>("poganka"),
                 //   [0x10002800] = Content.Load<Texture2D>("poganka small"),
                };

            return textureDict;
        }
        */
        private Dictionary<string, Texture2D> LoadHeroTextures()
        {
            var textureDict =
                new Dictionary<string, Texture2D>
                {
                    ["Health"] = Content.Load<Texture2D>("heart small"),
                    ["Tiredness"] = Content.Load<Texture2D>("tired small")
                };

            return textureDict;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var currentMouseState = Mouse.GetState();
            if (currentMouseState.LeftButton == ButtonState.Pressed && !_monoKnowledges.LButtonDown(currentMouseState.X, currentMouseState.Y))
            {
                _lButtonPressed = true;
            }
            else if (_lButtonPressed)
            {
                _lButtonPressed = false;

                if (!_monoControls.Any(ctrl => ctrl.MouseLClick(currentMouseState)) && !_monoKnowledges.MouseLClick(currentMouseState))
                {
                    _game.LClick(new Point(currentMouseState.X, currentMouseState.Y));
                }

                /*if (!_menu.MouseLClick(currentMouseState))
                {
                    if (!_inventory.MouseLClick(currentMouseState) && !_pauseSwitch.MouseLClick(currentMouseState) && !_knowledgeSwitch.MouseLClick(currentMouseState))
                        _game.LClick(new Point(currentMouseState.X, currentMouseState.Y));
                }*/
            }
            else if (currentMouseState.LeftButton == ButtonState.Released)
            {
                _monoKnowledges.LButtonUp(currentMouseState.X, currentMouseState.Y);
            }

            if (currentMouseState.RightButton == ButtonState.Pressed)
            {
                _rButtonPressed = true;
            }
            else if (_rButtonPressed)
            {
                _rButtonPressed = false;
                if (!_monoControls.Any(ctrl => ctrl.MouseRClick(currentMouseState)))
                {
                    _game.RClick(new Point(currentMouseState.X, currentMouseState.Y));
                }

              /*  if (!_menu.MouseRClick(currentMouseState))
                {
                    if (!_inventory.MouseRClick(currentMouseState) && !_pauseSwitch.MouseRClick(currentMouseState) && !_knowledgeSwitch.MouseRClick(currentMouseState))
                        _game.RClick(new Point(currentMouseState.X, currentMouseState.Y));
                }*/
            }

            if (!_monoKnowledges.MouseMove(currentMouseState.X, currentMouseState.Y) && !_menu.MouseOver(currentMouseState))
            {
                _inventory.MouseOver(currentMouseState);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray); //new Color(0, 80, 0));
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            _game.DrawChanges();

            _monoControls.ForEach(ctrl => ctrl.Draw(_spriteBatch));
/*
            _menu.Draw(_spriteBatch);

            _inventory.Draw(_spriteBatch);

            _pauseSwitch.Draw(_spriteBatch);

            _knowledgeSwitch.Draw(_spriteBatch);*/

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}