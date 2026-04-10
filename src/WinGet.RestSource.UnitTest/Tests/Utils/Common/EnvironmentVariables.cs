// -----------------------------------------------------------------------
// <copyright file="EnvironmentVariables.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Winget.RestSource.UnitTest.Tests.Utils.Common
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Msix.Utils.Logger;

    /// <summary>
    /// Some of the test require changes in environment variables.
    /// If we override them and don't restore them, we might have unexpected problems in the future.
    /// </summary>
    public class EnvironmentVariables
    {
        private static readonly Lazy<EnvironmentVariables> Instance = new Lazy<EnvironmentVariables>(() => new EnvironmentVariables());

        private readonly Dictionary<string, string> environmentVariables = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentVariables" /> class.
        /// </summary>
        private EnvironmentVariables()
        {
        }

        /// <summary>
        /// Get instance of the Environment object.
        /// </summary>
        /// <returns>instance.</returns>
        public static EnvironmentVariables GetInstance()
        {
            return Instance.Value;
        }

        /// <summary>
        /// For test cases that require certain environment variables to be set, this method helps set up those variables.
        /// Note: The method must call RestoreEnvironmentVariables method below. Must call restore after each use.
        /// </summary>
        /// <param name="envVariables">map between the variable name and value stored in the environment variable.</param>
        /// <returns><see cref="EnvironmentVariables" />.</returns>
        public static EnvironmentVariables PrepareEnvironmentVariable(Dictionary<string, string> envVariables)
        {
            var env = GetInstance();

            foreach (KeyValuePair<string, string> envVariable in envVariables)
            {
                env.BackupIfNeededAndReplace(envVariable.Key, envVariable.Value);
            }

            return env;
        }

        /// <summary>
        /// This is meant to be consequent of the function PrepareEnvironmentVariable.
        /// </summary>
        /// <param name="env"><see cref="EnvironmentVariables" />.</param>
        /// <param name="variables">List of variables to restore.</param>
        public static void RestoreEnvironmentVariables(EnvironmentVariables env, List<string> variables)
        {
            if (env == null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            env.Restore(variables);
        }

        /// <summary>
        /// Stores an environment variable and its value. It should only save the original value, so subsequents
        /// tries to back up will be ignored, but warned.
        /// </summary>
        /// <param name="name">name of environment variable.</param>
        /// <param name="newValue">new value.</param>
        public void BackupIfNeededAndReplace(string name, string newValue)
        {
            // Only backup the first time this environment variable is getting set for tests.
            if (!this.environmentVariables.ContainsKey(name))
            {
                string value = Environment.GetEnvironmentVariable(name);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Logger.Info($"Backing up variable {name}");
                    this.environmentVariables.Add(name, value);
                }
            }

            Logger.Info($"Setting environment variable {name}");
            Environment.SetEnvironmentVariable(name, newValue);
        }

        /// <summary>
        /// Restores all the environment variables and clears the dictionary.
        /// </summary>
        public void Restore()
        {
            foreach (KeyValuePair<string, string> entry in this.environmentVariables)
            {
                Environment.SetEnvironmentVariable(entry.Key, entry.Value);
            }

            this.environmentVariables.Clear();
        }

        /// <summary>
        /// Only restore a set of variables.
        /// </summary>
        /// <param name="environmentVariables">List of variable to restore.</param>
        public void Restore(List<string> environmentVariables)
        {
            foreach (string variable in environmentVariables)
            {
                bool found = this.environmentVariables.TryGetValue(variable, out string environmentVariableValue);
                if (found)
                {
                    Environment.SetEnvironmentVariable(variable, environmentVariableValue);
                    this.environmentVariables.Remove(variable);
                }
            }
        }
    }
}
