using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }
    private void OnTriggerEnter(Collider other)
    {
        //플레이어가 타격하는 대상의 태그
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    private IEnumerator AutoDisable()
    {
        //0.1초후에 오브젝트가 사라지도록한다.
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
