using UnityEngine;
using UnityEngine.UI;

public class ForceRebuildOnEnabled : MonoBehaviour
{
    void OnEnable()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform);
    }
}
