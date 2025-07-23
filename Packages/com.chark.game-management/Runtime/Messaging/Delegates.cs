using System.Threading;

#if UNITASK_INSTALLED
using AsyncTask = Cysharp.Threading.Tasks.UniTask;
#else
using AsyncTask = System.Threading.Tasks.Task;
#endif

namespace CHARK.GameManagement.Messaging
{
    public delegate void OnMessageReceived<in TMessage>(TMessage message) where TMessage : IMessage;

    public delegate AsyncTask OnMessageReceivedAsync<in TMessage>(
        TMessage message,
        CancellationToken cancellationToken = default
    ) where TMessage : IMessage;
}
