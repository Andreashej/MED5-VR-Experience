using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public Transform board, pivot;

    void Update()
    {

		//keyboard input
        if (Input.GetKeyDown("space"))
        {
            ActivateDoor();
        }
    }

    public void ActivateDoor()
    {
        if (isOpen) CloseDoor();
        else OpenDoor();
    }

    void OpenDoor()
    {
        if (isOpen) return;
		board.transform.RotateAround(pivot.position, Vector3.up, 90);
		isOpen = true;
    }

    void CloseDoor()
    {
        if (!isOpen) return;
		board.transform.RotateAround(pivot.position, Vector3.up, -90);
		isOpen = false;
    }

}
