using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlohaCorp.UI;
using System;
public class PopupComponents : MonoBehaviour
{

    [SerializeField] UIElementGroup uIElementGroup;
    public GameObject BG;
    float Onofftime = 0.5f;
    public string PopupId;
    public void On()
    {
        gameObject.SetActive(true);
        BG.SetActive(true);
        uIElementGroup.TurnOn(Onofftime);
    }
    public void Off()
    {
        uIElementGroup.TurnOff(Onofftime);
        BG.SetActive(false);
        //StartCoroutine(TurnOffPanel(Onofftime));
    }
}
