using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{

    public Transform tilePrefab;
    public Transform trackedObject;
    public Vector2 gridSize;
    public float tileScale; //for 4x3 tile map 0.5 meters is the correct scaling, otherwise the map is either too small or too big, or we'd need a non-int number to have correct size which is impossible.

    void Start()
    {
        GenerateGrid();
    }

    void Update(){
        GetTileID();
    }


    public void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-gridSize.x * (tileScale / 2f) + tileScale / 2f + x * tileScale, 0, -gridSize.y * (tileScale / 2f) + tileScale / 2f + y * tileScale); //offset so the grid is in the middle
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.transform.localScale = Vector3.one * tileScale; //scaling so the grid is in the middle
                newTile.GetComponent<Tile>().SetID((int)gridSize.y * x + y);
                newTile.parent = transform;
            }
        }
    }

    void GetTileID()
    {
        Ray ray = new Ray(trackedObject.transform.position, Vector3.down);
        RaycastHit hitInfo;
        int tileMask = LayerMask.GetMask("Tile");

        if (Physics.Raycast(ray, out hitInfo, 100, tileMask)) //Casts a ray down that only takes the Tile layer into consideration, puts the ray information in hitInfo
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red); //Draws a red line if you hit a tile
            Debug.Log(hitInfo.collider.GetComponent<Tile>().GetID()); //Writes the tile ID in the console (0 to 47 in this case)
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green); //Draws a green line if it hits anything else
        }
    }

}
