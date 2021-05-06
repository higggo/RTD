using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    public Image WarningImage = null;
    public Image BossRoundImage = null;
    public Image BorderImage = null;
    // Start is called before the first frame update
    void Start()
    {
        if (WarningImage == null) WarningImage = transform.Find("Warning").GetComponent<Image>();
        if (BossRoundImage == null) BossRoundImage = transform.Find("BossRound").GetComponent<Image>();
        if (BorderImage == null) BorderImage = transform.Find("Border").GetComponent<Image>();


        //StartCoroutine(BossAlarm());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(BossAlarm());

        }
    }
    public IEnumerator BossAlarm()
    {

        bool timeOver = false;
        float time = 0.0f;
        float speed = 0.2f;
        float colorDelta = Time.smoothDeltaTime;
        float min = 0.65f;
        float max = 0.9f;

        WarningImage.color = new Color(max, max, max, max);
        BossRoundImage.color = new Color(max, max, max, max);
        BorderImage.color = new Color(max, max, max, max);
        Color color = WarningImage.color;


        while (!(timeOver && color.r == 0))
        {
            if (color.r <= min || color.r >= max)
                colorDelta = -colorDelta;
            if(time > 3.5f && color.r >= max)
            {
                colorDelta = colorDelta < 0f ? colorDelta : -colorDelta;
                min = 0f;
                speed = 1f;
                timeOver = true;
            }
            color.r = Mathf.Clamp(color.r + colorDelta * speed, min, max);
            color.g = Mathf.Clamp(color.g + colorDelta * speed, min, max);
            color.b = Mathf.Clamp(color.b + colorDelta * speed, min, max);
            color.a = Mathf.Clamp(color.a + colorDelta * speed, min, max);
            WarningImage.color = color;
            BossRoundImage.color = color;
            BorderImage.color = color;
            time += Time.smoothDeltaTime;
            yield return null;
        }
    }
}
