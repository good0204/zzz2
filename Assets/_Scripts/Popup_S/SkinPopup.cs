using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AlohaCorp.UI;
public class SkinPopup : MonoBehaviour
{
    [SerializeField] Image trailSkinImage;

    [SerializeField] UIElementGroup _skinpopup;
    [SerializeField] UIElementGroup LoseitButton;
    [SerializeField] GameObject RewardBtn;
    [SerializeField] GameObject FirstSkinBtn;
    [System.Serializable]
    class TrailImages
    {
        public int Id;
        public Sprite TrailImage;
    }
    [SerializeField] List<TrailImages> trailImages = new List<TrailImages>();
    public void SkinPopupAction(int Id, bool GetFirstTrail)
    {
        gameObject.SetActive(true);



        for (int i = 0; i < trailImages.Count; i++)
        {
            if (trailImages[i].Id == Id)
            {
                trailSkinImage.sprite = trailImages[i].TrailImage;
            }
        }
        if (GetFirstTrail == false)
        {
            FirstSkinBtn.SetActive(true);
            RewardBtn.SetActive(false);
        }
        else
        {
            FirstSkinBtn.SetActive(false);
            RewardBtn.SetActive(true);
            Invoke("LosetButtonActive", 3f);
        }
        _skinpopup.TurnOn(0.5f);



    }
    void LosetButtonActive()
    {
        LoseitButton.TurnOn(0.5f);
    }
    public void Reset()
    {
        LoseitButton.TurnOff(0);
        _skinpopup.TurnOff(0);
        gameObject.SetActive(false);
    }
}
