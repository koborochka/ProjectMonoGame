using System;

namespace ProjectMonoGame;

public static class Program
{
    [STAThread]
    static void Main()
    {
        //using (var game = new GameCycleView())
        //var game = new GameCycleView();
        //game.Run();         
        GameplayPresenter game = new GameplayPresenter(new GameCycleView(), new GameCycleModel());
        game.LaunchGame();
    }
}