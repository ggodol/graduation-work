using UnityEngine;
using System.Collections;

public class EventActions : MonoBehaviour {
    private bool _isHit = false;
    private LeapInteractiveItem interactible;
    // Update is called once per frame

    void OnTriggerStay(Collider col) {

        // Create a ray that points forwards from the camera.

        if(col.gameObject.layer == LayerMask.NameToLayer("UI") && !_isHit) { 

        // Do the raycast forweards to see if we hit an interactive item

             interactible = col.GetComponent<LeapInteractiveItem>(); //attempt to get the LeapInteractiveItem on the hit object
            if (interactible._toggle)
                return;
                if (interactible.finger) {
                if(interactible.finger != transform)
                    return;
            }
            Action(col);



        }
       
}
    void OnTriggerEnter(Collider col)
    {

        // Create a ray that points forwards from the camera.
        if (col.gameObject.layer == LayerMask.NameToLayer("UI") && !_isHit)
        {

            // Do the raycast forweards to see if we hit an interactive item

            interactible = col.GetComponent<LeapInteractiveItem>(); //attempt to get the LeapInteractiveItem on the hit object
            if (!interactible._toggle)
                return;
            if (interactible.finger)
            {
                if (interactible.finger != transform)
                    return;
            }

            Action(col);


        }

    }

    void OnTriggerExit(Collider col)
    {
        LeapInteractiveItem _button = col.GetComponent<LeapInteractiveItem>();
        if (_button) {
            
            if (_button.finger == transform) {
                _button.finger = null;
            }
        }

    }



    void Action(Collider col) {

        interactible.finger = transform;
        LeapEventSystem._CurrentInteractible = interactible;
        //Sets the mouse positon at the colided point
        LeapEventSystem.eventSystem.position = transform.position;

        // If we hit an interactive item and it's not the same as the last interactive item, then call Over
        if (interactible && interactible != LeapEventSystem._LastInteractible)
        {

            LeapEventSystem.DeactiveLastInteractible();
        }


        LeapEventSystem.HandleHover();




        //We update the last interactible item
        LeapEventSystem._LastInteractible = interactible;


        LeapEventSystem.HandleClick(this);
        StartCoroutine(wait());
        _isHit = true;



    }
    private IEnumerator wait() {
        yield return new WaitForSeconds(.5f);
        LeapEventSystem.DeactiveLastInteractible();
        _isHit = false;

    }

    }

