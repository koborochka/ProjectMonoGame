using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public class SpaceShip : IObject
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    private const int TextureWidth = 70;
    private const int TextureHeight = 39;
    public int ImageId { get; set; }
    public int Id { get; set; }
    public Vector2 Speed { get; set; }
    public bool IsInBoundsOfScreen => Position.X + TextureWidth >= 0 
                                      && Position.Y + TextureHeight >= 0 
                                      && Position.X <= _mapWidth 
                                      && Position.Y <= _mapHeight;
    public RectangleCollider Collider { get; set; }
    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        private set
        {
            _position.X = MathHelper.Clamp(value.X, 0, _mapWidth - TextureWidth);
            _position.Y = MathHelper.Clamp(value.Y, 0, _mapHeight - TextureHeight);

            if (value.X < 0 || value.Y < 0 || value.X > _mapWidth - TextureWidth || value.Y > _mapHeight - TextureHeight)
            {
                Speed = Vector2.Zero;
            }
        }
    }
    
    public SpaceShip(int mapWidth, int mapHeight, int playerId)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        ImageId = 0;
        Id = playerId;
        Position = new Vector2(_mapWidth, _mapHeight) * 0.5f;
        Speed = Vector2.Zero;
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, TextureWidth, TextureHeight);
    }

    public void Update()
    {
        Position += Speed;
        MoveCollider(Position);
    }
    
    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, TextureWidth, TextureHeight);
    }
}
