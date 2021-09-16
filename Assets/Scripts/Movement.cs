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
        }
        else if (ThisAudioSource.isPlaying)
            ThisAudioSource.Stop();
    }

    void ProcessRotation()
    {
        var right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        var left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

        if (right && !left)
        {
            Rotate(-RotationSpeed);
        }
        else if (left && !right)
        {
            Rotate(RotationSpeed);
        }
        else if (left && right)
        {
            Debug.Log("Cant rotate conflict comands");
        }

    }

    private void Rotate(float rotationSpeed)
    {
        ThisRigitbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        ThisRigitbody.freezeRotation = false;
    }
}
