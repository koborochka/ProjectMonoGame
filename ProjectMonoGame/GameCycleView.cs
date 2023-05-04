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

    public event EventHandler CycleFinished = delegate { };
    public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };
    
    


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
        if (keys.Length > 0)
        {
            var k = keys[0];
            switch (k)
            {
                case Keys.W:
                {
                    PlayerMoved.Invoke(
                        this, 
                        new ControlsEventArgs { 
                            Direction = IGameplayModel.Direction.forward }
                    );
                    break;
                }
                case Keys.S:
                {
                    PlayerMoved.Invoke(
                        this, 
                        new ControlsEventArgs { 
                            Direction = IGameplayModel.Direction.backward }
                    );
                    break;                    
                }
                case Keys.D:
                {
                    PlayerMoved.Invoke(
                        this, 
                        new ControlsEventArgs { 
                            Direction = IGameplayModel.Direction.right }
                    );                    
                    break;
                }
                case Keys.A:
                {
                    PlayerMoved.Invoke(
                        this, 
                        new ControlsEventArgs { 
                            Direction = IGameplayModel.Direction.left }
                    );
                    break;                    
                }
                case Keys.Escape:
                {
                    Exit();
                    break;
                }
            }
        }  
        base.Update(gameTime);    
        CycleFinished.Invoke(this, new EventArgs());
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