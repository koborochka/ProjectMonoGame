using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IGameplayView
{
    //Включается в конце каждого цикла, чтобы обновить модель
    event EventHandler CycleFinished;
    event EventHandler<ControlsEventArgs> PlayerMoved;

    void LoadGameCycleParameters(Vector2 pos);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public IGameplayModel.Direction Direction { get; set; }
}