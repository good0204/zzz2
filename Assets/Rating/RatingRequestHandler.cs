using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlohaCorp.UI;
using UnityEngine;
using UnityEngine.UI;

public class RatingRequestHandler : MonoBehaviour
{
    [SerializeField] private UIElementGroup _uiElementGroup;

    [SerializeField] private Button _sureButton;
    [SerializeField] private Button _nextTimeButton;
    [SerializeField] private Button _closeButton;

    private TaskCompletionSource<bool> _closeTask = new TaskCompletionSource<bool>();
    private RatingSystem.Result _result;

    public async Task<RatingSystem.Result> Request(string url)
    {
        _result = RatingSystem.Result.Canceled;

        Open();
        _sureButton.gameObject.SetActive(true);
        _nextTimeButton.gameObject.SetActive(true);
        RegisterButtons(url);
        await _closeTask.Task;
        CleanUp();
        _uiElementGroup.TurnOff(.3f);
        await Task.Delay(300);
        return _result;
    }

    private void RegisterButtons(string url)
    {
        _sureButton.onClick.AddListener(async () =>
        {
            _result = RatingSystem.Result.Yes;
            Application.OpenURL(url);
            await Task.Delay(500);
            Close();
        });

        _nextTimeButton.onClick.AddListener(() =>
        {
            _result = RatingSystem.Result.No;
            Close();
        });

        _closeButton.onClick.AddListener(() =>
        {
            _result = RatingSystem.Result.Canceled;
            Close();
        });
    }

    private void CleanUp()
    {
        _sureButton.onClick.RemoveAllListeners();
        _nextTimeButton.onClick.RemoveAllListeners();
        _closeButton.onClick.RemoveAllListeners();
    }

    private void Open()
    {
        _uiElementGroup.TurnOn(.3f);
    }

    private void Close()
    {
        _closeTask.SetResult(true);
    }
}
