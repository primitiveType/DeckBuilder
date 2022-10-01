using System.Windows.Forms;

namespace PrefabEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            var componentService = new ComponentService();
            componentService.Start();
            propertyGrid1.SelectedObject = new TestClass();
        }
    }

    public class TestClass
    {
        public int Tester { get; set; }
        public string Test2 { get; set; }
    }
}