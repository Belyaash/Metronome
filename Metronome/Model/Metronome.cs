﻿using System;
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

    public int Bpm
    {
        get => Properties.Settings.Default.MetronomeBpm;
        set
        {
            SetBpmInRange(value);
            SetTimerInterval();
            OnPropertyChanged(nameof(Bpm));
        }
    }

    private static void SetBpmInRange(int value)
    {
        Properties.Settings.Default.MetronomeBpm = value switch
        {
            < 10 => 10,
            > 300 => 300,
            _ => value
        };
        Properties.Settings.Default.Save();
    }

    private void SetTimerInterval()
    {
        const int millisecondsInMinute = 60000;
        _timer.Interval = (double)millisecondsInMinute/Bpm;
    }

    public int CurrentMeasure
    {
        get => Properties.Settings.Default.MetronomeMeasure;
        set
        {
            Properties.Settings.Default.MetronomeMeasure = value;
            Properties.Settings.Default.Save();
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

    public Metronome()
    {
        Bpm = Properties.Settings.Default.MetronomeBpm;
        CurrentMeasure = Properties.Settings.Default.MetronomeMeasure;
        CurrentFaze = 0;
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
        var player = CurrentFaze == 1
            ? new SoundPlayer(Properties.Resources.HighMetronomeSound)
            : new SoundPlayer(Properties.Resources.LowMetronomeSound);
        player.Play();
    }

    public void StartOrStopMetronome()
    {
       _timer.Enabled = !_timer.Enabled;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}