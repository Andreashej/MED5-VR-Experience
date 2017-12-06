using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCone : MonoBehaviour
{
    public LightRevealSource LRSource;
    LightSphere ls;

    void Update()
    {
        if (LRSource.activeLightID != -1) //If the light is on
        {
            Ray ray = new Ray(transform.position, transform.forward); //Raycast initialisation
            RaycastHit hitInfo;
            int sphereMask = LayerMask.GetMask("Sphere"); //layermask so the ray only hits the lightsphere
            if (Physics.Raycast(ray, out hitInfo, 100, sphereMask)) //if we hit
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);

                ls = hitInfo.collider.GetComponentInParent<LightSphere>();
                ls.TurnLightOn(); //turn light on
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green); //Draws a green line if it doesn't hit anything
                if (ls != null) ls.TurnLightOff(); //If we already hit the sphere once turn off the light
            }
        }
        else if (ls != null) ls.TurnLightOff(); //if the light is off and we hit the sphere already (it exists), turn the light off
    }
}
