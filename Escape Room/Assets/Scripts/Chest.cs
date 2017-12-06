using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject lid;
    public Transform tracker;
    Collider lidCollider;
    public bool isTrackerVersion, isLocked;

    void Start()
    {
        isLocked = true;
        lidCollider = lid.GetComponentInChildren<MeshCollider>();
        lidCollider.enabled = false;
    }

    void Update()
    {
        /* //Keyboard input for debugging
        if (Input.GetKeyDown("return"))
        {
            UnlockChest();
            Debug.Log("unlocled");
        } */


        //If it's the tracker version then the opened chest's lid position is tracked via a Vive tracker.
        if (isTrackerVersion)
        {
            if (!isLocked)
            {
                lid.transform.localRotation = Quaternion.Euler(-tracker.eulerAngles.x - 90, 0, 0);
            }
        }
    }


    //Method for opening the chest. Enables the collider so it can be grabbed in VR
    public void UnlockChest()
    {
        isLocked = false;
        lidCollider.enabled = true;
    }
}
