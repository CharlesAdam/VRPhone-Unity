using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GlobalDatas
{
    public class GlobalScore
    {
        public List<NotificationInfo> notificationList { get; set; }

        //  public int Score;

        public GlobalScore()
        {
            notificationList = new List<NotificationInfo>();
        }
    }

    [System.Serializable]
    public class NotificationData
    {
        public List<NotificationInfo> notificationList;

    }

    [System.Serializable]
    public class NotificationInfo
    {
        public string title;

        public string content;

        public string app;

    }
}