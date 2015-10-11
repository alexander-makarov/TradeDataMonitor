using System;
using System.Reflection;

namespace TradeDataMonitorApp
{
    public static class AssemblyHelpers
    {
        /// <summary>
        /// Loading type from assembly
        /// </summary>
        /// <param name="assemblyFilePath">assembly file name or path to it</param>
        /// <param name="typeName">type name</param>
        /// <returns>loaded type object</returns>
        public static Type LoadTypeFromAssembly(string assemblyFilePath, string typeName)
        {
            Assembly a = Assembly.LoadFrom(assemblyFilePath);
            var type = a.GetType(typeName);
            if (type == null)
            {
                throw new Exception(String.Format("Can't find a type '{0}' in '{1}'", typeName, assemblyFilePath));
            }
            return type;
        }

        /// <summary>
        /// Loading type from assembly and instantiate it
        /// </summary>
        /// <typeparam name="T">type to which instance will be casted (likely to be an interface)</typeparam>
        /// <param name="assemblyFilePath">assembly file name or path to it</param>
        /// <param name="typeName">type name</param>
        /// <returns>Instance of loaded type, casted to T</returns>
        public static T LoadClassInstanceFromAssembly<T>(string assemblyFilePath, string typeName)
        {
            try
            {
                Type t = LoadTypeFromAssembly(assemblyFilePath, typeName);
                var obj = (T)Activator.CreateInstance(t);
                return obj;
            }
            catch (Exception exc)
            {
                throw new Exception(
                    String.Format("Error on creating instance of class '{0}' from '{1}' : {2}", typeName, assemblyFilePath, exc.Message),
                    exc);
            }
        }
    }
}
