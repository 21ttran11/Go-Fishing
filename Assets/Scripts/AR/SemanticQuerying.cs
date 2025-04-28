using Niantic.Lightship.AR.Semantics;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SemanticQuerying : MonoBehaviour
{
    public ARCameraManager _cameraMan;
    public ARSemanticSegmentationManager _semanticMan;
    //public TMP_Text _text;

    private string _channel = "ground";
    private float _timer = 0.0f;

    private float regionWidth = 0.2f;
    private float regionHeight = 0.2f;

    public string Channel => _channel; 

    void OnEnable()
    {
        if (_cameraMan != null)
        {
            _cameraMan.frameReceived += OnCameraFrameUpdate;
        }
    }

    void OnDisable()
    {
        if (_cameraMan != null)
        {
            _cameraMan.frameReceived -= OnCameraFrameUpdate;
        }
    }

    private void OnCameraFrameUpdate(ARCameraFrameEventArgs args)
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }
    }

    void Update()
    {
        if (!_semanticMan.subsystem.running)
        {
            return;
        }

        Vector2 centerPos = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 regionMin = new Vector2(centerPos.x - (Screen.width * regionWidth / 2), centerPos.y - (Screen.height * regionHeight / 2));
        Vector2 regionMax = new Vector2(centerPos.x + (Screen.width * regionWidth / 2), centerPos.y + (Screen.height * regionHeight / 2));

        _timer += Time.deltaTime;
        if (_timer > 0.05f)
        {
            var list = _semanticMan.GetChannelNamesAt((int)centerPos.x, (int)centerPos.y);

            if (list.Count > 0)
            {
                _channel = list[0];
                Debug.Log($"Detected semantic channel: {_channel}");

                //_text.text = $"Detected Channel: {_channel}";
            }
            else
            {
                Debug.Log("No semantic channel detected at the center.");

                //_text.text = "No channel detected";
            }

            _timer = 0.0f;
        }
    }
}