using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectMonoGame;

public partial class GameCycleView : Game, IGameplayView
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    
    private GameState _currentGameState;
    private Dictionary<int, IEntity> _objects;
    private readonly Dictionary<int, Texture2D> _textures = new();
    
    private DateTime _lastTimeExitButtonPressed = DateTime.Now;
    private int _playerId;
    private int _backgroundGameImageId;
    private int _backgroundMenuImageId;
    private int _menuCatImageId;
    private int _mapWidth;
    private int _mapHeight;
    public event EventHandler CycleFinished;
    public event EventHandler<ControlsEventArgs> PlayerMoved;
    public event EventHandler<TextureEventArgs> TexturesDownloaded;
    public event EventHandler StartNewGame;
    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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
        _backgroundGameImageId = 5;
        _textures.Add(_backgroundGameImageId, Content.Load<Texture2D>("background"));
        _textures.Add(6, Content.Load<Texture2D>("heart"));
        _textures.Add(7, Content.Load<Texture2D>("menu_cat"));
        _menuCatImageId = 7;
        _textures.Add(8, Content.Load<Texture2D>("menu_background"));
        _backgroundMenuImageId = 8;

        _font = Content.Load<SpriteFont>("font");
        
        TexturesDownloaded?.Invoke(this, new TextureEventArgs(_textures));
    }

    public void LoadGameCycleParameters(Dictionary<int, IEntity> objects, int playerId, GameState currentGameState)
    {
        _playerId = playerId;
        _objects = objects;
        _currentGameState = currentGameState;
    }

    protected override void Update(GameTime gameTime)
    {
        CheckMenuClickableButtons();

        var pressedKeys = Keyboard.GetState().GetPressedKeys();
        var directions = new List<IGameplayModel.Direction>();
        foreach (var key in pressedKeys)
        {
            if(_currentGameState.State == State.Game)
                CheckInGameButtons(key, directions);
            else
                CheckInMenuButtons(key);
        }
        
        base.Update(gameTime);
        
        CycleFinished?.Invoke(this, EventArgs.Empty);
    }

    private void CheckInGameButtons(Keys key, List<IGameplayModel.Direction> directions)
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
                _currentGameState.State = State.Menu;
                _lastTimeExitButtonPressed = DateTime.Now;
                break;         
        }
        PlayerMoved?.Invoke(this, new ControlsEventArgs (directions));
    }
    
    private void CheckMenuClickableButtons()
    {
        if (_currentGameState.State != State.Menu) return;
        
        var buttons = _objects.Values.Cast<Button>().ToArray();
        
        if (buttons[1].IsClicked())
            Environment.Exit(0);
        else if (buttons[0].IsClicked())
        {
            StartNewGame!(this, EventArgs.Empty);
            buttons[0].Clicked();
        }
    }
    private void CheckInMenuButtons(Keys key)
    {
        if ((DateTime.Now - _lastTimeExitButtonPressed).Milliseconds < 150 ) 
            return;
        if (key == Keys.Escape) 
            Exit();
        
        _lastTimeExitButtonPressed = DateTime.Now;
    }
}