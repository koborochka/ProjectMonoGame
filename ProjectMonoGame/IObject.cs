using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public interface IObject
{
    int ImageId { get; set; }   

    Vector2 Position { get; }
    
    void Update();  
}