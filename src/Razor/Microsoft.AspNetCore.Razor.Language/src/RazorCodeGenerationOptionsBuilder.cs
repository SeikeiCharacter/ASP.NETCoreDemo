// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Language
{
    public abstract class RazorCodeGenerationOptionsBuilder
    {
        public virtual RazorConfiguration Configuration => null;

        public abstract bool DesignTime { get; }

        public virtual string FileKind => null;

        public abstract int IndentSize { get; set; }

        public abstract bool IndentWithTabs { get; set; }

        /// <summary>
        /// Gets or sets the root namespace of the generated code.
        /// </summary>
        public virtual string RootNamespace { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to suppress the default <c>#pragma checksum</c> directive in the 
        /// generated C# code. If <c>false</c> the checkum directive will be included, otherwise it will not be
        /// generated. Defaults to <c>false</c>, meaning that the checksum will be included.
        /// </summary>
        /// <remarks>
        /// The <c>#pragma checksum</c> is required to enable debugging and should only be supressed for testing
        /// purposes.
        /// </remarks>
        public abstract bool SuppressChecksum { get; set; }

        /// <summary>
        /// Gets or setsa value that indicates whether to suppress the default metadata attributes in the generated 
        /// C# code. If <c>false</c> the default attributes will be included, otherwise they will not be generated.
        /// Defaults to <c>false</c> at run time, meaning that the attributes will be included. Defaults to
        /// <c>true</c> at design time, meaning that the attributes will not be included.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <c>Microsoft.AspNetCore.Razor.Runtime</c> package includes a default set of attributes intended
        /// for runtimes to discover metadata about the compiled code.
        /// </para>
        /// <para>
        /// The default metadata attributes should be suppressed if code generation targets a runtime without
        /// a reference to <c>Microsoft.AspNetCore.Razor.Runtime</c>, or for testing purposes.
        /// </para>
        /// </remarks>
        public virtual bool SuppressMetadataAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value that determines if an empty body is generated for the primary method.
        /// </summary>
        public virtual bool SuppressPrimaryMethodBody { get; set; }

        /// <summary>
        /// Gets or sets a value that determines if nullability type enforcement should be suppressed for user code.
        /// </summary>
        public virtual bool SuppressNullabilityEnforcement { get; set; }

        public abstract RazorCodeGenerationOptions Build();

        public virtual void SetDesignTime(bool designTime)
        {
        }
    }
}
