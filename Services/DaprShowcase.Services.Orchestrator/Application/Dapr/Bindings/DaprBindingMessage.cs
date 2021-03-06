// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Bindings
{
    /// <summary>
    /// Parameters for invoking a Dapr binding.
    /// </summary>
    public class DaprBindingMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DaprBindingMessage"/> class.
        /// </summary>
        /// <param name="data">The data of the message sent to the Dapr binding.</param>
        /// <param name="metadata">The bag of key value pairs for binding-specific metadata.</param>
        /// <param name="binding">The name of binding.</param>
        /// <param name="operation">The operation to do with the Dapr binding.</param>
        public DaprBindingMessage(object data, Dictionary<string, object>? metadata = null, string? binding = null, string? operation = null)
        {
            this.Data = JToken.FromObject(data ?? throw new ArgumentNullException(nameof(data)));
            this.Metadata = metadata;
            this.BindingName = binding;
            this.Operation = operation;
        }

        /// <summary>
        /// Gets or sets the data .
        /// </summary>
        [JsonProperty("data")]
        public JToken Data { get; set; }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        [JsonProperty("operation", NullValueHandling = NullValueHandling.Ignore)]
        public string? Operation { get; set; }

        /// <summary>
        /// Gets or sets the metadata required for this operation.
        /// </summary>
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the name of the binding.
        /// </summary>
        [JsonIgnore]
        public string? BindingName { get; set; }
    }
}