using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class CharacterGhost : MonoBehaviour
{
    [SerializeField] private float _activeTime = 0.1f;
    [SerializeField] private float _intensityChangeSpeed;
    
    private const float MaxIntensity = 1f;
    private const float MinIntensity = -0.1f;
    
    private readonly int _dissolveIntensity = Shader.PropertyToID("_DissolveIntensity");
    //private readonly int _characterSprite = Shader.PropertyToID("_MainTax");
    //private readonly int _dissolveScale = Shader.PropertyToID("_DissolveScale");
    
    private Character _character;
    private SpriteRenderer _characterSpriteRenderer;
    private SpriteRenderer _spriteRenderer;
    private Material _ghostMaterial;
    private Coroutine _currentCoroutine;
    
    private float _timeActivated;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _ghostMaterial = _spriteRenderer.material;
    }

    private void OnEnable()
    {
        if (_character == null)
        {
            return;
        }

        _ghostMaterial.SetFloat(_dissolveIntensity, MaxIntensity);
        
        _spriteRenderer.sprite = _characterSpriteRenderer.sprite;
        _spriteRenderer.flipX = _characterSpriteRenderer.flipX;
        
        //_ghostMaterial.SetTexture(_characterSprite, _spriteRenderer.sprite.texture);
        //_ghostMaterial.SetFloat(_dissolveScale, 300f);

        transform.position = _character.transform.position;

        _timeActivated = Time.time;
    }

    private void Update()
    {
        if (Time.time >= (_timeActivated + _activeTime))
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            _currentCoroutine = StartCoroutine(HideGhost());
        }
    }

    public void Init(Character character)
    {
        _character = character;
        _characterSpriteRenderer = _character.GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerID = _characterSpriteRenderer.sortingLayerID;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator HideGhost()
    {
        yield return ReduceIntensity();
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
        
        CharacterGhostsPool.Instance.Add(this);
    }
}