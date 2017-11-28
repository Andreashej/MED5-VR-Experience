using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCone : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("we just hit " + collision.transform.name);
        LightSphere lightSphere = collision.GetComponentInParent<LightSphere>();
        if (lightSphere != null && transform.position.y >= collision.transform.position.y)
        {
            //Debug.Log("found lightsphere");
            lightSphere.TurnLightOn();
        }
    }

	void OnTriggerStay(Collider collision)
    {
        Debug.Log("we just hit " + collision.transform.name);
        LightSphere lightSphere = collision.GetComponentInParent<LightSphere>();
        if (lightSphere != null && transform.position.y >= collision.transform.position.y)
        {
            //Debug.Log("found lightsphere");
            lightSphere.TurnLightOn();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        print("No longer in contact with " + collision.transform.name);
        LightSphere lightSphere = collision.GetComponentInParent<LightSphere>();
        if (lightSphere != null)
        {
            lightSphere.TurnLightOff();
        }
    }
}
