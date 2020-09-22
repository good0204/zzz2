using System.Collections;
using System.Collections.Generic;
using AlohaCorp.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AlohaCorp.UI
{
    [RequireComponent(typeof(Image))]
    public class FadeImageElement : UIElementBase
    {
        [SerializeField] private float onAlpha = 1;
        [SerializeField] private float offAlpha = 0;
        [SerializeField] private bool raycastTarget = true;

        private Image _image;
        private Color _originalColor;

        protected override void OnStart()
        {
            _image = GetComponent<Image>();
            _originalColor = _image.color;
        }

        protected override void OnProgressChanged(float progress)
        {
            _image.color = 
                new Color(_originalColor.r, _originalColor.g, _originalColor.b, 
                    Mathf.Lerp(offAlpha, onAlpha, progress));

            if (raycastTarget)
            {
                _image.raycastTarget = progress > .1f;   
            }
        }
    }
}