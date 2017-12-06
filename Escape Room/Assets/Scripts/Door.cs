using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerTracker tracker;
    //public RoomSwitcher switcher;
    LocDirID locationDirection = new LocDirID();
    public SwitchCondition openCondition;
    public SwitchCondition closeCondition;
    public bool isOpen, conditionFulfilled;
    bool secondCondition;
    public Transform board, pivot;

    void Update()
    {
        locationDirection = tracker.GetLocationAndDirection();
        //keyboard input
        /*if (Input.GetKeyDown("space"))
        {
            ActivateDoor();
        }*/

        if(CheckCondition(openCondition) && !conditionFulfilled) {
            ActivateDoor();
            conditionFulfilled = true;
        }

        if(closeCondition.tile != null) {
            if(CheckCondition(closeCondition) && !secondCondition) {
                ActivateDoor();
                secondCondition = true;
            }
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

    bool CheckCondition(LocDirID condition)
    {
        return (locationDirection.tile == condition.tile);
    }

    bool CheckCondition(SwitchCondition condition)
    {
        if (condition.tile.Length == 1) //if we only have one tile condition we can just use the old one, no need to enter a for loop..
        {
            return CheckCondition(new LocDirID(condition.tile[0], condition.direction));
        }
        else
        {
            bool tile = false; //Initializes the standing condition as false
            for (int i = 0; i < condition.tile.Length; i++) //The for runs til the last standing condition
            {
                if (locationDirection.tile == condition.tile[i]) //if we're standing on an "allowed" tile
                {
                    tile = true; //standing condition is true
                    break; //we don't have to check anymore
                }
            }
            return (tile);
        } //since we're standing on the correct tile, we check for looking tile and return the result
    }

}
