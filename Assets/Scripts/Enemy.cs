using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    bool isOnPlayer;

    public int health = 100;

    public bool isHit;
    bool isOn;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 2f)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
        }
        else 
        {
            agent.isStopped = true;
        }
        
        if (distance < 2.5f && !isOnPlayer)
        {
            StartCoroutine(HitPlayerHealt());
            isOnPlayer = true;
        }

        if (health <= 0)
        {
            player.GetComponent<Controller>().SetEXP();
            Destroy(gameObject);
        }

        if (isHit && !isOn)
        {
            StartCoroutine(HitHealt());
            isOn = true;
        }
    }

    IEnumerator HitPlayerHealt()
    {
        yield return new WaitForSeconds(1.5f);
        player.GetComponent<Controller>().SetHealthProgress(Random.Range(5, 11));
        isOnPlayer = false;
        Debug.Log("Player Health : " + player.GetComponent<Controller>().health);
    }

    IEnumerator HitHealt()
    {
        health -= 40;
        yield return new WaitForSeconds(0.2f);
        isOn = false;
        isHit = false;
        Debug.Log(health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hit"))
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hit"))
        {
            isHit = false;
        }
    }
}
