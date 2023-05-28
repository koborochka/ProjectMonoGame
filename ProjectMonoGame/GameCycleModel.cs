using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectMonoGame;

public partial class GameCycleModel : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;
    public int PlayerId { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    private const float ConstantAcceleration = 0.06f; 
    private int _currentId ; 
    private Timer _asteroidTimer;
    private Dictionary<int, Texture2D> _textures = new ();
    private readonly int _mapWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    private readonly int _mapHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    

    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
        var player = new SpaceShip(_mapWidth, _mapHeight, _currentId);
        Objects.Add(_currentId, player);
        PlayerId = _currentId;
        _currentId++;
        _asteroidTimer = new Timer(GenerateAsteroid, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1));
    }
    
    public void LoadTextures(Dictionary<int, Texture2D> textures)
    {
        _textures = textures;        
    }

    public void Update()
    {
        foreach (var obj in Objects.Values)
        {
            obj.Update();
            if (obj is Asteroid { IsInBoundsOfScreen: false } asteroid)
            {
                Objects.Remove(asteroid.Id);
            }
            
            var player = Objects[PlayerId];
            if (obj == player) continue;
            var solid = (ISolid)obj;
            if (RectangleCollider.IsCollided(solid.Collider, player.Collider))
            {
                Objects.Remove(obj.Id);
            }
        }
        Updated?.Invoke(this, new GameplayEventArgs (Objects));
    }

    public void MovePlayer(List<IGameplayModel.Direction> directions)
    {
        var player = (SpaceShip)Objects[PlayerId];
        foreach (var direction in directions)
        {
            var acceleration = direction switch
            {
                IGameplayModel.Direction.forward => new Vector2(0, -ConstantAcceleration),
                IGameplayModel.Direction.backward => new Vector2(0, ConstantAcceleration),
                IGameplayModel.Direction.right => new Vector2(ConstantAcceleration, 0),
                IGameplayModel.Direction.left => new Vector2(-ConstantAcceleration, 0),
                _ => Vector2.Zero
            };
            
            player.Speed += acceleration;
        }
    }

    private void GenerateAsteroid(object state)
    {
        var playerPosition = Objects[PlayerId].Position;
        var asteroid = new Asteroid(_mapWidth, _mapHeight, _currentId, playerPosition, _textures);
        Objects.Add(_currentId, asteroid);
        _currentId++;
    }
}