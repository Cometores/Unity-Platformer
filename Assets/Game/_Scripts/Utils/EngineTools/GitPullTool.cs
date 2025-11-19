#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game._Scripts.Utils.EngineTools
{
    public static class GitPullTool
    {
        [MenuItem("Tools/Pull the project", priority = 2000)]
        public static void PullProject()
        {
            bool confirm = EditorUtility.DisplayDialog(
                "Pull the project",
                "Эта операция выполнит ЖЁСТКИЙ git pull:\n\n" +
                "- Удалит все локальные изменения (modified, staged, untracked)\n" +
                "- Сбросит ветку к состоянию origin/<текущая ветка>\n\n" +
                "Продолжить?",
                "Да, удалить всё и pull",
                "Отмена"
            );

            if (!confirm)
                return;

            try
            {
                string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
                if (!Directory.Exists(Path.Combine(projectRoot, ".git")))
                {
                    EditorUtility.DisplayDialog(
                        "Ошибка",
                        "В корне проекта не найдена папка .git.\n" +
                        "Убедись, что Unity-проект находится в корне git-репозитория.",
                        "Ок"
                    );
                    return;
                }

                // 1. Проверяем, что git доступен
                if (!RunGitCommand("git --version", projectRoot, out string gitVerOut, out string gitVerErr))
                {
                    EditorUtility.DisplayDialog(
                        "Ошибка git",
                        "Не удалось выполнить 'git --version'. Убедись, что git установлен и добавлен в PATH.\n\n" +
                        gitVerErr,
                        "Ок"
                    );
                    return;
                }

                UnityEngine.Debug.Log($"[GitPullTool] Git detected: {gitVerOut}");

                // 2. fetch
                LogStep("git fetch --all");
                if (!RunGitCommand("git fetch --all", projectRoot, out var fetchOut, out var fetchErr))
                    throw new Exception("git fetch --all failed:\n" + fetchErr);

                // 3. определяем текущую ветку
                LogStep("git rev-parse --abbrev-ref HEAD");
                if (!RunGitCommand("git rev-parse --abbrev-ref HEAD", projectRoot, out var branchName, out var branchErr))
                    throw new Exception("Не удалось определить текущую ветку:\n" + branchErr);

                branchName = branchName.Trim();
                if (string.IsNullOrWhiteSpace(branchName))
                    throw new Exception("Пустое имя ветки от git rev-parse.");

                UnityEngine.Debug.Log($"[GitPullTool] Current branch: {branchName}");

                // 4. жёсткий reset к origin/<branch>
                LogStep($"git reset --hard origin/{branchName}");
                if (!RunGitCommand($"git reset --hard origin/{branchName}", projectRoot, out var resetOut, out var resetErr))
                    throw new Exception("git reset --hard origin/<branch> failed:\n" + resetErr);

                // 5. удаляем неотслеживаемые файлы/папки
                LogStep("git clean -fd");
                if (!RunGitCommand("git clean -fd", projectRoot, out var cleanOut, out var cleanErr))
                    throw new Exception("git clean -fd failed:\n" + cleanErr);

                UnityEngine.Debug.Log("[GitPullTool] Проект успешно обновлён из origin/" + branchName);
                EditorUtility.DisplayDialog(
                    "Готово",
                    $"Проект успешно обновлён из origin/{branchName}.\nВсе локальные изменения удалены.",
                    "Ок"
                );
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("[GitPullTool] Ошибка: " + ex);
                EditorUtility.DisplayDialog(
                    "Ошибка",
                    "Произошла ошибка во время git pull:\n\n" + ex.Message,
                    "Ок"
                );
            }
        }

        private static bool RunGitCommand(string command, string workingDirectory, out string stdout, out string stderr)
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = GetShell(),
                Arguments = GetShellArguments(command),
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            process.Start();
            stdout = process.StandardOutput.ReadToEnd();
            stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(stdout))
                UnityEngine.Debug.Log("[GitPullTool][OUT] " + stdout);
            if (!string.IsNullOrWhiteSpace(stderr))
                UnityEngine.Debug.LogWarning("[GitPullTool][ERR] " + stderr);

            return process.ExitCode == 0;
        }

        private static string GetShell()
        {
#if UNITY_EDITOR_WIN
            return "cmd.exe";
#else
        return "/bin/bash";
#endif
        }

        private static string GetShellArguments(string command)
        {
#if UNITY_EDITOR_WIN
            // /C — выполнить команду и выйти
            return "/C " + command;
#else
        return "-lc \"" + command.Replace("\"", "\\\"") + "\"";
#endif
        }

        private static void LogStep(string step)
        {
            UnityEngine.Debug.Log($"[GitPullTool] >>> {step}");
        }
    }
}
#endif
