using System;
using System.ComponentModel;
using System.Threading.Tasks;
using seniorCapstone.ViewModels;

namespace seniorCapstone.Services
{
    public interface INavService
    {
        bool CanGoBack { get; }

        Task GoBack ();

        Task NavigateTo<TVM> ()
            where TVM : MainViewModel;

        Task NavigateTo<TVM, TParameter> (TParameter parameter)
            where TVM : MainViewModel;

        void RemoveLastView ();

        void ClearBackStack ();

        void NavigateToUri (Uri uri);

        event PropertyChangedEventHandler CanGoBackChanged;
    }
}
