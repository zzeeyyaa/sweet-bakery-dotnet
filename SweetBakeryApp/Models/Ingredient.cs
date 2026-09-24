namespace SweetBakeryApp.Models;

public readonly struct Ingredient(string name, int amountInGrams)
{
    public string Name { get; } = name;
    public int AmountInGrams { get; } = amountInGrams;
}