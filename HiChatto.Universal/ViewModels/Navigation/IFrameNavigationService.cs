using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiChatto.Universal.ViewModels.Navigation
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
