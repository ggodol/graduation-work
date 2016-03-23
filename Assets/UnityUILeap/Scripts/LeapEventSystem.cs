using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This scripts allows interactions with objects in the scene based on a gaze.
/// This class casts a ray into the scene and if it finds a VRIneractive item it 
/// calls the events for the item to use.
/// need help? contact us: babilinapps.com
/// </summary>


public  class LeapEventSystem : MonoBehaviour
    {
     
    public  event Action<RaycastHit> OnRaycasthit;                   // This event is called every frame that the user's gaze is over a collider.

    [Tooltip("Layers excluded from the raycast.")]
    [SerializeField]
    private LayerMask _exclusionLayers;        
       
    [Tooltip("Adds ‘leap Interactive Item’ script to all UI objects.")]
    [SerializeField]
     private bool AutoAddLeapInteract = true;



    [HideInInspector]
    public static PointerEventData eventSystem;                           // Is used to send simple events
    [HideInInspector]
    public static LeapInteractiveItem _CurrentInteractible;                //The current interactive item
    [HideInInspector]
    public static LeapInteractiveItem _LastInteractible;                   //The last interactive item
    [HideInInspector]
    public static bool clicking;                            // calls action if time has passed or button is pressed.




  
        public delegate void Hover();
        public static event Hover OnHover;
        public delegate void Deselect();
        public static event Deselect OnDeselect;

        public static LeapEventSystem curEventSystem;
        
    // TODO Utility for other classes to get the current interactive item
    public LeapInteractiveItem CurrentInteractible
        {
            get { return _CurrentInteractible; }
        }


        void Awake()
        {
       

            if (curEventSystem == null)
            {
                curEventSystem = this;
                DontDestroyOnLoad(gameObject);

            }else
            {
                this.enabled = false;
            }

        SetUp();
    }

    void SetUp() {



        if(AutoAddLeapInteract)
        FindGameObjectsWithLayer(LayerMask.NameToLayer("UI"));

    }

        void Start()
        {
            //Gets the main Camera

            //Gets active event system
            eventSystem = new PointerEventData(EventSystem.current);




     

            UnityEngine.VR.InputTracking.Recenter(); // recenters the VR input
        }



        //Desctivates last 
        public static void DeactiveLastInteractible()
        {
            
            if (_LastInteractible == null)
                return;

            ExecuteEvents.Execute(_LastInteractible.gameObject, eventSystem, ExecuteEvents.deselectHandler);
          
            _LastInteractible = null;
            if (OnDeselect != null)
            {
                OnDeselect();
            }
        }

        // Deselects all 
        private static void HandleDeselect()
        {
           
            if (_LastInteractible == null)
                return;
            ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.deselectHandler);
            _LastInteractible = _CurrentInteractible;
            _CurrentInteractible = null;
            if(OnDeselect != null)
            {
                OnDeselect();
            }
        }

        //Hover Action
        public static void HandleHover()
        {
            if (_CurrentInteractible == null || clicking == true)
                return;


            if (OnHover != null) // Alows other scripts to subscribe to the  hover event.
                OnHover();

          ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.dragHandler);


        }

        //Key Up
        private void HandleUp()
        {
            if (_CurrentInteractible != null)
            {
               
                ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerUpHandler);
            }
        }

        //Key Down
        private void HandleDown()
        {
            if (_CurrentInteractible != null)
            {
              
              ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerDownHandler);
            }
        }

        //Click action ( button down)
        public static void HandleClick(MonoBehaviour script)
        {
      
            if (_CurrentInteractible != null)
            {
               
                if (clicking == false)
                {
                script.StartCoroutine(preformClick());
                    clicking = true;
                }
            }

        }

        //Makes sure that the press is rendered
       static IEnumerator preformClick()
        {
        if (_CurrentInteractible && !eventSystem.used && _CurrentInteractible.gameObject)
        {
            yield return new WaitForEndOfFrame();
            ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.deselectHandler);
            yield return new WaitForEndOfFrame();
            ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerDownHandler);
            yield return new WaitForEndOfFrame();
            ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerUpHandler);
            yield return new WaitForEndOfFrame();
            ExecuteEvents.Execute(_CurrentInteractible.gameObject, eventSystem, ExecuteEvents.pointerClickHandler);
        }
            clicking = false;
            HandleDeselect();

        }




        //Gets the active VREventSystem usefull for public bools
        public static LeapEventSystem GetCurrent()
        {
            return curEventSystem;
        }


        public void FindGameObjectsWithLayer (int layer) {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        for (int i = 0; i < goArray.Length; i++)
        { if ((goArray[i].layer == layer && !goArray[i].GetComponent<LeapInteractiveItem>()) && 
                (goArray[i].GetComponent<Toggle>() || goArray[i].GetComponent<Button>() || goArray[i].GetComponent<Slider>()
                || goArray[i].GetComponent<Scrollbar>() || goArray[i].GetComponent<EventTrigger>() || goArray[i].GetComponent<Dropdown>()))
            {
                goArray[i].AddComponent<LeapInteractiveItem>();
            }
        }
 
    }




        void DisableEvents()
        {
           curEventSystem.enabled = false;
        }
    }


    

