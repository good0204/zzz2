using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class DrawLine : MonoBehaviour
{
    [SerializeField] GameObject LinePointObj;
    List<GameObject> LineObjs = new List<GameObject>();
    [SerializeField] float f;
    [SerializeField] float c = 1.44f;
    [SerializeField] float RotationSpeed;
    [SerializeField] Transform Parent;
    [SerializeField] LineRenderer line;
    [SerializeField] List<ObjectAnimation> objectAnimations = new List<ObjectAnimation>();
    List<ObjectAnimation> castList;
    bool FirstTouch;
    public int direction = 1;
    public float rad = 0;
    Vector3[] lines = new Vector3[32];
    float theta;
    private void Start()
    {
        for (int i = 0; i < 32; i++)
        {
            GameObject a = Instantiate(LinePointObj, new Vector3(-9999, -9999, -9999), Quaternion.identity);
            a.transform.SetParent(Parent);
            a.gameObject.SetActive(false);
            LineObjs.Add(a);
        }
        line.enabled = false;

    }

    public void LineAdject(float PointX)
    {
        theta = Mathf.LerpAngle(transform.eulerAngles.y, PointX * 150, Time.deltaTime * RotationSpeed);

        if (theta >= 45f && theta <= 180)
        {
            theta = 45f;
        }
        else if (theta >= 180 && theta <= 315)
        {
            theta = 315f;
        }


        transform.eulerAngles = new Vector3(0, theta, 0);

        if (transform.localEulerAngles.y > 180)
        {
            float ConvertY;
            ConvertY = transform.localEulerAngles.y - 360;
            rad = ConvertY * Mathf.Deg2Rad;
        }
        else
        {
            rad = transform.localEulerAngles.y * Mathf.Deg2Rad;
        }
        if (PointX > 0)
        {
            direction = 1;
        }
        else if (PointX < 0)
        {
            direction = -1;
        }
        else if (PointX == 0)
        {
            direction = 0;
        }

        if (!FirstTouch)
        {
            FirstTouch = true;
            LineActive(true);
        }
        else
        {
            float a = 0;
            // if (castList.Count != 0)
            // {
            //     for (int i = 0; i < castList.Count; i++)
            //         castList[i].TerrifiedReset();
            // }
            castList = new List<ObjectAnimation>();
            for (int i = 0; i < 32; i++)
            {
                Vector3 LineObjPos = new Vector3((a * Mathf.Tan(rad) - (Mathf.Pow(rad, 2f)) * direction * f * (Mathf.Pow(a, 2f) / Mathf.Pow(Mathf.Cos(rad), 2))) * c, transform.position.y, transform.position.z + a);
                LineObjs[i].transform.position = LineObjPos;
                a += 0.5f;
                line.SetPosition(i, LineObjPos);
                RaycastHit hit;
                if (i != 31)
                {
                    // Debug.DrawRay(LineObjs[i].transform.position, LineObjs[i + 1].transform.position - LineObjs[i].transform.position, Color.red, 0.1f);
                    if (Physics.Raycast(LineObjs[i].transform.position, LineObjs[i + 1].transform.position - LineObjs[i].transform.position, out hit, 1f))
                    {
                        if (hit.collider.tag.Equals("Target"))
                        {
                            ObjectAnimation objectAnimation = hit.collider.GetComponent<ObjectAnimation>();
                            castList.Add(objectAnimation);
                            if (!objectAnimations.Contains(objectAnimation))
                            {
                                objectAnimations.Add(objectAnimation);
                                objectAnimation.Terrified();
                            }
                        }
                    }
                }
            }
            List<ObjectAnimation> Resetlist = objectAnimations.Except(castList).ToList();
            for (int i = 0; i < Resetlist.Count; i++)
            {
                Resetlist[i].TerrifiedReset();
                objectAnimations.Remove(Resetlist[i]);
            }
        }
    }

    public Vector3[] LinePositions()
    {
        for (int i = 0; i < LineObjs.Count; i++)
        {
            lines.SetValue(LineObjs[i].transform.localPosition, i);
        }
        return lines;
    }
    public void LineActive(bool IsActive)
    {
        line.enabled = IsActive;
        // for (int i = 0; i < LineObjs.Count; i++)
        // {
        //     LineObjs[i].SetActive(IsActive);
        // }
        if (IsActive == false)
        {
            FirstTouch = false;
        }
    }
}


