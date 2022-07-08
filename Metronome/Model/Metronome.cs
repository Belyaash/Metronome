using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Metronome.Annotations;

namespace Metronome.Model;
public class Metronome : INotifyPropertyChanged
{
    public Metronome(int bpm, int currentMeasure)
    {
        this.Bpm = bpm;
        this.CurrentMeasure = currentMeasure;
        this.CurrentFaze = 0;
        this.IsWorking = false;
        timer.AutoReset = true;
        timer.Enabled = false;
        timer.Elapsed += MetronomeWork;
    }

    private Timer timer = new();

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
            timer.Interval = 60000 / _bpm;
            OnPropertyChanged("Bpm");
        }
    }

    private int _currentFaze;

    public int CurrentFaze
    {
        get => _currentFaze;
        internal set
        {
            _currentFaze = value > CurrentMeasure ? 1 : value;
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

    private bool _isWorking;

    public bool IsWorking
    {
        get => _isWorking;
        set
        {
            _isWorking = value;

            if (value)
                timer.Start();
            else 
                timer.Stop();
        }
    }

    public void MetronomeWork(object source, ElapsedEventArgs e)
    {
        CurrentFaze++;
        if (CurrentFaze == 1)
        {
            SystemSounds.Beep.Play();
        }
        else
        {
            SystemSounds.Hand.Play();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
