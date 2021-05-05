using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class DatabaseInstance : MonoBehaviour
{
    void Start()
    {
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseDatabase.DefaultInstance
      .GetReference("CharHP")
      .GetValueAsync().ContinueWith(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
              Debug.Log(task);

          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              Debug.Log(snapshot.Value);
              GameObject.Find("ServerMsg").GetComponent<TMPro.TextMeshProUGUI>().text = snapshot.Value.ToString();
              // Do something with snapshot...
          }
      });
    }
    public void refresh()
    {
        FirebaseDatabase.DefaultInstance
      .GetReference("CharHP")
      .GetValueAsync().ContinueWith(task =>
      {
          if (task.IsFaulted)
          {
              // Handle the error...
              Debug.Log(task);

          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
              Debug.Log(snapshot.Value);
              GameObject.Find("ServerMsg").GetComponent<TMPro.TextMeshProUGUI>().text = snapshot.Value.ToString();
              // Do something with snapshot...
          }
      });
    }
}
