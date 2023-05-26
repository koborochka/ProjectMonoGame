using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectMonoGame;

public class GameCycleView : Game, IGameplayView
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _playerImage;
    private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();
    private readonly Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();
    public event EventHandler CycleFinished;
    public event EventHandler<ControlsEventArgs> PlayerMoved;

    public int MapWidth { get; private set; }
    public int MapHeight { get; private set; }


    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = MapWidth = GraphicsDevice.DisplayMode.Width;
        _graphics.PreferredBackBufferHeight = MapHeight = GraphicsDevice.DisplayMode.Height;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _textures.Add(1, Content.Load<Texture2D>("space_ship"));
        _textures.Add(2, Content.Load<Texture2D>("4698768"));
    }

    public void LoadGameCycleParameters(Dictionary<int, IObject> objects)
    {
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
        
        PlayerMoved?.Invoke(this, new ControlsEventArgs { Directions = directions });
        
        base.Update(gameTime);
        
        CycleFinished?.Invoke(this, EventArgs.Empty);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
        _spriteBatch.Begin();
        foreach (var obj in _objects.Values)
        {
            _spriteBatch.Draw(_textures[obj.ImageId],  obj.Position, Color.White);
        }  	
        _spriteBatch.End();  
    }
}