using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    float LoadLevelDelay = 1f;
    [SerializeField]
    AudioClip successClip;
    [SerializeField]
    AudioClip crashClip;

    AudioSource thisAudioSource;

    private void Start()
    {
        thisAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
         switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly.");
                break;
            case "Finish":
                Debug.Log("Congrads, yo, you finished!");
                StartSuccessSequence();
                break;
            default:
                Debug.Log("Sorry, you blew up!");
                //Invoke("ReloadLevel", 1.0f);
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        // todo add SFX opon crash
        thisAudioSource.PlayOneShot(crashClip);
        // todo add particle effect upon crash
        DisableControl();
        Invoke("ReloadLevel", LoadLevelDelay);
        //ReloadLevel();
    }

    void StartSuccessSequence()
    {
        // todo add SFX opon success
        thisAudioSource.PlayOneShot(successClip);
        // todo add particle effect upon success
        DisableControl();
        Invoke("LoadNextLevel", LoadLevelDelay);
    }

    void DisableControl()
    {
        Movement move = GetComponent<Movement>();
        move.enabled = false;
    }

    void ReloadLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);

    }
}
