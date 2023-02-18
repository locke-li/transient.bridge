#if UNITY_EDITOR
using System.IO;
using System.Text;
using System.Linq;
using Transient;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using static Transient.Bridge.PathDefine;

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
                if (sub.EndsWith("schema")) confRepoPath = sub;
            }
            if (string.IsNullOrEmpty(confRepoPath)) {
                Debug.LogError("failed to locate data repo");
                return;
            }
            var list = new List<(string, string, string[], string[], string[])>();
            if (data) list.Add((confRepoPath + "/output/bytes", AppPath.ConfInternal,
                new string[] { "*.bytes" }, null, null));
            if (schema) list.Add((confRepoPath + "/output/cs", $"Packages/Bridge.Schema/{ScriptSchema}",
                new string[] { "*.cs" }, null, null));
            if (script) list.Add((confRepoPath + "/output/lua", $"{ScriptEditor}/{ScriptSchema}",
                new string[] { "*.lua" }, null, null));
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

        [MenuItem("DevShortcut/Clear Option", priority = 1001)]
        [ExtendableTool("Clear Option", "Build Staging", priority: 1001)]
        public static void ClearOption() => ClearOption(true);

        public static void ClearOption(bool overwrite) {
            var path = AppPath.VersionOption;
            var option = new AppOption();
            if (!overwrite) {
                var s = File.ReadAllBytes(path);
                option.Parse(s);
            }
            option.Reset();
            //TODO set default
            var ss = option.Serialize();
            File.WriteAllBytes(path, ss);
            AssetDatabase.Refresh();
        }

        [MenuItem("DevShortcut/Clear Version", priority = 1002)]
        [ExtendableTool("Clear Version", "Build Staging", priority: 1002)]
        public static void ClearVersion() => ClearVersion(true);

        public static void ClearVersion(bool overwrite) {
            var path = AppPath.VersionInternal;
            var version = new AppVersion();
            if (!overwrite) {
                var s = File.ReadAllBytes(path);
                version.Parse(s);
            }
            version.Reset(0, 0, 0);
            var ss = version.Serialize();
            File.WriteAllBytes(path, ss);
            AssetDatabase.Refresh();
        }

        [MenuItem("DevShortcut/Reset Data Version", priority = 1003)]
        [ExtendableTool("Reset Data Version", "Build Staging", priority: 1003)]
        public static void ResetDataVersion() {
            //TODO
        }
    }
}
#endif