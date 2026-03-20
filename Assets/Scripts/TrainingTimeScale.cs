using UnityEngine;

public class TrainingTimeScale : MonoBehaviour
{
    public float timeScale = 20f;

    void Awake()
    {
        Time.timeScale = timeScale;
        Application.targetFrameRate = -1;
    }
}