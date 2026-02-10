using System;

using UnityEngine;
using UnityEngine.Events;

using Leap.Core.Tools;
using Leap.Core.Debug;
using Leap.Data.Web;

using Sirenix.OdinInspector;

public class NotificationReceiver : MonoBehaviour
{
    [Title("AppUser")]
    [SerializeField]
    UnityStringsEvent onRemoteLogin = null;

    [SerializeField]
    UnityEvent onAppUserLocked = null;

    [Title("Notification")]
    [SerializeField]
    UnityIntEvent onNotification = null;

    public void OnNotificationData(FirebaseData data)
    {
        if (DebugManager.Instance.DebugEnabled)
        {
            Debug.Log("-------------------------------------------------------");
            Debug.Log("WebSysUserId : " + data.WebSysUserId);
            Debug.Log("Action : " + data.Action);
            Debug.Log("Information : " + data.Information);
            Debug.Log("Parameter : " + data.Parameter);
            Debug.Log("DisplayMode : " + data.DisplayMode);
            Debug.Log("-------------------------------------------------------");
        }

        int webSysUserId = int.Parse(data.WebSysUserId);

        bool bValid = data.DisplayMode == "1";

        if (data.Action == "RemoteLogin")
        {
            onRemoteLogin.Invoke(data.Information.Split('^'));  // with Title
        }
        else if (data.Action == "PersonLocking")
        {
            if (!bValid)
                onAppUserLocked.Invoke();
            //else
            //    onPersonUnblocked.Invoke();
        }

        onNotification.Invoke(webSysUserId);
    }
}