using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public Transform GetClosestTile(Vector3 pos)
    {
        Transform closestTile = null;
        for(int i=0; i< transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible && closestTile == null)
            {
                closestTile = transform.GetChild(i);
                continue;
            }
            else if (transform.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible)
            {
                if (Vector3.Distance(transform.GetChild(i).position, pos) < Vector3.Distance(closestTile.position, pos))
                {
                    closestTile = transform.GetChild(i);
                }
            }
        }
        return closestTile;
    }

    public int GetPossibleTileCount()
    {
        int cnt = 0;
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Tile>().State == Tile.STATE.Possible)
                cnt++;
        }
        return cnt;
    }

    public void AllHide()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Tile>().State = Tile.STATE.Hide;
        }
    }
    public void AllAppear()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Tile>().AppearTile();
        }
    }
}
