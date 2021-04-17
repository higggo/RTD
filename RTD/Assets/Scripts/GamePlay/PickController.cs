using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickController : MonoBehaviour
{
    enum STATE
    {
        Normal,
        MouseButtonDown,
        MouseButtonDragging,
        MouseButtonUp,
        Disable
    }
    STATE state = STATE.Normal;

    public GameObject PickUpObject = null;
    Vector3 OriginPos = Vector3.zero;
    TileManager TileManager;
    CharacterInfoManager CharacterInfoManager;
    //BossZoneWarp BossZoneWarp;
    // Start is called before the first frame update
    void Start()
    {
        TileManager = GetComponent<TileManager>();
        CharacterInfoManager = GetComponent<CharacterInfoManager>();
        //BossZoneWarp = GameObject.Find("BossWarp").GetComponent<BossZoneWarp>();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    public void Init()
    {
        PickUpObject = null;
        OriginPos = Vector3.zero;
        ChangeState(STATE.Normal);
    }
    void ChangeState(STATE s)
    {
        if (state == s) return;
        state = s;

        switch(state)
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
                    // BossZone Warp
                    if (TileManager.IsBossTile())
                    {
                        Transform parent = TileManager.GetBossTile();
                        if (parent != null)
                        {
                            PickUpObject.transform.parent = parent;
                            PickUpObject.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            Debug.Log("BossZone Full");
                            PickUpObject.transform.position = OriginPos;
                        }
                    }
                    // Return Warp to Storage or Ground
                    else if (TileManager.IsReturnTile())
                    {
                        Transform parent = TileManager.GetEmptyStorageTile();
                        if(parent == null)
                            parent = TileManager.GetEmptyGroundTile();
                        if (parent != null)
                        {
                            PickUpObject.transform.parent = parent;
                            PickUpObject.transform.localPosition = Vector3.zero;
                        }
                        else
                        {
                            Debug.Log("BossZone Full");
                            PickUpObject.transform.position = OriginPos;
                        }
                    }
                    // Storage <--> Ground
                    else if (TileManager.GetPossibleTileCount() > 0)
                    {
                        Transform parent = TileManager.GetClosestTile(PickUpObject.transform.position);
                        PickUpObject.transform.parent = parent;
                        PickUpObject.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        PickUpObject.transform.position = OriginPos;
                    }
                    TileManager.AllHide();

                    PickUpObject = null;
                }
                break;
            case STATE.Disable:
                break;
        }
    }
    void StateProcess()
    {
        switch (state)
        {
            case STATE.Normal:
                // Control + 클릭 : 합치기
                if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
                {
                    Debug.Log("Control + 클릭 : 합치기");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        // 여기부터 작성
                        if (hits[i].transform.tag == "Player")
                        {
                            PickUpObject = hits[i].transform.gameObject;
                            GameObject target = CharacterInfoManager.GetUpgradeTarget(PickUpObject);
                            if (target != null)
                            {
                                string[] charList = CharacterInfoManager.GetGradeCharacterList(CharacterInfoManager.GetNextGrade(PickUpObject.GetComponent<CharController>().statInfo.grade));
                                GameObject upgradeCharacter = Instantiate(Resources.Load(charList[Random.Range(0, charList.Length)])) as GameObject;
                                upgradeCharacter.transform.parent = PickUpObject.transform.parent;
                                upgradeCharacter.transform.localPosition = Vector3.zero;
                                GetComponent<LevelUpManager>().UpdateCharacterLevel(upgradeCharacter);
                                Destroy(PickUpObject);
                                Destroy(target);
                            }
                            PickUpObject = null;
                        }
                    }
                }

                // Alt + 클릭 : 자동 옮기기
                else if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt))
                {
                    Debug.Log("Alt + 클릭 : 자동 옮기기");
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.tag == "Player")
                        {
                            Transform parent = TileManager.GetEmptyOtherFieldTile(hits[i].transform);
                            if(parent == null)
                            {
                                Debug.Log("이동할 곳이 가득찼습니다.");
                                break;
                            }
                            hits[i].transform.parent = parent;
                            hits[i].transform.localPosition = Vector3.zero;
                            //GetComponent<CharacterInfoManager>().UpdateCharacterField(hits[i].transform.gameObject);
                        }
                    }
                }

                // 좌클릭
                else if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f, ~(1 << LayerMask.NameToLayer("UI")));
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.tag == "Player")
                        {
                            Debug.Log("캐릭터 좌클릭");
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

            case STATE.Disable:
                break;
        }
    }
    public bool IsPicking(string tag)
    {
        if (state == STATE.MouseButtonDragging && PickUpObject != null)
        {
            if (PickUpObject.tag == tag)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public void SetDisable()
    {
        if (PickUpObject != null)
        {
            PickUpObject.transform.position = OriginPos;
            TileManager.AllHide();
            PickUpObject = null;
        }
        ChangeState(STATE.Disable);
    }

    public void SetNormal()
    {
        ChangeState(STATE.Normal);
    }
}
