using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using static SystemBackdropTypes.PInvoke.ParameterTypes;
using static SystemBackdropTypes.PInvoke.Methods;
using ModernWpf;

namespace SystemBackdropTypes
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Loaded += OnLoaded;
		}

		void OnLoaded(object sender, RoutedEventArgs e)
		{
			RefreshFrame();
			//RefreshDarkMode();
			SizeChanged += (_, _) => RefreshFrame();
			//ThemeManager.Current.ActualApplicationThemeChanged += (_, _) => RefreshDarkMode();
		}

		private void RefreshFrame()
		{
			IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
			HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
			mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

			System.Drawing.Graphics desktop = System.Drawing.Graphics.FromHwnd(mainWindowPtr);
			float DesktopDpiX = desktop.DpiX;

			MARGINS margins = new MARGINS();
			margins.cxLeftWidth = Convert.ToInt32(5 * (DesktopDpiX / 96));
			margins.cxRightWidth = Convert.ToInt32(5 * (DesktopDpiX / 96));
			margins.cyTopHeight = Convert.ToInt32(((int)ActualHeight + 5) * (DesktopDpiX / 96));
			margins.cyBottomHeight = Convert.ToInt32(5 * (DesktopDpiX / 96));

			ExtendFrame(mainWindowSrc.Handle, margins);
		}

		public bool? IsDark
		{
			get { return (bool?)GetValue(IsDarkProperty); }
			set { SetValue(IsDarkProperty, value); }
		}
		public static readonly DependencyProperty IsDarkProperty =
			DependencyProperty.Register(
				"IsDark",
				typeof(bool?),
				typeof(MainWindow),
				new PropertyMetadata(
					null,
					(d, e) => ((MainWindow)d).RefreshDarkMode()));

		private void RefreshDarkMode()
		{
			IsDark ??= ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark;
			int flag = IsDark.Value ? 1 : 0;
			SetWindowAttribute(
				new WindowInteropHelper(this).Handle, 
				DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, 
				flag);
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			int flag = int.Parse((string)((RadioButton)e.OriginalSource).Tag);
			SetWindowAttribute(
				new WindowInteropHelper(this).Handle, 
				DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, 
				flag);
		}

		private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();
		private void Button_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
	}
}
