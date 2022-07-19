using System.Numerics;
using Phader.Scene;
using Phader.GameObjects;

namespace TestGame
{
    public class GameScene : Scene
    {
        public GameScene() : base("GameScene") { }

        public override void Preload()
        {
            textures.AddTexture("silk", "assets/silk.png");
        }

        public override void Create()
        {
            Sprite Sprite = new Sprite(Game.width / 2, Game.height / 2, textures.GetTexture("silk"));
            Sprite.SetOrigin(0.5f, 0.5f);
            DisplayList.Add(Sprite);

            Sprite Sprite2 = new Sprite(Game.width / 2, Game.height / 2 - 100, textures.GetTexture("silk"));
            Sprite2.SetAlpha(0.5f).SetTint(new Vector3(1f, 0.5f, 0.5f)).SetOrigin(new Vector2(0.5f));
            DisplayList.Add(Sprite2);
        }

        public override void Update(double dt)
        {
            // throw new NotImplementedException();
        }

        public override void End()
        {
            Dispose();
        }
    }
}