using Omu.ValueInjecter;
using UniversalSample.Models;
using UniversalSample.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var p = new Product()
            {
                ModelName = "das name",
                Description = "bla",
                ModelNumber = "über",
                UnitCost = 1231
            };
            var vm = new ProductViewModel();
            gridProductDetails.DataContext = vm;
            vm.InjectFrom(p);
        }
    }
}
