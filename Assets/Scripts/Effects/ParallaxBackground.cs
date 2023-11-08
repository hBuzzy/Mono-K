using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 _parallaxEffectMultiplier;
    
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitSizeX;
    
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void Update()
    {
        Vector3 cameraPosition = _cameraTransform.position;
        Vector3 position = transform.position;
        Vector3 deltaMovement = cameraPosition - _lastCameraPosition;
        
        position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x,
            deltaMovement.y * _parallaxEffectMultiplier.y);
        
        _lastCameraPosition = cameraPosition;

        if (Mathf.Abs(_cameraTransform.position.x - position.x) >= _textureUnitSizeX)
        {
            float offsetPositionX = (_cameraTransform.position.x - position.x) % _textureUnitSizeX;
            position = new Vector3(cameraPosition.x + offsetPositionX, position.y);
        }
        
        transform.position = position;
    }
}
