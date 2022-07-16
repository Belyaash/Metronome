namespace Metronome.Model.Interfaces;

public interface IMetronome
{
    public int Bpm { get; set; }
    public int CurrentMeasure { get; set; }
    public int CurrentFaze { get; }

    public void StartOrStopMetronome();

}