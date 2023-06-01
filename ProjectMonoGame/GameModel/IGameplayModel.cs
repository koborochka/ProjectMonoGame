using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    void Update();
    void MovePlayer(List<Direction> direction);
    void LoadTextures(Dictionary<int, Texture2D> textures);
    Dictionary<int, IEntity> Objects { get; set; }
    void Initialize();
    void StartNewGame();
    void StopGenerateObjects();
    int PlayerId { get; set; }
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
    public Dictionary<int, IEntity> Objects { get; }
    public int PlayerId { get; }
    public GameState CurrentGameState { get;  }

    public GameplayEventArgs(Dictionary<int, IEntity> objects, int playerId, GameState currentGameState)
    {
        Objects = objects;
        PlayerId = playerId;
        CurrentGameState = currentGameState;
    }
}

