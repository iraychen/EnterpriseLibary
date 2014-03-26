namespace Enterprises.Framework.Plugin.Logging
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface ILoggerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ILogger CreateLogger(Type type);
    }
}

