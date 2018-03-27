using System;
using System.Collections.Generic;
using System.Text;
using Polly;

namespace Acme.RemoteFlights.Domain
{
    public static class SqlRetryPolicy
    {

        public const int DefaultRetryCount = 3;
        public static readonly TimeSpan DefaultRetryTimeout = TimeSpan.FromSeconds(5);

        public static Policy BasicPolicy
        {
            get
            {
                return Policy
                     .Handle<Exception>()
                     .WaitAndRetryAsync(DefaultRetryCount, retryAttempt => DefaultRetryTimeout);
            }
        }
    }
}
