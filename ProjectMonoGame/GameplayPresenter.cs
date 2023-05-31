using System;

namespace ProjectMonoGame;

public class GameplayPresenter
{
    private readonly IGameplayView _gameplayView;
    private readonly IGameplayModel _gameplayModel;

    public GameplayPresenter(
        IGameplayView gameplayView,
        IGameplayModel gameplayModel
    )
    {
        _gameplayView = gameplayView;
        _gameplayModel = gameplayModel;

        _gameplayView.CycleFinished += ViewModelUpdate;
        _gameplayView.PlayerMoved += ViewModelMovePlayer;
        _gameplayView.TexturesDownloaded += ViewTexturesDownloaded;
        _gameplayView.StartNewGame += ViewModelStartNewGame;
        _gameplayView.ReturnedToMenu += ViewModelReturnedToMenu;
        _gameplayModel.Updated += ModelViewUpdate;
        
        _gameplayModel.Initialize(); 
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
        _gameplayView.LoadGameCycleParameters(e.Objects, e.PlayerId, e.CurrentGameState);
    }

    private void ViewTexturesDownloaded(object sender, TextureEventArgs e)
    {
        _gameplayModel.LoadTextures(e.Textures);
    }

    private void ViewModelUpdate(object sender, EventArgs e)
    {
        _gameplayModel.Update();
    }
    private void ViewModelStartNewGame(object sender, EventArgs e)
    {
        _gameplayModel.StartNewGame();
    }

    private void ViewModelReturnedToMenu(object sender, EventArgs e)
    {
        _gameplayModel.StopGenerateObjects();
    }
}