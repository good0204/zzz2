using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Aloha.Save;
public class SoundManager : SingletonComponent<SoundManager>, ISaveable
{
    ReactiveProperty<bool> Sound = new ReactiveProperty<bool>();
    ReactiveProperty<bool> vibration = new ReactiveProperty<bool>();
    public string Key { get { return "SoundManger"; } }
    // [SerializeField] AudioSource SoundAudioSource;
    [SerializeField] AudioSource BGMAudioSource;

    [SerializeField] Sprite OnSprite;
    [SerializeField] Sprite OffSprite;
    [SerializeField] Image SoundBtnImg;
    [SerializeField] Image VibrationBtnImg;

    [SerializeField] private List<SoundInfo> soundInfos = null;


    [System.Serializable]
    private class SoundInfo
    {
        public string id = "";
        public AudioClip audioClip = null;
        public AudioSource SoundAudioSource;
        //public SoundType type = SoundType.SoundEffect;
        [Range(0, 1)] public float clipVolume = 1;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!SaveManager.Load(this))
        {
            Sound.Value = true;
            vibration.Value = true;
        }
        Sound.Subscribe(x =>
        {
            if (x)
            {
                //                SoundBtnImg.sprite = OnSprite;
            }
            else
            {
                //         SoundBtnImg.sprite = OffSprite;
            }

        });
        vibration.Subscribe(x =>
       {
           if (x)
           {
               //       VibrationBtnImg.sprite = OnSprite;
           }
           else
           {
               //        VibrationBtnImg.sprite = OffSprite;
           }

       });
    }
    public void Play(string Id)
    {
        if (Sound.Value)
        {
            if (Id == "Hit")
            {
                int Rand = Random.Range(0, 3);
                switch (Rand)
                {
                    case 0:
                        Id = "Hit";
                        break;
                    case 1:
                        Id = "Hit2";
                        break;
                    case 2:
                        Id = "Hit3";
                        break;
                }
            }
            SoundInfo soundinfo = GetSoundInfo(Id);
            soundinfo.SoundAudioSource.clip = soundinfo.audioClip;
            soundinfo.SoundAudioSource.volume = soundinfo.clipVolume;
            soundinfo.SoundAudioSource.Play();
        }
    }
    public void SoundOnOff()
    {
        if (Sound.Value)
        {
            Sound.Value = false;
        }
        else
        {
            Sound.Value = true;
        }
    }

    public void VibrationOnOff()
    {
        if (vibration.Value)
        {
            vibration.Value = false;
        }
        else
        {
            vibration.Value = true;
        }
    }

    private SoundInfo GetSoundInfo(string id)
    {
        for (int i = 0; i < soundInfos.Count; i++)
        {
            if (id == soundInfos[i].id)
            {
                return soundInfos[i];
            }
        }
        return null;
    }
    public void VibrationPlay(int strength)
    {
        if (!vibration.Value) return;
        Vibration.Vibrate(strength);
    }
    public IEnumerator Clear()
    {

        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < 11; i++)
        {
            VibrationPlay(10);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
        VibrationPlay(100);
    }
    public IEnumerator ResultPopup()
    {
        for (int i = 0; i < 20; i++)
        {
            VibrationPlay(1);
            yield return new WaitForSeconds(0.039f);
        }
    }

    #region Save&Load
    public Dictionary<string, object> GetSaveData()
    {
        Dictionary<string, object> json = new Dictionary<string, object>();

        json["Sound"] = Sound.Value;
        json["Vibration"] = vibration.Value;

        return json;
    }
    public void Load(Dictionary<string, object> saveData)
    {

        Sound.Value = (bool)saveData["Sound"];
        vibration.Value = (bool)saveData["Vibration"];
    }
    #endregion
}
