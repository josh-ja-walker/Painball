using System.Collections;
using UnityEngine;

public class FadeTrigger : MonoBehaviour {

    [SerializeField] private float waitTime;
    [SerializeField] private float timeScale;
    [SerializeField] private Animator fadeAnim;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeOut() {
        while (true) {
            fadeAnim.SetBool("fading", true);
            Time.timeScale = timeScale;

            yield return new WaitForSecondsRealtime(waitTime);

            fadeAnim.SetBool("fading", false);
            gameObject.SetActive(false);

            break;
        }

    }
}
