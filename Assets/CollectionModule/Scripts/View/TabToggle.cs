using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TabToggle : MonoBehaviour
{
    public List<TabLink> _tabLinks;

    void Start()
    {
        foreach (var tabLink in _tabLinks)
        {
            tabLink.toggle.OnValueChangedAsObservable()
                .Subscribe(v =>
                {
                    tabLink.gameObject.SetActive(v);
                });
        }
        _tabLinks[0].toggle.isOn = true;
    }

    [Serializable]
    public class TabLink
    {
        public Toggle toggle;
        public GameObject gameObject;
    }
}
