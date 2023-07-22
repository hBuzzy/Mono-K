using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class GhostTrail : MonoBehaviour
{
    [SerializeField] private float _activeTime = 0.1f;
    [SerializeField] private float _alpha = 0.8f;
    [SerializeField] private float _alphaMultiplier = 0.85f;
    [SerializeField] private Color _color;
    
    private Character _character;
    private SpriteRenderer _characterSpriteRenderer;
    private SpriteRenderer _spriteRenderer;
    
    private float _currentAlpha;
    //private Color _color;
    private float _timeActivated;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (_character == null)
        {
            return;
        }
        _currentAlpha = _alpha;
        _spriteRenderer.sprite = _characterSpriteRenderer.sprite;
        _spriteRenderer.flipX = _characterSpriteRenderer.flipX;

        transform.position = _character.transform.position;

        _timeActivated = Time.time;
    }

    private void Update()
    {
        _currentAlpha *= _alphaMultiplier;
        _color.a = _alpha;
        _spriteRenderer.color = _color;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            GhostTrailPool.Instance.Add(this);
        }
    }

    public void Init(Character character, int sortingLayer)
    {
        _character = character;
        _characterSpriteRenderer = _character.GetComponent<SpriteRenderer>();
        gameObject.layer = sortingLayer;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}