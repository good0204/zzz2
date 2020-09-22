using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AlohaCorp.UI
{
    public class UIElementGroup : MonoBehaviour
    {
        public bool IsOn { get; private set; }
        public event Action<bool> OnTransitionDone;

        public float Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnProgressChanged?.Invoke(_progress);
            }
        }

        [SerializeField] private bool turnedOnOnStart;

        private float _progress;

        private event Action<float> OnProgressChanged;
        private Coroutine transitionCoroutine;

        private void Awake()
        {
            if (turnedOnOnStart)
            {
                _progress = 1;
                IsOn = true;
            }
            else
            {
                _progress = 0;
                IsOn = false;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void Subscribe(Action<float> onProgressChanged)
        {
            onProgressChanged(_progress);
            OnProgressChanged += onProgressChanged;
        }

        public void TurnOn(float transitionTime)
        {
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionCoroutine(true, transitionTime));
            IsOn = true;
        }

        public void TurnOff(float transitionTime)
        {
            if (!gameObject.activeSelf)
                return;
            if (transitionCoroutine != null)
                StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionCoroutine(false, transitionTime));
            IsOn = false;
        }

        private IEnumerator TransitionCoroutine(bool on, float transitionTime)
        {
            var targetValue = on ? 1f : 0f;
            var startValue = _progress;
            var estimatedTime = Mathf.Abs(targetValue - startValue) / 1f * transitionTime;
            var timer = 0f;
            while (timer <= estimatedTime)
            {
                timer += Time.unscaledDeltaTime;
                Progress = Mathf.Lerp(startValue, targetValue, timer / estimatedTime);
                yield return null;
            }
            Progress = targetValue;
            OnTransitionDone?.Invoke(on);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}
