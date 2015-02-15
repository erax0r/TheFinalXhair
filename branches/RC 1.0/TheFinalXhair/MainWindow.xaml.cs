using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Configuration;

namespace TheFinalXhair
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private string _registrationcode;
        private bool _isregistered = false;
        private bool _crosshairenabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCrosshair"]);
        private double _crosshairthickness = Convert.ToDouble(ConfigurationManager.AppSettings["Crosshairthickness"]);
        private double _crosshairopacity = Convert.ToDouble(ConfigurationManager.AppSettings["CrosshairOpacity"]);
        private bool _gridenabled = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableGrid"]);
        private double _griddthickness = Convert.ToDouble(ConfigurationManager.AppSettings["GridThickness"]);
        private double _gridopacity = Convert.ToDouble(ConfigurationManager.AppSettings["GridOpacity"]);
        private string _gridcolor = ConfigurationManager.AppSettings["GridColor"];
        private bool _showintaskbar = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowInTaskbar"]);

        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();         
            CheckRegistrationCode();
            InitializeGrid();
            InitilializeCrosshair();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowState = WindowState.Maximized;
            this.ShowInTaskbar = this._showintaskbar;

        }
        #endregion

        #region Methods
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }

        public void LineVLengthChanged(double newval)
        {
            this.LineV.Y2 = newval;
        }

        private void CheckRegistrationCode()
        {
            _registrationcode = ConfigurationManager.AppSettings["RegistrationCode"];
            if (_registrationcode == "FCKGW-RHQQ2-YXRKT-8TG6W-2B7Q8")
                _isregistered = true;

            if (_isregistered == false)
            {
                throw new Exception("Invalid Registration Code");
            }
        }

        private void InitializeGrid()
        {
            if (this._gridenabled == true)
            {
                this.BigLineV.StrokeThickness = this._griddthickness;
                this.BigLineH.StrokeThickness = this._griddthickness;
                this.BigLineV.Opacity = this._gridopacity;
                this.BigLineH.Opacity = this._gridopacity;
            }
            else
            {
                this.BigLineH.Visibility = Visibility.Hidden;
                this.BigLineV.Visibility = Visibility.Hidden;
            }
        }

        private void InitilializeCrosshair()
        {
            if (this._crosshairenabled == true)
            {
                this.LineH.StrokeThickness = this._crosshairthickness;
                this.LineV.StrokeThickness = this._crosshairthickness;
                this.LineH.Opacity = this._crosshairopacity;
                this.LineV.Opacity = this._crosshairopacity;
            }
            else
            {
                this.LineH.Visibility = Visibility.Hidden;
                this.LineV.Visibility = Visibility.Hidden;
            }
        }
    }
        #endregion

    #region Additional Class
    public static class WindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }    
    #endregion
}
