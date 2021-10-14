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
    [SerializeField]
    ParticleSystem successParticles;
    [SerializeField]
    ParticleSystem crashParticles;

    AudioSource thisAudioSource;

    bool isTransitioning = false;
    bool collisionEnable = true;

    private void Start()
    {
        thisAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
            LoadNextLevel();
        if (Input.GetKey(KeyCode.C))
            collisionEnable = !collisionEnable;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || !collisionEnable)
            return;

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
        // todo add particle effect upon crash
        DisableControl();
        thisAudioSource.Stop();
        thisAudioSource.PlayOneShot(crashClip);
        crashParticles.Play();
        Invoke("ReloadLevel", LoadLevelDelay);
        //ReloadLevel();
        isTransitioning = true;
    }

    void StartSuccessSequence()
    {
        // todo add SFX opon success
        // todo add particle effect upon success
        DisableControl();
        thisAudioSource.Stop();
        thisAudioSource.PlayOneShot(successClip);
        successParticles.Play();
        Invoke("LoadNextLevel", LoadLevelDelay);
        isTransitioning = true;
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
        isTransitioning = false;
    }

    public void LoadNextLevel()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);
        isTransitioning = false;
    }
}
