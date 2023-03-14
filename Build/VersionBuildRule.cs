#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static Transient.Bridge.PathDefine;

namespace Transient.Bridge {
    public static class VersionBuildRule {
        [ExtendableTool("Build Version", "Build", priority:500)]
        public static void BuildVersion() {
            //TODO
        }

        private static string FindPath(string root_, string pattern_) {
            string path = null;
            foreach (var sub in Directory.EnumerateDirectories(root_)) {
                if (sub.EndsWith(pattern_)) {
                    path = Path.GetFullPath(sub);
                    break;
                }
            }
            if (path == null) {
                Debug.LogError($"failed to locate path in root={root_}, pattern={pattern_}");
            }
            return path;
        }

        public static async Task<bool> ExternalProcess(string name, string path, params string[] args) {
            void RedirectOutput(object sender, System.Diagnostics.DataReceivedEventArgs e) {
                var line = e.Data;
                if (line.Contains("|Warn")) Debug.LogWarning(line);
                else if (line.Contains("|Error")) Debug.LogError(line);
                else Debug.Log(line);
            }
            var info = new System.Diagnostics.ProcessStartInfo(path) {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var builder = new StringBuilder();
            builder.Append(name).Append(" info: ").Append(path);
            var arg = info.ArgumentList;
            foreach (var g in args) {
                arg.Add(g);
                builder.Append(' ').Append(g);
            }
            Debug.Log(builder.ToString());
            var ps = System.Diagnostics.Process.Start(info);
            ps.OutputDataReceived += RedirectOutput;
            ps.BeginOutputReadLine();
            var w = 0;
            var waitMS = 500;
            var waitCount = 20;
            while (w < waitCount) {
                await Task.Run(() => ps.WaitForExit(waitMS));
                if (ps.HasExited) break;
            }
            if (!ps.HasExited) {
                ps.Kill();
                Debug.LogError($"{name} timed out ({waitMS * waitCount / 1000})");
                return false;
            }
            if (ps.ExitCode != 0) {
                Debug.LogError($"{name} exited with error {ps.ExitCode}");
                return false;
            }
            Debug.Log($"{name} exited");
            return true;
        }

        [ExtendableTool("Build", "DesignData", priority: 1001)]
        private static void DesignDataBuild()
            => Task.Run(() => DesignDataBuildAsync());

        private static async Task<bool> DesignDataBuildAsync() {
            var toolExe = "SchemaFlow" + (AppEnv.IsPlatformUnix ? string.Empty : ".exe");
            var toolPath = "../../schemaflow";
            var path = Path.Combine(toolPath, toolExe);
            if (!File.Exists(path)) {
                toolPath = FindPath("../../../", "schemaflow");
                if (toolPath == null) {
                    Debug.LogError("failed to locate tool repo");
                    return false;
                }
                path = Path.Combine(toolPath, "SchemaFlow/bin/Release/net7.0", toolExe);
                if (!File.Exists(path)) {
                    Debug.LogError($"failed to locate {path}");
                    return false;
                }
            }
            var confRepoPath = FindPath("../../", "schema");
            if (confRepoPath == null) {
                Debug.LogError("failed to locate data repo");
                return false;
            }
            return await ExternalProcess(toolExe, path, "build", confRepoPath);
        }

        [ExtendableTool("Copy", "DesignData", priority: 1002)]
        public static void DesignDataCopyAll() => DesignDataCopy(data: true, schema: true, script: true);

        public static void DesignDataCopy(bool data = true, bool schema = true, bool script = false) {
            var confRepoPath = FindPath("../../", "schema");
            if (confRepoPath == null) {
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

        [MenuItem("DevShortcut/Sync DesignData", priority = 1000)]
        [ExtendableTool("Sync", "DesignData", priority: 1000)]
        private static void DesignDataSync()
            => Task.Run(DesignDataSyncAsync);
        private static async void DesignDataSyncAsync() {
            var r = await DesignDataBuildAsync();
            if (!r) return;
            DesignDataCopyAll();
        }

        [MenuItem("DevShortcut/Reset Option", priority = 1100)]
        [ExtendableTool("Reset Option", "Build Staging", priority: 1100)]
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

        [MenuItem("DevShortcut/Reset Version", priority = 1102)]
        [ExtendableTool("Reset Version", "Build Staging", priority: 1102)]
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

        [MenuItem("DevShortcut/Reset Version\\Data", priority = 1103)]
        [ExtendableTool("Reset Version\\Data", "Build Staging", priority: 1103)]
        public static void ResetDataVersion() {
            //TODO
        }
    }
}
#endif