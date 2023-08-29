using System;
using UnityEngine;

public class GameOverMenu : MonoBehaviour, IClosable
{
    [SerializeField] private CharacterDeath _deaths;
    [SerializeField] private PieBasket _basket;
    [SerializeField] private GameTimer _timer;
    
    public event Action Closed;
    public event Action Opened;

    private void OnEnable()
    {
        Opened?.Invoke();
    }

    private void OnDisable()
    {
        Closed?.Invoke();
    }

    public Score GetDeathsScore()
    {
        return new Score(_deaths.DeathsCount, _deaths.DeathsCount * -10);
    }

    public Score GetTImeScore()
    {
        return new Score(_timer.ElapsedTIme, (int)(_timer.ElapsedTIme * -0.1));
    }

    public Score GetCakesScore()
    {
        return new Score(_basket.PieCount, _basket.PieCount * 100);
    }
}