using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Normal.Realtime;

public class CustomPrefabLoadDelegate : MonoBehaviour, IRealtimePrefabLoadDelegate {
    [SerializeField]
    private List<AssetReference> _realtimePrefabsToPreload = new List<AssetReference>();
    private Dictionary<string, AsyncOperationHandle<GameObject>> _assets;

    private void Start() {
        // Preload all assets
        _assets = new Dictionary<string, AsyncOperationHandle<GameObject>>();
        foreach (AssetReference assetToLoad in _realtimePrefabsToPreload) {
            if (assetToLoad.RuntimeKeyIsValid() == false)
                continue;

            string key = assetToLoad.RuntimeKey as string;
            if (key == null)
                continue;

            _assets.Add(key, Addressables.LoadAssetAsync<GameObject>(key));
        }
    }

    private void OnDestroy() {
        // Unload all assets
        // Note: Make sure all of your realtime prefabs are destroyed before this script is destroyed otherwise it may unload assets that are in use by realtime prefabs in the scene.
        foreach (var assetHandle in _assets.Values) {
            Addressables.Release(assetHandle);
        }
    }

    public GameObject LoadRealtimePrefab(RealtimePrefabMetadata prefabMetadata) {
        string key = prefabMetadata.prefabName;
        AsyncOperationHandle<GameObject> asset = default;

        // Check if we have already preloaded this asset.
        if (_assets.TryGetValue(key, out asset) == false) {
            // Not found, log a warning that the asset will need to be loaded into memory.
            Debug.LogWarning($"CustomPrefabLoadDelegate: Asked to load a prefab that doesn't exist in our list of preloaded assets. Will load synchronously, but this may be slow if the asset isn't already loaded.");

            // Start loading the asset
            asset = Addressables.LoadAssetAsync<GameObject>(key);

            // Store the load operation for future calls
            _assets.Add(key, asset);
        }

        // Wait for the asset to load (anything preloaded will return instantly)
        return asset.WaitForCompletion();
    }
}
