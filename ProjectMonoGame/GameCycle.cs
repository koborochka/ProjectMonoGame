using System;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public class GameCycle : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated = delegate { };

    private Vector2 _pos = new Vector2(300, 300);

    public void Update()
    {
        Updated.Invoke(this, new GameplayEventArgs { PlayerPos = _pos });
    }

    public void MovePlayer(IGameplayModel.Direction dir)
    {
        switch (dir)
        {
            case IGameplayModel.Direction.forward:
            {
                _pos += new Vector2(0, -1);
                break;
            }
            case IGameplayModel.Direction.backward:
            {
                _pos += new Vector2(0, 1);
                break;
            }
            case IGameplayModel.Direction.right:
            {
                _pos += new Vector2(1, 0);
                break;
            }
            case IGameplayModel.Direction.left:
            {
                _pos += new Vector2(-1, 0);
                break;
            }
        }
    }
}