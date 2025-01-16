﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsAtBitmarck.MvvmFramework.EventAggregators
{
    /// <summary>
    /// Extensions for <see cref="IEventAggregator"/>.
    /// </summary>
    public static class EventAggregatorExtensions
    {
        /// <summary>
        /// Publishes a message on the current thread (synchrone).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task Publish(this IEventAggregator eventAggregator, object message)
        {
            return eventAggregator.PublishAsync(message, CancellationToken.None);
        }
    }
}