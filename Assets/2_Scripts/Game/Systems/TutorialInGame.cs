using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialInGame : MonoBehaviour
{
    void Start()
    {
        ItsFirtsTimePlaying();
    }

    public void ItsFirtsTimePlaying()
    {
        if (GameManager.instance.firstTimePlaying)
        {
            GameManager.instance.firstTimePlaying = false;
            UIManager.instance.TutorialFirstTimePlaying();
            Time.timeScale = 0;
        }
    }
}
