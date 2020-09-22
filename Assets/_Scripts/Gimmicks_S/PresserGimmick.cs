using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PresserGimmick : MonoBehaviour
{
    [SerializeField] Vector3 ArrivalPos;
    [SerializeField] float MoveTime;
    Tween Move;
    [SerializeField] bool IsKill;
    [SerializeField] Transform Rail;
    [SerializeField] Sprite[] SpeedSprites;
    [SerializeField] Color[] SpeedSpritesColor;
    [SerializeField] SpriteRenderer SpeedSprite;
    [HideInInspector] public Material material;
    [HideInInspector] public string Color;


    private void Start()
    {
        GenerateRail();
        // transform.LookAt(transform.parent.localPosition + ArrivalPos);
        if (IsKill)
        {
            SpeedSprite.sprite = SpeedSprites[0];
        }
        else
        {
            SpeedSprite.sprite = SpeedSprites[1];
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            if (IsKill)
            {
                //other.GetComponent<TargetGimmick>().PressClear();

                other.GetComponent<TargetGimmick>().BombClear(300, transform.position + Vector3.forward, 3, 20);
            }
            else
            {
                other.transform.SetParent(transform.parent);
            }
        }
    }

    public void PresserAction()
    {
        Move = transform.parent.DOLocalMove(transform.parent.localPosition + ArrivalPos, MoveTime).SetEase(Ease.Linear);
    }
    void GenerateRail()
    {
        Vector3 RailPos = (transform.parent.localPosition + ArrivalPos / 2f);
        RailPos.y = 0.075f;
        Transform rail = Instantiate(Rail, RailPos, Quaternion.identity);
        Material a = rail.GetComponent<Material>();
        rail.GetComponent<MeshRenderer>().material = material;
        ChangeSpeedSpritecolor();
        rail.SetParent(transform.parent.parent);
        rail.localPosition = RailPos;
        float magnitude = Vector3.Magnitude(ArrivalPos - transform.localPosition);
        rail.localScale = new Vector3(magnitude * 100, 100f, 100f);
        Vector3 v3 = RailPos - transform.parent.localPosition;
        rail.Rotate(0, -Mathf.Atan2(v3.z, v3.x) * Mathf.Rad2Deg, 0);
        transform.Rotate(0, (-Mathf.Atan2(v3.z, v3.x) * Mathf.Rad2Deg) + 90, 0);
    }
    void ChangeSpeedSpritecolor()
    {
        switch (Color)
        {
            case "Orange":
                SpeedSprite.color = SpeedSpritesColor[0];
                break;
            case "Blue":
                SpeedSprite.color = SpeedSpritesColor[1];
                break;
            case "Yellow":
                SpeedSprite.color = SpeedSpritesColor[2];
                break;
            case "Green":
                SpeedSprite.color = SpeedSpritesColor[3];
                break;
        }
    }

}
