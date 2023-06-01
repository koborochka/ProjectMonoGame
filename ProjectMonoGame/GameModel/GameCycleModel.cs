using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectMonoGame.Objects;

namespace ProjectMonoGame.GameModel;

public class GameCycleModel : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;
    public int PlayerId { get; set; }
    public Dictionary<int, IEntity> Objects { get; set; }
    private Dictionary<int, IEntity> _buttons;
    private Dictionary<int, Texture2D> _textures;
    
    private GameState _currentGameState = new();

    private const float ConstantAcceleration = 0.06f; 
    
    private int _currentId ;

    private int _asteroidTicker ;
    private int _spaceCatTicker ;

    private const int AsteroidGenerationInterval = 60; 
    private const int SpaceCatGenerationInterval = 300;
    
    private readonly int _mapWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    private readonly int _mapHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

    public void LoadTextures(Dictionary<int, Texture2D> textures)
    {
        _textures = textures;
    }

    public void StartNewGame()
    {
        _currentGameState.State = State.Game;
        Initialize();
    }
    
    public void Initialize()
    {
        switch (_currentGameState.State)
        {
            case State.Game:
                InitializeGame();
                break;
            
            case State.Menu:
                InitializeMenu();
                break;
        }
    }

    private void InitializeGame()
    {
        Objects = new Dictionary<int, IEntity>();
        var player = new SpaceShip(_mapWidth, _mapHeight, _currentId);
        Objects.Add(_currentId, player);
        PlayerId = _currentId;
        _currentId++;

        _asteroidTicker = 0;
        _spaceCatTicker = 0;
        _currentGameState.CatsCollectedCount = 0;
    }
    
    private void InitializeMenu()
    {
        var startGameButtonPosition = new Vector2(100, 515);
        var startGameButton = new Button( startGameButtonPosition, 200, 
            40, "Новая игра", 0);
        var exitButton = new Button(startGameButtonPosition + new Vector2(0, 100), 
            100, 40, "Выйти", 1);
        
        _buttons = new Dictionary<int, IEntity>
        {
            {0, startGameButton},
            {1, exitButton}
        };
        
        UpdateMenu();
    }

    public void Update()
    {
        switch (_currentGameState.State)
        {
            case State.Game:
                UpdateGame();
                break;
            
            case State.Menu:
                UpdateMenu();
                break;
        }
    }

    private void UpdateGame()
    {
        foreach (var obj in Objects.Values)
        {
            obj.Update();
            if (obj is Asteroid { IsInBoundsOfScreen: false } asteroid)
                Objects.Remove(asteroid.Id);
            else if (obj is SpaceCat { IsInBoundsOfScreen: false } spaceCat)
                Objects.Remove(spaceCat.Id);

            var player = (SpaceShip)Objects[PlayerId];
            if (obj == player) continue;
            var solid = (ISolid)obj;
            if (RectangleCollider.IsCollided(solid.Collider, player.Collider))
            {
                Objects.Remove(obj.Id);
                if (obj is SpaceCat)
                {
                    player.CatCaught += 1;
                    _currentGameState.CatsCollectedCount += 1;
                }
                else
                {
                    var currentAsteroid = (Asteroid)obj;
                    player.HealthPoints -= currentAsteroid.GetDamageByImageId(currentAsteroid.ImageId);
                    if (player.HealthPoints <= 0)
                    {
                        _currentGameState.State = State.Menu; 
                        InitializeMenu();
                        return;
                    }
                }
            }
        }
        
        if (++_asteroidTicker % AsteroidGenerationInterval == 0) GenerateAsteroid();
        if (++_spaceCatTicker % SpaceCatGenerationInterval == 0) GenerateSpaceCat();
        
        Updated?.Invoke(this, new GameplayEventArgs(Objects, PlayerId, _currentGameState));
    }
    private void UpdateMenu()
    {
        var mouseState = Mouse.GetState();
        foreach (var button in _buttons.Values.Cast<Button>())
            button.Update(mouseState);
        
        Updated?.Invoke(this, new GameplayEventArgs(_buttons, PlayerId, _currentGameState));
    }

    public void MovePlayer(List<IGameplayModel.Direction> directions)
    {
        var player = (SpaceShip)Objects[PlayerId];
        foreach (var direction in directions)
        {
            var acceleration = direction switch
            {
                IGameplayModel.Direction.forward => new Vector2(0, -ConstantAcceleration),
                IGameplayModel.Direction.backward => new Vector2(0, ConstantAcceleration),
                IGameplayModel.Direction.right => new Vector2(ConstantAcceleration, 0),
                IGameplayModel.Direction.left => new Vector2(-ConstantAcceleration, 0),
                _ => Vector2.Zero
            };
            
            player.Speed += acceleration;
        }
    }

    private void GenerateAsteroid()
    {
        GenerateObject<Asteroid>();
    }

    private void GenerateSpaceCat()
    {
        GenerateObject<SpaceCat>();
    }
    
    private void GenerateObject<T>() where T : IEntity
    {
        var playerPosition = Objects[PlayerId].Position;
        var obj = (T)Activator.CreateInstance(typeof(T), _mapWidth, _mapHeight, _currentId, playerPosition, _textures);
        Objects.Add(_currentId, obj);
        _currentId++;
    }
}
