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
using Metronome.Model.Interfaces;

namespace Metronome.Model;
public class Metronome : INotifyPropertyChanged, IMetronome
{
    #region Variables

    private readonly Timer _timer = new();

    private bool _isWorking;

    private int _bpm;
    public int Bpm
    {
        get => _bpm;
        set
        {
            SetBpmInRange(value);
            SetTimerInterval();
            OnPropertyChanged(nameof(Bpm));
        }
    }

    private void SetBpmInRange(int value)
    {
        _bpm = value switch
        {
            < 1 => 1,
            > 300 => 300,
            _ => value
        };
    }

    private void SetTimerInterval()
    {
        const int millisecondsInMinute = 60000;
        _timer.Interval = (double)millisecondsInMinute/Bpm;
    }

    private int _currentMeasure;
    public int CurrentMeasure
    {
        get => _currentMeasure;
        set
        {
            _currentMeasure = value;
            OnPropertyChanged(nameof(CurrentMeasure));
        }
    }

    private int _currentFaze;
    public int CurrentFaze
    {
        get => _currentFaze;
        private set
        {
            _currentFaze = value > CurrentMeasure ? 1 : value;
            OnPropertyChanged(nameof(CurrentFaze));
        }
    }

    #endregion

    public Metronome(int bpm, int currentMeasure)
    {
        Bpm = bpm;
        CurrentMeasure = currentMeasure;
        CurrentFaze = 0;
        _isWorking = false;
        _timer.AutoReset = true;
        _timer.Enabled = false;
        _timer.Elapsed += MetronomeWork;
    }

    private void MetronomeWork(object? source, ElapsedEventArgs e)
    {
        CurrentFaze++;
        CreatePlayerAndPlay();
    }

    private void CreatePlayerAndPlay()
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


    public void StartOrStopMetronome()
    {
        _isWorking = !_isWorking;
        StartOrStopTimer(_isWorking);
    }

    private void StartOrStopTimer(bool value)
    {
        if (value)
            _timer.Start();
        else
            _timer.Stop();
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}