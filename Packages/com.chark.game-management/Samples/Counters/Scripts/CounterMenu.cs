using TMPro;
using UnityEngine;

namespace CHARK.GameManagement.Samples.Counters
{
    internal sealed class CounterMenu : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text fixedUpdateCounterText;

        [SerializeField]
        private TMP_Text updateCounterText;

        private ICounterSystem counterSystem;

        private void Awake()
        {
            // You can fetch systems from GameManager in any component
            counterSystem = GameManager.GetActor<ICounterSystem>();
        }

        private void Start()
        {
            // Its best to avoid interacting with systems in Awake.
            // Due to this values are retrieved from the system in Start().
            // In this case though Awake() would work as well.
            fixedUpdateCounterText.text = counterSystem.FixedUpdateCount.ToString();
            updateCounterText.text = counterSystem.UpdateCount.ToString();
        }

        private void OnEnable()
        {
            // Listeners message should be added in OnEnable().
            // This way when the component is enabled (e.g., if it was disabled before), it will
            // it will once again start receiving messages.
            GameManager.AddListener<FixedUpdateCountMessage>(OnFixedUpdateCountMessage);
            GameManager.AddListener<UpdateCountMessage>(OnUpdateCountMessage);
        }

        private void OnDisable()
        {
            // Listeners message should be removed in OnDisable().
            // This way when the component is disabled, it will no longer receive messages.
            GameManager.RemoveListener<FixedUpdateCountMessage>(OnFixedUpdateCountMessage);
            GameManager.RemoveListener<UpdateCountMessage>(OnUpdateCountMessage);
        }

        private void OnFixedUpdateCountMessage(FixedUpdateCountMessage message)
        {
            fixedUpdateCounterText.text = message.Count.ToString();
        }

        private void OnUpdateCountMessage(UpdateCountMessage message)
        {
            updateCounterText.text = message.Count.ToString();
        }
    }
}
