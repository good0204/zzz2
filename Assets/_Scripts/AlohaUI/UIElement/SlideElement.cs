using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlohaCorp.UI
{
    public class SlideElement : UIElementBase
    {
        [SerializeField] private From from;
        
        private Vector2 AnchorCenter => (_anchorMax + _anchorMin) / 2;
        private Vector2 _anchoredPosition;
        private Vector2 _anchorMax;
        private Vector2 _anchorMin;
        private Vector2 _anchoredOffPosition;

        protected override void OnStart()
        {
            _anchorMax = rectTransform.anchorMax;
            _anchorMin = rectTransform.anchorMin;
            _anchoredPosition = rectTransform.anchoredPosition;
            SetOffPosition();
        }

        private void SetOffPosition()
        {
            var groupRectTransformX = groupRectTransform.rect.width;
            var groupRectTransformY = groupRectTransform.rect.height;
            if (from == From.Up)
            {
                var anchorBoundaryY = groupRectTransformY * (1 - AnchorCenter.y);
                _anchoredOffPosition = new Vector2(_anchoredPosition.x, anchorBoundaryY + rectTransform.sizeDelta.y + .3f);
            }
            else if (from == From.Down)
            {
                var anchorBoundaryY = groupRectTransformY * AnchorCenter.y;
                _anchoredOffPosition = new Vector2(_anchoredPosition.x, -anchorBoundaryY - rectTransform.sizeDelta.y - .3f);
            }
            else if (from == From.Left)
            {
                var anchorBoundaryX = groupRectTransformX * (1 - AnchorCenter.x);
                _anchoredOffPosition = new Vector2(-anchorBoundaryX - rectTransform.sizeDelta.x - .3f, _anchoredPosition.y);
            }
            else if (from == From.Right)
            {
                var anchorBoundaryX = groupRectTransformX * AnchorCenter.x;
                _anchoredOffPosition = new Vector2(anchorBoundaryX + rectTransform.sizeDelta.x + .3f, _anchoredPosition.y);
            }
        }

        protected override void OnProgressChanged(float progress)
        {
            gameObject.SetActive(progress != 0);
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(_anchoredOffPosition, _anchoredPosition, progress);
        }

        public enum From
        {
            Up, Down, Left, Right
        }
    }
}