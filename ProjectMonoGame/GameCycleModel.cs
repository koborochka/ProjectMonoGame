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
            Id = _currentId,
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
        var randomInt = random.Next(1, 5);
        var initialPosition = GetInitialAsteroidPosition(randomInt);
        var direction = Vector2.Normalize(Objects[PlayerId].Position - initialPosition);
        var randomAsteroidNumber = random.Next(2, 5);
        var asteroidId = _currentId;
        var asteroid = CreateNewAsteroid(randomAsteroidNumber, asteroidId, direction, initialPosition);
        Objects.Add(asteroidId, asteroid);
        _currentId++;
    }

    private Vector2 GetInitialAsteroidPosition(int randomInt)
    {
        var random = new Random();
        switch (randomInt)
        {
            case 1:
                return new Vector2(_mapWidth, random.Next(0, _mapHeight));
            case 2:
                return new Vector2(random.Next(0, _mapWidth), -_mapHeight);
            case 3:
                return new Vector2(random.Next(0, _mapWidth), _mapHeight);
            default:
                return new Vector2(-_mapWidth, random.Next(0, _mapHeight));
        }
    }

    private Asteroid CreateNewAsteroid(int imageId,int id, Vector2 direction, Vector2 initialPosition)
    {
        var asteroid = new Asteroid
        {
            Id = id,
            ImageId = imageId,
            Direction = direction,
            Speed = direction * GetSpeedCoefficientForAsteroid(imageId),
            Position = initialPosition
        };

        return asteroid;
    }

    private int GetSpeedCoefficientForAsteroid(int imageId)
    {
        return imageId switch
        {
            2 => 2,
            3 => 3,
            4 => 5,
            _ => 0
        };
    }

}