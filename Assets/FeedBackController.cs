using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using SA;
using SO;

public class FeedBackController : MonoBehaviour
{
    public FloatVariable timeSlowMultiplier;
    public MMFeedbacks cameraShake;
    public MMCameraShakeProperties cameraShakeProperties;
    MMCameraShakeProperties prevcameraShakeProperties;
    MMFeedbackCameraShake myCameraShake;
    // Start is called before the first frame update
    void Start()
    {
        myCameraShake = (MMFeedbackCameraShake)cameraShake.Feedbacks[0];
        prevcameraShakeProperties = myCameraShake.CameraShakeProperties;
        //myCameraShake.CameraShakeProperties = cameraShakeProperties;
    }


    public void OnTouchStart()
    {
        cameraShakeProperties.Frequency = prevcameraShakeProperties.Frequency / timeSlowMultiplier.value;
        cameraShakeProperties.Amplitude = prevcameraShakeProperties.Amplitude / timeSlowMultiplier.value;
        cameraShakeProperties.Duration = prevcameraShakeProperties.Duration / timeSlowMultiplier.value;
        myCameraShake.CameraShakeProperties = cameraShakeProperties;
    }


    public void OnTouchEnd()
    {
        myCameraShake.CameraShakeProperties = prevcameraShakeProperties;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
