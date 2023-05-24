using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<ControlsEventArgs> PlayerMoved;
    void LoadGameCycleParameters(Vector2 pos);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public List<IGameplayModel.Direction> Directions { get; set; }
}