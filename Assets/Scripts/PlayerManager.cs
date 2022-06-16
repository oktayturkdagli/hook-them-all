using System;
using UnityEngine;
using System.Collections.Generic;
using Lean.Touch;

public class PlayerManager: MonoBehaviour
{
    [SerializeField] private Projectile3D projectile3D;
    
    private void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
    }
    
    void OnStartGame()
    {
        
    }

    public void OnUp(LeanFinger finger)
    {
        projectile3D.isShooted = true;
    }

    private void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
    }
}