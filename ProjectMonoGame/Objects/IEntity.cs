using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IEntity : ISolid
{
    int ImageId { get; set; } 
    int Id { get; set; }
    Vector2 Position { get; }
    void Update();  
}