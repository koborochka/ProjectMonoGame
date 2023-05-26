using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class SpaceShip : IObject
{
    public int ImageId { get; set; }
    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position.X = (value.X < 0) ? 0 : (value.X > _mapWidth - 50) ? _mapWidth - 50 : value.X;
            _position.Y = (value.Y < 0) ? 0 : (value.Y > _mapHeight - 50) ? _mapHeight - 50 : value.Y;
        
            if (value.X < 0 || value.Y < 0 || value.X > _mapWidth - 50 || value.Y > _mapHeight - 50)
            {
                Speed = Vector2.Zero;
            }
        }
    }


    public Vector2 Speed { get; set; }

    private readonly int _mapWidth;
    private readonly int _mapHeight;

    public SpaceShip(int mapWidth, int mapHeight)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
    }

    public void Update()
    {
        Position += Speed;
    }
}
