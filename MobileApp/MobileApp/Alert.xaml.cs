using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp;

public partial class Alert : ContentPage
{
    public Alert()
    {
        InitializeComponent();
    }

    protected async void EndScan(object _, EventArgs e)
    {
		await Application.Current?.MainPage?.Navigation.PopModalAsync()!;
    }
}
