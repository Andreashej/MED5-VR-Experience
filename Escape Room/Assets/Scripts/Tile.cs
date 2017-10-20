using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public int ID;

    public void SetID(int initID)
    {
        ID = initID;
    }

    public int GetID()
    {
        return ID;
    }

	
}
