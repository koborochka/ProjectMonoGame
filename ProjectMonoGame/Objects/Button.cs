using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectMonoGame;

public class Button : IEntity
{
    private readonly int _height;
    private readonly int _width;
    private bool _isClicked;
    public RectangleCollider Collider { get; set; }
    public readonly string Text;
    public readonly Color TextColor;
    public Vector2 Position { get; private set; }
    public int ImageId { get; set; }
    public int Id { get; set; }
    public bool IsClicked() => _isClicked;
    public void Clicked() => _isClicked = false;
    public void Update() {}

    public Button(Vector2 position, int buttonWidth, int buttonHeight, string text, int id)
    {
        Position = position;
        Text = text;
        _height = buttonHeight;
        _width = buttonWidth;
        Id = id;
        
        TextColor = Color.PowderBlue;
        Collider = new RectangleCollider((int)position.X, (int)position.Y, buttonWidth, buttonHeight);
    }
    public void Update(MouseState mouseState)
    {
        var mouseCollider = new RectangleCollider(mouseState.X, mouseState.Y, 1, 1);
        
       if(RectangleCollider.IsCollided(mouseCollider, Collider))
            _isClicked = mouseState.LeftButton == ButtonState.Pressed;
       else
            _isClicked = false;
    }
    
    public void MoveCollider(Vector2 newPos)
    {
        Position = newPos;
        Collider = new RectangleCollider((int)newPos.X, (int)newPos.Y, _width, _height);
    }
}
