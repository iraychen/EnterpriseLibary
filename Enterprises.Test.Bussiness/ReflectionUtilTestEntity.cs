using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Enterprises.Test.Bussiness
{
    public class ReflectionUtilTestEntity : IReflectionUtilTest
    {
        [YaolifengTest("姚立峰")]
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string GetName()
        {
            return "Hello,姚立峰";
        }
    }

    public interface IReflectionUtilTest
    {
        string GetName();
    }


    public class YaolifengTestAttribute : Attribute
    {
        public string Name { get; set; }

        public YaolifengTestAttribute(string name)
        {
            Name = name;
        }
    }

    public enum LeaveType
    {
        [Description("进行中")]
        Execute = 1,

        [Description("到期结束")]
        Finish = 2,

        [Description("活动取消")]
        Cancel = 3,

        [Description("审核中")]
        Applove = 4,

        [Description("审核通过")]
        ApplovePass = 5,

        [Description("审核失败")]
        ApploveCancal = 6
    }
}
