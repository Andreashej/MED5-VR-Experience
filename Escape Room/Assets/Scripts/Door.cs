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
        //keyboard input for debugging
        /*if (Input.GetKeyDown("space"))
        {
            ActivateDoor();
        }*/

        //The first trigger condition of the door
        if(CheckCondition(openCondition) && !conditionFulfilled) {
            ActivateDoor();
            conditionFulfilled = true;
        }

        //The second trigger condition of the door
        if(closeCondition.tile != null) {
            if(CheckCondition(closeCondition) && !secondCondition) {
                ActivateDoor();
                secondCondition = true;
            }
        }

    }

    //Opens or closes the door
    public void ActivateDoor()
    {
        if (isOpen) CloseDoor();
        else OpenDoor();
    }

    //Opens the door
    void OpenDoor()
    {
        if (isOpen) return;
        board.transform.RotateAround(pivot.position, Vector3.up, 90);
        isOpen = true;
    }

    //Closes the door
    void CloseDoor()
    {
        if (!isOpen) return;
        board.transform.RotateAround(pivot.position, Vector3.up, -90);
        isOpen = false;
    }

    //Same method as in the RoomSwitcher class but ignores look direction
    bool CheckCondition(LocDirID condition)
    {
        return (locationDirection.tile == condition.tile);
    }

    //Same method as in the RoomSwitcher class but ignores look direction
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
            return (tile); //we return the value of "tile" after the loop, if we broke out of the loop it'll be true otherwise false
        }
    }

}
