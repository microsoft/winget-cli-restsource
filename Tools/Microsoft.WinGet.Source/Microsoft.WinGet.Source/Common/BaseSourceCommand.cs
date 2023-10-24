// -----------------------------------------------------------------------
// <copyright file="BaseSourceCommand.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.Source.Common
{
    using System.Management.Automation;
    using System.Reflection;

    /// <summary>
    /// Base class for all commands in this assembly.
    /// </summary>
    public class BaseSourceCommand : PSCmdlet
    {
        static BaseSourceCommand()
        {
            AssemblyName name = Assembly.GetExecutingAssembly().GetName();
            Serialization.ProducedBy = string.Join(" ", name.Name, name.Version.ToString());
        }
    }
}
