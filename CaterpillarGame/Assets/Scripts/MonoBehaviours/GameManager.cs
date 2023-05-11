using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _audioSourceContainerPrefab;

    public void Awake()
    {
        ServiceLocator.Setup();
        ServiceLocator.Instance.Register<ConsumerService>(new ConsumerService());
        ServiceLocator.Instance.Register<ConsumabilityService>(new ConsumabilityService());
        ServiceLocator.Instance.Register<ConsumableSpawningService>(new ConsumableSpawningService());
        ServiceLocator.Instance.Register<AudioPlaybackService>(new AudioPlaybackService());

        MonoBehaviourLocator.Setup();
    }

    public void Start()
    {
        GameObject.Instantiate(_audioSourceContainerPrefab, transform);

        AudioPlaybackService audioPlaybackService = ServiceLocator.Instance.Get<AudioPlaybackService>();
        audioPlaybackService.Initialise();

        ConsumableSpawningService consumableSpawningService = ServiceLocator.Instance.Get<ConsumableSpawningService>();
        StartCoroutine(consumableSpawningService.ReplenishConsumablesCoroutine());
    }
}
