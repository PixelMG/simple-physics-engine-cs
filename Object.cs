using Microsoft.Xna.Framework;

namespace physics
{
    public class GameObject
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public Vector2 direction;
        public Color color;

        public bool hasCollided = false;

        public GameObject(float x, float y, float width, float height, Vector2 direction)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.direction = direction;

            this.color = Color.YellowGreen;
        }
        
        public void HandleCollision(GameObject obj)
        {
            float overlapX = (x + width) - (obj.x + obj.width);
            float overlapY = (y + height) - (obj.y + obj.height);

            x += overlapX;
            y += overlapY;
        }
    }

    public class Rect
    {
        public float width;
        public float height;
        public float x;
        public float y;

        public Rect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool Contains(Collider obj) => (x + width > obj.rX && x < obj.rX + obj.width && y + height > obj.rY && y < obj.rY + obj.height);
    }
}