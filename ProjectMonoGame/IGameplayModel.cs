using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;

    void Update();
    void MovePlayer(List<Direction> direction);
    Dictionary<int, IObject> Objects { get; set; }
    void Initialize();    
    int PlayerId { get; set; }
    public void ReceiveScreenValues(int mapWidth, int mapHeight);

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
    public Dictionary<int, IObject> Objects { get; set; }
}

