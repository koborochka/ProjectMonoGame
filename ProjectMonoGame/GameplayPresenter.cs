using System;

namespace ProjectMonoGame;

public class GameplayPresenter
{
    private IGameplayView _gameplayView = null;
    private IGameplayModel _gameplayModel = null;

    public GameplayPresenter(
        IGameplayView gameplayView,
        IGameplayModel gameplayModel
    )
    {
        _gameplayView = gameplayView;
        _gameplayModel = gameplayModel;

        _gameplayView.CycleFinished += ViewModelUpdate;
        _gameplayView.PlayerMoved += ViewModelMovePlayer;
        _gameplayView.MapSizeRequested += ViewModelTransferScreenValues;
        _gameplayModel.Updated += ModelViewUpdate;
        
        _gameplayModel.Initialize(); //Инициализируем игровой цикл
    }

    private void ViewModelTransferScreenValues(object sender, ScreenEventArgs e)
    {
        _gameplayModel.ReceiveScreenValues(e.MapWidth,e.MapHeight);
    }

    public void LaunchGame()
    {
        _gameplayView.Run();
    }

    private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
    {
        _gameplayModel.MovePlayer(e.Directions);
    }

    private void ModelViewUpdate(object sender, GameplayEventArgs e)
    {
        _gameplayView.LoadGameCycleParameters(e.Objects);
    }

    private void ViewModelUpdate(object sender, EventArgs e)
    {
        _gameplayModel.Update();
    }
}