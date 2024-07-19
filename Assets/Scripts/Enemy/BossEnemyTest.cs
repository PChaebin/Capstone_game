using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossEnemyTest : MonoBehaviour
{
    [SerializeField] private Animator bossEnemyAnim;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject enemyAttackCollision;
    [SerializeField] private float sensingRange;
    [SerializeField] private float bossEnemyMoveSpeed;
    [SerializeField] private float enemyHealth = 100;

    private float beginningMoveSpeed;

    private GameObject player;
    private PlayerMove playerScript;
    private CharacterController playerRb;
    private CharacterController bossEnemyRb;
    private Vector3 bossEnemyDir;

    private bool isCognize = false;

    void Start()
    {
        player = GameObject.Find("Player (1)");
        playerRb = player.GetComponent<CharacterController>();

        bossEnemyRb = GetComponent<CharacterController>();

        beginningMoveSpeed = bossEnemyMoveSpeed;
    }

    void Update()
    {
        CognizePlayer();

        if (isCognize)
        {
            MoveBossEnemy();
        }
        else
        {
            bossEnemyAnim.SetBool("isMoving", false);
        }
    }

    private void CognizePlayer()
    {
        if (!isCognize && Physics.CheckSphere(transform.position, sensingRange, playerLayer))
        {
            isCognize = true;
        }
    }

    private void MoveBossEnemy()
    {
        bossEnemyAnim.SetBool("isMoving", true);

        if (bossEnemyAnim.GetBool("isWalkHit"))
        {
            beginningMoveSpeed = 0;
        }

        bossEnemyDir = (playerRb.transform.position - transform.position).normalized;
        Vector3 move = bossEnemyDir * beginningMoveSpeed * Time.deltaTime;
        bossEnemyRb.Move(move);

        bossEnemyRb.transform.LookAt(player.transform);

        beginningMoveSpeed = bossEnemyMoveSpeed;
        Debug.Log(beginningMoveSpeed);
    }    

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, sensingRange);
    }
}
