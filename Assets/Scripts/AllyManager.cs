using System;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class AllyManager: MonoBehaviour
{
    private Animator allyAnimator;
    private Transform _Firepoint;
    private TextMeshPro allyMultiplier;
    private int unit = 1;

    private void Start()
    {
        allyAnimator = GetComponent<Animator>();
        allyMultiplier = transform.GetChild(0).GetComponent<TextMeshPro>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("EnemyProjectile"))
        {
            allyAnimator.SetTrigger("Pull");
            transform.GetComponent<WanderingAI>().enabled = false;
            transform.GetComponent<NavMeshAgent>().enabled = false;
            EventManager.current.OnAllyHit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bridge"))
        {
            unit = 1;
            string allyText = allyMultiplier.text.Replace("+", String.Empty);
            Debug.Log(allyText);
            int enemyNumber = Int32.Parse(allyText);
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
            allyMultiplier.text = "+" + result;
            allyMultiplier.gameObject.SetActive(true);
        }
        else if (other.gameObject.tag.Equals("EnemyArea"))
        {
            allyAnimator.SetTrigger("Stop");
            EventManager.current.OnAllyPulled();
            EventManager.current.OnLostAlly(unit);
            transform.gameObject.SetActive(false);
        }
    }
}