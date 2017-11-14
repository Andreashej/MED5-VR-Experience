using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightRevealSource : MonoBehaviour {

    public Material reveal;
    public Light light;

    public Texture red;
    public Texture blue;
    public Texture yellow;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey("r"))
        {
            light.color = Color.red;
            reveal.SetTexture("_MainTex", red);
			reveal.SetColor("_Color", Color.red);
        } else if (Input.GetKey("b"))
        {
            light.color = Color.blue;
            reveal.SetTexture("_MainTex", blue);
			reveal.SetColor("_Color", Color.blue);
        } else if (Input.GetKey("y"))
        {
            light.color = Color.yellow;
            reveal.SetTexture("_MainTex", yellow);
			reveal.SetColor("_Color", Color.yellow);
        }
        else if(Input.GetKey("w"))
        {
            light.color = Color.white;
			reveal.SetColor("_Color", Color.white);
        }

        reveal.SetVector("_LightPosition", light.transform.position);
        reveal.SetVector("_LightDirection", light.transform.forward);
        reveal.SetFloat("_LightAngle", light.spotAngle);

    }
}