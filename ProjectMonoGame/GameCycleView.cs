using System;
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
    public event EventHandler NothingHappens;
    

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
        foreach (var key in keys)
        { 
            switch (key)
            {
                case Keys.W:
                {
                    InvokeDirection(IGameplayModel.Direction.forward);
                    break;
                }
                case Keys.S:
                {
                    InvokeDirection(IGameplayModel.Direction.backward);
                    break;
                }
                case Keys.D:
                {
                    InvokeDirection(IGameplayModel.Direction.right);
                    break;
                }
                case Keys.A:
                {
                    InvokeDirection(IGameplayModel.Direction.left);
                    break;
                }
                case Keys.Escape:
                    Exit();
                    break;
                default:
                    InvokeNothingHappens();
                    break;
            }
        }

        if (keys.Length == 0)
        {
            InvokeNothingHappens();
        }
        
        base.Update(gameTime);
        CycleFinished?.Invoke(this, EventArgs.Empty);
    }

    private void InvokeDirection(IGameplayModel.Direction direction)
    {
        PlayerMoved?.Invoke(
            this,
            new ControlsEventArgs
            { Direction = direction }
            );
    }

    private void InvokeNothingHappens()
    {
        NothingHappens?.Invoke(this, EventArgs.Empty);

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