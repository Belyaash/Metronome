using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;
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
        _timer.AutoReset = true;
        _timer.Enabled = false;
        _timer.Elapsed += MetronomeWork;
    }

    private Timer _timer = new();

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
            _timer.Interval = 60000 / _bpm;
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
                _timer.Start();
            else 
                _timer.Stop();
        }
    }

    public void MetronomeWork(object source, ElapsedEventArgs e)
    {
        SoundPlayer player;
         CurrentFaze++;
        if (CurrentFaze == 1)
        {
            player = new SoundPlayer(Properties.Resources.HighMetronomeSound);
            player.Play();
        }
        else
        {
            player = new SoundPlayer(Properties.Resources.LowMetronomeSound);
            player.Play();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
