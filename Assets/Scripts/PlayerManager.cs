using System;
using UnityEngine;
using System.Collections.Generic;
using Lean.Touch;

public class PlayerManager: MonoBehaviour
{
    private Animator playerAnimator;
    private void Start()
    {
        EventManager.current.onStartGame += OnStartGame;
        playerAnimator = transform.GetComponent<Animator>();
    }
    
    void OnStartGame()
    {
        
    }

    public void OnTap(LeanFinger finger)
    {
        Debug.Log("Tapped!");
    }

    private void OnDestroy()
    {
        EventManager.current.onStartGame -= OnStartGame;
    }
}