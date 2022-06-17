using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    private void Awake()
    {
        current = this;
    }

    //Events are created
    public event Action onStartGame;
    public event Action onFinishGame;
    public event Action onWinGame;
    public event Action onLoseGame;
    public event Action onEnemyHit;
    public event Action onEnemyPulled;
    public event Action<int> onEarnAlly;
    public event Action<int> onLostAlly;


    //Events cannot be triggered directly from another class so they are triggered via functions
    public void OnStartGame()
    {
        onStartGame?.Invoke();
    }

    public void OnFinishGame()
    {
        onFinishGame?.Invoke();
    }
    
    public void OnWinGame()
    {
        onWinGame?.Invoke();
    }
    
    public void OnLoseGame()
    {
        onLoseGame?.Invoke();
    }
    
    public void OnEnemyHit()
    {
        onEnemyHit?.Invoke();
    }
    
    public void OnEnemyPulled()
    {
        onEnemyPulled?.Invoke();
    }
    
    public void OnEarnAlly(int unit)
    {
        onEarnAlly?.Invoke(unit);
    }
    
    public void OnLostAlly(int unit)
    {
        onLostAlly?.Invoke(unit);
    }
    
}

