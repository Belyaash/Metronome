using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Metronome.Model;

namespace Metronome.ViewModel;
public class MainWindowViewModel : INotifyPropertyChanged
{
    public MainWindowViewModel()
    {
        Bpm = 90;
        CurrentMeasure = 2;
        MeasuresList = new List<int>();
        for (int i = 2; i <= 8; i++)
        {
            MeasuresList.Add(i);
        }

        CurrentFaze = 0;

    }
    private int _bpm;

    public int Bpm
    {
        get => _bpm;
        set
        {
            if (value < 1)
                _bpm = 1;
            else if (value > 300)
                _bpm = 300;
            else
                _bpm = value;
            OnPropertyChanged("Bpm");
        }
    }

    private int _currentFaze;

    public int CurrentFaze
    {
        get => _currentFaze;
        internal set
        {
            _currentFaze = value;
            OnPropertyChanged("CurrentFaze");
        }
    }

    private int _currentMeasure;

    public int CurrentMeasure
    {
        get => _currentMeasure;
        set
        {
            _currentMeasure = value;
            OnPropertyChanged("CurrentMeasure");
        }
    }

    public List<int> MeasuresList { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
