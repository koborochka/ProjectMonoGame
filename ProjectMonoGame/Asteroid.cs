using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class Asteroid : IObject
{
    public int ImageId { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }

    public Vector2 Direction { get; set; }

    public void Update()
    {
        Position += Speed;
    }
    
}