﻿using System.Reactive.Linq;
using ReactiveUI;
using BotClient.ViewModels;
using Lab2Server.Models;

namespace BotClient
{
    //public class MainWindowBase : ReactiveWindow<MainViewModel>
    //{
    //}

    //public partial class MainWindow : MainWindowBase
    //{
    //    public MainWindow()
    //    {
    //        InitializeComponent();

    //        _ = this.WhenActivated(cd =>
    //        {
    //            if (ViewModel is null)
    //                return;

    //            cd.Add(ViewModel.CreateUserEvent.RegisterHandler(interaction =>
    //            {
    //                var eventViewModel = new ReminderViewModel();
    //                var eventView = new ReminderWindow
    //                {
    //                    Owner = this,
    //                    ViewModel = eventViewModel
    //                };

    //                // No async version of ShowDialog...
    //                return Observable.Start(() =>
    //                {
    //                    _ = eventView.ShowDialog();
    //                    interaction.SetOutput(eventView.Tag as UserEvent);
    //                }, RxApp.MainThreadScheduler);
    //            }));
    //            cd.Add(ViewModel.EditUserEvent.RegisterHandler(interaction =>
    //            {
    //                var eventViewModel = new EventViewModel() { };
    //                eventViewModel.EventName = ViewModel.SelectedUserEvent!.EventName;
    //                eventViewModel.DateNTime = ViewModel.SelectedUserEvent!.DateNTime.ToString("dd.MM.yyyy HH:mm");
    //                eventViewModel.EventFrequency = ViewModel.SelectedUserEvent!.EventFrequency.ToString();
    //                var eventView = new EventWindow
    //                {
    //                    Owner = this,
    //                    ViewModel = eventViewModel
    //                };

    //                // No async version of ShowDialog...
    //                return Observable.Start(() =>
    //                {
    //                    _ = eventView.ShowDialog();
    //                    interaction.SetOutput(eventView.Tag as UserEvent);
    //                }, RxApp.MainThreadScheduler);
    //            }));
    //        });
    //    }

    //    private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    //    {

    //    }
    //}
}