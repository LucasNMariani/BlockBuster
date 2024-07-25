using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
using Unity.VisualScripting;
using System;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    AndroidNotificationChannel _notificationChannel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        _notificationChannel = new AndroidNotificationChannel()
        {
            Id = "stamina_notification",
            Name = "Stamina Notification",
            Description = "This is my descrition",
            Importance = Importance.High
        };

        AndroidNotificationCenter.RegisterNotificationChannel(_notificationChannel);

        DisplayNotification("Block Buster", "No te olvides de jugar los niveles que aún no terminaste o rejugá los que ya completaste", DateTime.Now.AddDays(2));
    }

    public int DisplayNotification(string title, string text, DateTime time)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.SmallIcon = "small_icon";
        //notification.LargeIcon = "big_icon";
        notification.FireTime = time;

        return AndroidNotificationCenter.SendNotification(notification, _notificationChannel.Id);
    }

    public void CancelNotification(int id)
    {
        AndroidNotificationCenter.CancelScheduledNotification(id);
    }

}
