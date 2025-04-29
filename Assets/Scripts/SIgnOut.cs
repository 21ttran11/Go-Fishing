using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;


public class SignOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateChild()
    {
        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(true);
        }
    }

    public void DeactivateChild()
    {
        if (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(false);
        }
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("signed out");
    }
}