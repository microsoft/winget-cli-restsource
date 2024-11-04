// -----------------------------------------------------------------------
// <copyright file="PredicateGenerator.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. Licensed under the MIT License.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.WinGet.RestSource.Cosmos
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.WinGet.RestSource.Utils.Common;
    using Microsoft.WinGet.RestSource.Utils.Constants.Enumerations;
    using Microsoft.WinGet.RestSource.Utils.Models.ExtendedSchemas;
    using Microsoft.WinGet.RestSource.Utils.Models.Objects;
    using Microsoft.WinGet.RestSource.Utils.Models.Schemas;
    using PredicateOperator = LinqKit.PredicateOperator;

    /// <summary>
    /// Expression predicate generator that is safe for usage with Cosmos DB.
    /// </summary>
    public class PredicateGenerator
    {
        private Expression<Func<CosmosPackageManifest, bool>> rootPredicate;
        private Expression<Func<VersionExtended, bool>> versionPredicate;
        private Expression<Func<Installer, bool>> installerPredicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateGenerator"/> class.
        /// </summary>
        public PredicateGenerator()
        {
            this.rootPredicate = PredicateBuilder.False<CosmosPackageManifest>();
            this.versionPredicate = PredicateBuilder.False<VersionExtended>();
            this.installerPredicate = PredicateBuilder.False<Installer>();
        }

        /// <summary>
        /// Adds a condition to this predicate based on specified condition and operator.
        /// </summary>
        /// <param name="condition">Condition to add to predicate.</param>
        /// <param name="predicateOperator">Operator to apply to new condition.</param>
        public void AddCondition(SearchRequestPackageMatchFilter condition, PredicateOperator predicateOperator)
        {
            var op = GetOperator(condition.RequestMatch.MatchType);

            if (GetRootClause(condition, op, out Expression<Func<CosmosPackageManifest, bool>> rootClause))
            {
                this.rootPredicate = predicateOperator == PredicateOperator.Or ? this.rootPredicate.Or(rootClause) : this.rootPredicate.And(rootClause);
            }
            else if (GetVersionClause(condition, op, out Expression<Func<VersionExtended, bool>> versionClause))
            {
                this.versionPredicate = predicateOperator == PredicateOperator.Or ? this.versionPredicate.Or(versionClause) : this.versionPredicate.And(versionClause);
            }
            else if (GetInstallerClause(condition, op, out Expression<Func<Installer, bool>> installerClause))
            {
                this.installerPredicate = predicateOperator == PredicateOperator.Or ? this.installerPredicate.Or(installerClause) : this.installerPredicate.And(installerClause);
            }
        }

        /// <summary>
        /// Merge all sub-predicates into a single predicate, simplifying where possible.
        /// </summary>
        /// <param name="predicateOperator">Operator to use for merging.</param>
        /// <returns>Merged root predicate.</returns>
        public Expression<Func<CosmosPackageManifest, bool>> Generate(PredicateOperator predicateOperator)
        {
            if (this.versionPredicate.IsStarted() || this.installerPredicate.IsStarted())
            {
                if (this.installerPredicate.IsStarted())
                {
                    Expression<Func<VersionExtended, bool>> installerClause = v => v.Installers.Any(this.installerPredicate.Compile());
                    this.versionPredicate = predicateOperator == PredicateOperator.Or ? this.versionPredicate.Or(installerClause) : this.versionPredicate.And(installerClause);
                }

                Expression<Func<CosmosPackageManifest, bool>> versionClause = m => m.Versions.Any(this.versionPredicate.Compile());
                this.rootPredicate = predicateOperator == PredicateOperator.Or ? this.rootPredicate.Or(versionClause) : this.rootPredicate.And(versionClause);
            }

            return this.rootPredicate;
        }

        /// <summary>
        /// Determine if the predicate is the default predicate.
        /// </summary>
        /// <returns>Returns true if the predicate is not default, false otherwise.</returns>
        public bool IsStarted()
        {
            return this.rootPredicate.IsStarted() || this.versionPredicate.IsStarted() || this.installerPredicate.IsStarted();
        }

        private static Expression<Func<string, string, bool>> GetOperator(string matchType)
        {
            return matchType switch
            {
                MatchType.Exact => (field, keyword) => field != null && field.Equals(keyword),
                MatchType.CaseInsensitive => (field, keyword) => field != null && field.Equals(keyword, StringComparison.OrdinalIgnoreCase),
                MatchType.StartsWith => (field, keyword) => field != null && field.StartsWith(keyword, StringComparison.OrdinalIgnoreCase),
                MatchType.Substring => (field, keyword) => field != null && field.Contains(keyword, StringComparison.OrdinalIgnoreCase),
                _ => null,
            };
        }

        private static bool GetRootClause(SearchRequestPackageMatchFilter filter, Expression<Func<string, string, bool>> op, out Expression<Func<CosmosPackageManifest, bool>> clause)
        {
            string keyword = filter.RequestMatch.KeyWord;

            clause = filter.PackageMatchField switch
            {
                PackageMatchFields.PackageIdentifier => m => LinqKit.Extensions.Invoke(op, m.PackageIdentifier, keyword),
                _ => null,
            };

            return clause != null;
        }

        private static bool GetVersionClause(SearchRequestPackageMatchFilter filter, Expression<Func<string, string, bool>> op, out Expression<Func<VersionExtended, bool>> clause)
        {
            string keyword = filter.RequestMatch.KeyWord;

            clause = filter.PackageMatchField switch
            {
                PackageMatchFields.PackageName => v => LinqKit.Extensions.Invoke(op, v.DefaultLocale.PackageName, keyword),
                PackageMatchFields.Publisher => v => LinqKit.Extensions.Invoke(op, v.DefaultLocale.Publisher, keyword),
                PackageMatchFields.Moniker => v => LinqKit.Extensions.Invoke(op, v.DefaultLocale.Moniker, keyword),
                PackageMatchFields.Tag => v => v.DefaultLocale.Tags.Any(t => LinqKit.Extensions.Invoke(op, t, keyword)),
                _ => null,
            };

            return clause != null;
        }

        private static bool GetInstallerClause(SearchRequestPackageMatchFilter filter, Expression<Func<string, string, bool>> op, out Expression<Func<Installer, bool>> clause)
        {
            string keyword = filter.RequestMatch.KeyWord;

            clause = filter.PackageMatchField switch
            {
                PackageMatchFields.PackageFamilyName => i => LinqKit.Extensions.Invoke(op, i.PackageFamilyName, keyword),
                PackageMatchFields.ProductCode => i => LinqKit.Extensions.Invoke(op, i.ProductCode, keyword) || i.AppsAndFeaturesEntries.Any(e => LinqKit.Extensions.Invoke(op, e.ProductCode, keyword)),
                PackageMatchFields.Command => i => i.Commands.Any(c => LinqKit.Extensions.Invoke(op, c, keyword)),
                PackageMatchFields.UpgradeCode => i => i.AppsAndFeaturesEntries.Any(e => LinqKit.Extensions.Invoke(op, e.UpgradeCode, keyword)),
                PackageMatchFields.HasInstallerType => i => LinqKit.Extensions.Invoke(op, i.InstallerType, keyword) || LinqKit.Extensions.Invoke(op, i.NestedInstallerType, keyword),
                _ => null,
            };

            return clause != null;
        }
    }
}
