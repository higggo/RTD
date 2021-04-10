using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Transform GroundSpace;
    public Transform StorageSpace;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Init()
    {
        DestroyAllCharacter();
    }
    public Transform GetClosestTile(Vector3 pos)
    {
        Transform closestTile = null;
        //for(int i=0; i< transform.childCount; i++)
        //{
        //    if (transform.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible && closestTile == null)
        //    {
        //        closestTile = transform.GetChild(i);
        //        continue;
        //    }
        //    else if (transform.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible)
        //    {
        //        if (Vector3.Distance(transform.GetChild(i).position, pos) < Vector3.Distance(closestTile.position, pos))
        //        {
        //            closestTile = transform.GetChild(i);
        //        }
        //    }
        //}
        for (int i = 0; i < GroundSpace.childCount; i++)
        {
            if (GroundSpace.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible)
            {
                if (closestTile == null)
                {
                    closestTile = GroundSpace.GetChild(i);
                    continue;
                }
                if (Vector3.Distance(GroundSpace.GetChild(i).position, pos) < Vector3.Distance(closestTile.position, pos))
                {
                    closestTile = GroundSpace.GetChild(i);
                }
            }
        }
        for (int i = 0; i < StorageSpace.childCount; i++)
        {
            if (StorageSpace.GetChild(i).GetComponent<Tile>().State == Tile.STATE.Possible)
            {
                if (closestTile == null)
                {
                    closestTile = StorageSpace.GetChild(i);
                    continue;
                }
                if (Vector3.Distance(StorageSpace.GetChild(i).position, pos) < Vector3.Distance(closestTile.position, pos))
                {
                    closestTile = StorageSpace.GetChild(i);
                }
            }
        }
        return closestTile;
    }

    public int GetPossibleTileCount()
    {
        int cnt = 0;
        //foreach(Transform child in transform)
        //{
        //    if (child.GetComponent<Tile>().State == Tile.STATE.Possible)
        //        cnt++;
        //}
        foreach (Transform child in GroundSpace)
        {
            if (child.GetComponent<Tile>().State == Tile.STATE.Possible)
                cnt++;
        }
        foreach (Transform child in StorageSpace)
        {
            if (child.GetComponent<Tile>().State == Tile.STATE.Possible)
                cnt++;
        }
        return cnt;
    }

    public void AllHide()
    {
        //foreach (Transform child in transform)
        //{
        //    child.GetComponent<Tile>().State = Tile.STATE.Hide;
        //}
        foreach (Transform child in GroundSpace)
        {
            child.GetComponent<Tile>().State = Tile.STATE.Hide;
        }
        foreach (Transform child in StorageSpace)
        {
            child.GetComponent<Tile>().State = Tile.STATE.Hide;
        }
    }
    public void AllAppear()
    {
        //foreach (Transform child in transform)
        //{
        //    child.GetComponent<Tile>().AppearTile();
        //}
        foreach (Transform child in GroundSpace)
        {
            child.GetComponent<Tile>().AppearTile();
        }
        foreach (Transform child in StorageSpace)
        {
            child.GetComponent<Tile>().AppearTile();
        }
    }

    public int GetCountStorageCharacter()
    {
        int cnt = 0;
        foreach (Transform child in StorageSpace)
        {
            if (child.childCount > 0)
                cnt++;
        }
        return cnt;
    }
    public int GetCountGroundCharacter()
    {
        int cnt = 0;
        foreach (Transform child in GroundSpace)
        {
            if (child.childCount > 0)
                cnt++;
        }
        return cnt;
    }

    public Transform GetEmptyOtherFieldTile(Transform obj)
    {
        Transform emptyTile = null;
        if (obj.parent.parent.parent.name == GroundSpace.parent.name)
        {
            foreach (Transform child in StorageSpace)
            {
                if (child.childCount == 0)
                    emptyTile = child;
            }
        }
        else if (obj.parent.parent.parent.name == StorageSpace.parent.name)
        {
            foreach (Transform child in GroundSpace)
            {
                if (child.childCount == 0)
                    emptyTile = child;
            }
        }
        return emptyTile;
    }

    public void DestroyAllCharacter()
    {
        foreach (Transform child in GroundSpace)
        {
            if(child.childCount > 0) Destroy(child.GetChild(0).gameObject);
        }
        foreach (Transform child in StorageSpace)
        {
            if (child.childCount > 0) Destroy(child.GetChild(0).gameObject);
        }
    }
}
