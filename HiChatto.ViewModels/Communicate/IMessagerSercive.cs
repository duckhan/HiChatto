﻿using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.ViewModels.Communicate
{
    public interface IMessagerSercive : INavigationService
    {
        object Parameter { get; }
        void ShowMessage(string message, string title);
        void ShowMessage(string message, string title, string buttonText, Action afterHideCallback);
        Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback);
        void ShowError(Exception error, string title, string buttonText, Action afterHideCallback);
        void PushToastNotification(string xmlTemplate);
    }
}
