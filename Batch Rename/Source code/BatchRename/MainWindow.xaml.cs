using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using OpenFileDialog = Microsoft.Win32.OpenFileDialog;


namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<StringOperation> _property = null;

            _property = new List<StringOperation>()
            {
                new ReplaceOperation(){ Args = new ReplaceArgs() { From = "", To = "" } },
                new NewCaseOperation(){ Args = new NewCaseArgs() { From = "", } },
                new FullnameNormalizeOperation() { Args = new FullnameNormalizeArgs() { From = "", } },
                new MoveOperation() { Args = new MoveArgs() { From = "" } },
                new UniqueNameOperation() { Args = new UniqueNameArgs() { } },
            };

            ActionComboBox.ItemsSource = _property;
            loadPresetFile();

        }

        private void loadPresetFile()
        {
            presetComboBox.Items.Clear();
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}";
            string[] filePreset = Directory.GetFiles(path,"*.txt");

            foreach(string preset in filePreset)
            {
                string[] tokens = preset.Split(new string[] { "\\" }, StringSplitOptions.None);
                string[] tokendots = tokens[tokens.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
                string extensions = tokendots[tokendots.Length - 1];

                InfoFile temp = new InfoFile();
                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    temp.Path += tokens[i] + "\\";
                }
                temp.Name = tokendots[0];
                presetComboBox.Items.Add(temp);
            }
        }

        private void btnAddAction_Click(object sender, RoutedEventArgs e)
        {
            var property = ActionComboBox.SelectedItem as StringOperation;

            var instance = property.Clone();

            ActionListBox.Items.Add(instance);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var action = ActionListBox.SelectedItem as StringOperation;
            action.ShowEditDialog();
        }

        public static string _pathFile;
        public static string _pathFolder;

        List<InfoFile> _files = new List<InfoFile>();
        List<InfoFile> _folder = new List<InfoFile>();
     
        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            FileListView.Items.Clear();
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _pathFile = fbd.SelectedPath;
                loadFile();
            }
        }

        private void loadFile()
        {
            FileListView.Items.Clear();
            _files = new List<InfoFile>();

            string[] files = Directory.GetFiles(_pathFile);

            foreach (var file in files)
            {
                InfoFile temp = new InfoFile();
                string[] tokens = file.Split(new string[] { "\\" }, StringSplitOptions.None);
                string[] tokendots = tokens[tokens.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
                string extensions = tokendots[tokendots.Length - 1];

                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    temp.Path += tokens[i] + "\\";
                }

                temp.Name = tokendots[0] + "." + extensions;

                _files.Add(temp);
            }

            foreach (var file in _files)
            {
                FileListView.Items.Add(file);
            }
        }
        private void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog screen = new FolderBrowserDialog();
            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _pathFolder = screen.SelectedPath;
                loadFolder();
            }
        }

        private void loadFolder()
        {
            FolderListView.Items.Clear();

            _folder = new List<InfoFile>();

            string[] dirs = Directory.GetDirectories(_pathFolder);
            foreach (string dir in dirs)
            {
                InfoFile temp = new InfoFile();
                string[] tokens = dir.Split(new string[] { "\\" }, StringSplitOptions.None);
                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    temp.Path += tokens[i] + "\\";
                }

                temp.Name = tokens[tokens.Length-1];

                _folder.Add(temp);
            }

            foreach (InfoFile folder in _folder)
            {
                FolderListView.Items.Add(folder);
            }

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (Tab.SelectedIndex == 0)
            {
                foreach (InfoFile file in _files)
                {
                    string filename = file.Path + file.Name;
                    string result = filename;
                    string temp = filename + "_";
                    foreach (StringOperation action in ActionListBox.Items)
                    {
                        result = action.Processor.Invoke(result);
                    }

                    try
                    {
                        System.IO.Directory.Move(filename, temp);

                        System.IO.Directory.Move(temp, result);
                    }

                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.ToString());
                    }

                }

                loadFile();
                System.Windows.MessageBox.Show("All Done");
            }
            else
            {
                foreach (InfoFile folder in _folder)
                {
                    string foldername = folder.Path + folder.Name;
                    string result = foldername;
                    string temp = foldername + "_";
                    foreach (StringOperation action in ActionListBox.Items)
                    {
                        result = action.Processor.Invoke(result);
                    }
                    try
                    {
                        System.IO.Directory.Move(foldername, temp);

                        System.IO.Directory.Move(temp, result);
                    }

                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.ToString());
                    }
                }

                loadFolder();
                System.Windows.MessageBox.Show("All Done");
            }
            
        }

        private void btnOpenPreset_Click(object sender, RoutedEventArgs e)
        {
            List<StringOperation> result = new List<StringOperation>();

            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;
                var lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    var tokens = line.Split(new string[]
                        { "|"}, StringSplitOptions.RemoveEmptyEntries);

                    switch (tokens[0].ToString())
                    {
                        case "Replace":
                            {
                                ReplaceOperation replace = new ReplaceOperation() { Args = new ReplaceArgs() { From = tokens[1], To = tokens[2] } };
                                result.Add(replace);
                                break;
                            }
                        case "NewCase":
                            {
                                NewCaseOperation newcase = new NewCaseOperation() { Args = new NewCaseArgs() { From = tokens[1], } };
                                result.Add(newcase);
                                break;
                            }
                        case "Fullname Normalize":
                            {
                                FullnameNormalizeOperation newcase = new FullnameNormalizeOperation() { Args = new FullnameNormalizeArgs() { From = tokens[1], } };
                                result.Add(newcase);
                                break;
                            }
                        case "Move":
                            {
                                MoveOperation newcase = new MoveOperation() { Args = new MoveArgs() { From = tokens[1], } };
                                result.Add(newcase);
                                break;
                            }
                    }
                }
            }
            foreach(StringOperation item in result)
            {
                ActionListBox.Items.Add(item);
            }
        }

        private void btnSavePreset_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FillinFileNameDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName + ".txt";
                var writer = new StreamWriter(filename);

                foreach (StringOperation action in ActionListBox.Items)
                {
                    switch (action.Name)
                    {
                        case "Replace":
                            {
                                ReplaceArgs temp = action.Args as ReplaceArgs;
                                writer.WriteLine($"{action.Name}|{temp.From}|{temp.To}");
                                break;
                            }
                        case "NewCase":
                            {
                                NewCaseArgs temp = action.Args as NewCaseArgs;
                                writer.WriteLine($"{action.Name}|{temp.From}");
                                break;
                            }
                        case "Fullname Normalize":
                            {
                                FullnameNormalizeArgs temp = action.Args as FullnameNormalizeArgs;
                                writer.WriteLine($"{action.Name}|{temp.From}");
                                break;
                            }
                        case "Move":
                            {
                                MoveArgs temp = action.Args as MoveArgs;
                                writer.WriteLine($"{action.Name}|{temp.From}");
                                break;
                            }
                    }
                }
                writer.Close();
                loadPresetFile();
            }
        }

        private void btnRemoveAction_Click(object sender, RoutedEventArgs e)
        {
            if (ActionListBox.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("chua chon item!");
            }
            else
            {
                ActionListBox.Items.RemoveAt(ActionListBox.SelectedIndex);
            }
        }

        private void presetChange(object sender, SelectionChangedEventArgs e)
        {
            InfoFile selected = presetComboBox.SelectedItem as InfoFile;

            if(selected != null)
            {
                List<StringOperation> result = new List<StringOperation>();
                var filename = selected.Name + ".txt";
                var lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    var tokens = line.Split(new string[]
                        { "|"}, StringSplitOptions.RemoveEmptyEntries);

                    switch (tokens[0].ToString())
                    {
                        case "Replace":
                            {
                                ReplaceOperation replace = new ReplaceOperation() { Args = new ReplaceArgs() { From = tokens[1], To = tokens[2] } };
                                result.Add(replace);
                                break;
                            }
                        case "NewCase":
                            {
                                NewCaseOperation newcase = new NewCaseOperation() { Args = new NewCaseArgs() { From = tokens[1], } };
                                result.Add(newcase);
                                break;
                            }
                        case "Fullname Normalize":
                            {
                                FullnameNormalizeOperation newcase = new FullnameNormalizeOperation() { Args = new FullnameNormalizeArgs() { From = tokens[1], } };
                                result.Add(newcase);
                                break;
                            }
                        case "Move":
                            {
                                MoveOperation newcase = new MoveOperation() { Args = new MoveArgs() { From = tokens[1], } };
                                result.Add(newcase);
                                break;
                            }
                    }
                }
                foreach (StringOperation item in result)
                {
                    ActionListBox.Items.Add(item);
                }
            }
        }

        private void btnRefesh_Click(object sender, RoutedEventArgs e)
        {
            FileListView.Items.Clear();
            FolderListView.Items.Clear();
            ActionListBox.Items.Clear();
        }
    }
}
