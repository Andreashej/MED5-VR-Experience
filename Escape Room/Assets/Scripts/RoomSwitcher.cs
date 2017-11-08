using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitcher : MonoBehaviour
{

    public PlayerTracker tracker;
    LocDirID locationDirection = new LocDirID();

    public GameObject[] Room;
    public LocDirID startPosition;
    public SwitchCondition[] switchCondition;
    public int currentRoom = 0; //could make public if we want to start in different rooms I guess.
    int maxRooms;

    // Use this for initialization
    void Start()
    {
        maxRooms = Room.Length;
    }

    void Update()
    {
        locationDirection = tracker.GetLocationAndDirection();
        //Debug.Log(locationDirection.tile.ToString() + " , " + locationDirection.dir.ToString());

        if (currentRoom == 0)
        {
            if (CheckCondition(startPosition))
            {
                StartRooms();
                currentRoom = 2;
            }
        }
        if (currentRoom != 0 && currentRoom < maxRooms - 1) //It doesn't check for the switch condition in the last room.
        {
            if (CheckCondition(switchCondition[currentRoom - 2]))
            {
                SwitchRoom();
                currentRoom++;
            }
        }
    }

    void StartRooms(){
        Room[0].SetActive(false);
        Room[1].SetActive(true);
        Room[2].SetActive(true);
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

    //Old conditioncheck with only one tile for standing. We can use this for calibration.
    bool CheckCondition(LocDirID condition)
    {
        return (locationDirection.tile == condition.tile && locationDirection.dir == condition.dir);
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
            return (tile && (locationDirection.dir == condition.direction));
        } //since we're standing on the correct tile, we check for looking tile and return the result
    }
}

[System.Serializable]
public struct SwitchCondition
{
    public int[] tile; //So multiple tiles can be used for standing condition
    public int direction;
}