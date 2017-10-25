using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{

    public PlayerTracker tracker;
    LocDirID locationDirection = new LocDirID();

    public GameObject[] Room;
    public LocDirID[] switchCondition;
    int currentRoom = 0; //could make public if we want to start in different rooms I guess.
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
            if (CheckCondition(switchCondition[currentRoom]))
            {
                SwitchRoom();
                currentRoom++;
            }
        }
    }

    void SwitchRoom()
    {
        //double safeguard, I've fixed overflowing in Update() but I'll do it here in case we call it somewhere else
        if (currentRoom < maxRooms - 1) //It doesn't change if you're already in the last room.
        {
            Room[currentRoom].SetActive(false);
            Room[currentRoom + 1].SetActive(true);
        }
    }

    bool CheckCondition(LocDirID condition)
    {
        return (locationDirection.tile == condition.tile && locationDirection.dir == condition.dir);
    }
}