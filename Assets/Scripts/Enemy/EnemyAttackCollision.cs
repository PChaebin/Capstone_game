using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollision : MonoBehaviour
{
    [SerializeField] private Animator bossEnemyAnim;
    [SerializeField] private float attackStrenge = 10;

    private PlayerMove playerScript;
    private bool isInCollider = false;
    private bool isAttacking = false;

    void Start()
    {
        
    }

    void Update()
    {
           
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInCollider = true;
            bossEnemyAnim.SetBool("isWalkHit", true);

            playerScript = GetComponent<PlayerMove>();

            if (!isAttacking && playerScript != null)
            {
                Attack();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        { 
            isInCollider = false;
            bossEnemyAnim.SetBool("isWalkHit", false);
            StopCoroutine(attack());
        }
    }

    private void Attack()
    {
        if(playerScript.playerHealth >= 0)
        {
            StartCoroutine(attack());
           
            Debug.Log("playerHealth : " + playerScript.playerHealth.ToString());
        }              
    }

    IEnumerator attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(1.0f);

        playerScript.playerHealth -= attackStrenge;

        isAttacking = false;
    }

}
