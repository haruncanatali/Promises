using System.ComponentModel;

namespace Promises.Domain.Enums;

public enum ZodiacSign
{
    [Description("Koç")]
    Aries = 1,
    [Description("Boğa")]
    Taurus,
    [Description("İkizler")]
    Gemini,
    [Description("Yengeç")]
    Cancer,
    [Description("Aslan")]
    Leo,
    [Description("Başak")]
    Virgo,
    [Description("Terazi")]
    Libra,
    [Description("Akrep")]
    Scorpio,
    [Description("Yay")]
    Sagittarius,
    [Description("Oğlak")]
    Capricorn,
    [Description("Kova")]
    Aquarius,
    [Description("Balık")]
    Pisces
}