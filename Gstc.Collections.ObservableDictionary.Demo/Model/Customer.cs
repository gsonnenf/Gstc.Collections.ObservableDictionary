using System;
using System.Collections.Generic;

namespace Gstc.Collections.ObservableDictionary.Demo.Model;
public class Customer {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public double PurchaseAmount { get; set; }
    public string TransactionId { get; set; } = "Invalid_Id";

    public int Version { get; set; }

    private static Random RandomGenerator { get; } = new Random();

    private static string GetRandomFirstName() => FirstNameList[RandomGenerator.Next(FirstNameList.Count)];
    private static string GetRandomLastName() => LastNameList[RandomGenerator.Next(LastNameList.Count)];

    private static int _idCounter = 0;

    private static string GenerateId(string lastName) => lastName.Substring(0, (lastName.Length < 6) ? lastName.Length : 6) + "_" + Guid.NewGuid().ToString().Substring(0, 6);

    public static Customer GenerateCustomer() {
        var firstName = GetRandomFirstName();
        var lastName = GetRandomLastName();
        return new Customer() {
            FirstName = firstName,
            LastName = lastName,
            BirthDate = DateTime.Now.AddDays(-1 * RandomGenerator.Next(30000) - 3000),
            PurchaseAmount = 1 + RandomGenerator.Next(10000) * 0.01,
            TransactionId = GenerateId(lastName),
            Version = 1
        };

    }

    public static List<Customer> GenerateCustomerList(int count) {
        var list = new List<Customer>();
        for (int i = 0; i < count; i++) list.Add(GenerateCustomer());
        return list;
    }



    private readonly static List<string> FirstNameList = new List<string>() {
        "Emma",
        "Sophia",
        "Olivia",
        "Isabella",
        "Ava",
        "Mason",
        "Noah",
        "Lucas",
        "Jacob",
        "Jack"
    };

    private readonly static List<string> LastNameList = new List<string>() {
        "Smith",
        "Trujillo",
        "Jackson",
        "Lee",
        "Mercado",
        "Sonnenfeld",
        "Valdez",
        "Johnson",
        "Parker",
        "Warren"
    };
}
