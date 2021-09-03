// using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace physics
{
    public class Physics
    {
        private List<Collider> colliders;

        public List<QuadTree> qTrees;
        public float worldWidth { get; private set; }
        public float worldHeight { get; private set; }

        public Physics()
        {
            colliders = new List<Collider>();
            // qTrees = new List<QuadTree>();
        }

        public void SetWorldBounds(float width, float height)
        {
            worldWidth = width;
            worldHeight = height;
        }

        public void AddCollider(Collider collider)
        {
            colliders.Add(collider);
        }

        public void Update()
        {
            qTrees = new List<QuadTree>();

            qTrees.Add(new QuadTree(new Rect(0, 0, worldWidth / 2, worldHeight / 2), 5));
            qTrees.Add(new QuadTree(new Rect(0, 0, worldWidth / 2, worldHeight / 2), 5));
            qTrees.Add(new QuadTree(new Rect(worldWidth / 2, 0, worldWidth / 2, worldHeight / 2), 5));
            qTrees.Add(new QuadTree(new Rect(0, worldHeight / 2, worldWidth / 2, worldHeight / 2), 5));

            foreach(Collider col in colliders)
            {
                col.Update();
                qTrees[0].Insert(col);
                qTrees[1].Insert(col);
                qTrees[2].Insert(col);
                qTrees[3].Insert(col);
            }
            
            foreach(QuadTree qTree in qTrees)
            {
                int lastCol = 0;
                foreach(Collider col in qTree.objects)
                {
                    var colA = col;
                    for(int i = lastCol; i < qTree.objects.Count; i++)
                    {
                        var colB = qTree.objects[i];
                        if(colA != colB)
                        {
                            if(colA.hasCollided(colB))
                            {
                                colA.obj.hasCollided = true;
                                colA.obj.direction.X *= -1;
                                colA.obj.direction.Y *= -1;
                                colA.obj.HandleCollision(colB.obj);

                                colB.obj.hasCollided = true;
                                colB.obj.direction.X *= -1;
                                colB.obj.direction.Y *= -1;
                                colB.obj.HandleCollision(colA.obj);
                                break;
                            }
                        }
                    }
                    lastCol++;
                }
            }
        }
    }

    public class QuadTree
    {
        public Rect boundary { get; private set; }
        public int capacity { get; private set; }
        public List<Collider> objects { get; private set; }
        public List<QuadTree> subTrees;
        // private bool divided = false;
        private Physics physicsObject;

        public QuadTree(Rect boundary, int capacity)
        {
            this.boundary = boundary;
            this.capacity = capacity;

            objects = new List<Collider>();
        }

        public void AttachPhysicsObject(Physics physics)
        {
            physicsObject = physics;
        }

        public void Subdivide()
        {
            subTrees = new List<QuadTree>();

            // nw
            subTrees.Add(new QuadTree(new Rect(boundary.x, boundary.y, boundary.width / 2, boundary.height / 2), capacity));
            // ne
            subTrees.Add(new QuadTree(new Rect(boundary.x + boundary.width / 2, boundary.y, boundary.width / 2, boundary.height / 2), capacity));
            // sw
            subTrees.Add(new QuadTree(new Rect(boundary.x, boundary.y + boundary.height / 2, boundary.width / 2, boundary.height / 2), capacity));
            // se
            subTrees.Add(new QuadTree(new Rect(boundary.x + boundary.width / 2, boundary.y + boundary.height / 2, boundary.width / 2, boundary.height / 2), capacity));
            
            // divided = true;
        }

        public void Insert(Collider obj)
        {
            if(!boundary.Contains(obj)) return;

            // if(objects.Count < capacity)
            // {
            objects.Add(obj);
                // return;
            // }

            // if(objects.Count >= capacity && !divided)
            // {
            //     Subdivide();
            // }

            // subTrees[0].Insert(obj);
            // subTrees[1].Insert(obj);
            // subTrees[2].Insert(obj);
            // subTrees[3].Insert(obj);
        }
    }

    public abstract class Collider
    {
        public float x;
        public float y;
        public float rX;
        public float rY;
        public float width;
        public float height;
        public GameObject obj { get; protected set; }

        public Collider(float x, float y, float width, float height, GameObject obj)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.obj = obj;
            this.rX = this.obj.x - this.x;
            this.rY = this.obj.y - this.y;
        }

        public void Update()
        {
            rX = obj.x - this.x;
            rY = obj.y - this.y;
            obj.hasCollided = false;
        }

        public virtual bool hasCollided(Collider b) => false;
    }

    public class BoxCollider : Collider
    {
        public BoxCollider(float x, float y, float width, float height, GameObject obj) : base(x, y, width, height, obj) {}

        public override bool hasCollided(Collider b)
        {
            var aX = rX;
            var aY = rY;
            var aW = width;
            var aH = height;

            var bX = b.rX;
            var bY = b.rY;
            var bW = b.width;
            var bH = b.height;

            if(aX + aW >= bX &&
                aX <= bX + bW &&
                aY + aH >= bY &&
                aY <= bY + bH)
            {
                return true;
            }
            return false;
        }
    }
}