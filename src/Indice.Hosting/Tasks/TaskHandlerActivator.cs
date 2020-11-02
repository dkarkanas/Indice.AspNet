﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Indice.Hosting
{
    internal class TaskHandlerActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public TaskHandlerActivator(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task Invoke(Type jobHandlerType, object workItem = null) {
            var methods = jobHandlerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var handler = _serviceProvider.GetService(jobHandlerType);
            var processMethod = methods.Where(x => "Process".Equals(x.Name, StringComparison.OrdinalIgnoreCase)).First();
            object[] arguments;
            if (workItem != null) {
                var workItemType = workItem.GetType();
                arguments = processMethod
                    .GetParameters()
                    .Select(x => workItemType.IsAssignableFrom(x.ParameterType) ? workItem : _serviceProvider.GetService(x.ParameterType))
                    .ToArray();
            } else {
                arguments = processMethod
                        .GetParameters()
                        .Select(x => _serviceProvider.GetService(x.ParameterType))
                        .ToArray();
            }
            var isAwaitable = typeof(Task).IsAssignableFrom(processMethod.ReturnType);
            if (isAwaitable) {
                await (Task)processMethod.Invoke(handler, arguments);
            } else {
                processMethod.Invoke(handler, arguments);
            }
        }
    }
}
