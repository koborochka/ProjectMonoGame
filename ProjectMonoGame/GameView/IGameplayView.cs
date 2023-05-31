using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<ControlsEventArgs> PlayerMoved;
    event EventHandler<TextureEventArgs> TexturesDownloaded;
    event EventHandler StartNewGame;
    event EventHandler ReturnedToMenu;

    void LoadGameCycleParameters(Dictionary<int, IEntity> objects, int playerId, GameState currentGameState);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public List<IGameplayModel.Direction> Directions { get; }
    public ControlsEventArgs(List<IGameplayModel.Direction> directions)
    {
        Directions = directions;
    }
}

public class TextureEventArgs : EventArgs
{
    public Dictionary<int, Texture2D> Textures { get; }
    public TextureEventArgs(Dictionary<int, Texture2D> textures)
    {
        Textures = textures;
    }
}