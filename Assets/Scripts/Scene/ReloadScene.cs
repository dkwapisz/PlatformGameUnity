using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReloadScene : MonoBehaviour
{
    [SerializeField] float reloadOffset = 2;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (player.GetComponent<CharacterBehaviour>().health <= 0)
        {
            player.GetComponent<CharacterController2D>().HandleDeath();
            pauseGame();
            StartCoroutine(reloadScene());
        }
    }

    IEnumerator reloadScene()
    {
        Debug.Log("YOU DIED. In " + reloadOffset + " the level will be reloaded");
        yield return new WaitForSecondsRealtime(reloadOffset);
        resumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void pauseGame()
    {
        Time.timeScale = 0;
    }
    void resumeGame()
    {
        Time.timeScale = 1;
    }
}
