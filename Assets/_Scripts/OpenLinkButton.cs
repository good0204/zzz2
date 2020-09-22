using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenLinkButton : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private string url;

    #endregion

    #region Unity Methods

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => { Application.OpenURL(url); });
        if (url == "https://play.google.com/store/apps/details?id=kr.co.alohacorp.Aloha1010")
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => FireBase.Instance.CrossMarketing());
        }
    }

    #endregion
}

