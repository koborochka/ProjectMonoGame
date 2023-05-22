using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;

    void Update();
    void MovePlayer(Direction direction);

    void SlowDownPlayer();


    public enum Direction : byte
    {
        forward,
        backward,
        right,
        left
    }
}

public class GameplayEventArgs : EventArgs
{
    public Vector2 PlayerPos { get; set; }
}