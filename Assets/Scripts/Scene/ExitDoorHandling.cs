using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorHandling : MonoBehaviour {

    [SerializeField] public Sprite closeDoorSprite;
    [SerializeField] public Sprite openDoorSprite;
    private SpriteRenderer spriteRenderer;
    private int allMarksOnScene;
    private bool doorOpened = false;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        var allMarks = GameObject.FindGameObjectsWithTag("Mark");
        allMarksOnScene = allMarks.Length;
    }

    
    void Update() {
        var allMarks = GameObject.FindGameObjectsWithTag("Mark");
        allMarksOnScene = allMarks.Length;

        var bossOne = GameObject.Find("FirstBoss");
        var bossTwo = GameObject.Find("SecondBoss");
        var bossThree = GameObject.Find("ThirdBoss");
        
        if (allMarksOnScene == 0 &&
            bossOne == null && bossTwo == null && bossThree == null) {
            OpenDoor();
        }
    }

    void OpenDoor() {
        spriteRenderer.sprite = openDoorSprite;
        doorOpened = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (doorOpened) {
            if (collision.gameObject.tag.Equals("Player")) {
                if (SceneManager.GetActiveScene().name.Equals("Level3")) {
                    SceneManager.LoadScene("Menu");
                } else if (SceneManager.GetActiveScene().name.Equals("Level2")) {
                    SceneManager.LoadScene("Level3");
                } else if (SceneManager.GetActiveScene().name.Equals("Level1")) {
                    SceneManager.LoadScene("Level2");
                }
            }
        }
    }
}
