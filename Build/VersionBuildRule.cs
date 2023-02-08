#if UNITY_EDITOR
using System.IO;
using System.Text;
using System.Linq;
using Transient;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using static Transient.Bridge.PathSetting;
using System.Text.RegularExpressions;

namespace Transient.Bridge {
    public static class VersionBuildRule {
        [ExtendableTool("Build Version", "Build", priority:500)]
        public static void BuildVersion() {
            //TODO
        }


        [MenuItem("DevShortcut/Sync Conf", priority = 1000)]
        [ExtendableTool("Sync Conf", "Build Staging", priority: 1000)]
        public static void SyncConfAll() => SyncConf(script: true);

        public static void SyncConf(bool data = true, bool schema = true, bool script = false) {
            var confRepoPath = string.Empty;
            foreach(var sub in Directory.EnumerateDirectories("../../")) {
                if (sub.EndsWith("data")) confRepoPath = sub;
            }
            if (string.IsNullOrEmpty(confRepoPath)) {
                Debug.LogError("failed to locate data repo");
                return;
            }
            var list = new List<(string, string, string[], string[], string[])>();
            if (data) list.Add((confRepoPath + "/gen/rawdata/client", Path.Combine(Application.streamingAssetsPath, ConfPath),
                new string[] { "*.bytes" }, null, null));
            if (schema) list.Add((confRepoPath + "/gen/csharp", "Packages/NS.Bridge.Conf/gen",
                new string[] { "*.cs" }, new string[] { "conf", "rawdata" }, null));
            if (script) list.Add((confRepoPath + "/gen/lua", $"Assets/{ScriptPathEditor}/gen",
                new string[] { "*.lua" }, null, new string[] { "dbstate" }));
            foreach (var (src, _, _, _, _) in list) {
                if (!Directory.Exists(src)) {
                    Debug.LogError($"Config repo path unavailable:{src}");
                    return;
                }
            }
            foreach (var (srcRoot, dstRoot, pattern, white, black) in list) {
                if (Directory.Exists(dstRoot)) Directory.Delete(dstRoot, true);
                Directory.CreateDirectory(dstRoot);
                var srcDir = new DirectoryInfo(srcRoot);
                foreach (var sub in white ?? srcDir.EnumerateDirectories().Select(d => d.Name)) {
                    if (black != null && black.Any(p => sub.Contains(p))) continue;
                    var src = Path.Combine(srcRoot, sub);
                    var dst = Path.Combine(dstRoot, sub);
                    Debug.Log($"{src} -> {dst}");
                    FileUtility.CopyDirectory(src, dst, pattern);
                }
                foreach (var p in pattern) {
                    foreach (var file in srcDir.EnumerateFiles(p)) {
                        file.CopyTo(Path.Combine(dstRoot, $"{file.Name}"), true);
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        [MenuItem("DevShortcut/Clear Version Cache", priority = 1001)]
        [ExtendableTool("Clear Version Cache", "Build Staging", priority: 1001)]
        public static void ClearVersionCache() => ClearVersionCache(true);

        public static void ClearVersionCache(bool overwrite) {
            //TODO
        }

        [MenuItem("DevShortcut/Reset Data Version", priority = 1002)]
        [ExtendableTool("Reset Data Version", "Build Staging", priority: 1002)]
        public static void ResetDataVersion() {
            //TODO
        }
    }
}
#endif