﻿namespace RuleDesignPattern.RuleExecutor.Rules;

public interface IUnlinkedRule<in TRequest, in TResponse> : IRule<TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse, new()
{
}