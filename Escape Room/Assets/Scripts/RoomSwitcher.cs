using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{

    public PlayerTracker tracker;
    LocDirID locationDirection = new LocDirID();

    public GameObject[] Room;
    public LocDirID[] switchCondition;
    int currentRoom = 1; //could make public if we want to start in different rooms I guess.
    int maxRooms;

    // Use this for initialization
    void Start()
    {
        maxRooms = Room.Length;
    }

    void Update()
    {
        locationDirection = tracker.GetLocationAndDirection();
        Debug.Log(locationDirection.tile.ToString() + " , " + locationDirection.dir.ToString());

        if (currentRoom < maxRooms - 1) //It doesn't check for the switch condition in the last room.
        {
            if (CheckCondition(switchCondition[currentRoom - 1]))
            {
                SwitchRoom();
                currentRoom++;
            }
        }
    }

    void SwitchRoom()
    {
        //double safeguard, I've fixed overflowing in Update() but I'll do it here in case we call it somewhere else
        if (currentRoom < maxRooms - 1 && currentRoom != 0) //It doesn't change if you're already in the last room, or if the current room is set to be the 1st corridor
        {
            Room[currentRoom - 1].SetActive(false);
            Room[currentRoom + 1].SetActive(true);
        }
    }

    bool CheckCondition(LocDirID condition)
    {
        return (locationDirection.tile == condition.tile && locationDirection.dir == condition.dir);
    }
}