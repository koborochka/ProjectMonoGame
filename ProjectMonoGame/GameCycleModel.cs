using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class GameCycleModel : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;
    public int PlayerId { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }

    private const float ConstantAcceleration = 0.05f; // Константа ускорения
    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
        var player = new SpaceShip
        {
            Position = new Vector2 (250, 250),
            ImageId = 1,
            Speed = new Vector2 (0, 0)
        };
        Objects.Add(1, player);
        PlayerId = 1;
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