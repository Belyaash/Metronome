using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Metronome.Model.Interfaces;
using oLeg_Company_App;

namespace Metronome.ViewModel;
public class MainWindowViewModel : INotifyPropertyChanged
{

    public IMetronome Metronome { get; }  

    public List<int> MeasuresList { get; }

    public MainWindowViewModel()
    {
        InitCommands();
        Metronome = new Model.Metronome();
        MeasuresList = new List<int>();
        for (int i = 1; i <= 8; i++)
        {
            MeasuresList.Add(i);
        }
    }

    private void InitCommands()
    {
        this.StartStopCommand = new DelegateCommand(ExecuteToStartStop);
    }

    public IDelegateCommand StartStopCommand { private set; get; }

    private void ExecuteToStartStop(object param)
    {
        Metronome.StartOrStopMetronome();
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
