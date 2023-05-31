using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public class SpaceCat : IEntity
{
    private readonly int _mapWidth;
    private readonly int _mapHeight;
    private readonly Texture2D _texture;
    private Vector2 Speed { get; }
    public int ImageId { get; set; } = 4;
    public int Id { get; set; }
    public bool IsInBoundsOfScreen => Position.X + _texture.Width >= 0 
                                      && Position.Y + _texture.Height >= 0 
                                      && Position.X <= _mapWidth 
                                      && Position.Y <= _mapHeight;

    public RectangleCollider Collider { get; set; }
    public Vector2 Position { get; private set; }


    public void Update()
    {
        Position += Speed;
        MoveCollider(Position);
    }

    public void MoveCollider(Vector2 newPos)
    {
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
    }
    
    public SpaceCat(int mapWidth, int mapHeight, int id, Vector2 playerPosition ,Dictionary<int, Texture2D> textures)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        Id = id;
        _texture = textures[ImageId];
        Position = GetRandomInitialSpaceCatPosition();
        Speed = Vector2.Normalize(playerPosition - Position) * 5;
        Collider = new RectangleCollider((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
    }
    
    private Vector2 GetRandomInitialSpaceCatPosition()
    {
        var random = new Random();
        var randomSideCreated = random.Next(1, 5);
        return randomSideCreated switch
        {
            1 => new Vector2(_mapWidth, random.Next(0, _mapHeight)),
            2 => new Vector2(random.Next(0, _mapWidth), 0 - _texture.Height),
            3 => new Vector2(random.Next(0, _mapWidth), _mapHeight),
            _ => new Vector2(0 - _texture.Width, random.Next(0, _mapHeight))
        };
    }
}