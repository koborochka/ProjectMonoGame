using System;
using ProjectMonoGame.GameModel;

namespace ProjectMonoGame;

public static class Program
{
    private static void Main()
    {
        var game = new GameplayPresenter(new GameCycleView(), new GameCycleModel());
        game.LaunchGame();
    }
}