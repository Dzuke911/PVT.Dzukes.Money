using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PVT.Money.Shell.Web.Container
{
    ///////////////////////////////////////
    /// ANTIPATTERN: HARDCODE
    ///////////////////////////////////////
    public class MoneyContainer : IMoneyContainer
    {
        private List<Type> list = new List<Type>(); 

        public void Add<T>()
        {
            if(!list.Any(t => t == typeof(T)))
                list.Add(typeof(T));
        }

        public T Create<T>()
        {
            if(!list.Any(t => t == typeof(T)))
                throw new InvalidOperationException($"{typeof(T).ToString()} wasn`t added to IMoneyContainer");

            ICollection<ConstructorInfo> constructors = typeof(T).GetConstructors();

            if (constructors.Count == 0)
                throw new InvalidOperationException($"There is no proper constructors in {typeof(T).ToString()}");

            List<ConstructorInfo> constructorsList = constructors.ToList();

            ConstructorInfo result = constructorsList[0];
            foreach (ConstructorInfo cInfo in constructors)
            {
                if (cInfo.GetParameters().Length <= result.GetParameters().Length)
                    result = cInfo;
            }

            if (result.GetParameters().Length == 0)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                MethodInfo method = typeof(MoneyContainer).GetMethod("Create");
                List<object> parameters = new List<object>();
                ParameterInfo[] parameterInfos = result.GetParameters();
                foreach(ParameterInfo pInfo in parameterInfos)
                {
                    Type type = pInfo.ParameterType;                    

                    MethodInfo genericMethod = method.MakeGenericMethod(type);
                    parameters.Add( genericMethod.Invoke(this,null));
                }
                object[] args = parameters.ToArray();
                return (T)Activator.CreateInstance(typeof(T), args);
            }
        }
    }
}
