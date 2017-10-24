using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{

    public Transform trackedObject;
    LocDirID locationDirection = new LocDirID();
    Coroutine lastCoroutine = null;


    void Start()
    {
        if (lastCoroutine == null)
        {
            lastCoroutine = StartCoroutine(CheckLocationAndDirection());
        }
    }

    // void Update()
    // {
    //     if (locationDirection.tile == 28 && locationDirection.dir == 100) //Test conditions, we'll remove this once proper condition handling is done
    //     {
    //         StopCoroutine(lastCoroutine);
    //     }
    // }

    void GetTileID()
    {
        Ray ray = new Ray(trackedObject.transform.position, Vector3.down);
        RaycastHit hitInfo;
        int tileMask = LayerMask.GetMask("Tile");

        if (Physics.Raycast(ray, out hitInfo, 100, tileMask)) //Casts a ray down that only takes the Tile layer into consideration, puts the ray information in hitInfo
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red); //Draws a red line if you hit a tile
            //Debug.Log(hitInfo.collider.GetComponent<Tile>().GetID()); //Writes the tile ID in the console (0 to 47 in this case)
            locationDirection.tile = hitInfo.collider.GetComponent<Tile>().GetID();
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green); //Draws a green line if it hits anything else
			locationDirection.tile = -1;
        }
    }

    void LookingAtTile()
    {
        Ray ray = new Ray(trackedObject.transform.position, trackedObject.transform.forward);
        RaycastHit hitInfo;
        int tileMask = LayerMask.GetMask("DirectionTile");

        if (Physics.Raycast(ray, out hitInfo, 100, tileMask))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            locationDirection.dir = hitInfo.collider.GetComponent<Tile>().GetID(); //Maybe make a separate script for wall tiles
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
            locationDirection.dir = -1;
        }

    }

    public IEnumerator CheckLocationAndDirection() //checking ID and look direction is now a coroutine, we can change the update time in the yield return
    {
        while (true)
        {
            yield return new WaitForSeconds(0f);
            GetTileID();
            LookingAtTile();
        }
    }

    public LocDirID GetLocationAndDirection()
    {
		Debug.Log("aasd" + locationDirection.dir);
        return locationDirection;
    }
}

[System.Serializable]
public struct LocDirID
{
    public int tile, dir; //tile and direction IDs stored

    public LocDirID(int p1, int p2)
    {
        tile = p1;
        dir = p2;
    }
}