using CHARK.GameManagement.Serialization;

namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which does nothing (placeholder).
    /// </summary>
    internal sealed class DefaultStorage : Storage
    {
        public static DefaultStorage Instance { get; } = new();

        private DefaultStorage() : base(DefaultSerializer.Instance, default)
        {
        }

        public override bool TryGetValue<TValue>(string path, out TValue value)
        {
            value = default;
            return false;
        }

        public override void SetValue<TValue>(string path, TValue value)
        {
        }

        protected override string GetString(string path)
        {
            return default;
        }

        protected override void SetString(string path, string value)
        {
        }

        protected override void Delete(string path)
        {
        }
    }
}
