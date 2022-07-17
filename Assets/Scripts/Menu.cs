using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] CanvasGroup fade;
    int sceneToLoad = -1;
    [SerializeField] AudioSource audioSource;
    // Update is called once per frame
    void Update()
    {
		if (sceneToLoad != -1)
		{
            fade.blocksRaycasts = true;
            fade.alpha += Time.deltaTime * 1.5f;
            audioSource.volume -= Time.deltaTime * 1.7f;
			if (fade.alpha == 1)
			{
                SceneManager.LoadScene(sceneToLoad);
            }
		}
    }

    public void PlaySound(AudioClip clip) => audioSource.PlayOneShot(clip);
    public void LoadScene(int i) => sceneToLoad = i;
    public void Quit() => Application.Quit();
}
