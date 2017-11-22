using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{

    public Transform startPoint, middlePoint, endPoint, target;
    public int numberOfSteps;
    [Range(0, 360)] //steps capped at 360, a full rotation
    public int currentPos;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //this is to check if GetPoint works, it does. The target gameobject is not necessary, we'll use the LookAt method for a spotlight in the sphere
        target.position = GetPoint((float)currentPos);


        //This here would be for drawing the bezier curve in the inspector, but I couldn't get it work yet
        /*Vector3 position;
        for (int i = 0; i < numberOfSteps; i++)
        {
            position = GetPoint(i / (float)numberOfSteps - 1f);
        }*/
    }

    public Vector3 GetPoint(float pos) //here we can give the lightsphere's rotation to the bezier curve. neat.
    {
        float t = pos / (numberOfSteps - 1f);

        Vector3 p0 = startPoint.position;
        Vector3 p1 = middlePoint.position;
        Vector3 p2 = endPoint.position;

        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }

}
