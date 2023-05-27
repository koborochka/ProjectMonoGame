using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public class SpaceShip : IObject
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    private const int TextureWidth = 50;
    private const int TextureHeight = 50;
    public int ImageId { get; set; }
    public int Id { get; set; }
    private Vector2 _position;
    public Vector2 Speed { get; set; }
    public bool IsInBoundsOfScreen =>
        Position.X > 0 && Position.Y > 0 && Position.X < _mapWidth && Position.Y < _mapHeight;
    public Vector2 Position
    {
        get => _position;
        private set
        {
            _position.X = (value.X < 0) ? 0 : (value.X > _mapWidth - TextureWidth) ? _mapWidth - TextureWidth : value.X;
            _position.Y = (value.Y < 0) ? 0 : (value.Y > _mapHeight - TextureHeight) ? _mapHeight - TextureHeight : value.Y;
        
            if (value.X < 0 || value.Y < 0 || value.X > _mapWidth - TextureWidth || value.Y > _mapHeight - TextureHeight)
            {
                Speed = Vector2.Zero;
            }
        }
    }
    
    public SpaceShip(int mapWidth, int mapHeight ,int imageId, int playerId, Vector2 position,
        Vector2 speed)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        ImageId = imageId;
        Id = playerId;
        Position = position;
        Speed = speed;
        Collider = new RectangleCollider((int)position.X, (int)position.Y, TextureWidth, TextureHeight);
    }

    public void Update()
    {
        Position += Speed;
        MoveCollider(Position);
    }
    
    public RectangleCollider Collider { get; set; }
    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, TextureWidth, TextureHeight);
    }
}
