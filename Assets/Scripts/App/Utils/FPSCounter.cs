using TMPro;  
using UnityEngine;  


public class FPSCounter : MonoBehaviour  
{  
    [SerializeField] private TextMeshProUGUI _fpsText;  
    private static FPSCounter _instance;  
    private float _deltaTime;  
    private int _fps;  
    private void Awake()  
    {  
        if (_instance != null && _instance != this)  
        {  
            Destroy(gameObject);  
            return;  
        }  
        _instance = this;  
        // DontDestroyOnLoad(gameObject);  
        Application.targetFrameRate = -1;  
    }  
    private void Update()  
    {  
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;  
        _fps = Mathf.CeilToInt(1.0f / _deltaTime);  
        _fpsText.text = _fps.ToString();  
    }  
}