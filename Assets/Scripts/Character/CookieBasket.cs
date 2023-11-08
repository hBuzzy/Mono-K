using System;
using UnityEngine;

public class CookieBasket : MonoBehaviour
{
    private int _cookiesCount;

    public int CookiesCount => _cookiesCount;

    public event Action<int> CookieAdded;
    
    public void AddPie()
    {
        CookieAdded?.Invoke(++_cookiesCount);
    }
}