using System.Collections;
using System.Collections.Generic;
using AlohaCorp.UI;
using UnityEngine;

public class ScaleElement : UIElementBase
{
    [SerializeField] private float offScale;
    [SerializeField] private float onScale;

    protected override void OnProgressChanged(float progress)
    {
        var scale = Mathf.LerpUnclamped(offScale, onScale, progress);
        rectTransform.localScale =
            new Vector3(scale, scale, scale);

        gameObject.SetActive(scale > offScale);
    }
}
