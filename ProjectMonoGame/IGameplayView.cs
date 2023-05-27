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

    void LoadGameCycleParameters(Dictionary<int, IObject> objects);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public List<IGameplayModel.Direction> Directions { get; set; }
}

public class TextureEventArgs : EventArgs
{
    public Dictionary<int, Texture2D> Textures { get; set; }
}