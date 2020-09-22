using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using AlohaCorp.UI;
public class ClearPopup : PopupComponents
{

    [SerializeField] ProgressManager progressManager;
    [SerializeField] SkinManager skinManager;
    [SerializeField] SkinPopup skinPopup;
    [SerializeField] GameObject Boxparent;
    [SerializeField] Image Box;
    [SerializeField] GameObject CoinImg;
    [SerializeField] Text SkinGaugeText;
    [SerializeField] SkeletonGraphic boxAni;
    [SerializeField] GameObject Buttons;
    [SerializeField] UIElementGroup NextButton;


    private void Awake()
    {

        Box.fillAmount = progressManager.CurrentProgress() / 100;
        SkinGaugeText.text = string.Format("{0:0}%", double.Parse(progressManager.CurrentProgress().ToString()));

        boxAni.gameObject.SetActive(false);
        boxAni.AnimationState.Complete += delegate
        {
            skinPopup.SkinPopupAction(skinManager.SkinToGetThisTime(), skinManager.GetFirstTrail);
            boxAni.AnimationState.SetEmptyAnimation(0, 0f);
            boxAni.enabled = false;
            boxAni.gameObject.SetActive(false);
        };
    }
    public void ClearPopupAction(bool IsBonus)
    {
        StartCoroutine(PopupAction(IsBonus));
    }
    IEnumerator PopupAction(bool IsBonus)
    {
        bool AllSkin = skinManager.CheckAllGetProgressSkins();
        if (IsBonus || AllSkin)
        {
            Boxparent.SetActive(false);
            CoinImg.SetActive(true);
            Buttons.SetActive(true);
            Invoke("ButtonsDelay", 3);
        }
        else
        {
            Boxparent.SetActive(true);
            CoinImg.SetActive(false);

            float Progress = progressManager.ProgressUp();
            if (Progress == 100)
            {
                Buttons.SetActive(false);
            }
            else
            {
                Buttons.SetActive(true);
                Invoke("ButtonsDelay", 3);
            }

            SkinGaugeText.text = string.Format("{0:0}%", double.Parse(Progress.ToString()));
            yield return new WaitForSeconds(0.5f);
            SoundManager.Instance.Play("Gauge");
            Box.DOFillAmount(Progress / 100, 0.8f).OnComplete(() =>
            {
                if (Box.fillAmount == 1f)
                {
                    boxAni.gameObject.SetActive(true);
                    Boxparent.SetActive(false);
                    Box.fillAmount = 0f;
                    boxAni.enabled = true;
                    SoundManager.Instance.Play("chest_open");
                    boxAni.AnimationState.SetAnimation(0, "open", false);
                }
            });

        }
    }
    void ButtonsDelay()
    {
        if (BG.activeSelf)
            NextButton.TurnOn(0.5f);
    }
    public void Reset()
    {
        if (Buttons.gameObject.activeSelf)
            NextButton.TurnOff(0);
        if (skinPopup.gameObject.activeSelf)
            skinPopup.Reset();
    }

}
