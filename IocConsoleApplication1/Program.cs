using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocConsoleApplication1
{
    
    class Program
    {
        private static IKernel kernel = new StandardKernel(new MyModule());//注入工具
        static void Main(string[] args)
        {
            //获取接口对应的对象
            ITester tester = kernel.Get<ITester>();
            tester.Test();
            Console.WriteLine("continues..");
            Console.Read();
        }
    }

    //绑定接口和实现的对应关系
    internal class MyModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<FlatFileLogger>();
            Bind<ITester>().To<IocTester>();
        }
    }

    #region 接口

    public interface ILogger
    {
        void Write(string message);
    }
    public class FlatFileLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine(String.Format("Message:{0}", message));
            Console.WriteLine("Target:FlatFile");
        }
    }

    interface ITester
    {
        void Test();
    }


    class IocTester : ITester
    {
        private ILogger _logger;
        public IocTester(ILogger logger)
        {
            _logger = logger;
        }

        public void Test()
        {
            _logger.Write("Bruce Say: Hello Ninject!");
        }
    } 
    #endregion
}
