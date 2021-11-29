using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleterBehaviour : MonoBehaviour
{
    public delegate void DeleterDelegate();
    public static event DeleterDelegate DeleterActivated;

    private void OnTriggerEnter(Collider other)
    {
        // check if this object is supposed to be deleted
        if (other.CompareTag("Environment") || other.CompareTag("Collectible"))
        {
            // delete the other object
            Destroy(other.gameObject);

            // fire the DeleterActivated event
            if (DeleterActivated != null) DeleterActivated();
        }
    }
}
