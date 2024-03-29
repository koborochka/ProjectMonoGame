using Microsoft.Xna.Framework;

namespace ProjectMonoGame.Objects;

public class SpaceShip : IEntity
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    private const int TextureWidth = 70;
    private const int TextureHeight = 39;
    private Vector2 _position;
    public int ImageId { get; set; } = 0;
    public int Id { get; set; }
    public Vector2 Speed { get; set; }
    public RectangleCollider Collider { get; set; }
    public int CatCaught { get; set; }
    public int HealthPoints { get; set; } = 10;
    public Vector2 Position
    {
        get => _position; 
        set
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
