using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BepInEx.Logging;

namespace Tierless;

public static class Utils
{
    public static string FullID(string guid, string suffix)
    {
        return $"{guid}:{suffix}";
    }
    
    public static void AppendExpression(this StringBuilder stringBuilder, Expression<Func<object>> expression)
    {
        var body = expression.Body;
        var expressionName = body switch
        {
            MemberExpression me => me.Member.Name,
            UnaryExpression { Operand: MemberExpression me2 } =>
                me2.Member.Name,
            _ => body.ToString()
        };
        var result = expression.Compile().Invoke();
        stringBuilder.Append(expressionName);
        stringBuilder.Append(" = ");
        stringBuilder.Append(result);
    }

    public static void LogExpressions(this ManualLogSource logger, string logName, params Expression<Func<object>>[] args)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(logName);
        stringBuilder.Append(" <");
        if (args.Length > 0)
        {
            stringBuilder.AppendExpression(args[0]);
            for (int i = 1; i < args.Length; i++)
            {
                stringBuilder.Append(", ");
                stringBuilder.AppendExpression(args[i]);
            }

            stringBuilder.Append(">");
        }

        logger.LogWarning(stringBuilder.ToString());
    }
    
    public static string ToStringInDetails(this object arg, int maxDepth = 3, string depthIndent = "    ", string lineSeparator = "\n")
    {
        var visited = new HashSet<object>();
        var stringBuilder = new StringBuilder();
        ToStringInDetailsImplementation(arg, stringBuilder, 0, maxDepth, visited, depthIndent, lineSeparator, true);
        return stringBuilder.ToString();
    }

    private static void ToStringInDetailsImplementation(object? arg, StringBuilder stringBuilder, int depth, int maxDepth, HashSet<object> visited, string depthIndent, string lineSeparator, bool toIndent)
    {
        var indent = new StringBuilder();
        for (int i = 0; i < depth; i++)
        {
            indent.Append(depthIndent);
        }

        if (toIndent)
            stringBuilder.Append(indent);
        
        if (arg == null)
        {
            stringBuilder.Append("null");
            stringBuilder.Append(lineSeparator);
            return;
        }

        var type = arg.GetType();
        if (visited.Contains(arg))
        {
            stringBuilder.Append("<Circular Reference to ");
            stringBuilder.Append(type.Name);
            stringBuilder.Append(">");
            stringBuilder.Append(lineSeparator);
            return;
        }

        if (type.IsPrimitive || arg is string || arg is DateTime || arg is decimal)
        {
            stringBuilder.Append(arg);
            stringBuilder.Append(lineSeparator);
            return;
        }

        if (depth >= maxDepth)
        {
            stringBuilder.Append("<");
            stringBuilder.Append(type.Name);
            stringBuilder.Append("...>");
            stringBuilder.Append(lineSeparator);
            return;
        }

        visited.Add(arg);

        stringBuilder.Append(type.Name);
        stringBuilder.Append(" {");
        stringBuilder.Append(lineSeparator);
        if (arg is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                ToStringInDetailsImplementation(item, stringBuilder, depth + 1, maxDepth, visited, depthIndent, lineSeparator, true);
            }
        }
        else
        {
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object value;
                try
                {
                    value = property.GetValue(arg);
                }
                catch
                {
                    value = "<error>";
                }

                stringBuilder.Append(indent);
                stringBuilder.Append(property.Name);
                stringBuilder.Append(" = ");
                ToStringInDetailsImplementation(value, stringBuilder, depth + 1, maxDepth, visited, depthIndent, lineSeparator, false);
            }
        }
        stringBuilder.Append(indent);
        stringBuilder.Append("}");

        visited.Remove(arg);
        stringBuilder.Append(lineSeparator);
    }
}
