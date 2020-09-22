using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AlohaCorp.UI;
public class GrazeEffectManager : SingletonComponent<GrazeEffectManager>
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] UIElementGroup uI;
    string[] textstrings = new string[4] { "Near Miss!", "Tricky!", "Sweet!", "Dashing!" };
    bool Once = false;

    public void StartEffect()
    {
        if (!Once)
        {
            text.text = textstrings[Random.Range(0, 4)];
            uI.TurnOn(0.5f);
            Invoke("Off", 1);
            Once = true;
        }
    }
    void Off()
    {
        uI.TurnOff(0.5f);
    }

    public void Reset()
    {
        Once = false;
    }

}
