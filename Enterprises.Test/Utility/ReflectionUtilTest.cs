using System;
using System.Collections.Generic;
using System.Diagnostics;
using Enterprises.Framework.Utility;
using Enterprises.Test.Bussiness;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprises.Test.Utility
{
    [TestClass]
    public class ReflectionUtilTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GetDescriptionTest()
        {
            var desc=ReflectionUtil.GetDescription(typeof (LeaveType), 3);
            TestContext.WriteLine(desc);
        }

        [TestMethod]
        public void GetEnumeratorsTest()
        {
            var desc = ReflectionUtil.GetEnumerators(typeof(LeaveType));
            foreach (var item in desc)
            {
                TestContext.WriteLine(string.Format("{0},{1}",item.Key,item.Value));
            }
        }


        [TestMethod]
        public void HasAttributeTest()
        {
            var result = ReflectionUtil.HasAttribute<YaolifengTestAttribute>(typeof(ReflectionUtilTestEntity), "Name");
            TestContext.WriteLine(result.ToString());
        }

        [TestMethod]
        public void GetGetAttributeTest()
        {
            var desc = ReflectionUtil.GetAttribute<YaolifengTestAttribute>(typeof(ReflectionUtilTestEntity), "Name");
            TestContext.WriteLine(string.Format("{0}", desc.Name));
        }

        [TestMethod]
        public void ExtractGenericInterface()
        {
            Type type = ReflectionUtil.ExtractGenericInterface(typeof(ReflectionUtilTestEntity), typeof(IEnumerable<>));
            if (type != null)
            {
                TestContext.WriteLine(string.Format("Name:{0}", type.Name));
            }

            Type type2 = ReflectionUtil.ExtractGenericInterface(typeof(ReflectionUtilTestEntity), typeof(IReflectionUtilTest));
            if (type2 != null)
            {
                TestContext.WriteLine(string.Format("Name:{0}", type2.Name));
            }

        }
    }


    
}
