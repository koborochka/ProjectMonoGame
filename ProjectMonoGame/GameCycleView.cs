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
    private Vector2 _playerPos = Vector2.Zero;
    private Texture2D _playerImage;

    public event EventHandler CycleFinished;
    public event EventHandler<ControlsEventArgs> PlayerMoved;

    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    public void LoadGameCycleParameters(Vector2 playerPos)
    {
        _playerPos = playerPos;
        _playerImage = Content.Load<Texture2D>("space_ship");
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
        _spriteBatch.Draw(_playerImage, _playerPos, Color.White);
        _spriteBatch.End();  
    }
}