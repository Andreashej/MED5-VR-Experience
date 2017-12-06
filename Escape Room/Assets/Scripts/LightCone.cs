using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCone : MonoBehaviour
{
    public LightRevealSource LRSource;
    LightSphere ls;

    void Update()
    {
        if (LRSource.activeLightID != -1)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            int sphereMask = LayerMask.GetMask("Sphere");
            if (Physics.Raycast(ray, out hitInfo, 100, sphereMask))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);

                ls = hitInfo.collider.GetComponentInParent<LightSphere>();
                ls.TurnLightOn();
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green); //Draws a green line if it hits anything else
                if (ls != null) ls.TurnLightOff();
            }
        }
        else if (ls != null) ls.TurnLightOff();
    }
}
