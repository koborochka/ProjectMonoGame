using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IObject : ISolid
{
    int ImageId { get; set; }   
    public int Id { get; set; }

    Vector2 Position { get; }
    public bool IsInBoundsOfScreen { get; }

    void Update();  
}