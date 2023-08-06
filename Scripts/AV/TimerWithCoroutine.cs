using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimerWithCoroutine : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    private int _timer;

    /// <summary>
    /// Ienumerator which will holds the Ienumerator method that is used to run the timer.
    /// </summary>
    private IEnumerator _timerCoroutine;

    /// <summary>
    /// Triggered when timer is completed and timeout is to be called.
    /// </summary>
    public UnityEvent OnTimeOutEvent;

    private void Start()
    {
        StartTimer(0, null);
    }

    public void StartTimer(int timer, Action onTimeOUt)
    {
        // Reset the timer to 0 before starting the timer
        _timer = 120;
        _timerCoroutine = StartTimer(timer);
        StartCoroutine(_timerCoroutine);
    }

    private IEnumerator StartTimer(int totalTime)
    {
        while (totalTime < _timer)
        {
            // Waiting 1 second in real time and decreasing the timer value
            yield return new WaitForSecondsRealtime(1);
            _timer--;
            timerText.text = _timer.ToString();
            //Debug.Log("Timer is : " + _timer);
        }

        // Trigger the timeout event to inform that the time is up.
        OnTimeOutEvent.Invoke();
    }

    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }
}
