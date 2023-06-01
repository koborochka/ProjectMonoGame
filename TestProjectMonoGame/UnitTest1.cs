using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using ProjectMonoGame;
using ProjectMonoGame.Objects;

namespace TestProjectMonoGame
{
    [TestFixture]
    public class SpaceShipTests
    {
        private const int MapWidth = 800;
        private const int MapHeight = 600;

        [Test]
        public void Position_SetWithinBounds_DoesNotChange()
        {
            var spaceship = new SpaceShip(MapWidth, MapHeight, 1);
            var position = new Vector2(100, 200);
            
            spaceship.Position = position;

            Assert.AreEqual(position, spaceship.Position);
        }

        [Test]
        public void Update_SpeedAppliedToPosition()
        {
            var spaceship = new SpaceShip(MapWidth, MapHeight, 1);
            spaceship.Speed = new Vector2(1, 2);
            var initialPosition = spaceship.Position;
            
            spaceship.Update();
            var newPosition = spaceship.Position;
            
            Assert.AreEqual(initialPosition + spaceship.Speed, newPosition);
        }
    }
}