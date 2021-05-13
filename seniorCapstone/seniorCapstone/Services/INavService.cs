/****************************************************************************
 * File		INavService.cs
 * Author	Audrey Lincoln
 * Date		3/20/2021
 * Purpose	Navigation service - not used but would be better design to 
 *          refactor and use this
 ****************************************************************************/

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
