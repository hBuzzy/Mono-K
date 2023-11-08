using System;
using UnityEngine;

public class SpikeHeadHitBoxes : MonoBehaviour
{
    [SerializeField] private HitBox _leftHitBox;
    [SerializeField] private HitBox _rightHitBox;
    [SerializeField] private HitBox _upHitBox;
    [SerializeField] private HitBox _downHitBox;

    public event Action<Sides> CharacterEntered;
    
    private void OnEnable()
    {
        _leftHitBox.CharacterEntered += OnLeftHit;
        _rightHitBox.CharacterEntered += OnRightHit;
        _upHitBox.CharacterEntered += OnUpHit;
        _downHitBox.CharacterEntered += OnDownHit;
    }
    
    private void OnDisable()
    {
        _leftHitBox.CharacterEntered -= OnLeftHit;
        _rightHitBox.CharacterEntered -= OnRightHit;
        _upHitBox.CharacterEntered -= OnUpHit;
        _downHitBox.CharacterEntered -= OnDownHit;
    }

    private void OnDownHit()
    {
        CharacterEntered?.Invoke(Sides.Bottom);
    }

    private void OnUpHit()
    {
        CharacterEntered?.Invoke(Sides.Top);
    }

    private void OnRightHit()
    {
        CharacterEntered?.Invoke(Sides.Right);
    }

    private void OnLeftHit()
    {
        CharacterEntered?.Invoke(Sides.Left);
    }
}

public enum Sides
{
    Left = 0,
    Right = 1,
    Top = 2,
    Bottom = 3
}