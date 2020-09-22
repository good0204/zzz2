using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PopupManager : SingletonComponent<PopupManager>
{
    public Action<bool> ClearPopupAction;
    [SerializeField] List<PopupComponents> popups = new List<PopupComponents>();
    PopupComponents ActivePopup;

    private void Start()
    {
        for (int i = 0; i < popups.Count; i++)
        {
            if (popups[i].TryGetComponent(out ClearPopup clearPopup))
            {
                ClearPopupAction += clearPopup.ClearPopupAction;
            }
        }
    }
    public void On(string PopupId)
    {
        for (int i = 0; i < popups.Count; i++)
        {
            if (popups[i].PopupId == PopupId)
            {
                ActivePopup = popups[i];
                ActivePopup.On();
            }
        }
    }
    public void Off()
    {
        if (ActivePopup.PopupId == "ClearPopup")
        {
            ActivePopup.GetComponent<ClearPopup>().Reset();
        }
        ActivePopup.Off();
    }
    public void ClearPopup(bool IsBonus)
    {
        ClearPopupAction?.Invoke(IsBonus);
    }

}
