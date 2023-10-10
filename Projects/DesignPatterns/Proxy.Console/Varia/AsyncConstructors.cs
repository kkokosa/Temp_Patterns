using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Proxy.Console.Varia
{
    class AsyncConstructors
    {
        public static async Task Main(string[] args)
        {
            var asyncDependency = new AsyncDependency();
            var service1 = new Service(asyncDependency); // Dangerous!
            var service2 = await Service2.CreateAsync(asyncDependency); // OK!

            await TestContainer<Service>();
            //await TestContainer<Service2>(); // would throw exception as constructor is private
            //await TestContainer<Service3>(); // would throw exception as constructor is private
            await TestContainer<Service4>();
        }

        private static async Task TestContainer<T>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<T>().As<IService>();
            builder.RegisterType<AsyncDependency>().As<IAsyncDependency>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IService>();
                if (service is IAsyncInitialization a)
                {
                    await a.Initialization;
                }
                //await ((service as IAsyncInitialization)?.Initialization ?? Task.CompletedTask);
                System.Console.WriteLine(service.Data?.X);
            }
        }

        class Service : IService
        {
            private readonly SomeData _data;
            public SomeData Data => _data;
            public Service(IAsyncDependency asyncDependency)
            {
                // DON'T as it is a classic sync over async:
                _data = asyncDependency.DoAsync().Result;

                // Also DON'T do:
                // _data = asyncDependency.DoAsync();
                // as it just kicks-off asynchronous initialization but returns an object in uninitialized state
                // AND there is no error handling

                // Also DON'T do:
                // _data = Task.Run(async () => await asyncDependency.DoAsync()).Result;
                // Because of inefficiency. Like in any other sync-over-async case
            }
        }

        class Service2 : IService
        {
            private readonly SomeData _data;
            public SomeData Data => _data;
            private Service2(SomeData data)
            {
                _data = data;
            }

            public static async Task<Service2> CreateAsync(IAsyncDependency asyncDependency)
            {
                var data = await asyncDependency.DoAsync();
                return new Service2(data);
            }
        }

        // Small variation of Service2 if you want to split initialization into asynchronous Initialize
        // and async-elided Create. And you are ok with making fields not-readonly.
        class Service3 : IService
        {
            private SomeData _data;
            public SomeData Data => _data;
            private Service3()
            {
            }

            private async Task<Service3> InitializeAsync(IAsyncDependency asyncDependency)
            {
                _data = await asyncDependency.DoAsync();
                return this;
            }

            public static Task<Service3> CreateAsync(IAsyncDependency asyncDependency)
            {
                var result = new Service3();
                return result.InitializeAsync(asyncDependency);
            }
        }

        public interface IAsyncInitialization
        {
            Task Initialization { get; }
        }

        class Service4 : IService, IAsyncInitialization
        {
            public SomeData Data { get; private set; }

            public Task Initialization { get; }

            private async Task InitializeAsync(IAsyncDependency asyncDependency)
            {
                Data = await asyncDependency.DoAsync();
            }

            public Service4(IAsyncDependency asyncDependency)
            {
                Initialization = InitializeAsync(asyncDependency);
            }
        }

        class Service5
        {
            private IAsyncDependency _asyncDependency;

            private AsyncLazy<SomeData> _data => new AsyncLazy<SomeData>(
                async () => await _asyncDependency.DoAsync());

            public Service5(IAsyncDependency asyncDependency)
            {
                _asyncDependency = asyncDependency;
            }

            public async Task DoAsync()
            {
                var data = await _data;
                // Use data
            }
        }

        interface IAsyncDependency
        {
            Task<SomeData> DoAsync();
        }

        class SomeData
        {
            public readonly string X = "Hello world!";
        }

        class AsyncDependency : IAsyncDependency
        {
            public Task<SomeData> DoAsync()
            {
                return Task.FromResult(new SomeData());
            }
        }

        interface IService
        {
            SomeData Data { get; }
        }

        public sealed class AsyncLazy<T>
        {
            private readonly Lazy<Task<T>> instance;

            public AsyncLazy(Func<Task<T>> factory)
            {
                instance = new Lazy<Task<T>>(() => Task.Run(factory));
            }

            public TaskAwaiter<T> GetAwaiter()
            {
                return instance.Value.GetAwaiter();
            }
        }
    }
}
