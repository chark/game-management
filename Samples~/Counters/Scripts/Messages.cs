using CHARK.GameManagement.Messaging;

namespace CHARK.GameManagement.Samples.Counters
{
    internal readonly struct FixedUpdateCountMessage : IMessage
    {
        public long Count { get; }

        public FixedUpdateCountMessage(long count)
        {
            Count = count;
        }
    }

    internal readonly struct UpdateCountMessage : IMessage
    {
        public long Count { get; }

        public UpdateCountMessage(long count)
        {
            Count = count;
        }
    }
}
