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

    private const float ConstantAcceleration = 0.05f; // Константа ускорения
    private int _currentId; 
    private Timer _asteroidTimer;

    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
        _currentId = 1;
        var player = new SpaceShip
        {
            Position = new Vector2 (250, 250),
            ImageId = 1,
            Speed = new Vector2 (0, 1)
        };
        Objects.Add(1, player);
        PlayerId = _currentId;
        _currentId++;
        _asteroidTimer = new Timer(GenerateAsteroid, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
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
        var asteroid = new Asteroid
        {
            ImageId = 2,
            Position = new Vector2(random.Next(0, 800), random.Next(0, 600)),
            Speed = new Vector2(3,0)
        };
    
        Objects.Add(_currentId, asteroid);
        _currentId++;
    }
    
   /* public void GenerateAsteroid()
    {
        GameCycleView.
        var random = new Random();
        var initialPosition = new Vector2(random.Next(0, MapWidth), random.Next(0, MapHeight));
        var direction = Vector2.Normalize(Objects[_playerId].Position - initialPosition);

        var asteroid = new Asteroid(direction);
        asteroid.Position = initialPosition;
        asteroid.Speed = direction * ConstantSpeed;

        var asteroidId = _currentId;
        Objects.Add(asteroidId, asteroid);
        _currentId++;
    }*/

}

public class SpaceShip : IObject
{
    public int ImageId { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; } 

    public void Update()
    {
        Position += Speed;
    }
}

public class Asteroid : IObject
{
    public int ImageId { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }

    public Vector2 Direction { get; set; }

    public void Update()
    {
        Position += Speed;
    }
}