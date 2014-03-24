using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using Enterprises.Framework.Services;
using Microsoft.Pex.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprises.Framework.WindowsServer
{
    [TestClass, PexClass]
    public partial class SysDataToDataBase : ICommand
    {
        public void Execute()
        {
            Write2DB();
        }

        private void Write2DB()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory + @"\Log";
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }

            try
            {
                var strW = new StreamWriter(appPath + @"\db.txt", true);
                strW.WriteLine("{0} \r\n", DateTime.Now);
                strW.Flush();
                strW.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}