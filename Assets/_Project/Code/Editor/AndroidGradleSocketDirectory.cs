using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

[assembly: InternalsVisibleTo("Threadline.Foundation.EditorTests")]

namespace Threadline.Editor
{
    public sealed class AndroidGradleSocketDirectory : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private static string previousTemp;
        private static string previousTmp;
        private static bool overridden;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            Restore();
            if (Application.platform != RuntimePlatform.WindowsEditor || report.summary.platform != BuildTarget.Android)
                return;

            var directory = FindConfiguredDirectory(Environment.GetEnvironmentVariable("JAVA_TOOL_OPTIONS"));
            if (directory == null || !Directory.Exists(directory))
                return;

            // Unity strips JAVA_TOOL_OPTIONS; the JDK also accepts TEMP for its Unix-domain socket directory.
            Apply(directory);
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            Restore();
        }

        internal static string FindConfiguredDirectory(string options)
        {
            var match = Regex.Match(options ?? string.Empty, @"(?:^|\s)-Djdk\.net\.unixdomain\.tmpdir=(?:""([^""]+)""|(\S+))");
            if (!match.Success)
                return null;
            return match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
        }

        internal static void Apply(string directory)
        {
            Restore();
            previousTemp = Environment.GetEnvironmentVariable("TEMP");
            previousTmp = Environment.GetEnvironmentVariable("TMP");
            overridden = true;
            Environment.SetEnvironmentVariable("TEMP", directory);
            Environment.SetEnvironmentVariable("TMP", directory);
            EditorApplication.update += RestoreAfterBuild;
            AssemblyReloadEvents.beforeAssemblyReload += Restore;
        }

        private static void RestoreAfterBuild()
        {
            if (!BuildPipeline.isBuildingPlayer)
                Restore();
        }

        internal static void Restore()
        {
            if (!overridden)
                return;
            Environment.SetEnvironmentVariable("TEMP", previousTemp);
            Environment.SetEnvironmentVariable("TMP", previousTmp);
            overridden = false;
            EditorApplication.update -= RestoreAfterBuild;
            AssemblyReloadEvents.beforeAssemblyReload -= Restore;
        }
    }
}
