using ClickOnce_Deployment_Manager_64.Pages;
using ClickOnceDeployment.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ClickOnce_Deployment_Manager_64.MessageBox
{
    // added by balamurugan on 28/04/2016
    /// <summary>
    /// Interaction logic for CustomFileMessageBox.xaml
    /// </summary>
    public partial class CustomFileMessageBox : Window
    {
        public enum ButtonType
        {
            YesorNo, Ok, Add
        }

        #region Fields

        private static CustomFileMessageBox _objCOMessageBox;
        private FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        //private static SubFolderListBox _subFoladerListBox;

        #endregion Fields

        #region Constructor

        public CustomFileMessageBox()
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

        public static bool ShowMessage(string message, string title)
        {
            try
            {
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(title))
                {
                    _objCOMessageBox = new CustomFileMessageBox();
                    _objCOMessageBox.Message = message;
                    _objCOMessageBox.TitleText = title;
                    _objCOMessageBox.ShowDialog();
                }
                return (bool)_objCOMessageBox.DialogResult;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion ShowMessage

        //Cancel Button Click
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            _objCOMessageBox.DialogResult = false;
            _objCOMessageBox.Close();
        }

        //Accept Buton Click
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            _objCOMessageBox.DialogResult = true;
            string[] filenames = CustomFiles.dgOpen.FileNames;
            if (filenames.Length > 0)
            {
                FileInfo fileInfo = new FileInfo(filenames[0]);
                if (fileInfo.DirectoryName.StartsWith(ApplicationDetails.Instance().AppFolderName))
                    CustomFileDetails.Instance().SelectedFolder = fileInfo.DirectoryName.ToString();
                else
                    CustomFileDetails.Instance().SelectedFolder = ApplicationDetails.Instance().AppFolderName;
            }
            _objCOMessageBox.Close();
        }

        //Subfolder Button Click
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                folderDlg.ShowNewFolderButton = true;
                folderDlg.SelectedPath = ApplicationDetails.Instance().AppFolderName;
                System.Windows.Forms.DialogResult dialogresult = folderDlg.ShowDialog();
                if (dialogresult == System.Windows.Forms.DialogResult.OK)
                {
                    if (folderDlg.SelectedPath + "\\" == ApplicationDetails.Instance().AppFolderName || folderDlg.SelectedPath.StartsWith(ApplicationDetails.Instance().AppFolderName))
                    {
                        CustomFileDetails.Instance().SelectedFolder = folderDlg.SelectedPath;
                        _objCOMessageBox.DialogResult = true;
                    }
                    else
                    {
                        if (MessageBox.ExitMessageBox.ShowMessage("The File cannot copy on selected loaction.Its will be copied only on " + ApplicationDetails.Instance().AppFolderName, "Error Message", MessageBox.ExitMessageBox.ButtonType.Ok))
                        {
                            CustomFileDetails.Instance().SelectedFolder = string.Empty;
                        }
                    }
                }
                else
                {
                    _objCOMessageBox.DialogResult = false;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

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