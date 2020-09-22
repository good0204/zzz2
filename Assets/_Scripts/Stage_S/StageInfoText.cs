using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using AlohaCorp.UI;
public class StageInfoText : MonoBehaviour
{
    ReactiveProperty<int> StageNum = new ReactiveProperty<int>();
    //ReactiveProperty<int> bulletCount = new ReactiveProperty<int>();
    // [SerializeField] Image[] BulletEmptyImage;
    [SerializeField] Image[] BulletImbage;
    [SerializeField] GameObject[] StageImage;
    [SerializeField] Image[] ClearStageImage;
    [SerializeField] Image[] CurrentStageImage;
    [SerializeField] Text AreaText;
    [SerializeField] Image CurrentbulletImage;
    [SerializeField] RectTransform BulletBG;
    Tween ShineTween;
    bool First;
    private void Start()
    {
        StageNum.Subscribe(x =>
        {
            for (int i = 0; i < StageImage.Length; i++)
            {
                if (i == x)
                {
                    ClearStageImage[i].transform.DOScale(0, 0f);
                    CurrentStageImage[i].transform.DOScale(1, 0.5f);
                }
                else if (i < x)
                {
                    CurrentStageImage[i].transform.DOScale(0, 0f);
                    ClearStageImage[i].transform.DOScale(1, 0.5f);
                }
                else
                {
                    CurrentStageImage[i].transform.DOScale(0, 0f);
                    ClearStageImage[i].transform.DOScale(0, 0f);
                }
            }
        });


    }
    public void ChangeStageText(int StageNum)
    {
        this.StageNum.Value = StageNum;
    }
    public void ChageTotalStage(int TotalStageCount)
    {
        for (int i = 0; i < StageImage.Length; i++)
        {
            if (i < TotalStageCount)
            {
                StageImage[i].SetActive(true);
            }
            else
            {
                StageImage[i].SetActive(false);
            }
        }
    }
    public void ChangeBulletImage(int bullectCount)
    {
        //this.bulletCount.Value = bullectCount;
        for (int i = 0; i < BulletImbage.Length; i++)
        {
            if (i < bullectCount)
            {
                if (i == bullectCount - 1)
                {
                    CurrentbulletImage = BulletImbage[i];

                }
                BulletImbage[i].DOFade(1f, 0);

            }
            else
            {
                BulletImbage[i].DOFade(0f, 0.25f);
            }

        }
    }
    public void ChangeBGHeight(int bullectCount)
    {

        BulletBG.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 210 + ((bullectCount - 1) * 150));
    }
    public void ChangeAreaNum(bool IsBonus, int AreaNum)
    {
        if (!IsBonus)
        {
            AreaText.text = "Level " + (AreaNum + 1).ToString();
        }
        else
        {
            AreaText.text = "Bonus";
        }
    }
    public void BulletShine(bool IsShine)
    {
        ShineTween.Kill();
        if (IsShine)
        {
            ShineTween = CurrentbulletImage.DOFade(0f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }

}
