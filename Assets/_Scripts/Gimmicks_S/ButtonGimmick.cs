using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonGimmick : MonoBehaviour
{
    [SerializeField] List<BombGimmick> Bombs = new List<BombGimmick>();
    [SerializeField] List<PresserGimmick> Presser = new List<PresserGimmick>();
    [SerializeField] string ButtonName;
    bool IsBombBtn;
    bool IsPressBtn;
    private void Awake()
    {
        if (Bombs.Count != 0)
            IsBombBtn = true;
        else
        {
            IsPressBtn = true;
            for (int i = 0; i < Presser.Count; i++)
            {
                Presser[i].material = GetComponent<MeshRenderer>().materials[0];
                Presser[i].Color = ButtonName;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsBombBtn)
        {
            if (other.tag == "Bullet")
            {
                transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f);
                StartCoroutine(other.GetComponent<Bullet>().ButtonDestroy());
                for (int i = 0; i < Bombs.Count; i++)
                {
                    Bombs[i].BombAction(true);
                }
                SoundManager.Instance.Play("button_click");
            }
        }
        else if (IsPressBtn)
        {
            transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f);
            for (int i = 0; i < Presser.Count; i++)
            {
                Presser[i].PresserAction();
            }
        }
    }
}
