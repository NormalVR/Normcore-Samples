#if UNITY_2020_2_OR_NEWER
using UnityEditor;
using UnityEngine;

namespace Normal {
    [FilePath("ProjectSettings/NormcorePackageManager.asset", FilePathAttribute.Location.ProjectFolder)]
    public class PackageManagerOptions : ScriptableSingleton<PackageManagerOptions> {
        [SerializeField]
        private bool _devMode;

        public bool devMode => _devMode;
    }
}
#else
using System;
using UnityEditorInternal;
using UnityEngine;

namespace Normal {
    public class PackageManagerOptions : ScriptableObject {
        private const string __filePath = "ProjectSettings/NormcorePackageManager.asset";

        [SerializeField]
        private bool _devMode;

        private static PackageManagerOptions _instance;

        public static PackageManagerOptions instance {
            get {
                if (_instance == null) {
                    try {
                        // Try to load the serialized file
                        var results = InternalEditorUtility.LoadSerializedFileAndForget(__filePath);
                        if (results.Length > 0 && results[0] is PackageManagerOptions data) {
                            _instance = data;
                        }
                    }
                    catch (Exception) {
                        // Suppress all exceptions related to loading
                    }
                }

                // The file hasn't been created yet, or the loading has failed
                if (_instance == null) {
                    _instance = CreateInstance<PackageManagerOptions>();
                }

                return _instance;
            }
        }

        public bool devMode => _devMode;
    }
}
#endif
