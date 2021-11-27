namespace RobotCleanerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RobotCleaner robotCleaner = new RobotCleaner(this, 5, 5);
            this.Show();

        }
    }
}