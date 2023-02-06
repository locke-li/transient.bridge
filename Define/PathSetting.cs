
using System.IO;
using System.Text;
using UnityEngine;

namespace Transient.Bridge {
    public static class PathSetting {
#if UNITY_EDITOR
        public static string ManifestPrefix = "_";//should match those in graphs
        public static string LuaPathEditor = "SrcLua";
        public static string BuildStagingPath = "Assets/AssetStaging/_build";
        public static string AssetManifestPackedPath => $"{BuildStagingPath}/{AssetManifestFile}{PackExtension}";
#endif
        public static string LuaPathStaging = "_lua_src";
        public static string LuaPathConf = "lua";

        public const string ConfPathUpdated = "conf_updated";
        public const string ConfPath = "conf";
        public const string ConfExt = ".bytes";

        public static string PackExtension = ".pack";
        public static string PackedAssetPath => Path.Combine(Application.streamingAssetsPath, "asset");
        public static string AssetManifestPath => Path.Combine(Application.streamingAssetsPath, $"{AssetManifestFile}{PackExtension}");
        public static string AssetManifestFile => "asset_list";
    }
}