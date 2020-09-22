using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DebugMode : MonoBehaviour
{
    [SerializeField] AreaManager areaManager;
    [SerializeField] InputField inputField;

    public void ChangeArea()
    {
        areaManager.DebugModeChangeArea(Convert.ToInt32(inputField.text));
    }
}
