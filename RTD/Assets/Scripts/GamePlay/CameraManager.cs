using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public struct CameraMovePos
{
    public Vector3 startPos;
    public Vector3 startRot;
    public Vector3 endPos;
    public Vector3 lookTarget;
    public float moveSec;
    public float rotSec;

    public CameraMovePos(Vector3 startPos, Vector3 startRot, Vector3 endPos, Vector3 lookTarget, float moveSec, float rotSec)
    {
        this.startPos = startPos;
        this.startRot = startRot;
        this.endPos = endPos;
        this.lookTarget = lookTarget;
        this.moveSec = moveSec;
        this.rotSec = rotSec;
    }
}

public class CameraManager : MonoBehaviour
{
    List<CameraMovePos> MovePos = new List<CameraMovePos>();
    public Camera MainCamera = null;
    public Camera DirectionCamera = null;
    public Camera BossZoneCamera = null;

    bool bBreakTime = false;

    //public UnityAction MoveDone;
    // Start is called before the first frame update
    void Start()
    {
        if (MainCamera == null)
            MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (BossZoneCamera == null)
            BossZoneCamera = GameObject.Find("BossZoneCamera").GetComponent<Camera>();

        // 1
        MovePos.Add(new CameraMovePos(
            new Vector3(-3.19f, 7.25f, -9.21f),
            new Vector3(14.107f, 0.0f, 0.0f),
            new Vector3(-3.19f, 2.14f, 11.14f),
            new Vector3(14.107f, 0.0f, 0.0f),
            2f,
            2f));

        // 2
        MovePos.Add(new CameraMovePos(
            new Vector3(45.54f, -60.04f, -10.76f),
            new Vector3(13.264f, -32.897f, 0.0f),
            new Vector3(61.0f, -60.04f, -29.70f),
            new Vector3(13.264f, -32.897f, 0.0f),
            1f,
            1f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator BossSpawn(UnityAction done)
    {
        DirectionCamera.depth = 0;
        foreach (CameraMovePos info in MovePos)
        {
            DirectionCamera.transform.position = info.startPos;
            DirectionCamera.transform.rotation = Quaternion.Euler(info.startRot);
            float gauge = 0.0f;
            while (gauge <= info.moveSec)
            {
                gauge += Time.smoothDeltaTime;
                DirectionCamera.transform.position = Vector3.Lerp(info.startPos, info.endPos, gauge / Mathf.Clamp(info.moveSec, 0.1f, info.moveSec));
                yield return null;
            }
        }
        done?.Invoke();
    }
    public IEnumerator LookAroundBoss(Transform obj)
    {
        //DirectionCamera.depth = 0;
        float time = 4f;
        float delta = 0.0f;
        while(delta <= time)
        {
            delta += Time.deltaTime;
            Vector3 pos = obj.position + (obj.forward * 13.0f);
            pos.y += 2.0f;
            DirectionCamera.transform.position = pos;
            DirectionCamera.transform.forward = -obj.forward;
            yield return null;
        }
        DirectionCamera.depth = -2;
    }

    public IEnumerator LookAroundGroundCharacter()
    {
        DirectionCamera.depth = 0;
        bBreakTime = true;
        float time = 2f;
        List<GameObject> characters = new List<GameObject>();
        characters = GetComponent<TileManager>().GetGroundCharacters();

        while (bBreakTime)
        {
            if (characters.Count == 0)
            {
                bBreakTime = false;
                break;
            }
            int ran = Random.Range(0, characters.Count);
            if (characters[ran] == null)
            {
                characters.RemoveAt(ran);
                continue;
            }
            Transform obj = characters[ran].transform;
            float delta = 0.0f;
            while (delta <= time)
            {
                if (!bBreakTime ||
                    characters[ran] == null)
                    break;
                if (characters[ran].GetComponent<CharController>().characterState == CharacterKit.BASICSTATE.DEAD)
                    break;

                //characters[ran].GetComponent<CharacterKit.Damageable>().IsDead;



                delta += Time.deltaTime;
                Vector3 pos = obj.position + (-obj.forward * 6.0f);
                pos.y += 2.0f;
                DirectionCamera.transform.position = pos;
                DirectionCamera.transform.forward = obj.forward;

                yield return null;
            }
            yield return null;
        }
        DirectionCamera.depth = -2;
    }

    public IEnumerator LookAroundBossCharacter()
    {
        DirectionCamera.depth = 0;
        bBreakTime = true;
        float time = 2f;
        List<GameObject> characters = new List<GameObject>();
        characters = GetComponent<TileManager>().GetBossCharacters();

        while (bBreakTime)
        {
            if (characters.Count == 0)
            {
                bBreakTime = false;
                break;
            }
            int ran = Random.Range(0, characters.Count);
            if (characters[ran] == null)
            {
                characters.RemoveAt(ran);
                continue;
            }
            Transform obj = characters[ran].transform;
            float delta = 0.0f;
            while (delta <= time) 
            {
                if (!bBreakTime || characters[ran] == null)
                    break;
                if (characters[ran].GetComponent<CharController>().characterState == CharacterKit.BASICSTATE.DEAD)
                    break;

                //characters[ran].GetComponent<CharacterKit.Damageable>().IsDead;

                delta += Time.deltaTime;
                Vector3 pos = obj.position + (-obj.forward * 6.0f);
                pos.y += 2.0f;
                DirectionCamera.transform.position = pos;
                DirectionCamera.transform.forward = obj.forward;

                yield return null;
            }
            yield return null;
        }
        DirectionCamera.depth = -2;
    }

    public void StopDirectionCamera()
    {
        bBreakTime = false; 
        DirectionCamera.depth = -2;
    }
}
