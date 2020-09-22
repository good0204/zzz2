using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlohaCorp.UI
{
    public class UIElementGroupTester : MonoBehaviour
    {
        [SerializeField] private KeyCode key;
        [SerializeField] private UIElementGroup targetGroup;
        [SerializeField] private bool on;
        [SerializeField] private float time;

        void Start()
        {
            if (on) targetGroup.TurnOn(time);
            else targetGroup.TurnOff(time);
        }

        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                on = !on;
                if (on) targetGroup.TurnOn(time);
                else targetGroup.TurnOff(time);
            }
        }
    }
}