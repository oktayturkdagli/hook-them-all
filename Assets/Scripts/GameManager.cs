using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem pulledEffect;

    void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        EventManager.current.onFinishGame += OnFinishGame;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
        EventManager.current.onEnemyHit += OnEnemyHit;
        EventManager.current.onEnemyPulled += OnEnemyPulled;
        EventManager.current.OnStartGame();
    }

    void OnStartGame()
    {
        // Debug.Log("Game is START!");
        Invoke(nameof(LateStart), 1f); // Game starts 1 second late to wait for all classes to load correctly
    }

    void OnFinishGame()
    {
        // Debug.Log("Game is OVER!");
    }
    
    void OnWinGame()
    {
        
    }
    
    void OnLoseGame()
    {
        
    }
    
    void OnEnemyHit()
    {
        
    }
    
    void OnEnemyPulled()
    {
        pulledEffect.Play();
    }

    void LateStart()
    {
        
    }

    void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
        EventManager.current.onFinishGame -= OnFinishGame;
        EventManager.current.onWinGame -= OnWinGame;
        EventManager.current.onLoseGame -= OnLoseGame;
        EventManager.current.onEnemyHit -= OnEnemyHit;
        EventManager.current.onEnemyPulled -= OnEnemyPulled;
    }
}
