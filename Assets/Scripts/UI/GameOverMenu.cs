using System;
using UnityEngine;

public class GameOverMenu : MonoBehaviour, IClosable
{
    [SerializeField] private CharacterDeath _deaths;
    [SerializeField] private CookieBasket _basket;
    [SerializeField] private GameTimer _timer;
    [SerializeField] private Cookies _cookies;
    
    public event Action Closed;
    public event Action<Score> Opened;

    private void OnEnable()
    {
        Opened?.Invoke(GetScore());
    }

    private void OnDisable()
    {
        Closed?.Invoke();
    }

    private Score GetScore()
    {
        return new Score(_timer.ElapsedTime, _deaths.DeathsCount,
            _basket.CookiesCount, _cookies.CookiesCount);
    }
}