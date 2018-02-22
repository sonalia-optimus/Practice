using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.PointOfService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BarcodeScanningDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public BarcodeScanning BarcodeScanInitialiser = new BarcodeScanning();
        public MainPage()
        {
            this.InitializeComponent();
            
            ScanButton.Click += new RoutedEventHandler(btn_Click);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            BarcodeScanInitialiser.GetScanningDevice();
            if (BarcodeScanInitialiser.ScanningData != null)
            {
                TextBlockData.Text = BarcodeScanInitialiser.ScanningData;
            }
            else
            {
                ErrorMessageBlock.Text = BarcodeScanInitialiser.ErrorMsg;
            }
        }
    }

    public class BarcodeScanning
    {
        private static BarcodeScanner _barcodeScanner;

        private static ClaimedBarcodeScanner _claimedBarcodeScanner;

        public string ScanningData;

        public string ErrorMsg;

        public async void GetScanningDevice()
        {
            _barcodeScanner = await BarcodeScanner.GetDefaultAsync();
            if (_barcodeScanner != null)
            {
                _claimedBarcodeScanner = await _barcodeScanner.ClaimScannerAsync();
            }
            else
            {
                ErrorMsg = "scanning device not found";
                return ;
            }
            if (_claimedBarcodeScanner != null)
            {
                _claimedBarcodeScanner.DataReceived += ClaimedBarcode_DataRecieved;
                _claimedBarcodeScanner.IsDecodeDataEnabled = true;
                await _claimedBarcodeScanner.EnableAsync();
            }
            else
            {
                ErrorMsg = "scanning device cant be claimed ";
                return;
            }
        }

        private void ClaimedBarcode_DataRecieved(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args)
        {
            string symbologyName = BarcodeSymbologies.GetName(args.Report.ScanDataType);
            var scanDataLabelReader = DataReader.FromBuffer(args.Report.ScanDataLabel);
            string barcode = scanDataLabelReader.ReadString(args.Report.ScanDataLabel.Length);
            ScanningData = barcode;
        }
    }
}
