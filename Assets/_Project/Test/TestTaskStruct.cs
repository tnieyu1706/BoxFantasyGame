using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Test
{
    public class TestTaskStruct : MonoBehaviour
    {
        public float progress;

        async void Start()
        {
            await UniTask.Delay(3000);

            CancellationTokenSource cts = new CancellationTokenSource();

            await UniTask.WhenAny(
                TestFunctionAsync(cts.Token),
                UniTask.Delay(4000, cancellationToken: cts.Token)
            );

            cts.Cancel();
            try
            {
                await TestFunctionAsync(cts.Token);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
            }
        }

        async UniTask TestFunctionAsync(CancellationToken token)
        {
            for (int i = 1; i <= 100; i++)
            {
                token.ThrowIfCancellationRequested();
                await UniTask.Delay(50, cancellationToken: token);
                progress = (float)i / 100;
            }
        }
    }
}