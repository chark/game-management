namespace CHARK.GameManagement.Storage
{
    /// <summary>
    /// Game storage which does nothing (placeholder).
    /// </summary>
    internal sealed class DefaultGameStorage : GameStorage
    {
        public static DefaultGameStorage Instance { get; } = new DefaultGameStorage();

        private DefaultGameStorage() : base(default)
        {
        }

        public override bool TryGetValue<TValue>(string key, out TValue value)
        {
            value = default;
            return false;
        }

        public override void SetValue<TValue>(string key, TValue value)
        {
        }

        protected override string GetString(string key)
        {
            return default;
        }

        protected override void SetString(string key, string value)
        {
        }

        protected override void DeleteKey(string key)
        {
        }
    }
}
