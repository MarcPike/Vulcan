using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Currency;

public class CoidCountry
{
    public string Coid { get; set; }
    public List<string> Countries { get; set; } = new List<string>();
    public string Region { get; set; }
    public CurrencyType DefaultCurrency { get; set; }
    public string SuperRegion { get; set; }

    public static List<CoidCountry> GetDefaults()
    {


        var result = new List<CoidCountry>();
        result.Add(new CoidCountry()
        {
            Coid = "INC",
            Countries = new List<string>(){"United States"},
            Region = "Western Hemisphere",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("USD"),
            SuperRegion = "WH Super Region"});
        result.Add(new CoidCountry()
        {
            Coid = "CAN",
            Countries = new List<string>(){"Canada"},
            Region = "Western Hemisphere",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("CAD"),
            SuperRegion = "WH Super Region"
        });
        result.Add(new CoidCountry()
        {
            Coid = "EUR",
            Countries = new List<string>() {"England","Scotland","Irvine","Norway", "Bredbury" },
            Region = "Europe",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("GBP"),
            SuperRegion = "Europe Super Region"
        });
        result.Add(new CoidCountry()
        {
            Coid = "MSA",
            Countries = new List<string>() {"Malaysia"},
            Region = "South East Asia",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("USD"),
            SuperRegion = "MEAP Super Region"
        });
        result.Add(new CoidCountry()
        {
            Coid = "SIN",
            Countries = new List<string>(){"Singapore"},
            Region = "South East Asia",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("USD"),
            SuperRegion = "MEAP Super Region"
        });
        result.Add(new CoidCountry()
        {
            Coid = "CHN",
            Countries = new List<string>() {"China"},
            Region = "South East Asia",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("CNY"),
            SuperRegion = "MEAP Super Region"
        });
        result.Add(new CoidCountry()
        {
            Coid = "DUB",
            Countries = new List<string>() { "Dubai" },
            Region = "Dubai",
            DefaultCurrency = CurrencyType.GetCurrencyTypeFor("USD"),
            SuperRegion = "MEAP Super Region"
        });

        return result;
    }
}