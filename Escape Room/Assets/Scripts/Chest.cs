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
        if(Input.GetKeyDown("return")){
            UnlockChest();
            Debug.Log("unlocled");
        }
        if (isTrackerVersion)
        {
            if (!isLocked)
            {
                lid.transform.localRotation = Quaternion.Euler(-tracker.eulerAngles.x-90, 0,0);
            }
        }
    }

    public void UnlockChest()
    {
        isLocked = false;
        lidCollider.enabled = true;
    }
}
