using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using VendingMachine.Core;

namespace VendingMachine.Wpf
{
    public class MachineViewModel : ViewModelBase
    {
        private readonly Machine _machine;
        
        private ICommand _insertFivePoundNoteCommand;
        private ICommand _insertTenPoundNoteCommand;

        private ICommand _insertTenPenceCommand;
        private ICommand _insertTwentyPenceCommand;
        private ICommand _insertFiftyPenceCommand;
        private ICommand _insertOnePoundCommand;

        private string _credit;
        private ICommand _cancelCommand;
        private ICommand _vendCommand;

        public MachineViewModel() : this(new Machine())
        {
        }

        public MachineViewModel(Machine machine)
        {
            _machine = machine;
            _machine.IsOutOfStockChanged += IsMachineOutOfStockChanged;
            _machine.ChangeGiven += OnChangeGiven;
            Locations = new ObservableCollection<LocationViewModel>(_machine.Locations.Values.Select(l => new LocationViewModel(l)));
            Stock();
        }

        private void OnChangeGiven(object sender, ChangeGivenEventArgs e)
        {
            Credit = _machine.Credit.ToString("c");
        }

        private void IsMachineOutOfStockChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Machine.IsOutOfStock)) return;
            SetOutOfStockStatus((sender as Machine).IsOutOfStock);
        }

        private void SetOutOfStockStatus(bool isOutOfStock)
        {
            Credit = isOutOfStock
                ? "Out of order"
                : _machine.Credit.ToString("c");
        }

        private void Stock()
        {
            _machine.Locations["A1"].Stock(Product.CokeCan);
            _machine.Locations["A2"].Stock(Product.CokeBottle);
        }

        public ObservableCollection<LocationViewModel> Locations { get; private set; }

        public ICommand InsertFivePoundNoteCommand
        {
            get => _insertFivePoundNoteCommand ??
                (_insertFivePoundNoteCommand = new RelayCommand(x => InsertNote(NoteDenomination.FivePound), x => CanAcceptCash));
        }

        public ICommand InsertTenPoundNoteCommand
        {
            get => _insertTenPoundNoteCommand ??
                (_insertTenPoundNoteCommand = new RelayCommand(x => InsertNote(NoteDenomination.TenPound), x => CanAcceptCash));
        }

        public ICommand InsertTenPenceCommand
        {
            get => _insertTenPenceCommand ??
                (_insertTenPenceCommand = new RelayCommand(x => InsertCoin(CoinDenomination.TenPence), x => CanAcceptCash));
        }

        public ICommand InsertTwentyPenceCommand
        {
            get => _insertTwentyPenceCommand ??
                (_insertTwentyPenceCommand = new RelayCommand(x => InsertCoin(CoinDenomination.TwentyPence), x => CanAcceptCash));
        }

        public ICommand InsertFiftyPenceCommand
        {
            get => _insertFiftyPenceCommand ??
                (_insertFiftyPenceCommand = new RelayCommand(x => InsertCoin(CoinDenomination.FiftyPence), x => CanAcceptCash));
        }

        public ICommand InsertOnePoundCommand
        {
            get => _insertOnePoundCommand ??
                (_insertOnePoundCommand = new RelayCommand(x => InsertCoin(CoinDenomination.OnePound), x => CanAcceptCash));
        }

        public ICommand CancelCommand { get => _cancelCommand ?? (_cancelCommand = new RelayCommand(x => _machine.Cancel(), x => CanCancel)); }

        public ICommand VendCommand { get => _vendCommand ?? (_vendCommand = new RelayCommand(location => DoVend((string)location))); }

        private void DoVend(string vendLocation)
        {
            var result = _machine.Vend(vendLocation);
            switch (result)
            {
                case VendResult.Success:
                    Credit = "Enjoy your drink";
                    break;

                case VendResult.InsufficientCredit:
                    Credit = "Insert more credit";
                    break;

                case VendResult.ProductNotAvailable:
                    Credit = "Unavailable";
                    break;
            }
        }
               
        public string Credit
        {
            get { return _credit; }
            set
            {
                SetProperty(ref _credit, value);
            }
        }

        private void InsertNote(NoteDenomination denomination)
        {
            if (_machine.NoteCreditProvider.InsertNote(denomination))
            {
                Credit = _machine.NoteCreditProvider.Total.ToString("c");
            }
            else
            {
                // Set status
            }
        }

        private void InsertCoin(CoinDenomination denomination)
        {
            if (_machine.CoinCreditProvider.InsertCoin(denomination))
            {
                Credit = _machine.CoinCreditProvider.Total.ToString("c");
            }
            else
            {
                // Set status
            }
        }

        private bool CanAcceptCash
        {
            get => !_machine.IsOutOfStock;
        }

        private bool CanCancel
        {
            get => _machine.Credit > 0;
        }
    }
}