using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Exit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay =  1f;
void OnTriggerEnter2D(Collider2D other)
{
    StartCoroutine(NextLevel());

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex + 1);
}

IEnumerator NextLevel()
{
    yield return new WaitForSecondsRealtime(levelLoadDelay);  
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex + 1);
}


}
