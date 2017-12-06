using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightRevealSource : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private SteamVR_TrackedController controller;
    public Vector2 btnCoords;

    public Material reveal;
    public Light lightSource;

    public Texture redTexture;
    public Texture blueTexture;
    public Texture greenTexture;
    public int activeLightID;

    void Start()
    {
        activeLightID = -1;
        lightSource.intensity = 0;
        trackedObj = GetComponentInParent<SteamVR_TrackedObject>();
        controller = GetComponentInParent<SteamVR_TrackedController>();
        controller.PadClicked += Controller_BtnPress;
        reveal.SetFloat("_IntensityScalar", lightSource.intensity);
    }

    private void Controller_BtnPress(object sender, ClickedEventArgs e)
    {
        if (device.GetAxis().x != 0 || device.GetAxis().y != 0)
        {
            Debug.Log(device.GetAxis().x + " " + device.GetAxis().y);

            btnCoords = new Vector2(device.GetAxis().x, device.GetAxis().y);

            if (btnCoords.x < 0 && btnCoords.y > 0)
            {
                activeLightID = 1;
                lightSource.intensity = 5f;
                lightSource.color = Color.green;
                reveal.SetTexture("_DecalTex", greenTexture);
                reveal.SetFloat("_IntensityScalar", lightSource.intensity);
            }
            else if (btnCoords.x < 0 && btnCoords.y < 0)
            {
                activeLightID = 0;
                lightSource.intensity = 5f;
                lightSource.color = Color.red;
                reveal.SetTexture("_DecalTex", redTexture);
                reveal.SetFloat("_IntensityScalar", lightSource.intensity);
            }
            else if (btnCoords.x > 0 && btnCoords.y > 0)
            {
                activeLightID = 2;
                lightSource.intensity = 5f;
                lightSource.color = Color.blue;
                reveal.SetTexture("_DecalTex", blueTexture);
                reveal.SetFloat("_IntensityScalar", lightSource.intensity);
            }
            else if (btnCoords.x > 0 && btnCoords.y < 0)
            {
                activeLightID = -1;
                lightSource.intensity = 0;
                reveal.SetFloat("_IntensityScalar", lightSource.intensity);
            }
        }
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObj.index);
        /*
        if (Input.GetKey("r"))
        {
            lightSource.color = Color.red;
            reveal.SetTexture("_DecalTex", redTexture);
        }
        else if (Input.GetKey("b"))
        {
            lightSource.color = Color.blue;
            reveal.SetTexture("_DecalTex", blueTexture);
        }
        else if (Input.GetKey("g"))
        {
            lightSource.color = Color.green;
            reveal.SetTexture("_DecalTex", greenTexture);
        }*/

        reveal.SetVector("_LightPosition", lightSource.transform.position);
        reveal.SetVector("_LightDirection", lightSource.transform.forward);
        reveal.SetFloat("_LightAngle", lightSource.spotAngle);
        reveal.SetColor("_Color", lightSource.color);


    }
}
