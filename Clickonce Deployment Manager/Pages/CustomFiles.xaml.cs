using ClickOnceDeployment.Core;
using ClickOnceDeployment.Data;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ClickOnce_Deployment_Manager_64.Pages
{
    /// <summary>
    /// Interaction logic for CustomFiles.xaml
    /// </summary>
    public partial class CustomFiles : Page
    {
        public static Microsoft.Win32.OpenFileDialog dgOpen;
        private Pointel.Logger.Core.ILog _logger;

        public CustomFiles()
        {
            InitializeComponent();
            _logger = Pointel.Logger.Core.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ClickOnce Deployment");
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                LoadFiles();
                dgOpen = new Microsoft.Win32.OpenFileDialog();
                dgOpen.Filter = "All Files (*.*)|*.*";
                dgOpen.InitialDirectory = ApplicationDetails.Instance().AppFolderName;
                dgOpen.Multiselect = true;
                dgOpen.CheckFileExists = true;
                dgOpen.CheckPathExists = true;
                dgOpen.RestoreDirectory = false;
                dgOpen.FileOk += new System.ComponentModel.CancelEventHandler(dlg_FileOk);
            }
            catch (Exception generalException)
            {
                _logger.Error("WndCustomFiles Window Load Event :" + generalException.ToString());
                MessageBox.ExitMessageBox.ShowMessage(generalException.Message, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
            }
        }

        #region Btn Add Click

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            dgOpen.ShowDialog();
        }

        #endregion Btn Add Click

        #region Btn Remove Click

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            _logger.Trace("Remove button Clicked..");
            var grid = DGCustomFiles as DataGrid;
            var selected = grid.SelectedItems;
            if (DGCustomFiles.SelectedIndex >= 0)
            {
                _logger.Trace("No of custom file selected" + DGCustomFiles.SelectedIndex);
                if (MessageBox.ExitMessageBox.ShowMessage("\n Are you sure you want to remove(s) the file ?", "Requested message", MessageBox.ExitMessageBox.ButtonType.YesorNo))
                {
                    FileReference fileReference = null;
                    if (DGCustomFiles.SelectedItems.Count > 1)
                    {
                        foreach (var file in DGCustomFiles.SelectedItems)
                        {
                            fileReference = file as FileReference;
                            if (ClickOnceManager.Instance().RemoveFile(fileReference.TargetPath).OperationStatus == OperationStatus.Success)
                            {
                                _logger.Trace("The file '" + fileReference.TargetPath + "' removed successfully.");
                            }
                        }
                        LoadFiles();
                    }
                    else
                    {
                        fileReference = DGCustomFiles.Items[DGCustomFiles.SelectedIndex] as FileReference;
                        if (ClickOnceManager.Instance().RemoveFile(fileReference.TargetPath).OperationStatus == OperationStatus.Success)
                        {
                            _logger.Trace("The file '" + fileReference.TargetPath + "' removed successfully.");
                            LoadFiles();
                        }
                    }
                }
            }
            else
            {
                MessageBox.ExitMessageBox.ShowMessage("Please select the file to remove.", "Requested message", MessageBox.ExitMessageBox.ButtonType.Ok);
            }
        }

        #endregion Btn Remove Click

        #region File Browse Dialog Click Ok

        private void dlg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<string> existedFile = new List<string>();
            List<string> CopyFiles = new List<string>();
            List<string> SamelocationFiles = new List<string>();
            try
            {
                //modified by balamurgan 30/11/2016
                //Need to copy file in the application path and then need to add it as custom file
                string[] files = dgOpen.FileNames;
                foreach (string selectedFile in files)
                {
                    FileInfo fileInfo = new FileInfo(selectedFile);
                    if (File.Exists(ApplicationDetails.Instance().AppFolderName + @"\" + Path.GetFileName(selectedFile)))
                    {
                        if (fileInfo.DirectoryName.ToString() + "\\" != ApplicationDetails.Instance().AppFolderName)
                            existedFile.Add(selectedFile);
                        else
                            SamelocationFiles.Add(selectedFile);
                    }
                    else
                        CopyFiles.Add(selectedFile);
                }
                //from different loaction
                if (existedFile.Count > 0)
                {
                    if (MessageBox.ExitMessageBox.ShowMessage("The file already exist within the application.Are you sure want to override this file?", "Clickonce Deployment Request", MessageBox.ExitMessageBox.ButtonType.YesorNo))
                    {
                        foreach (string file in existedFile)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            FileInfo destFile = new FileInfo(Path.Combine(ApplicationDetails.Instance().AppFolderName, fileInfo.Name));
                            if (destFile.Exists)
                            {
                                // now you can safely overwrite it
                                fileInfo.CopyTo(destFile.FullName, true);
                            }
                            if (ClickOnceManager.Instance().AddNewFile(ApplicationDetails.Instance().AppFolderName + "\\" + Path.GetFileName(file)).OperationStatus == OperationStatus.Success)
                            {
                                _logger.Trace("The file '" + file + "' added successfully.");
                                LoadFiles();
                            }
                            else
                                MessageBox.ExitMessageBox.ShowMessage("Some error occured in file reference", "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                        }
                    }
                }
                //copy file to subfolder
                else if (CopyFiles.Count != 0)
                {
                    if (MessageBox.CustomFileMessageBox.ShowMessage("\n The Selected file not within the application.To proceed the further steps, Need to copy this file within the application. Are you sure want to proceed it?", "Requested message"))
                    {
                        foreach (string file in CopyFiles)
                        {
                            if (!dgOpen.FileName.StartsWith(dgOpen.InitialDirectory) || dgOpen.FileName.Contains(dgOpen.InitialDirectory))
                            {
                                if (!string.IsNullOrEmpty(CustomFileDetails.Instance().SelectedFolder))
                                {
                                    if (file != CustomFileDetails.Instance().SelectedFolder + "\\" + Path.GetFileName(file))
                                        File.Copy(file, CustomFileDetails.Instance().SelectedFolder + "\\" + Path.GetFileName(file), true);
                                    if (ClickOnceManager.Instance().AddNewFile(CustomFileDetails.Instance().SelectedFolder + "\\" + Path.GetFileName(file)).OperationStatus == OperationStatus.Success)
                                    {
                                        _logger.Trace("The file '" + file + "' added successfully.");
                                        LoadFiles();
                                    }
                                    else
                                        MessageBox.ExitMessageBox.ShowMessage("Some error occured in file reference", "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                                }
                            }
                            else
                            {
                                if (ClickOnceManager.Instance().AddNewFile(file).OperationStatus == OperationStatus.Success)
                                {
                                    _logger.Trace("The file '" + file + "' added successfully.");
                                    LoadFiles();
                                }
                            }
                        }
                    }
                    CopyFiles.Clear();
                }
                else if (SamelocationFiles.Count > 0)
                {
                    foreach (string fileItem in SamelocationFiles)
                    {
                        if (ClickOnceManager.Instance().AddNewFile(fileItem).OperationStatus == OperationStatus.Success)
                        {
                            _logger.Trace("The file '" + fileItem + "' added successfully.");
                            LoadFiles();
                        }
                        else
                            MessageBox.ExitMessageBox.ShowMessage("Some error occured in file reference", "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
                    }
                    SamelocationFiles.Clear();
                }
            }
            catch (Exception generalException)
            {
                _logger.Error("CustomFiles dlg_FileOk :" + generalException.ToString());
                MessageBox.ExitMessageBox.ShowMessage("Some error occured in file reference :" + generalException.Message, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
            }
            finally
            {
                existedFile.Clear();
                CopyFiles.Clear();
            }
        }

        #endregion File Browse Dialog Click Ok

        #region Load Files in Data Grid

        private void LoadFiles()
        {
            try
            {
                DGCustomFiles.ItemsSource = ClickOnceManager.Instance().FileReference;
                DGCustomFiles.Items.Refresh();
            }
            catch (Exception generalException)
            {
                _logger.Error("WndCustomFilesForm LoadFiles() :" + generalException.ToString());
                MessageBox.ExitMessageBox.ShowMessage(generalException.Message, "Clickonce Deployment Error", MessageBox.ExitMessageBox.ButtonType.Ok);
            }
        }

        #endregion Load Files in Data Grid
    }
}