using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

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
    public VoidDelGameObject UpgradeDelegate = null;
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

        switch (state)
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
                if (PickUpObject != null)
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
                        if (parent == null)
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
                    // 합치기 or 이동
                    else if (TileManager.GetPossibleTileCount() > 0)
                    {
                        Transform targetTile = TileManager.GetClosestTile(PickUpObject.transform.position);
                        GameObject target = targetTile.childCount > 0 ? targetTile.GetChild(0).gameObject : null;

                        // 합치기
                        if (CharacterInfoManager.IsUpgradeTaret(PickUpObject, target))
                        {
                            // Success
                            UpgradeCharacter(PickUpObject.transform.gameObject, target);
                        }
                        //이동
                        else if (target == null)
                        {
                            PickUpObject.transform.parent = targetTile;
                            PickUpObject.transform.localPosition = Vector3.zero;
                        }
                        // 서로 자리 바꾸기
                        else
                        {
                            Transform myTile = PickUpObject.transform.parent;
                            PickUpObject.transform.parent = targetTile;
                            PickUpObject.transform.localPosition = Vector3.zero;

                            target.transform.parent = myTile;
                            target.transform.localPosition = Vector3.zero;
                        }
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
                        if (hits[i].transform.tag == "Player")
                        {
                            if (UpgradeCharacter(hits[i].transform.gameObject, null))
                            {
                                // Success
                            }
                            else
                            {
                                // Fail
                            }
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
                            if (parent == null)
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
                    RaycastHit[] hits = Physics.RaycastAll(ray, 500.0f, ~(1 << LayerMask.NameToLayer("UI")));
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
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 50.0f, (~(1 << LayerMask.NameToLayer("UI")) & (1 << LayerMask.NameToLayer("Map")))))
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

    public bool UpgradeCharacter(GameObject obj, GameObject target)
    {
        bool rt = false;
        Transform parent;
        if (target == null)
        {
            target = CharacterInfoManager.GetUpgradeTarget(obj);
            parent = obj.transform.parent;
        }
        else
        {
            parent = target.transform.parent;
        }

        if (target != null)
        {
            string[] charList = CharacterInfoManager.GetGradeUnionCharacters(CharacterInfoManager.GetNextGrade(CharUtils.SetCharacterGrade(obj.GetComponent<CharController>().statInfo.id)), obj.GetComponent<CharController>().statInfo.union);
            GameObject upgradeCharacter = Instantiate(Resources.Load(charList[Random.Range(0, charList.Length)])) as GameObject;
            upgradeCharacter.transform.parent = parent;
            upgradeCharacter.transform.localPosition = Vector3.zero;
            UpgradeDelegate?.Invoke(upgradeCharacter);
            GetComponent<LevelUpManager>().UpdateCharacterLevel(upgradeCharacter);
            Destroy(obj);
            Destroy(target);
            rt = true;
        }
        else
        {
            rt = false;
        }
        return rt;
    }
}
