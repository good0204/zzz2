using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Tutorial/Arrow")]
public class TutorialArrow : ScriptableObject
{
    [SerializeField] SpriteRenderer ArrowImg;

    [SerializeField] List<string> ObjectNames = new List<string>();

    public Transform ArrowStart(string ObjectName, Transform ObjectPos)
    {
        SpriteRenderer a = Instantiate(ArrowImg);
        a.transform.SetParent(ObjectPos);
        a.transform.position = ObjectPos.position + new Vector3(0, 3f, 0);
        return a.transform;
    }
    // public bool Checktutorial(string ObjectName)
    // {
    //     if (!ObjectNames.Contains(ObjectName))
    //     {
    //         return true;
    //     }
    //     return false;
    // }
}
