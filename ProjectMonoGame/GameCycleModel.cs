using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class GameCycleModel : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;

    private Vector2 _pos = new Vector2(300, 300);
    private const float ConstantAcceleration = 0.05f; // Константа ускорения
    private Vector2 _velocity = Vector2.Zero;
    
    public void Update()
    {
        Updated?.Invoke(this, new GameplayEventArgs { PlayerPos = _pos });
    }

    public void MovePlayer(IGameplayModel.Direction direction)
    {
        // Применяем ускорение в соответствии с направлением
        Vector2 acceleration = direction switch
        {
            IGameplayModel.Direction.forward => new Vector2(0, -ConstantAcceleration),
            IGameplayModel.Direction.backward => new Vector2(0, ConstantAcceleration),
            IGameplayModel.Direction.right => new Vector2(ConstantAcceleration, 0),
            IGameplayModel.Direction.left => new Vector2(-ConstantAcceleration, 0),
            _ => Vector2.Zero
        };

        _velocity += acceleration; // Добавляем ускорение к текущей скорости
        
        _pos += _velocity; // Обновляем позицию персонажа на основе скорости
    }

    public void SlowDownPlayer()
    {
        _velocity *= 0.99f;
        _pos += _velocity;
    }
}