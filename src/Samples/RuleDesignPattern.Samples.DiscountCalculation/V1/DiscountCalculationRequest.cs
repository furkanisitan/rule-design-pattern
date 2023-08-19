﻿namespace RuleDesignPattern.Samples.DiscountCalculation.V1;

public class DiscountCalculationRequest
{
    public decimal Amount { get; set; }
    public bool IsCitizen { get; set; }
    public bool IsStudent { get; set; }
    public bool IsMarried { get; set; }
    public bool IsVictim { get; set; }
}