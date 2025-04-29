using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class UserAuthentication : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text feedbackText;

    [Header("Animation references")]
    public Animator fishTransition;
    public GameObject fields;

    public UnityEvent signIn;
    private FirebaseAuth auth;

    private void Start()
    {
        StartCoroutine(InitializeFirebaseWhenReady());
    }

    private IEnumerator InitializeFirebaseWhenReady()
    {
        while (!FirebaseInitializer.Instance.Ready)
        {
            yield return null;
        }
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Firebase Auth ready.");

        if (auth.CurrentUser != null)
        {
            Debug.Log("User already signed in: " + auth.CurrentUser.Email);
            feedbackText.text = "Welcome back, " + auth.CurrentUser.Email;
            signIn?.Invoke(); 
        }
        else
        {
            Debug.Log("No user is currently signed in.");
            feedbackText.text = "Please log in.";
            if(fishTransition != null)
            {
                fishTransition.Play("fish transition");
            }
            if(fields != null)
            {
                fields.SetActive(true);
            }
        }
    }

    public void RegisterUser()
    {
        if (auth == null)
        {
            feedbackText.text = "Firebase is not ready yet!";
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) 
        {
            feedbackText.text = "Please fill out both fields";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Firebase.Auth.AuthResult authResult = task.Result;
                FirebaseUser newUser = authResult.User;
                Debug.Log($"User registered successfully: {newUser.Email}");
                feedbackText.text = "Registration successful!";
            }
            else
            {
                Debug.LogError($"Registration failed: {task.Exception}");
                feedbackText.text = "Registration failed. Please try again.";
            }
        });
    }

    public void LoginUser()
    {
        if (auth == null)
        {
            feedbackText.text = "Firebase is not ready yet!";
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty (email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please fill out both fields";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Firebase.Auth.AuthResult authResult = task.Result;
                FirebaseUser User = authResult.User;
                feedbackText.text = "Login successful!";
                signIn?.Invoke();
            }
            else
            {
                Debug.LogError($"Login failed: {task.Exception}");
                feedbackText.text = "Login failed. Check your email and password.";
            }
        });
    }
}
