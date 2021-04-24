using FarmSim.TimeBased;
using FarmSim.Grid;
using FarmSim.Items;
using FarmSim.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FarmSim.Serialization;

namespace FarmSim.Planteables
{
    /// <class name="Sprinkler">
    ///     <summary>
    ///         Manages a sprinkler water source and what nodes it interacts with.
    ///     </summary>
    /// </class>
    public class Sprinkler : MonoBehaviour, ITimeBased, IInteractable, ISavable, ILoadable
    {
        [SerializeField] private ItemType sprinklerItem;

        [Header("Sprinkling distance")]
        [SerializeField] private int xDist = 1;
        [SerializeField] private int yDist = 1;

        [Header("Animation Settings")]
        [SerializeField] private float animationInterval = 15f;
        [SerializeField] private string animationName = "Sprinkle";

        private Animator animator;
        private NodeGrid nodeGrid;

        private readonly WaitForSeconds initialWait = new WaitForSeconds(1.5f);
        private WaitForSeconds animationWait;

        /// <summary>
        ///     The previous <see cref="IInteractable"/> that was held in the <see cref="Node"/> this sprinkler lies on.
        ///     
        ///     <para>
        ///         The <see cref="Node"/>'s <see cref="IInteractable"/> is reassigned  to this when the sprinkler is removed.
        ///     </para>
        /// </summary>
        private IInteractable prevInteractable;

        public int X { get; set; }
        public int Y { get; set; }

        private readonly string guid = Guid.NewGuid().ToString();

        private List<Node> nodesToWater;
        private Node middleNode;

        private void Awake()
        {
            nodeGrid = FindObjectOfType<NodeGrid>();
            animator = GetComponent<Animator>();
            animationWait = new WaitForSeconds(animationInterval);

            middleNode = nodeGrid.GetNodeFromXY(X, Y);
            nodesToWater = nodeGrid.GetNodesFromDimensions(middleNode, xDist, yDist);

            InitializeNodeInfo();
            ModifyAsWaterSource(true);
        }

        private void InitializeNodeInfo()
        {
            Node node = nodeGrid.GetNodeFromVector2(gameObject.transform.position);
            X = node.Data.x;
            Y = node.Data.y;
            prevInteractable = node.Interactable;
            node.Interactable = this;
        }

        /// <summary>
        ///     Either add or remove as a water source from <see cref="IInteractable"/>'s that implement <see cref="IWaterSourceRefsGUIDs"/>.
        /// </summary>
        /// <param name="add">Whether to add or remove water source.</param>
        private void ModifyAsWaterSource(bool add)
        {
            if (middleNode == null)
                Debug.LogError("Middle node of a sprinkler was null");

            foreach (Node n in nodesToWater)
            {
                // if the interactable is able to hold reference to waterSources
                if (n.Interactable is IWaterSourceRefsGUIDs waterSources)
                {
                    if (add)
                    {
                        // add this water source as reference
                        waterSources.WaterSrcGuids.Add(guid);
                    }
                    else
                    {
                        // remove this water source from reference
                        waterSources.WaterSrcGuids.Remove(guid);
                    }
                }
            }
        }

        public void WaterNeighbours()
        {
            foreach(Node n in nodesToWater)
            {
                n.Interactable.OnInteract(ToolTypes.WateringCan);
            }
        }

        public void OnTimePass(int daysPassed = 1)
        {
            WaterNeighbours();
            StartCoroutine(SprinkleCo());
        }

        public void OnInteract(ToolTypes toolType, GameObject gameObject = null, Action onSuccessful = null)
        {
            if (toolType == ToolTypes.Hoe)
            {
                RemoveSprinkler();
            }
        }

        private void RemoveSprinkler()
        {
            // drop the world item
            var worldItem = Instantiate(sprinklerItem.WorldItemPrefab);
            worldItem.transform.position = gameObject.transform.position;

            Node middleNode = nodeGrid.GetNodeFromXY(X, Y);

            // reassign to the previous interactable
            middleNode.Interactable = prevInteractable;

            // remove as a water source from neighbours
            ModifyAsWaterSource(false);

            Destroy(gameObject);
        }

        private IEnumerator SprinkleCo()
        {
            yield return initialWait;

            StopCoroutine(AnimationCo());
            StartCoroutine(AnimationCo());
        }

        private IEnumerator AnimationCo()
        {
            animator.SetTrigger(animationName);

            yield return animationWait;

            StartCoroutine(AnimationCo());
        }
        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}