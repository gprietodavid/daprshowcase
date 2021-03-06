// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// ------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DaprShowcase.Services.Orchestrator.Application.Dapr.Bindings
{
    /// <summary>
    /// Payload for outbound Dapr pub/sub events.
    /// </summary>
    public class DaprPubSubEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DaprPubSubEvent"/> class.
        /// </summary>
        /// <param name="payload">The payload of the outbound pub/sub event.</param>
        /// <param name="pubSubName">The pub/sub name of the outbound pub/sub event.</param>
        /// <param name="topic">The topic of the outbound pub/sub event.</param>
        public DaprPubSubEvent(JToken payload, string? pubSubName = null, string? topic = null)
        {
            this.Payload = payload;
            this.PubSubName = pubSubName;
            this.Topic = topic;
        }

        /// <summary>
        /// Gets the name of the pub/sub.
        /// </summary>
        /// <remarks>
        /// If the pub/sub name is not specified, it is inferred from the
        /// <see cref="DaprPublishAttribute"/> binding attribute.
        /// </remarks>
        [JsonProperty("pubsubname")]
        public string? PubSubName { get; internal set; }

        /// <summary>
        /// Gets the name of the topic.
        /// </summary>
        /// <remarks>
        /// If the topic name is not specified, it is inferred from the
        /// <see cref="DaprPublishAttribute"/> binding attribute.
        /// </remarks>
        [JsonProperty("topic")]
        public string? Topic { get; internal set; }

        /// <summary>
        /// Gets the payload of the pub/sub event.
        /// </summary>
        /// <remarks>
        /// The subscribers will receive this payload as the body of a Cloud Event envelope.
        /// </remarks>
        [JsonProperty("payload")]
        public JToken Payload { get; }
    }
}
