using System;
using System.Windows;

namespace ClickOnce_Deployment_Manager_64.MessageBox
{
    /// <summary>
    /// Interaction logic for ExitMessageBox.xaml
    /// </summary>
    public partial class ExitMessageBox : Window
    {
        public enum ButtonType
        {
            YesorNo, Ok
        }

        #region Fields

        private static ExitMessageBox _objCOMessageBox;

        #endregion Fields

        #region Constructor

        public ExitMessageBox()
        {
            try
            {
                InitializeComponent();
                this.SourceInitialized += (x, y) =>
                {
                    this.HideMinimizeAndMaximizeButtons();
                };
            }
            catch (System.Exception)
            {
            }
        }

        #endregion Constructor

        #region Properties

        public string Message
        {
            get
            {
                return TxtBlockMessage.Text;
            }
            set
            {
                TxtBlockMessage.Text = value;
            }
        }

        public string TitleText
        {
            get
            {
                return Setup_Title.Text;
            }
            set
            {
                Setup_Title.Text = value;
            }
        }

        #endregion Properties

        #region ShowMessage

        public static bool ShowMessage(string message, string title, ButtonType buttonType)
        {
            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(title))
            {
                _objCOMessageBox = new ExitMessageBox();
                if (buttonType == ButtonType.Ok)
                {
                    _objCOMessageBox.BtnNo.Content = ButtonType.Ok;
                    _objCOMessageBox.BtnYes.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _objCOMessageBox.BtnNo.Content = "No";
                    _objCOMessageBox.BtnYes.Visibility = Visibility.Visible;
                }
                _objCOMessageBox.Message = message;
                _objCOMessageBox.TitleText = title;
                _objCOMessageBox.ShowDialog();
            }
            return (bool)_objCOMessageBox.DialogResult;
        }

        #endregion ShowMessage

        //public bool ShowMessBox(Window currentWindow,string message)
        //{
        // if (!string.IsNullOrEmpty(message)&& currentWindow!=null)
        // {
        //     currentWindowHandle = currentWindow;
        //     TxtBlockMessage.Text = message;
        //     this.Show();
        // }
        // return true;
        //}

        #region BtnYes_Click

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        #endregion BtnYes_Click

        #region BtnNo_Click

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            if ("No" == _objCOMessageBox.BtnNo.Content as string)
                this.DialogResult = false;
            else
                this.DialogResult = true;
            this.Close();
        }

        #endregion BtnNo_Click

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
            }
        }
    }
}