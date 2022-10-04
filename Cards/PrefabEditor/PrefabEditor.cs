using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Api;
using Microsoft.WindowsAPICodePack.Dialogs;
using SummerJam1.Statuses;
using Component = Api.Component;

namespace PrefabEditor
{
    public partial class PrefabEditor : Form
    {
        public ComponentService Service { get; private set; }
        public CommonOpenFileDialog openFileDialog1;
        private Proxy CurrentProxy;
        public PrefabEditor()
        {
            openFileDialog1 = new CommonOpenFileDialog();
            openFileDialog1.IsFolderPicker = true;
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            Service = new ComponentService();
            propertyGrid1.PropertySort = PropertySort.NoSort;
            propertyGrid1.PropertyValueChanged += PropertyGrid1_PropertyValueChanged;
            
            Service.PropertyChanged += Service_PropertyChanged;
            buttonPrefabDirectory.Click += buttonPrefabDirectory_Click;
            addComponentListBox.DoubleClick += AddComponentListBox_DoubleClick;
            UpdateAddComponentListBox();
        }

        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            CurrentProxy.SetProperty(e.ChangedItem.PropertyDescriptor, e.ChangedItem.Value);
        }

        private void UpdateAddComponentListBox()
        {
            addComponentListBox.Items.Clear();

            Regex regex = new Regex(textBox1.Text, RegexOptions.IgnoreCase);
            foreach ( var type in Service.ComponentTypes)
            {
                string name = type.FullName;
                if (regex.IsMatch(name))
                {
                    addComponentListBox.Items.Add(type);
                }
                
            }
        }

        private void AddComponentListBox_DoubleClick(object sender, EventArgs e)
        {
            Service.CurrentEntity.AddComponent((Type)addComponentListBox.SelectedItem);
            UpdateComponentList();  
        }

        private void Service_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Service.CurrentEntity))
            {
                UpdateComponentList();
                UpdatePropertyGrid();
            }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void buttonPrefabDirectory_Click(object sender, EventArgs e)
        {
            
            if (openFileDialog1.ShowDialog() == CommonFileDialogResult.Ok)
            {
                try
                {
                    var di = new DirectoryInfo(openFileDialog1.FileName);
                    Service.Start(di.FullName);

                    UpdateAddComponentListBox();

                    UpdatePrefabsList();

                    UpdateComponentList();

                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void UpdatePrefabsList()
        {
            prefabsListBox.Items.Clear();
            var files = Service.GetFilelist();
            Regex regex = new Regex(prefabsSearch.Text, RegexOptions.IgnoreCase);

            foreach (var file in files)
            {
                string name = file;
                if (regex.IsMatch(name))
                {
                    prefabsListBox.Items.Add(file);
                }
            }

            
        }

        private void UpdateComponentList()
        {
            componentsListBox.Items.Clear();
            Regex regex = new Regex(currentComponentsSearch.Text, RegexOptions.IgnoreCase);
            if(Service?.CurrentEntity == null)
            {
                return;
            }
            foreach (var component in Service.CurrentEntity.Components)
            {
                string name = component.GetType().FullName;
                if (regex.IsMatch(name))
                {
                    componentsListBox.Items.Add(component);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void componentsListBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdatePropertyGrid();

        }

        private void UpdatePropertyGrid()
        {
            object selected = componentsListBox.SelectedItem;
            if (componentsListBox.SelectedItem != null)
            {
                CurrentProxy = new Proxy(componentsListBox.SelectedItem);
                selected = CurrentProxy;
            }
            else
            {
                CurrentProxy = null;
            }

            propertyGrid1.SelectedObject = selected;
        }

        private void prefabsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Service.LoadPrefab(prefabsListBox.SelectedItem.ToString());
            UpdateComponentList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Service.SaveCurrentPrefab();
            UpdateComponentList();
            UpdatePrefabsList();
        }

        private void addComponentListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Service.CurrentEntity.RemoveComponent((Component)componentsListBox.SelectedItem);
            UpdateComponentList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateAddComponentListBox();
        }

        private void currentComponentsSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateComponentList();
        }

        private void prefabsSearch_TextChanged(object sender, EventArgs e)
        {
            UpdatePrefabsList();
        }
    }

    public class TestClass
    {
        public int Tester { get; set; }
        public string Test2 { get; set; }
    }
}
