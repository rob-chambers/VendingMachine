using System.ComponentModel;
using VendingMachine.Core;

namespace VendingMachine.Wpf
{
    public class LocationViewModel : NotifyPropertyChangedBase
    {
        private readonly Location _model;
        private string _status;

        public LocationViewModel(Location model)
        {
            _model = model;
            _model.ProductChanged += OnProductChanged;
            SetStatus(_model);
        }

        private void OnProductChanged(object sender, PropertyChangedEventArgs e)
        {
            SetStatus(sender as Location);
        }

        private void SetStatus(Location location)
        {
            Status = location.OutOfStock 
                ? "Not Available" 
                : location.Product.Price.ToString("c");
        }

        public string Code => _model.Code;

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public string ProductName => _model.Product?.Name;
    }
}
