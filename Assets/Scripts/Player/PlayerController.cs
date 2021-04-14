using FarmSim.Attributes;
using FarmSim.Entity;
using FarmSim.Enums;
using FarmSim.Grid;
using FarmSim.Serialization;
using System;
using UnityEngine;

namespace FarmSim.Player
{
    /// <class name="PlayerController">
    ///     <summary>
    ///         Controls and manages the components of the Player.
    ///     </summary>
    /// </class>
    [Savable(true)]
    public class PlayerController : MonoBehaviour, ISavable, ILoadable
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject tileRing;
        [SerializeField] private GameObject inventoryUI;

        public Action OnPlant { private get; set; }
        public ToolTypes ToolToUse { get; set; }

        private Animator animator;
        private EntityPathFind pathFind;

        private void Awake()
        {
            pathFind = new EntityPathFind(TriggerAnimation, gameObject, speed);
            //to hide the curser
            Cursor.visible = false;
            animator = GetComponent<Animator>();
        }

        private void LateUpdate()
        {
            KeyHandler();
            ChangeRingPosition();
        }

        private void KeyHandler()
        {
            if (!pathFind.HasPath())
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    pathFind.StopMoving();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                pathFind.RequestPath();
            }
            pathFind.MoveOnPath();
        }

        private void ChangeRingPosition()
        {
            Node node = NodeGrid.Instance.GetNodeFromMousePosition();
            if (node != null)
            {
                Vector2 pos = node.Data.pos;
                tileRing.transform.position = pos;
            }
        }

        private bool CheckForPlanting()
        {
            // if there is an action for on plant that means we must plant something
            if (OnPlant != null)
            {
                // invoke the action
                OnPlant.Invoke();

                // empty the action
                OnPlant = null;

                return true;
            }

            return false;
        }

        public void TriggerAnimation()
        {
            // if we have planted return
            if (CheckForPlanting())
                return;


            // otherwise check if a tool was used
            switch (ToolToUse)
            {
                case ToolTypes.Hoe:
                    animator.SetTrigger("Hoe");
                    break;
                case ToolTypes.WateringCan:
                    animator.SetTrigger("Water");
                    break;
                case ToolTypes.Sickle:
                    animator.SetTrigger("Sickle");
                    break;
                default:
                    break;
            }
        }

        public void Save()
        {
            PlayerData.Current.position = transform.position;
        }

        public void Load()
        {
            if (PlayerData.Current.position == Vector2.zero)
                return;

            transform.position = PlayerData.Current.position;
        }
    }
}
