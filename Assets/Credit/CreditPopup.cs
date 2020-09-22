using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreditPopup : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _developersText;

    [SerializeField, TextArea] private string _gameTitle;
    [SerializeField] private List<string> _developers;
    
    void Start()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    void OnEnable()
    {
        SetupTexts();
    }

    private void SetupTexts()
    {
        _titleText.text = _gameTitle;
        var developers = _developers.ToArray();
        for (int i = 0; i < developers.Length - 2; i++)
        {
            var tmp = developers[i];
            var randIdx = Random.Range(i + 1, developers.Length);
            developers[i] = developers[randIdx];
            developers[randIdx] = tmp;
        }

        var developersString = developers[0];
        for (int i = 1; i < developers.Length; i++)
        {
            developersString += $"\n{developers[i]}";
        }
        _developersText.text = developersString;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
