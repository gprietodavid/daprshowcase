﻿namespace DaprShowcase.Services.Orchestrator.Application.Messages
{
    public class WorkerProcessResult
    {
        public bool HasErrors { get; set; }
        public string[] Messages { get; set; }
        public string JsonResult { get; set; }
    }
}