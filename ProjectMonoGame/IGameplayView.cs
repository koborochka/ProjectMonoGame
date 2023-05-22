﻿using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<ControlsEventArgs> PlayerMoved;
    event EventHandler NothingHappens;

    void LoadGameCycleParameters(Vector2 pos);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public IGameplayModel.Direction Direction { get; set; }
}