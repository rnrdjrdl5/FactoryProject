using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TimerAbility : Ability
{
    [SerializeField] float timerInterval = 1f;
    [SerializeField] bool useUnscaledTime = false;
    float? overrideInterval;

    public event Action OnTimer;

    CancellationTokenSource timerCts;

    public override void Initialize(Parameter parameter)
    {
        base.Initialize(parameter);

        StartTimer();
    }

    public override void Uninitialize()
    {
        StopTimer();

        base.Uninitialize();
    }

    void OnDisable()
    {
        StopTimer();
    }

    public void SetTimerInterval(float interval)
    {
        overrideInterval = interval;
        StartTimer();
    }

    public void ClearTimerIntervalOverride()
    {
        overrideInterval = null;
        StartTimer();
    }

    void StartTimer()
    {
        StopTimer();

        var interval = GetCurrentInterval();
        if (interval <= 0f)
        {
            return;
        }

        timerCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
        UniTask.Void(async () => await TimerLoopAsync(interval, useUnscaledTime, timerCts.Token));
    }

    void StopTimer()
    {
        if (timerCts == null)
        {
            return;
        }

        timerCts.Cancel();
        timerCts.Dispose();
        timerCts = null;
    }

    float GetCurrentInterval()
    {
        return overrideInterval ?? timerInterval;
    }

    async UniTask TimerLoopAsync(float interval, bool ignoreTimeScale, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(interval, ignoreTimeScale: ignoreTimeScale, cancellationToken: cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            OnTimer?.Invoke();
        }
    }
}
