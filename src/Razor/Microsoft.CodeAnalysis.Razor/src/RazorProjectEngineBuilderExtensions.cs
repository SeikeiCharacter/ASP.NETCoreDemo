// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.CodeAnalysis.Razor
{
    /// <summary>
    /// Roslyn specific <see cref="RazorProjectEngineBuilder"/> extensions.
    /// </summary>
    public static class RazorProjectEngineBuilderExtensions
    {
        /// <summary>
        /// Sets the C# language version to target when generating code. 
        /// </summary>
        /// <param name="builder">The <see cref="RazorProjectEngineBuilder"/>.</param>
        /// <param name="csharpLanguageVersion">The C# <see cref="LanguageVersion"/>.</param>
        /// <returns>The <see cref="RazorProjectEngineBuilder"/>.</returns>
        public static RazorProjectEngineBuilder SetCSharpLanguageVersion(this RazorProjectEngineBuilder builder, LanguageVersion csharpLanguageVersion)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var existingFeature = builder.Features.OfType<ConfigureParserForCSharpVersionFeature>().FirstOrDefault();
            if (existingFeature != null)
            {
                builder.Features.Remove(existingFeature);
            }

            // This will convert any "latest", "default" or "LatestMajor" LanguageVersions into their numerical equivalent.
            var effectiveCSharpLanguageVersion = LanguageVersionFacts.MapSpecifiedToEffectiveVersion(csharpLanguageVersion);
            builder.Features.Add(new ConfigureParserForCSharpVersionFeature(effectiveCSharpLanguageVersion));

            return builder;
        }

        // Internal for testing
        internal class ConfigureParserForCSharpVersionFeature : IConfigureRazorCodeGenerationOptionsFeature
        {
            public ConfigureParserForCSharpVersionFeature(LanguageVersion csharpLanguageVersion)
            {
                CSharpLanguageVersion = csharpLanguageVersion;
            }

            public LanguageVersion CSharpLanguageVersion { get; }

            public int Order { get; set; }

            public RazorEngine Engine { get; set; }

            public void Configure(RazorCodeGenerationOptionsBuilder options)
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                if (options.Configuration != null && options.Configuration.LanguageVersion.Major < 3)
                {
                    // Prior to 3.0 there were no C# version specific controlled features. Suppress nullability enforcement.
                    options.SuppressNullabilityEnforcement = true;
                    return;
                }

                if (CSharpLanguageVersion < LanguageVersion.CSharp8)
                {
                    // Having nullable flags < C# 8.0 would cause compile errors.
                    options.SuppressNullabilityEnforcement = true;
                }
                else
                {
                    // Given that nullability enforcement can be a compile error we only turn it on for C# >= 8.0. There are
                    // cases in tooling when the project isn't fully configured yet at which point the CSharpLanguageVersion
                    // may be Default (value 0). In those cases that C# version is equivalently "unspecified" and is up to the consumer
                    // to act in a safe manner to not cause unneeded errors for older compilers. Therefore if the version isn't
                    // >= 8.0 (Latest has a higher value) then nullability enforcement is suppressed.
                    //
                    // Once the project finishes configuration the C# language version will be updated to reflect the effective 
                    // language version for the project by our workspace change detectors. That mechanism extracts the correlated
                    // Roslyn project and acquires the effective C# version at that point.
                    options.SuppressNullabilityEnforcement = false;
                }
            }
        }
    }
}
