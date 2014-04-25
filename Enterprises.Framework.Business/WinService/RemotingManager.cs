using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprises.Framework.Winservice
{
    /// <summary>
    /// 
    /// </summary>
    public class RemotingManager
    {
        private static readonly object FInitServerRemotingObjectHasExecutedLockObject = new object();
        private static bool _fInitServerRemotingObjectHasExecuted = false;

        private static readonly object FDirectRemotingManagerLockObject = new object();
        /// <summary>
        /// 在服务端注册RemotingService服务，注意：这个方法要在Global中调用以初始化服务。
        /// </summary>
        public static void InitServerRemotingObject()
        {
            if (_fInitServerRemotingObjectHasExecuted)
            {
                return;
            }
            lock (FInitServerRemotingObjectHasExecutedLockObject)
            {
                if (!_fInitServerRemotingObjectHasExecuted)
                {
                    _fInitServerRemotingObjectHasExecuted = true;
                }
            }
        }
    }
}
