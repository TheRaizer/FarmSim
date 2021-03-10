using System.Collections.Generic;
using UnityEngine;

namespace FarmSim.Utility
{
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
                for (int i = 0; i < p.numOfGameObjects; i++)
                {
                    var gameObject = Instantiate(p.prefab);
                    gameObject.SetActive(false);
                    gameObject.transform.position = Vector3.zero;
                    p.objects.Enqueue(gameObject);
                }
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