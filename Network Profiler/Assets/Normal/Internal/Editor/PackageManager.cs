using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using UnityEngine;
using Normal.Internal.SimpleJson;

namespace Normal {
    [InitializeOnLoad]
    public class PackageManager {
        private class UninstallHelper : UnityEditor.AssetModificationProcessor {
            private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions _) {
                var packageManagerScriptPath = AssetDatabase.GUIDToAssetPath(__packageManagerFileGUID);

                if (packageManagerScriptPath != null && packageManagerScriptPath.StartsWith(path)) {
                    var shouldRemovePackage = EditorUtility.DisplayDialog(
                        "Uninstall Normcore?",
                        "Normcore Assets folder has been deleted. Would you also like to uninstall the Normcore UPM Package?\n\nPackages can be managed at any time in \"Window > Package Manager\".",
                        "Remove the Normcore package",
                        "Ignore"
                    );

                    if (shouldRemovePackage) {
                        Client.Remove(__bundleID);
                    }
                }

                return AssetDeleteResult.DidNotDelete;
            }
        }

        private const  string __registryURL = "https://normcore-registry.normcore.io";
        private const  string __bundleID = "com.normalvr.normcore";
        private static bool   __debugLogging = false;

        // The GUID of this file, to accelerate the path lookup
        private const string __packageManagerFileGUID = "da51cf8c2795e431297ad1a1ece46c8d";

        private static ListRequest __listRequest;
        private static AddRequest  __addRequest;

        static PackageManager() {
            // Check if the package is already to the project
            CheckIfPackageExists();
        }

        private static void CheckIfPackageExists() {
            // For our internal projects, keep the installation flag file (for distribution) and don't install the package
            if (PackageManagerOptions.instance.devMode) {
                flaggedForInstall = true;
                return;
            }

            // Have we already installed the package once before?
            if (flaggedForInstall == false) {
                return;
            }

            if (__listRequest != null) {
                Debug.LogError("Normcore Package Manager: List request already running. Ignoring.");
                return;
            }

            if (__debugLogging) Debug.Log("Normcore Package Manager: Checking if Normcore is installed");
            __listRequest = Client.List(true);
            EditorApplication.update += CheckListRequestProgress;
        }

        private static void CheckListRequestProgress() {
            if (__listRequest == null) {
                EditorApplication.update -= CheckListRequestProgress;
                return;
            }

            if (__listRequest.IsCompleted) {
                if (__listRequest.Status == StatusCode.Success) {
                    bool normcoreFound = __listRequest.Result.Any(p => p.name == __bundleID);
                    if (__debugLogging) Debug.Log("Normcore Package Manager: Normcore found: " + normcoreFound);

                    if (normcoreFound == false)
                        TryAddNormcore();
                    else
                        flaggedForInstall = false;
                }
                
                __listRequest = null;
            }
        }

        private static void TryAddNormcore() {
            if (AddScopedRegistryIfNeeded())
                AddPackage();
        }

        private static bool AddScopedRegistryIfNeeded() {
            // Load packages.json
            string packageManifestPath = Application.dataPath.Replace("/Assets", "/Packages/manifest.json");
            string packageManifestJSON = File.ReadAllText(packageManifestPath);
            
            // Deserialize
            SimpleJson.TryDeserializeObject(packageManifestJSON, out object packageManifestObject);
            JsonObject packageManifest = packageManifestObject as JsonObject;
            if (packageManifest == null) {
                Debug.LogError("Normcore Package Manager: Failed to read project package manifest. Unable to add Normcore.");
                return false;
            }

            
            // Create scoped registries array if needed
            packageManifest.TryGetValue("scopedRegistries", out object scopedRegistriesObject);
            JsonArray scopedRegistries = scopedRegistriesObject as JsonArray;
            if (scopedRegistries == null)
                packageManifest["scopedRegistries"] = scopedRegistries = new JsonArray();
            
            // Check for Normal registry
            bool normalRegistryFound = scopedRegistries.Any(registryObject => {
                JsonObject registry = registryObject as JsonObject;
                if (registry == null) return false;

                return registry.TryGetValue("url", out object registryURL) && (registryURL as string) == __registryURL;
            });
            if (normalRegistryFound) {
                if (__debugLogging) Debug.Log("Normcore Package Manager: Found Normal registry");
                return true;
            }

            // Add Normal registry
            JsonObject normalRegistry = new JsonObject();
            normalRegistry["name"] = "Normal";
            normalRegistry["url"]  = __registryURL;
            normalRegistry["scopes"] = new JsonArray { "com.normalvr", "io.normcore" };
            scopedRegistries.Add(normalRegistry);

            // Serialize and save
            string packageManifestJSONUpdated = SimpleJson.SerializeObject(packageManifest);
            File.WriteAllText(packageManifestPath, packageManifestJSONUpdated);
            if (__debugLogging) Debug.Log("Normcore Package Manager: Added Normal registry");

            return true;
        }

        private static void AddPackage() {
            if (__addRequest != null) {
                Debug.LogError("Normcore Package Manager: Add request already running. Ignoring.");
                return;
            }

            Debug.Log("Normcore Package Manager: Adding Normcore package to project.");
            __addRequest = Client.Add(__bundleID);
            EditorApplication.update += CheckAddRequestProgress;
        }

        private static void CheckAddRequestProgress() {
            if (__addRequest == null) {
                EditorApplication.update -= CheckListRequestProgress;
                return;
            }

            if (__addRequest.IsCompleted) {
                if (__addRequest.Status == StatusCode.Success) {
                    if (__debugLogging) Debug.Log("Normcore Package Manager: Success!");
                } else if (__addRequest.Status >= StatusCode.Failure)
                   Debug.LogError("Normcore Package Manager: Failed to add normcore package: " + __addRequest.Error.message);
                
                __addRequest = null;
            }
        }

        private const string __installFlagFileName = "PackageInstallFlag.asset";

        private static bool TryGetInstallFlagFilePath(out string path) {
            var scriptPath = AssetDatabase.GUIDToAssetPath(__packageManagerFileGUID);
            if (string.IsNullOrEmpty(scriptPath)) {
                path = default;
                return false;
            }

            var directoryPath = Path.GetDirectoryName(scriptPath);
            if (string.IsNullOrEmpty(directoryPath)) {
                path = default;
                return false;
            }

            path = Path.Combine(directoryPath, __installFlagFileName);
            return true;
        }

        private static bool flaggedForInstall {
            get {
                if (TryGetInstallFlagFilePath(out var path) == false) {
                    return false;
                }

                var fileExists = File.Exists(path);
                return fileExists;
            }

            set {
                if (TryGetInstallFlagFilePath(out var path) == false) {
                    return;
                }

                var fileShouldExist = value;
                var fileExists = File.Exists(path);

                if (fileShouldExist == fileExists) {
                    return;
                }

                if (fileShouldExist) {
                    var file = new TextAsset();
                    EditorUtility.SetDirty(file);
                    AssetDatabase.CreateAsset(file, path);
                }
                else {
                    AssetDatabase.DeleteAsset(path);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
