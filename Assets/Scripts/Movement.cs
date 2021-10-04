using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    float ThrustSpeed = 1000;
    [SerializeField]
    float RotationSpeed = 30;
    [SerializeField]
    AudioClip MainEngine;
    [SerializeField]
    ParticleSystem mainBooster;
    [SerializeField]
    ParticleSystem leftBooster;
    [SerializeField]
    ParticleSystem rightBooster;

    Rigidbody ThisRigitbody = null;
    AudioSource ThisAudioSource = null;

    bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        ThisRigitbody = GetComponent<Rigidbody>();
        ThisAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ThisRigitbody.AddRelativeForce(Vector3.up * ThrustSpeed * Time.deltaTime);
            if (!ThisAudioSource.isPlaying)
                ThisAudioSource.PlayOneShot(MainEngine);
            if (!mainBooster.isPlaying)
                mainBooster.Play();
        }
        else
        {
            if (ThisAudioSource.isPlaying)
                ThisAudioSource.Stop();
            if (mainBooster.isPlaying)
                mainBooster.Stop();
        }
    }

    void ProcessRotation()
    {
        var right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        var left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

        if (right && !left)
        {
            Rotate(-RotationSpeed);
            if (!leftBooster.isPlaying)
                leftBooster.Play();
        }
        else if (left && !right)
        {
            Rotate(RotationSpeed);
            if (!rightBooster.isPlaying)
                rightBooster.Play();
        }
        else if (left && right)
        {
            Debug.Log("Cant rotate conflict comands");
        }
        if (!left && rightBooster.isPlaying)
            rightBooster.Stop();
        if (!right && leftBooster.isPlaying)
            leftBooster.Stop();
    }

    private void Rotate(float rotationSpeed)
    {
        ThisRigitbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        ThisRigitbody.freezeRotation = false;
    }
}
