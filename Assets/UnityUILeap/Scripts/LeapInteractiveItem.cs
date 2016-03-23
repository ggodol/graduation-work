using UnityEngine;
using System;
using UnityEngine.UI;


// This class should be added to any gameobject in the scene
// that should react to input based on the user's gaze.
// It contains events that can be subscribed to by classes that
// need to know about input specifics to this gameobject.


    [RequireComponent(typeof(BoxCollider))]
    public class LeapInteractiveItem : MonoBehaviour
    {
        private bool autoScale = true; // scale the Collider with the button.

        [HideInInspector]
        public Transform finger;
        [HideInInspector]
        public Toggle _toggle;

        protected bool m_IsOver;
        public bool IsOver
        {
            get { return m_IsOver; }              // Is the gaze currently over this object?
        }

       
        void Start() // Scales the colider correctly on buttons
        {
            if (GetComponent<RectTransform>() != null && autoScale == true)
            {
                RectTransform Rtransform = GetComponent<RectTransform>();
                transform.GetComponent<BoxCollider>().size = Rtransform.rect.size;
            transform.GetComponent<BoxCollider>().isTrigger = true;
            }

        _toggle = GetComponent<Toggle>();
    }

    void LateUpdate() {
        if (transform.Find("Dropdown List")) {
            Transform[] Children = transform.Find("Dropdown List").GetComponentsInChildren<Transform>();
            foreach (Transform _child in Children) {
                if(_child)
                if (!_child.GetComponent<LeapInteractiveItem>() && _child.GetComponent<Toggle>()) {
                       if(_child.gameObject)
                    _child.gameObject.AddComponent<LeapInteractiveItem>();
                }
            }

        }
    }

}
