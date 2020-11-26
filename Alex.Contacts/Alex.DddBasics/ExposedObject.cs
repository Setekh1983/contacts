using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Alex.DddBasics
{
  internal class ExposedObject : DynamicObject
  {
    const BindingFlags PrivateInstanceFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    readonly object _UnderlyingObject;

    readonly Dictionary<Type, MethodInfo> _EventTypeMapping;

    public ExposedObject(object underlyingObject)
    {
      _ = underlyingObject ?? throw new ArgumentNullException(nameof(underlyingObject));

      this._EventTypeMapping = new Dictionary<Type, MethodInfo>();
      this._UnderlyingObject = underlyingObject;

      this.Initialize(underlyingObject.GetType());
    }

    private void Initialize(Type type)
    {
      MethodInfo[] methodInfos = type.GetMethods(PrivateInstanceFlags);

      foreach (MethodInfo method in methodInfos)
      {
        if (method.Name == "Apply")
        {
          ParameterInfo[] parameters = method.GetParameters();

          if (parameters.Length == 1)
          {
            ParameterInfo parameter = parameters[0];
            this._EventTypeMapping.Add(parameter.ParameterType, method);
          }
        }
      }
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
    {
      MethodInfo methodInfo = this._EventTypeMapping[args[0].GetType()];
      result = methodInfo.Invoke(this._UnderlyingObject, args);

      return true;
    }
  }
}
