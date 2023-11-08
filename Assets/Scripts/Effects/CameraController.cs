using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Cutscenes _cutscenes; 
    [SerializeField] private CinemachineVirtualCamera _defaultCamera;
    [SerializeField] private Character _character;
    [SerializeField, Range(0, 0.4f)] private float _fallPanAmount = 0.25f;
    [SerializeField, Range(0, 0.5f)] private float _fallYPanTime = 0.35f;
    [SerializeField, Range(-20f, -10f)] private float _yDampingMaxSpeedChange = -15f;
    
    public static CameraController Instance { get; private set; }

    private Coroutine _lerpCoroutine;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;
    private bool _isCutsceneActive;
    private bool _lerpFromFalling;

    public CinemachineVirtualCamera CurrentCamera => _currentCamera;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SetCurrentCamera(_defaultCamera);
    }

    private void OnEnable()
    {
        _cutscenes.ActiveChanged += OnCutsceneActiveChanged;
    }

    private void OnDisable()
    {
        _cutscenes.ActiveChanged -= OnCutsceneActiveChanged;
    }

    private void Update()
    {
        if (_isCutsceneActive)
            return;
        
        if (_character.Velocity.y < _yDampingMaxSpeedChange &&
            _lerpFromFalling == false)
        {
            LerpYDamping(true);
        } 
        else if (_character.Velocity.y >= 0 && _lerpFromFalling)
        {
            _lerpFromFalling = false;
            LerpYDamping(false);
        }
    }

    public void SetCamera(CinemachineVirtualCamera cameraTo)
    {
        if (cameraTo == _currentCamera)
            return;
        
        SwapCameras(_currentCamera, cameraTo);
    }

    public void SetDefaultCamera()
    {
        SetCamera(_defaultCamera);
    }

    private void LerpYDamping(bool isFalling)
    {
        if (_lerpCoroutine != null)
            StopCoroutine(_lerpCoroutine);
        
        _lerpCoroutine = StartCoroutine(LerpYAction(isFalling));
    }

    private IEnumerator LerpYAction(bool isFalling)
    {
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount;

        if (isFalling)
        {
            endDampAmount = _fallPanAmount;
            _lerpFromFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        float elapsedTime = 0f;
        
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            
            _framingTransposer.m_YDamping = 
                Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));

            yield return null;
        }

        _framingTransposer.m_YDamping = endDampAmount;
        _lerpCoroutine = null;
    }

    private void SwapCameras(CinemachineVirtualCamera cameraFrom, CinemachineVirtualCamera cameraTo)
    {
        if (_lerpCoroutine != null)
        {
            StopCoroutine(_lerpCoroutine);
            _lerpCoroutine = null;
        }
        
        cameraTo.enabled = true;
        cameraFrom.enabled = false;

        SetCurrentCamera(cameraTo);
    }

    private void SetCurrentCamera(CinemachineVirtualCamera virtualCamera)
    {
        _currentCamera = virtualCamera;

        var framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        if (!framingTransposer) 
            return;
        
        _framingTransposer = framingTransposer;
        _normYPanAmount = _framingTransposer.m_YDamping;
    }

    private void OnCutsceneActiveChanged(bool isActive)
    {
        _isCutsceneActive = isActive;
    }
}