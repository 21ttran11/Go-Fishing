using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HidePassword : MonoBehaviour
{
    private TMP_InputField password;
    // Start is called before the first frame update
    void Start()
    {
        password = gameObject.GetComponent<TMP_InputField>();

        password.contentType = TMP_InputField.ContentType.Password;
        password.ForceLabelUpdate();
    }

}
