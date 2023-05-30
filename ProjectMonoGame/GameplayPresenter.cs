using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class GameplayPresenter
{
    private IGameplayView _gameplayView = null;
    private IGameplayModel _gameplayModel = null;
    private GameTime _gameTime = new();
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
        _gameplayView.LoadGameCycleParameters(e.Objects);
    }

    private void ViewTexturesDownloaded(object sender, TextureEventArgs e)
    {
        _gameplayModel.LoadTextures(e.Textures);
    }

    private void ViewModelUpdate(object sender, EventArgs e)
    {
        _gameplayModel.Update(_gameTime);
    }
}