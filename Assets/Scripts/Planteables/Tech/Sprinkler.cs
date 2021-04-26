using FarmSim.Attributes;
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
    ///         Manages a sprinkler water source and what nodes surrounding it.
    ///         
    ///         <para>This GameObject is always instantiated after post load has occured, meaning all interactables have already been loaded in.</para>
    ///     </summary>
    /// </class>
    [Savable(false)]
    public class Sprinkler : MonoBehaviour, ITimeBased, IInteractable, ISavable, ITech
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

        private const string SPRINKLER_PREFAB_PATH = "Sprinkler";

        public TechData Data { private get; set; }

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
            InitNodeInfo();

            // if this sprinkler was not loaded from save
            if (Data == null)
            {
                Data = new TechData(transform.position, SPRINKLER_PREFAB_PATH, Guid.NewGuid().ToString());
                InitSurroundings();
            }
        }

        private void InitSurroundings()
        {
            // we init the surroundings once this Sprinkler is created which is always after data has been injected and interactables initiated.
            ModifyAsWaterSource(true);
        }

        private void InitNodeInfo()
        {
            middleNode = nodeGrid.GetNodeFromVector2(transform.position);
            X = middleNode.Data.x;
            Y = middleNode.Data.y;

            Debug.Log(middleNode.Interactable);

            // store the nodes interactable that we will replace
            prevInteractable = middleNode.Interactable;
            middleNode.Interactable = this;

            nodesToWater = nodeGrid.GetNodesFromDimensions(middleNode, xWaterDim, yWaterDim);
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
                if (n == middleNode)
                    continue;

                // if the interactable is able to hold reference to waterSources
                if (n.Interactable is IWaterSourceRefsGUIDs waterSources)
                {
                    if (add)
                    {
                        // add this water source as reference
                        waterSources.AddToWaterSources(Data.guid);
                    }
                    else
                    {
                        // remove this water source from reference
                        waterSources.RemoveFromWaterSources(Data.guid);
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
            sprinklerItem.SpawnWorldItem(transform.position, 1);

            Node middleNode = nodeGrid.GetNodeFromXY(X, Y);

            // reassign to the previous interactable
            middleNode.Interactable = prevInteractable;

            // remove as a water source from neighbours
            ModifyAsWaterSource(false);
            SectionData.Current.techDatas.Remove(Data);

            Destroy(gameObject);
        }

        private void Sprinkle()
        {
            StopCoroutine(AnimationCo());
            StartCoroutine(AnimationCo());
        }

        private IEnumerator AnimationCo()
        {
            animator.SetBool(animParamName, true);

            yield return animPlay;

            animator.SetBool(animParamName, false);
            
            // watering neighbours has to happen a bit after the initial time passes or the dirt may not be affected by it.
            WaterNeighbours();

            yield return animWait;

            StartCoroutine(AnimationCo());
        }

        public void Save()
        {
            if (!SectionData.Current.techDatas.Contains(Data))
            {
                SectionData.Current.techDatas.Add(Data);
            }

            StopCoroutine(AnimationCo());
        }
    }
}