﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectMonoGame;

public class GameCycleView : Game, IGameplayView
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Dictionary<int, IEntity> _objects = new ();
    private readonly Dictionary<int, Texture2D> _textures = new ();
    private int _backgroundImageId;
    private int _mapWidth;
    private int _mapHeight;
    private SpriteFont _font;
    private int _playerId;
    public event EventHandler CycleFinished;
    public event EventHandler<ControlsEventArgs> PlayerMoved;
    public event EventHandler<TextureEventArgs> TexturesDownloaded;
    
    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        _mapWidth = GraphicsDevice.DisplayMode.Width;
        _mapHeight = GraphicsDevice.DisplayMode.Height;
        _graphics.PreferredBackBufferWidth = _mapWidth;
        _graphics.PreferredBackBufferHeight = _mapHeight;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _textures.Add(0, Content.Load<Texture2D>("space_ship"));
        _textures.Add(1, Content.Load<Texture2D>("asteroid_1"));
        _textures.Add(2, Content.Load<Texture2D>("asteroid_2"));
        _textures.Add(3, Content.Load<Texture2D>("asteroid_3"));
        _textures.Add(4, Content.Load<Texture2D>("space_cat"));
        _backgroundImageId = 5;
        _textures.Add(_backgroundImageId, Content.Load<Texture2D>("background"));
        _textures.Add(6, Content.Load<Texture2D>("heart"));
        _font = Content.Load<SpriteFont>("font");


        TexturesDownloaded?.Invoke(this, new TextureEventArgs(_textures));
    }

    public void LoadGameCycleParameters(Dictionary<int, IEntity> objects, int playerId)
    {
        _playerId = playerId;
        _objects = objects;        
    }

    protected override void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState().GetPressedKeys();
        var directions = new List<IGameplayModel.Direction>();
        foreach (var key in keys)
        { 
            switch (key)
            {
                case Keys.W:
                {
                    directions.Add(IGameplayModel.Direction.forward);
                    break;
                }
                case Keys.S:
                {
                    directions.Add(IGameplayModel.Direction.backward);
                    break;
                }
                case Keys.D:
                {
                    directions.Add(IGameplayModel.Direction.right);
                    break;
                }
                case Keys.A:
                {
                    directions.Add(IGameplayModel.Direction.left);
                    break;
                }
                case Keys.Escape:
                    Exit();
                    break;
            }
        }
        
        PlayerMoved?.Invoke(this, new ControlsEventArgs (directions));
        
        base.Update(gameTime);
        
        CycleFinished?.Invoke(this, EventArgs.Empty);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
        var player = (SpaceShip)_objects[_playerId];
        _spriteBatch.Begin();
        var destinationRectangle = new Rectangle(0, 0, _mapWidth, _mapHeight);
        _spriteBatch.Draw(_textures[_backgroundImageId], destinationRectangle, Color.White);

        foreach (var obj in _objects.Values)
        {
            _spriteBatch.Draw(_textures[obj.ImageId],  obj.Position, Color.White);
        }  	
        _spriteBatch.DrawString(_font, "Ебать метеориты держи жопу настороже",
            new Vector2((float)_graphics.PreferredBackBufferWidth/4 , (float)_graphics.PreferredBackBufferHeight / 16), Color.Ivory);
        _spriteBatch.DrawString(_font, $"x{player.CatCaught}",
            new Vector2(1325, 40), Color.Ivory);
        for (var i = 0; i < player.HealthPoints ; i++)
        {
            var currentX = 10 + 30 * i;
            _spriteBatch.Draw(_textures[6],  new Vector2(currentX, 40), Color.White);
        }
        _spriteBatch.End();  
    }
}