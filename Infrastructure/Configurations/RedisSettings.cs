namespace Infrastructure.Configurations
{
    /// <summary>
    /// Configuration settings for connecting to and interacting with a Redis server.
    /// Bound from configuration (e.g., appsettings.json).
    /// </summary>
    public sealed class RedisSettings
    {
        /// <summary>
        /// The Redis connection string.
        /// Example: "localhost:6379,password=yourpassword"
        /// </summary>
        public string Connection { get; set; } = default!;

        /// <summary>
        /// Logical prefix used for all cache keys.
        /// Helps isolate keys between multiple applications sharing the same Redis server.
        /// </summary>
        public string InstanceName { get; set; } = default!;

        /// <summary>
        /// Indicates whether the client should throw an exception
        /// if the initial connection to Redis fails.
        /// </summary>
        public bool AbortOnConnectFail { get; set; }

        /// <summary>
        /// Time (in milliseconds) the client waits while establishing
        /// a TCP connection to Redis.
        /// Defines how long the client waits for the server to respond
        /// during initial connection.
        /// </summary>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        /// Maximum time (in milliseconds) a synchronous Redis operation
        /// is allowed to execute before timing out.
        /// </summary>
        public int SyncTimeout { get; set; }

        /// <summary>
        /// Maximum time (in milliseconds) an asynchronous Redis operation
        /// is allowed to execute before timing out.
        /// </summary>
        public int AsyncTimeout { get; set; }

        /// <summary>
        /// Number of times the client retries connecting
        /// before giving up.
        /// </summary>
        public int ConnectRetry { get; set; }
    }
}
