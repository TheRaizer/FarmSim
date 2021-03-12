using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Utility
{
    /// <class name="ObjectPooler">
    ///     <summary>
    ///         Manages pools of <see cref="GameObject"/>'s that are accessible through a string id.
    ///     </summary>
    /// </class>
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField]
        private List<Pool> pools;

        private readonly Dictionary<string, Pool> poolDict = new Dictionary<string, Pool>();

        [System.Serializable]
        public class Pool
        {
            public string id;
            public int numOfGameObjects;
            public GameObject prefab;
            public Queue<GameObject> objects = new Queue<GameObject>();
        }

        public void Awake()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (Pool p in pools)
            {
                // instantiate a certain number of gameObjects, set them unactive, and add them to the corrosponding pool's queue.
                for (int i = 0; i < p.numOfGameObjects; i++)
                {
                    var gameObject = Instantiate(p.prefab);
                    gameObject.SetActive(false);
                    gameObject.transform.position = Vector3.zero;
                    p.objects.Enqueue(gameObject);
                }
                // add the pool to the dictionary with the pool's id as the key.
                poolDict.Add(p.id, p);
            }
        }

        public GameObject SpawnGameObject(string id, Vector2 position, Quaternion rotation)
        {
            Pool pool = poolDict[id];

            var gameObject = pool.objects.Dequeue();
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;

            gameObject.SetActive(true);

            pool.objects.Enqueue(gameObject);

            return gameObject;
        }
    }
}