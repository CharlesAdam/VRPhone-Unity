using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalDatas;
using MessageReader;
using UnityEngine;
using UnityEngine.UI;

public class ChangingText : MonoBehaviour
{
    public Text text;  // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestUi(){
        Debug.Log("Adding Text CLicked");
        ChangeText("Douglas");
    }

    public void FetchNotification(){
        Task<NotificationData> task = Task.Run (() => new MessageReaderService().FetchMessage());
        StartCoroutine(FetchAndShowNotification(task));
    }
    private void ChangeText(String textToShow){
        Debug.Log("Adding Text " + textToShow);
        String currentText = text.text;
        text.text = String.Format("{0} \n{1}", currentText, textToShow);
    }

    private IEnumerator FetchAndShowNotification(Task<NotificationData> task){
        Debug.Log("Running Fetch Notification");
        yield return new WaitUntil(() => task.IsCompleted);
        var result = task.Result;
        Debug.Log("Notification Completed ");

        result.notificationList.ForEach(
                delegate (NotificationInfo notification)
                {
                    if(notification.app == "com.whatsapp" || notification.app == "org.telegram.messenger"){
                        ChangeText(notification.title + "  " + notification.content);
                    }
                }
        );
    }
    
}

