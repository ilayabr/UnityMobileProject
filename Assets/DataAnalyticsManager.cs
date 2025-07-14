using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DataAnalyticsManager : Singleton<DataAnalyticsManager>
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("data-consent") || PlayerPrefs.GetInt("data-consent") != 1)
        {
            var analyticsConfirmScreen = Resources.Load<GameObject>("ACS");
            Instantiate(analyticsConfirmScreen, transform);
        }
    }

    public void TrackData(string customEventName, Dictionary<string, object> eventData = null)
    {
        if (!PlayerPrefs.HasKey("data-consent") || PlayerPrefs.GetInt("data-consent") != 1) return;

        if (eventData == null)
            Analytics.CustomEvent(customEventName);
        else
            Analytics.CustomEvent(customEventName, eventData);
    }
}
