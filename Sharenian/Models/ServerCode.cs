using System.ComponentModel;

namespace Sharenian.Models;

public enum ServerCode
{
    [Description("리부트 2")]
    Reboot2 = 1,

    [Description("리부트")]
    Reboot = 2,

    [Description("오로라")]
    Aurora = 3,

    [Description("레드")]
    Red = 4,

    [Description("이노시스")]
    Enosis = 5,

    [Description("유니온")]
    Union = 6,

    [Description("스카니아")]
    Scania = 7,

    [Description("루나")]
    Luna = 8,

    [Description("제니스")]
    Zenith = 9,

    [Description("크로아")]
    Croa = 10,

    [Description("베라")]
    Bera = 11,

    [Description("엘리시움")]
    Elysium = 12,

    [Description("아케인")]
    Arcane = 13,

    [Description("노바")]
    Nova = 14
}