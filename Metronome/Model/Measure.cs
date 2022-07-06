using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metronome.Model;

public static class MeasureInfo
{
    public static string MeasureToString(Measure measure)
    {
        return measure switch
        {
            Measure.Two => "2",
            Measure.Three => "3",
            Measure.Four => "4",
            Measure.Five => "5",
            Measure.ThreePlusTwo => "3+2",
            Measure.TwoPlusThree => "2+3",
            Measure.Six => "6",
            Measure.Seven => "7",
            Measure.ThreePlusFour => "3+4",
            Measure.FourPlusThree => "4+3",
            Measure.Eight => "8",
            _ => throw new ArgumentOutOfRangeException(nameof(measure), measure, null)
        };
    }
}
public enum Measure
{
    Two,
    Three,
    Four,
    Five,
    ThreePlusTwo,
    TwoPlusThree,
    Six,
    Seven,
    ThreePlusFour,
    FourPlusThree,
    Eight
}

