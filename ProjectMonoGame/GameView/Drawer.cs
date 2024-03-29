﻿using Microsoft.Xna.Framework;
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
        IsMouseVisible = false;
        base.Draw(gameTime);
        var player = (SpaceShip)_objects[_playerId];
        
        _spriteBatch.Begin();
        DrawBackground(_backgroundGameImageId);

        foreach (var obj in _objects.Values)
        {
            _spriteBatch.Draw(_textures[obj.ImageId],  obj.Position, Color.White);
        }
        _spriteBatch.DrawString(_font, $"Котиков собрано: {player.CatCaught}",
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
        IsMouseVisible = true;
        base.Draw(gameTime);

        _spriteBatch.Begin();
        DrawBackground(_backgroundMenuImageId);    
        DrawButton(_objects[0] as Button);
        DrawButton(_objects[1] as Button);
        var menuCatImage = _textures[_menuCatImageId];
        _spriteBatch.Draw(menuCatImage,new Vector2(_mapWidth -menuCatImage.Width, _mapHeight - menuCatImage.Height) / 2, Color.White);
        _spriteBatch.DrawString(_font, "Собирай котиков и уклоняйся от астероидов! ",
            new Vector2(_mapWidth / 3, 80), Color.Orchid);
        _spriteBatch.DrawString(_font, "Управление клавишами W,A,S,D",
            new Vector2(_mapWidth / 3 + 50, _mapHeight - 150), Color.Plum);
        _spriteBatch.DrawString(_font, $"Предыдущий результат : {_currentGameState.CatsCollectedCount}",
            new Vector2(_mapWidth / 3 + 130, 150), Color.Orchid);
        _spriteBatch.End();
    }
    
    private void DrawButton(Button button)
    {
        _spriteBatch.DrawString(_font, button.Text, button.Position, button.TextColor);
    }

    private void DrawBackground(int imageId)
    {
        var destinationRectangle = new Rectangle(0, 0, _mapWidth, _mapHeight);
        _spriteBatch.Draw(_textures[imageId], destinationRectangle, Color.White);
    }
}