using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<ControlsEventArgs> PlayerMoved;
    public event EventHandler<ScreenEventArgs> MapSizeRequested;

    void LoadGameCycleParameters(Dictionary<int, IObject> objects);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public List<IGameplayModel.Direction> Directions { get; set; }
}

public class ScreenEventArgs : EventArgs
{
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
}