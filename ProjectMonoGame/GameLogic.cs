using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectMonoGame;

public partial class GameCycleModel
{
    private void CheckCollision(IEnumerable<IObject> currentEntities)
    {
        var player = Objects[PlayerId];
        player.Update();
        foreach (var o in Objects.Values)
        {
            o.Update();
            if (o == player) continue;
            var solid = (ISolid)o;
            if (RectangleCollider.IsCollided(solid.Collider, player.Collider))
            {
                Objects.Remove(o.Id);
            }
        }

        Updated.Invoke(this, new GameplayEventArgs(Objects));
    }
}

