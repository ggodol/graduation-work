using UnityEngine;


public class assistant : MonoBehaviour {
    [SerializeField]
    private bool thumb = false;
    [SerializeField]
    private bool index = true;
    [SerializeField]
    private bool middle = true;
    [SerializeField]
    private bool pinky = false;
    [SerializeField]
    private bool ring = false;

	// Update is called once per frame
	void Update () {
        //attaches the EventActtions script to the hands
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.name == "bone3" && !child.GetComponent<EventActions>())
            {
         
               if(thumb && child.parent.name == "thumb")
                child.gameObject.AddComponent<EventActions>();

                if (index && child.parent.name == "index")
                    child.gameObject.AddComponent<EventActions>();

                if (middle && child.parent.name == "middle")
                    child.gameObject.AddComponent<EventActions>();

                if (pinky && child.parent.name == "pinky")
                    child.gameObject.AddComponent<EventActions>();

                if (ring && child.parent.name == "ring")
                    child.gameObject.AddComponent<EventActions>();

            }
        }
    }
}
