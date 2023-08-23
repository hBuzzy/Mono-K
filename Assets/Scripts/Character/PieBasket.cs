using System;
using TMPro;
using UnityEngine;

public class PieBasket : MonoBehaviour //TODO: Finish
{
    [SerializeField] private TMP_Text _text;

    private int _pieCount;

    public event Action<int> Changed;
    
    public void AddPie()
    {
        _text.text = $"x{++_pieCount}";
    }
}