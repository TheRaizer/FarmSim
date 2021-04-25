using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Items;
using FarmSim.Serialization;
using FarmSim.TimeBased;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private int xWaterDim = 3;
        [SerializeField] private int yWaterDim = 3;

        [Header("Animation Settings")]
        [SerializeField] private float animInterval = 10f;
        [SerializeField] private string animParamName = "Sprinkle";
        [SerializeField] private float animPlayTime = 3f;

        private Animator animator;
        private NodeGrid nodeGrid;

        private WaitForSeconds animWait;
        private WaitForSeconds animPlay;

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
            animWait = new WaitForSeconds(animInterval);
            animPlay = new WaitForSeconds(animPlayTime);
        }

        private void Start()
        {
            InitializeNodeInfo();

            middleNode = nodeGrid.GetNodeFromXY(X, Y);
            nodesToWater = nodeGrid.GetNodesFromDimensions(middleNode, xWaterDim, yWaterDim);

            ModifyAsWaterSource(true);

            Sprinkle();
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
            foreach (Node n in nodesToWater)
            {
                if (n == middleNode)
                    continue;
                n.Interactable.OnInteract(ToolTypes.WateringCan);
            }
        }

        public void OnTimePass(int daysPassed = 1)
        {
            Sprinkle();
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

        private void Sprinkle()
        {
            WaterNeighbours();
            StopCoroutine(AnimationCo());
            StartCoroutine(AnimationCo());
        }

        private IEnumerator AnimationCo()
        {
            animator.SetBool(animParamName, true);

            yield return animPlay;

            animator.SetBool(animParamName, false);

            yield return animWait;

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