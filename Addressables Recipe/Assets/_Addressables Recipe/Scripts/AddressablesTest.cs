using UnityEngine;
using UnityEngine.AddressableAssets;
using Normal.Realtime;

public class AddressablesTest : MonoBehaviour {
    [SerializeField] private Realtime       _realtime;
    [SerializeField] private AssetReference _testAsset;

    private void Awake() {
        _realtime.didConnectToRoom += DidConnectToRoom;
    }

    private void DidConnectToRoom(Realtime realtime) {
        // Get the RuntimeKey as a string
        string assetKey = _testAsset.RuntimeKey as string;

        // Instantiate the object
        GameObject gameObject = Realtime.Instantiate(assetKey, Realtime.InstantiateOptions.defaults);

        // Done!
    }
}
