using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController_Assassin : SkillController
{
    CharController controller;

    // Start is called before the first frame update
    void Start()
    {
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void InitComponents()
    {
        controller = GetComponentInParent<CharController>();

        if (controller != null)
            animEvent = controller.animEvent;

        animEvent.SkillStartDel += SkillOn;
        animEvent.SkillReadyDel += SkillReady;
        animEvent.SkillEndDel += EndUseSkill;
        ChangeState(SKILLSTATE.WAIT);
    }
    
}
