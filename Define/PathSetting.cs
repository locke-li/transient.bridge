
using System.IO;
using System.Text;
using UnityEngine;

namespace Transient.Bridge {
    public static class PathSetting {
#if UNITY_EDITOR
        public static string ManifestPrefix = "_";//should match those in graphs
        public static string ScriptPathEditor = "SrcScript";
        public static string BuildStagingPath = "Assets/AssetStaging/_build";
        public static string AssetManifestPackedPath => $"{BuildStagingPath}/{AssetManifestFile}{PackExtension}";
#endif
        public static string ScriptPathStaging = "script";
        public static string ScriptPath = "script";

        public static string ConfPathStaging = "conf";
        public static string ConfPath = "conf";
        public static string ConfExt = ".bytes";

        public static string PackExtension = ".pack";
        public static string PackedAssetPath => Path.Combine(Application.streamingAssetsPath, "asset");
        public static string AssetManifestPath => Path.Combine(Application.streamingAssetsPath, $"{AssetManifestFile}{PackExtension}");
        public static string AssetManifestFile => "asset_list";
        public static string InternalRoot = "";
        public static string ExternalRoot = "_data";

        public static string InternalPath(string path) => Path.Combine(Application.streamingAssetsPath, InternalRoot, path);

        public static string ExternalPath(string path) => Path.Combine(Application.persistentDataPath, ExternalRoot, path);
    }
}