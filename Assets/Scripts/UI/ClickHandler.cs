using UnityEngine;
using UnityEngine.SceneManagement; 

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; 

    private void OnMouseDown()
    {
        Debug.Log("Object clicked: " + gameObject.name);
        LoadScene();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Object touched: " + gameObject.name);
                    LoadScene();
                }
            }
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("No scene name set in ClickHandler!");
        }
    }
}