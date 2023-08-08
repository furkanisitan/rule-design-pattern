﻿using System.Reflection;

namespace RuleDesignPattern.RuleExecutor;

public static class RuleExecutor
{
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        var response = new TResponse();

        var assemblyList = assemblies.ToList();
        assemblyList.Add(Assembly.GetCallingAssembly());

        foreach (var ruleInfo in GetRuleInfos(assemblyList, typeof(TRule)))
        {
            if (Activator.CreateInstance(ruleInfo.RuleType) is not IRule<TRequest, TResponse> rule) continue;
            if (!rule.CanApply(request)) continue;

            rule.Apply(request, response);
            if (ruleInfo.RuleOption is { RuleType: RuleType.StartBreak }) break;
        }

        return response;
    }

    public static IEnumerable<RuleInfo> GetRuleInfos(IEnumerable<Assembly> assemblies, Type ruleType)
    {
        return assemblies
            .Where(a => !a.IsDynamic)
            .Distinct()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(ruleType))
            .Select(t => new RuleInfo
            {
                RuleType = t,
                RuleOption = Attribute.GetCustomAttribute(t, typeof(RuleOptionAttribute)) as RuleOptionAttribute
            })
            .OrderByDescending(r => r.RuleOption?.RuleType ?? RuleType.None)
            .ToList();
    }
}