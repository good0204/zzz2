using System.Collections;
using System.Collections.Generic;
using AlohaCorp.UI;
using UnityEngine;
using UnityEngine.UI;

namespace AlohaCorp.UI
{
    [RequireComponent(typeof(Text))]
    public class FadeTextElement : UIElementBase
    {
        [SerializeField] private float onAlpha = 1;
        [SerializeField] private float offAlpha = 0;

        private Text _text;
        private Color _originalColor;

        protected override void OnStart()
        {
            _text = GetComponent<Text>();
            _originalColor = _text.color;
        }

        protected override void OnProgressChanged(float progress)
        {
            _text.color = 
                new Color(_originalColor.r, _originalColor.g, _originalColor.b, 
                    Mathf.Lerp(offAlpha, onAlpha, progress));
            
            _text.raycastTarget = progress > .1f;
        }
    }
}