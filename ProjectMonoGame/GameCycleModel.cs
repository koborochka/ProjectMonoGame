using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public class GameCycleModel : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;
    public int PlayerId { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }

    private const float ConstantAcceleration = 0.06f; 
    private int _currentId; 
    private Timer _asteroidTimer;
    private readonly int _mapWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    private readonly int _mapHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    

    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
        _currentId = 1;
        var player = new SpaceShip(_mapWidth, _mapHeight)
        {
            Position = new Vector2 (_mapWidth / 2,_mapHeight / 2),
            ImageId = 1,
            Speed = Vector2.Zero
        };
        Objects.Add(_currentId, player);
        PlayerId = _currentId;
        _currentId++;
        _asteroidTimer = new Timer(GenerateAsteroid, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1));
    }

    public void Update()
    {
        foreach (var obj in Objects.Values)
        {
            obj.Update();
        }
        Updated?.Invoke(this, new GameplayEventArgs { Objects = this.Objects });
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
        var random = new Random();
        Vector2 initialPosition;
        var randomInt = random.Next(1, 5);

        switch (randomInt)
        {
            case 1: 
                initialPosition = new Vector2(_mapWidth, random.Next(0, _mapHeight));
                break;
            case 2: 
                initialPosition = new Vector2(random.Next(0, _mapWidth), -_mapHeight);
                break;
            case 3: 
                initialPosition = new Vector2(random.Next(0, _mapWidth), _mapHeight);
                break;
            default: 
                initialPosition = new Vector2(-_mapWidth, random.Next(0, _mapHeight));
                break;
        }
        var direction = Vector2.Normalize(Objects[PlayerId].Position - initialPosition);

        var randomAsteroidNumber = random.Next(2, 5);
        var asteroid = new Asteroid();
        switch (randomAsteroidNumber)
        {
            case 2:
                CreateNewAsteroid(asteroid,randomAsteroidNumber,direction,3,initialPosition);
                break;
            case 3:
                CreateNewAsteroid(asteroid,randomAsteroidNumber,direction,2,initialPosition);
                break;
            case 4:
                CreateNewAsteroid(asteroid,randomAsteroidNumber,direction,5,initialPosition);
                break;
                
        }

        var asteroidId = _currentId;
        Objects.Add(asteroidId, asteroid);
        _currentId++;
    }

    private void CreateNewAsteroid(Asteroid asteroid, int imageId, Vector2 direction, int speedCoefficient, Vector2 initialPosition)
    {
        asteroid.ImageId = imageId;
        asteroid.Direction = direction;
        asteroid.Speed = direction * speedCoefficient;
        asteroid.Position = initialPosition;
    }
}