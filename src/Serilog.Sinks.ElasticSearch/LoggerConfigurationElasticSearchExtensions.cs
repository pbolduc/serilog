﻿using System;
using Serilog.Configuration;
using Serilog.Events;
// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Serilog.Sinks.ElasticSearch;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.RavenDB() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationElasticSearchExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events as documents to an ElasticSearch index.
        /// This works great with the Kibana web interface when using the default settings.
        /// Make sure to add a template to ElasticSearch like the one found here:
        /// https://gist.github.com/mivano/9688328
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="indexFormat">The index format where the events are send to. It defaults to the logstash index per day format. It uses a String.Format using the DateTime.UtcNow parameter.</param>
        /// <param name="server">The server where ElasticSearch is running. When null it falls back to http://localhost:9200</param>
        /// <param name="connectionTimeOutInMilliseconds">The connection time out in milliseconds. Default value is 5000.</param>
        /// <param name="restrictedToMinimumLevel">The minimum log event level required in order to write an event to the sink.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <returns>
        /// Logger configuration, allowing configuration to continue.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">loggerConfiguration</exception>
        /// <exception cref="ArgumentNullException">A required parameter is null.</exception>
        public static LoggerConfiguration ElasticSearch(
            this LoggerSinkConfiguration loggerConfiguration,
            string indexFormat = "logstash-{0:yyyy.MM.dd}",
            Uri server = null,
            int connectionTimeOutInMilliseconds = 5000,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = ElasticSearchSink.DefaultBatchPostingLimit,
            TimeSpan? period = null,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");

            var defaultedPeriod = period ?? ElasticSearchSink.DefaultPeriod;
            if (server == null)
                server = new Uri("http://localhost:9200");

            return loggerConfiguration.Sink(
                new ElasticSearchSink(server, indexFormat, batchPostingLimit, connectionTimeOutInMilliseconds, defaultedPeriod, formatProvider),
                restrictedToMinimumLevel);
        }
    }
}
