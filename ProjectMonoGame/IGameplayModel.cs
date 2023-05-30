using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;

    void Update(GameTime gameTime);
    void MovePlayer(List<Direction> direction);
    void LoadTextures(Dictionary<int, Texture2D> textures);
    Dictionary<int, IObject> Objects { get; set; }
    void Initialize();    
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
    public Dictionary<int, IObject> Objects { get; }
    public GameplayEventArgs(Dictionary<int, IObject> objects)
    {
        Objects = objects;
    }
}

