using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ConsumableFactory
{
    private static Dictionary<ConsumableType, AssetReferenceGameObject> _prefabs = new Dictionary<ConsumableType, AssetReferenceGameObject>();

    public static void Create(ConsumableSpawnpoint consumableSpawnpoint, ConsumableType consumableType)
    {
        AssetReferenceGameObject prefabReference = GetPrefab(consumableType);

        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(prefabReference, consumableSpawnpoint.transform);
        
        handle.Completed += (o) =>
        {
            if (o.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject monumentComponentGO = o.Result;
                OnAssetLoaded(handle, consumableSpawnpoint, consumableType);
            }
            else
            {
                Debug.LogError($"Failed to instantiate consumable");
            }
        };
    }

    private static void OnAssetLoaded(AsyncOperationHandle<GameObject> handle, ConsumableSpawnpoint consumableSpawnpoint, ConsumableType consumableType)
    {
        IConsumable consumable = handle.Result.GetComponent<IConsumable>();
        consumableSpawnpoint.SetConsumable(consumable);
    }

    private static AssetReferenceGameObject GetPrefab(ConsumableType consumableType)
    {
        if(_prefabs.TryGetValue(consumableType, out AssetReferenceGameObject prefab))
        {
            return prefab;
        }

        AssetReferenceGameObject prefabReference = new AssetReferenceGameObject($"Consumables/{consumableType}.prefab");

        if (prefabReference == null)
        {
            Debug.LogError($"Could not find an asset for {consumableType} consumable");
        }

        if (!(_prefabs.TryGetValue(consumableType, out AssetReferenceGameObject prefab2)))
        {
            _prefabs.Add(consumableType, prefabReference);
        }

        return prefabReference;
    }
}