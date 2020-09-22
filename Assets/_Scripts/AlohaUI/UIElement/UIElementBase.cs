using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlohaCorp.UI
{
    public abstract class UIElementBase : MonoBehaviour
    {
        [SerializeField] protected UIElementGroup group;
        [SerializeField] protected AnimationCurve easeCurve;

        [Header("Animating Range")]
        [MinMax(0, 1)]
        [SerializeField] protected Vector2 animatingRange = new Vector2(0, 1);
        protected RectTransform groupRectTransform;
        protected RectTransform rectTransform;

        protected void Start()
        {
            if (group == null)
            {
                var parent = transform.parent;
                while (parent != null && !parent.GetComponent<UIElementGroup>())
                    parent = parent.parent;

                if (parent == null)
                    return;

                group = parent.GetComponent<UIElementGroup>();
            }

            rectTransform = GetComponent<RectTransform>();
            groupRectTransform = group.GetComponent<RectTransform>();
            var transitionGroup = group.GetComponent<UIElementGroup>();
            OnStart();

            transitionGroup.Subscribe(OnCurvedProgress);
        }

        protected void OnCurvedProgress(float progress)
        {
            var range = animatingRange.y - animatingRange.x;
            var progressMapped = Mathf.Clamp01((progress - animatingRange.x) / range);
            OnProgressChanged(easeCurve.Evaluate(progressMapped));
        }

        protected virtual void OnStart() { }
        protected virtual void OnProgressChanged(float progress) { }
    }
}
