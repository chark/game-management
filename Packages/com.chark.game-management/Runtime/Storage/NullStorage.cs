using System.IO;
using CHARK.GameManagement.Serialization;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Storage which does nothing, used as a fall-back when regular storage cannot be created.
    /// </summary>
    internal sealed class NullStorage : Storage
    {
        public static NullStorage Instance { get; } = new();

        private NullStorage() : base(DefaultSerializer.Instance, default)
        {
        }

        public override bool TryReadData<TValue>(string path, out TValue data)
        {
            data = default;
            return false;
        }

        public override void SaveData<TValue>(string path, TValue data)
        {
        }

        protected override Stream ReadStream(string path)
        {
            return Stream.Null;
        }

        protected override void SaveString(string path, string value)
        {
        }

        protected override void SaveStream(string path, Stream stream)
        {
        }

        protected override void Delete(string path)
        {
        }
    }
}
