using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{

    public Transform startPoint, middlePoint, endPoint;
    public int numberOfSteps;
    public Vector3 GetPoint(float pos) //here we can give the lightsphere's rotation to the bezier curve. neat.
    {
        float t = pos / (numberOfSteps - 1f);

        Vector3 p0 = startPoint.position;
        Vector3 p1 = middlePoint.position;
        Vector3 p2 = endPoint.position;

        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }

}
