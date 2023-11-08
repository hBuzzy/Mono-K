using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class CharacterGhost : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _activeTime;
    [SerializeField] private float _intensityChangeSpeed;
    
    private const float MaxIntensity = 0f;
    private const float MinIntensity = 1f;

    private readonly int _dissolveIntensity = Shader.PropertyToID("_DissolveIntensity");

    private Character _character;
    private SpriteRenderer _characterSpriteRenderer;
    private CharacterGhostsPool _ghostsPool;
    private SpriteRenderer _spriteRenderer;
    private Material _ghostMaterial;
    private Coroutine _currentCoroutine;

    private Vector3 _position;
    private float _activatedTime;
    private bool _isHiding;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _ghostMaterial = _spriteRenderer.material;
    }

    private void OnEnable()
    {
        if (_character == null)
            return;

        transform.position = _character.transform.position;
        
        _ghostMaterial.SetFloat(_dissolveIntensity, MaxIntensity);
        
        _spriteRenderer.sprite = _characterSpriteRenderer.sprite;
        _spriteRenderer.flipX = _characterSpriteRenderer.flipX;

        _activatedTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= (_activatedTime + _activeTime) && _isHiding == false)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(Hide());
        }
    }

    public void Init(Character character, SpriteRenderer characterSpriteRenderer, CharacterGhostsPool pool)
    {
        _character = character;
        _ghostsPool = pool;
        _characterSpriteRenderer = characterSpriteRenderer;
        _spriteRenderer.sortingLayerID = _characterSpriteRenderer.sortingLayerID;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _isHiding = false;
    }

    private IEnumerator Hide()
    {
        _isHiding = true;
        yield return ReduceIntensity();
        _ghostsPool.Add(this);
    }

    private IEnumerator ReduceIntensity()
    {
        float currentIntensity = _ghostMaterial.GetFloat(_dissolveIntensity);
        
        while (currentIntensity != MinIntensity)
        {
            currentIntensity =
                Mathf.MoveTowards(currentIntensity, MinIntensity, _intensityChangeSpeed * Time.deltaTime);
            _ghostMaterial.SetFloat(_dissolveIntensity, currentIntensity);
            
            yield return null;
        }
    }
}