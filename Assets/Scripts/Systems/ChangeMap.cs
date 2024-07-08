using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeMap : MonoBehaviour
{
    public string scenename;
    public float delayTime = 3.0f;
    public TextMeshProUGUI countdownText;
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.tag); // 디버깅용 로그

        if (other.CompareTag("Player") && !isTeleporting)
        {
            Debug.Log("Player entered the portal. Starting teleport to " + scenename);
            StartCoroutine(TeleportDelay());
        }
    }

    private IEnumerator TeleportDelay()
    {
        isTeleporting = true;

        float remainingTime = delayTime;
        while (remainingTime > 0)
        {
            Debug.Log("Teleporting in " + remainingTime.ToString("F1") + " seconds"); // 디버깅용 로그
            countdownText.text = "Teleporting in " + remainingTime.ToString("F1") + " seconds";
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        countdownText.text = "";
        Debug.Log("Loading scene: " + scenename); // 디버깅용 로그
        SceneManager.LoadScene(scenename);
    }
}
