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
            StartThrusting();
        else
            StopThrusting();
    }

    void ProcessRotation()
    {
        var right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        var left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

        if (right && !left)
        {
            StartRightRotation();
        }
        else if (left && !right)
        {
            StartLeftRotation();
        }

        if (!left && rightBooster.isPlaying)
            StopLeftRotation();
        if (!right && leftBooster.isPlaying)
            StopRightRotation();
    }

    private void StartThrusting()
    {
        ThisRigitbody.AddRelativeForce(Vector3.up * ThrustSpeed * Time.deltaTime);
        if (!ThisAudioSource.isPlaying)
            ThisAudioSource.PlayOneShot(MainEngine);
        if (!mainBooster.isPlaying)
            mainBooster.Play();
    }

    private void StopThrusting()
    {
        if (ThisAudioSource.isPlaying)
            ThisAudioSource.Stop();
        if (mainBooster.isPlaying)
            mainBooster.Stop();
    }

    private void StartRightRotation()
    {
        Rotate(-RotationSpeed);
        if (!leftBooster.isPlaying)
            leftBooster.Play();
    }

    private void StartLeftRotation()
    {
        Rotate(RotationSpeed);
        if (!rightBooster.isPlaying)
            rightBooster.Play();
    }

    private void StopLeftRotation()
    {
        rightBooster.Stop();
    }

    private void StopRightRotation()
    {
        leftBooster.Stop();
    }

    private void Rotate(float rotationSpeed)
    {
        ThisRigitbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        ThisRigitbody.freezeRotation = false;
    }
}
