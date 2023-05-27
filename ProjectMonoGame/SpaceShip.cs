using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class SpaceShip : IObject
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    public int ImageId { get; set; }
    public int Id { get; set; }
    private Vector2 _position;
    public Vector2 Speed { get; set; }
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
    
    public SpaceShip(int mapWidth, int mapHeight)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
    }

    public void Update()
    {
        Position += Speed;
        MoveCollider(Position);
    }
    
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, 50, 50);
    }
}
