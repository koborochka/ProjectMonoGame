using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class Asteroid : IObject 
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
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
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, 20, 20);
    }
}