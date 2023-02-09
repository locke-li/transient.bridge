
using System.IO;
using System.Text;
using UnityEngine;
using static Transient.Bridge.PathDefine;

namespace Transient.Bridge {
    public static class PathDefine {
#if UNITY_EDITOR
        public static string ManifestPrefix = "_";//should match those in graphs
        public static string ScriptEditor = "SrcScript";
        public static string BuildStaging = "Assets/AssetStaging/_build";
        public static string AssetManifestPacked => $"{BuildStaging}/{AssetManifestFile}{PackExtension}";
#endif
        public static string ScriptStaging = "script";
        public static string ScriptPersist = "script";

        public static string ConfStaging = "conf";
        public static string ConfPersist = "conf";
        public static string ConfExt = ".bytes";

        public static string VersionStaging = "app_version.info";
        public static string VersionPersist = "app_version";
        public static string VersionOption = "app_option.info";

        public static string PackExtension = ".pack";
        public static string AssetRoot = "asset";
        public static string AssetManifestFile = "asset_list";
        public static string InternalRoot = "";
        public static string ExternalRoot = "_data";
    }

    public static class AppPath {
        public static string ScriptInternal => InternalPath(ScriptStaging);
        public static string ScriptExternal => ExternalPath(ScriptPersist);

        public static string ConfInternal => InternalPath(ConfStaging);
        public static string ConfExternal => ExternalPath(ConfPersist);

        public static string VersionInternal => InternalPath(VersionStaging);
        public static string VersionExternal => ExternalPath(VersionPersist);
        public static string VersionOption => InternalPath(PathDefine.VersionOption);

        public static string PackedAsset => Path.Combine(Application.streamingAssetsPath, AssetRoot);
        public static string AssetManifest => Path.Combine(Application.streamingAssetsPath, $"{AssetManifestFile}{PackExtension}");

        public static string InternalPath(string path) => Path.Combine(Application.streamingAssetsPath, InternalRoot, path);
        public static string ExternalPath(string path) => Path.Combine(Application.persistentDataPath, ExternalRoot, path);
    }
}