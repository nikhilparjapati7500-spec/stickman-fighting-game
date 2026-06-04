using UnityEngine;
using System.Collections;

/// <summary>
/// Manages cinematic camera behavior for the fighting arena.
/// Includes dynamic zoom, shake effects, and slow-motion support.
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform opponent;
    [SerializeField] private float baseDistance = 8f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float lookAheadDistance = 1f;
    
    [Header("Effects")]
    [SerializeField] private float shakeIntensity = 0.1f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float zoomIntensity = 0.5f;
    
    private Camera mainCamera;
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;
    private Coroutine zoomCoroutine;
    private static CameraManager instance;
    
    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraManager>();
            }
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        originalPosition = transform.position;
    }
    
    private void Update()
    {
        if (player != null && opponent != null)
        {
            UpdateCameraPosition();
        }
    }
    
    private void UpdateCameraPosition()
    {
        // Calculate midpoint between characters
        Vector3 midpoint = (player.position + opponent.position) / 2f;
        
        // Add look-ahead distance
        midpoint += Vector3.forward * lookAheadDistance;
        
        // Position camera
        Vector3 targetPosition = midpoint + Vector3.back * baseDistance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2f);
        
        // Look at center point
        transform.LookAt(midpoint + Vector3.up * 1f);
    }
    
    public void Shake(float intensity = 1f)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(intensity));
    }
    
    private IEnumerator ShakeCoroutine(float intensity)
    {
        float elapsed = 0f;
        Vector3 originalPos = transform.position;
        
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity * intensity;
            transform.position = originalPos + randomOffset;
            
            yield return null;
        }
        
        transform.position = originalPos;
    }
    
    public void Zoom(float zoomAmount, float duration = 0.3f)
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCoroutine(zoomAmount, duration));
    }
    
    private IEnumerator ZoomCoroutine(float zoomAmount, float duration)
    {
        float originalFOV = mainCamera.fieldOfView;
        float targetFOV = originalFOV - zoomAmount;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            mainCamera.fieldOfView = Mathf.Lerp(originalFOV, targetFOV, t);
            yield return null;
        }
        
        mainCamera.fieldOfView = originalFOV;
    }
    
    public void SlowMotion(float timeScale = 0.3f, float duration = 0.5f)
    {
        StartCoroutine(SlowMotionCoroutine(timeScale, duration));
    }
    
    private IEnumerator SlowMotionCoroutine(float timeScale, float duration)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
    
    public void FocusOnCharacter(Transform character, float duration = 0.5f)
    {
        StartCoroutine(FocusCoroutine(character, duration));
    }
    
    private IEnumerator FocusCoroutine(Transform character, float duration)
    {
        Vector3 originalPos = transform.position;
        Vector3 focusPos = character.position + Vector3.back * (baseDistance * 0.6f) + Vector3.up * height;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(originalPos, focusPos, t);
            yield return null;
        }
    }
}
