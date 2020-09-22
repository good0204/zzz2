using System;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Rating System")]
public class RatingSystem : ScriptableObject
{
    public event Action<int, Result> OnRatingRequested;

    private const string LAST_REQUEST_KEY = "last_request_rating";
    private const string REQUEST_COUNT_KEY = "request_rating_count";

    [SerializeField] private string _url;
    [SerializeField] private RatingRequestHandler _ratingRequestHandlerPrefab;
    
    public async Task Request()
    {
        if (!PlayerPrefs.HasKey(LAST_REQUEST_KEY))
        {
            PlayerPrefs.SetInt(LAST_REQUEST_KEY, GetHourOffset(DateTime.UtcNow - TimeSpan.FromDays(2)));
        }

        if (!PlayerPrefs.HasKey(REQUEST_COUNT_KEY))
        {
            PlayerPrefs.SetInt(REQUEST_COUNT_KEY, 0);
        }
        
        var lastRequestTime = PlayerPrefs.GetInt(LAST_REQUEST_KEY);
        if (GetHourOffset(DateTime.UtcNow) - lastRequestTime >= 24)
        {
            PlayerPrefs.SetInt(REQUEST_COUNT_KEY, PlayerPrefs.GetInt(REQUEST_COUNT_KEY) + 1);
            PlayerPrefs.SetInt(LAST_REQUEST_KEY, GetHourOffset(DateTime.UtcNow));
            var instance = Instantiate(_ratingRequestHandlerPrefab);
            var result = await instance.Request(_url);
            if (result == Result.Yes)
            {
                PlayerPrefs.SetInt(LAST_REQUEST_KEY, GetHourOffset(DateTime.UtcNow+ TimeSpan.FromDays(1000)));
            }
            OnRatingRequested?.Invoke(PlayerPrefs.GetInt(REQUEST_COUNT_KEY, 1), result);
            Destroy(instance);
        }
    }

    private int GetHourOffset(DateTime time)
    {
        return (int)(time - new DateTime(2019,8,9)).TotalHours;
    }

    public enum Result
    {
        Yes, No, Canceled
    }
}
