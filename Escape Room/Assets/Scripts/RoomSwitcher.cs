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

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        locationDirection = tracker.GetLocationAndDirection();
        Debug.Log(locationDirection.tile.ToString() + " , " + locationDirection.dir.ToString());

        //add a condition for max number of rooms to avoid array index out of bounds error
        if (CheckCondition(switchCondition[currentRoom]))
        {
            SwitchRoom();
            currentRoom++;
        }
    }

    void SwitchRoom()
    {
        Room[currentRoom].SetActive(false);
		Room[currentRoom+1].SetActive(true);
		//fix array overflow here too
    }

    bool CheckCondition(LocDirID condition)
    {
        return (locationDirection.tile == condition.tile && locationDirection.dir == condition.dir);
    }
}