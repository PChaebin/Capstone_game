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
        //�÷��̾ Ÿ���ϴ� ����� �±�
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    private IEnumerator AutoDisable()
    {
        //0.1���Ŀ� ������Ʈ�� ����������Ѵ�.
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }
}
