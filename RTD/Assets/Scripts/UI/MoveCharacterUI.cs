using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterUI : MonoBehaviour
{
    enum STATE
    {
        Normal,
        MouseButtonDown,
        MouseButtonDragging,
        MouseButtonUp
    }
    STATE myState = STATE.Normal;

    public GameObject PickUpObject = null;
    Vector3 OriginPos = Vector3.zero;
    TileManager TileManager;
    // Start is called before the first frame update
    void Start()
    {
        TileManager = GetComponent<TileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch(myState)
        {
            case STATE.Normal:
                break;
            case STATE.MouseButtonDown:
                if (PickUpObject != null)
                {
                    OriginPos = PickUpObject.transform.position;
                }
                break;
            case STATE.MouseButtonDragging:
                if (PickUpObject != null)
                {
                    TileManager.AllAppear();
                }
                break;
            case STATE.MouseButtonUp:
                if(PickUpObject != null)
                {
                    if(TileManager.GetPossibleTileCount() > 0)
                    {
                        Transform parent = TileManager.GetClosestTile(PickUpObject.transform.position);
                        PickUpObject.transform.parent = parent;
                        PickUpObject.transform.localPosition = Vector3.zero;
                        //PickUpObject.layer = LayerMask.NameToLayer("Ground");
                    }
                    else
                    {
                        PickUpObject.transform.position = OriginPos;
                    }
                    TileManager.AllHide();

                    PickUpObject = null;
                }
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Normal:
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.tag == "Player")
                        {
                            PickUpObject = hits[i].transform.gameObject;
                        }
                    }
                    ChangeState(STATE.MouseButtonDown);
                }
                break;
            case STATE.MouseButtonDown:
                    ChangeState(STATE.MouseButtonDragging);
                break;
            case STATE.MouseButtonDragging:
                if (PickUpObject != null)
                {
                    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1<<LayerMask.NameToLayer("UI") & (1 << LayerMask.NameToLayer("Map"))));
                    //for (int i = 0; i < hits.Length; i++)
                    //{
                    //    if (hits[i].transform.gameObject.tag != "Tile" && hits[i].transform.gameObject.tag != "Player")
                    //    {
                    //        PickUpObject.transform.position = new Vector3(
                    //                hits[i].point.x,
                    //                PickUpObject.transform.position.y,
                    //                hits[i].point.z
                    //        );
                    //    }
                    //}
                    
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, 50.0f, (~(1<<LayerMask.NameToLayer("UI")) & (1 << LayerMask.NameToLayer("Map")))))
                    {
                        PickUpObject.transform.position = new Vector3(
                            hit.point.x,
                            PickUpObject.transform.position.y,
                            hit.point.z
                        );
                    }
                }
                
                if (Input.GetMouseButtonUp(0))
                {
                    ChangeState(STATE.MouseButtonUp);
                }
                break;
            case STATE.MouseButtonUp:
                ChangeState(STATE.Normal);
                break;
        }
    }
    public bool IsPicking(string tag)
    {
        if (myState == STATE.MouseButtonDragging && PickUpObject != null)
        {
            if (PickUpObject.tag == tag)
                return true;
            else
                return false;
        }
        else
            return false;
    }

}
