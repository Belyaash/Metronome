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

    #region Variables

    private readonly Timer _timer = new();


    private int _bpm;

    public int Bpm
    {
        get => _bpm;
        set
        {
            _bpm = value switch
            {
                < 1 => 1,
                > 300 => 300,
                _ => value
            };
            UpdateTimerInterval();
            OnPropertyChanged("Bpm");
        }
    }

    private void UpdateTimerInterval()
    {
        const int millisecondsInMinute = 60000;
        _timer.Interval = (double)millisecondsInMinute/Bpm;
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
            StartOrStopTimer(value);
        }
    }

    private void StartOrStopTimer(bool value)
    {
        if (value)
            _timer.Start();
        else
            _timer.Stop();
    }

    #endregion

    private void MetronomeWork(object source, ElapsedEventArgs e)
    {
        CurrentFaze++;
        SelectSoundAndPlay();
    }

    private void SelectSoundAndPlay()
    {
        var player = CreatePlayerByCurrentFaze();
        player.Play();
    }

    private SoundPlayer CreatePlayerByCurrentFaze()
    {
        var player = CurrentFaze == 1
            ? new SoundPlayer(Properties.Resources.HighMetronomeSound)
            : new SoundPlayer(Properties.Resources.LowMetronomeSound);
        return player;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}