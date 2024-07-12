using System.Runtime.InteropServices;
using UnityEngine;

namespace Normal.Realtime.Utility {
	public class ARFoundationSetDeviceNativeFramerate : MonoBehaviour {
		private void Awake() {
			Application.targetFrameRate = 120;
			QualitySettings.vSyncCount = 1;
		}
	}
}
