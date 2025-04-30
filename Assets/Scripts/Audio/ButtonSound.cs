using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour
{
    public AudioClip clickSound;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    public void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudio(clickSound);
        }
    }
}