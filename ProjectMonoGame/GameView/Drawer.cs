using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectMonoGame.Objects;

namespace ProjectMonoGame;

public partial class GameCycleView
{
    protected override void Draw(GameTime gameTime)
    {
        if (_currentGameState.State == State.Menu)
            DrawMenu(gameTime);
        else
            DrawGame(gameTime);
    }

    private void DrawGame(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
        var player = (SpaceShip)_objects[_playerId];
        
        _spriteBatch.Begin();
        
        var destinationRectangle = new Rectangle(0, 0, _mapWidth, _mapHeight);
        _spriteBatch.Draw(_textures[_backgroundImageId], destinationRectangle, Color.White);

        foreach (var obj in _objects.Values)
        {
            _spriteBatch.Draw(_textures[obj.ImageId],  obj.Position, Color.White);
        }
        _spriteBatch.DrawString(_font, $"x{player.CatCaught}",
            new Vector2(1325, 40), Color.Ivory);
        for (var i = 0; i < player.HealthPoints ; i++)
        {
            var currentX = 10 + 30 * i;
            _spriteBatch.Draw(_textures[6],  new Vector2(currentX, 40), Color.White);
        }
        _spriteBatch.End();  
    }
    
    private void DrawMenu(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        base.Draw(gameTime);
        _spriteBatch.Begin();
        DrawButton(_objects[0] as Button);
        DrawButton(_objects[1] as Button);
        _spriteBatch.End();
    }
    
    private void DrawButton(Button button)
    {
        _spriteBatch.DrawString(_font, button.Text, button.Position, button.TextColor);
    }
}