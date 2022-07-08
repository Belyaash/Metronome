using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Metronome.Model;
using oLeg_Company_App;

namespace Metronome.ViewModel;
public class MainWindowViewModel : INotifyPropertyChanged
{
    public MainWindowViewModel()
    {
        InitCommands();
        Metronome = new Model.Metronome(90, 2);
        MeasuresList = new List<int>();
        for (int i = 2; i <= 8; i++)
        {
            MeasuresList.Add(i);
        }
    }
    
    public Model.Metronome Metronome { get; set; }  

    public List<int> MeasuresList { get; }

    private void InitCommands()
    {
        this.StartStopCommand = new DelegateCommand(ExecuteToStartStop);
    }

    public IDelegateCommand StartStopCommand { protected set; get; }

    void ExecuteToStartStop(object param)
    {
        Metronome.IsWorking = !Metronome.IsWorking;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
