using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public partial class GameCycleModel : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;
    public int PlayerId { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    private const float ConstantAcceleration = 0.06f; 
    private const float BlinkDuration = 4000f; // Длительность мигания в секундах
    private float _blinkTimer = 0f; // Таймер мигания
    private bool _isPlayerBlinking = false; // Флаг мигания игрока
    private int _currentId ; 
    private Timer _asteroidTimer;
    private Timer _spaceCatTimer;
    private Dictionary<int, Texture2D> _textures = new ();
    private readonly int _mapWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    private readonly int _mapHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
        var player = new SpaceShip(_mapWidth, _mapHeight, _currentId);
        Objects.Add(_currentId, player);
        PlayerId = _currentId;
        _currentId++;
        _asteroidTimer = new Timer(GenerateAsteroid, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)); 
        _spaceCatTimer = new Timer(GenerateSpaceCat, null, TimeSpan.FromSeconds(7), TimeSpan.FromSeconds(5));
        //_asteroidTimer.Change(TimeSpan.FromSeconds(20), Timeout.InfiniteTimeSpan);
        //_asteroidTimer = new Timer(GenerateAsteroid, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
// Остановить вызов метода GenerateAsteroid через 20 секунд
        var stopTimer = new TimerCallback(StopGenerateAsteroid);
        _asteroidTimer = new Timer(StopGenerateAsteroid, null, TimeSpan.FromSeconds(5), Timeout.InfiniteTimeSpan);
    }

// Метод, который останавливает вызов GenerateAsteroid
        private void StopGenerateAsteroid(object state)
        {
            _asteroidTimer.DisposeAsync();
        }
        

    public void LoadTextures(Dictionary<int, Texture2D> textures)
    {
        _textures = textures;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var obj in Objects.Values)
        {
            obj.Update();
            if (obj is Asteroid { IsInBoundsOfScreen: false } asteroid)
                Objects.Remove(asteroid.Id);
            else if (obj is SpaceCat { IsInBoundsOfScreen: false } spaceCat)
                Objects.Remove(spaceCat.Id);

            var player = Objects[PlayerId];
            if (obj == player) continue;
            var solid = (ISolid)obj;
            if (RectangleCollider.IsCollided(solid.Collider, player.Collider))
            {
                // Удалите объект и запустите эффект мигания игрока
                Objects.Remove(obj.Id);
                StartPlayerBlink();
            }
        }

        if (_isPlayerBlinking)
        {
            // Обновите таймер мигания
            _blinkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_blinkTimer >= BlinkDuration)
            {
                // Завершите эффект мигания
                _isPlayerBlinking = false;
                _blinkTimer = 0f;
            }
            var player = Objects[PlayerId] as IShimmering;

            if (_blinkTimer < 1 || (_blinkTimer > 200 && _blinkTimer < 300) || _blinkTimer > 3500)
            {
                if (player != null) player.IsShimmering = true;
            }
            else
            {
                if (player != null) player.IsShimmering = false;
            }
            _blinkTimer += (float)gameTime.TotalGameTime.TotalMilliseconds;
        }

        Updated?.Invoke(this, new GameplayEventArgs(Objects));
    }
    private void StartPlayerBlink()
    {
        // Запустите эффект мигания игрока
        _isPlayerBlinking = true;
        _blinkTimer = 0f;
        //var timer = new Timer();
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

    private void GenerateAsteroid(object state)
    {
        var playerPosition = Objects[PlayerId].Position;
        var asteroid = new Asteroid(_mapWidth, _mapHeight, _currentId, playerPosition, _textures);
        Objects.Add(_currentId, asteroid);
        _currentId++;
    }
    
    private void GenerateSpaceCat(object state)
    {
        var playerPosition = Objects[PlayerId].Position;
        var spaceCat = new SpaceCat(_mapWidth, _mapHeight, _currentId, playerPosition, _textures);
        Objects.Add(_currentId, spaceCat);
        _currentId++;
    }
}