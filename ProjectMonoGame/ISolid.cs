using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface ISolid
{
    RectangleCollider Collider { get; set; }
    void MoveCollider(Vector2 newPos);
}

public class RectangleCollider
{
    private Rectangle Boundary { get; }
    
    public RectangleCollider(int x, int y, int width, int height)
    {
        Boundary = new Rectangle(x, y, width, height);
    }

    public static bool IsCollided(RectangleCollider r1, RectangleCollider r2)
    {
        return r1.Boundary.Intersects(r2.Boundary);
    }
}

