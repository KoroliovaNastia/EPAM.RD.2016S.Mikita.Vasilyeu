using System;
using System.Reflection;
using MyInterfaces;
using System.Linq;

namespace DoSomethingClient
{
    public class DomainAssemblyLoader : MarshalByRefObject
    {
        // Before making this call make sure that MyInterface assembly is signed with mykey.snk file. See Signing tab in MyInterface project properties editor.
        // Usage:
        // result = loader.Load("MyLibrary, Version=1.2.3.4, Culture=neutral, PublicKeyToken=f46a87b3d9a80705", input)
        public Result Load(string assemblyString, Input data)
        {
            // LoadFile() doesn't bind through Fusion at all - the loader just goes ahead and loads exactly what the caller requested.
            // It doesn't use either the Load or the LoadFrom context.
            // LoadFile() has a catch. Since it doesn't use a binding context, its dependencies aren't automatically found in its directory. 

            var assembly = Assembly.Load(assemblyString);
            var types = assembly.GetTypes();

            // TODO: Find first type that has DoSomething attribute and implements IDoSomething.
            // TODO: Create an instance of this type.

            Type foundType = null;
            foreach (var type in types)
            {
                if (type.GetInterfaces().Contains(typeof(IDoSomething))
                    && (DoSomethingAttribute)type.GetCustomAttribute(typeof(DoSomethingAttribute), false) != null)
                {
                    foundType = type;
                    break;
                }
            }
            var instance = Activator.CreateInstance(foundType);
            IDoSomething doSomethingService = (IDoSomething)instance;

            //IDoSomething doSomethingService = null; // TODO Save instance to variable.

            return doSomethingService.DoSomething(data);
        }

        // Usage:
        // var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"MyDomain\MyLibrary.dll");
        // result = loader.Load(path, input);
        public Result LoadFile(string path, Input data)
        {
            // LoadFrom() goes through Fusion and can be redirected to another assembly at a different path
            // but with that same identity if one is already loaded in the LoadFrom context.

            var assembly = Assembly.LoadFile(path);
            var types = assembly.GetTypes();

            Type foundType = null; // TODO: Find first type that has DoSomething attribute and don't implement IDoSomething.

            foreach (var type in types)
            {
                if (!type.GetInterfaces().Contains(typeof(IDoSomething))
                    && (DoSomethingAttribute)type.GetCustomAttribute(typeof(DoSomethingAttribute), false) != null)
                {
                    foundType = type;
                    break;
                }
            }
            // TODO: MethodInfo mi = type.GetMethod("DoSomething");

            MethodInfo mi = foundType.GetMethod("DoSomething");

            Result result = null;
            // TODO: result = mi.Invoke();

            result = (Result)mi.Invoke(Activator.CreateInstance(foundType), new object[] { data });

            return result;
        }

        // More details: http://stackoverflow.com/questions/1477843/difference-between-loadfile-and-loadfrom-with-net-assemblies
        public Result LoadFrom(string fileName, Input data)
        {
            var assembly = Assembly.LoadFrom(fileName);
            var types = assembly.GetTypes();

            // TODO: Find first type that has DoSomething attribute and implements IDoSomething.
            // TODO: Create an instance of this type.

            Type foundType = null;
            foreach (var type in types)
            {
                if (type.GetInterfaces().Contains(typeof(IDoSomething))
                    && (DoSomethingAttribute)type.GetCustomAttribute(typeof(DoSomethingAttribute), false) != null)
                {
                    foundType = type;
                    break;
                }
            }

            var instance = Activator.CreateInstance(foundType);

            //IDoSomething doSomethingService = null; // TODO Save instance to variable.
            IDoSomething doSomethingService = (IDoSomething)instance;

            return doSomethingService.DoSomething(data);
        }
    }
}
