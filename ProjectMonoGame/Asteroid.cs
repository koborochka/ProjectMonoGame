using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public class Asteroid : IObject 
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    private readonly Texture2D _texture;
    public int ImageId { get; set; }
    public int Id { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }
    public Vector2 Direction { get; set; }

    public RectangleCollider Collider { get; set; }
    public void Update()
    {
        Position += Speed;
        MoveCollider(Position);
    }
    
    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
    }
    
    public Asteroid(int mapWidth, int mapHeight ,int imageId, int id, Vector2 position, Vector2 direction, int speedCoefficient, Texture2D texture)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        ImageId = imageId;
        Id = id;
        Position = position;
        Speed = direction * speedCoefficient;
        _texture = texture;
        Collider = new RectangleCollider((int)position.X, (int)position.Y, _texture.Width, _texture.Height);
    }
}