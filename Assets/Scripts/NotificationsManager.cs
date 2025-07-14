using System;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class NotificationsManager : Singleton<NotificationsManager>
{
    void Start()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            ReqAuth();

            var channel = new AndroidNotificationChannel()
            {
                Id = "return_channel",
                Name = "Return channel",
                Importance = Importance.Default,
                Description = "notifications to let the player know they haven played in a while",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            if (PlayerPrefs.HasKey("return_notification_id"))
            {
                int id = PlayerPrefs.GetInt("return_notification_id");
                AndroidNotificationCenter.CancelScheduledNotification(id);
                PlayerPrefs.DeleteKey("return_notification_id");
            }
        }
    }

    void OnApplicationQuit()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            DateTime fireTime = DateTime.Now.AddHours(24);

            var notification = new AndroidNotification
            {
                Title = "come back pls :pleading_face:",
                Text = "We are annoying and evil and what u to come back to our game pls do :3",
                SmallIcon = "icon_0",
                LargeIcon = "icon_1",
                FireTime = fireTime,
            };
            int id = AndroidNotificationCenter.SendNotification(notification, "return_channel");
            PlayerPrefs.SetInt("return_notification_id", id);
            PlayerPrefs.Save();
        }
    }


    void ReqAuth()
    {
        if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) return;

        Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
    }
}