using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightRevealSource : MonoBehaviour
{

    public Material reveal;
    public Light lightSource;

    public Texture redTexture;
    public Texture blueTexture;
    public Texture greenTexture;
    void Update()
    {

        if (Input.GetKey("r"))
        {
            lightSource.color = Color.red;
            reveal.SetTexture("_MainTex", redTexture);
            reveal.SetColor("_Color", Color.red);
        }
        else if (Input.GetKey("b"))
        {
            lightSource.color = Color.blue;
            reveal.SetTexture("_MainTex", blueTexture);
            reveal.SetColor("_Color", Color.blue);
        }
        else if (Input.GetKey("y"))
        {
            lightSource.color = Color.green;
            reveal.SetTexture("_MainTex", greenTexture);
            reveal.SetColor("_Color", Color.green);
        }
        else if (Input.GetKey("w"))
        {
            lightSource.color = Color.white;
            reveal.SetColor("_Color", Color.white);
        }

        reveal.SetVector("_LightPosition", lightSource.transform.position);
        reveal.SetVector("_LightDirection", lightSource.transform.forward);
        reveal.SetFloat("_LightAngle", lightSource.spotAngle);

    }
}