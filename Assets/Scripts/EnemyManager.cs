using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private Animator enemyAnimator;
    private Transform _Firepoint;
    private TextMeshPro enemyMultiplier;
    private int unit = 1;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyMultiplier = transform.GetChild(0).GetComponent<TextMeshPro>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Projectile"))
        {
            enemyAnimator.SetTrigger("Pull");
            transform.GetComponent<WanderingAI>().enabled = false;
            transform.GetComponent<NavMeshAgent>().enabled = false;
            EventManager.current.OnEnemyHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bridge"))
        {
            unit = 1;
            string enemyText = enemyMultiplier.text.Replace("+", String.Empty);
            Debug.Log(enemyText);
            int enemyNumber = Int32.Parse(enemyText);
            TextMeshPro bridgeMultiplier = other.transform.GetChild(0).GetComponent<TextMeshPro>();
            string bridgeText = bridgeMultiplier.text.ToString().ToLower();
            string operation = bridgeText.Substring(0,1);
            int bridgeNumber = Int32.Parse(bridgeText.Substring(1, bridgeText.Length - 1));
            int result = enemyNumber;
            switch (operation)
            {
                case "+": result += bridgeNumber; break;
                case "-": result -= bridgeNumber; break;
                case "x": result *= bridgeNumber; break;
                case "/": result /= bridgeNumber; break;
            }
            
            if (result < 1)
                result = 1;

            unit = result;
            enemyMultiplier.text = "+" + result;
            enemyMultiplier.gameObject.SetActive(true);
        }
        else if (other.gameObject.tag.Equals("AllyArea"))
        {
            enemyAnimator.SetTrigger("Stop");
            EventManager.current.OnEnemyPulled();
            EventManager.current.OnEarnAlly(unit);
            transform.gameObject.SetActive(false);
        }
    }
    
}
