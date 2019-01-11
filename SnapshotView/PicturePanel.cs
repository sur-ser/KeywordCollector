using CollectorCore.Model;
using CollectorCore.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapshotView
{
    public class PicturePanel : GroupBox
    {
        public PictureBox PictureBox { get;private set; }
        private Panel PictureBoxPanel { get; set; }
        public CheckBox CheckBox { get; private set; }
        public Action<PicturePanel> OnPanelClick { get; set; }
        public Action<PicturePanel> OnPictureMouseHover { get; set; }        
        public int Index { get; set; }
        public SnapshotModel Model { get; set; }
        public Image Image
        {
            get
            {
                return PictureBox.Image;
            }
            set
            {
                PictureBox.Image = value;
            }
        }

        private string fileName;
        public string ImageFile
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                this.Image = ImageUtils.GetImage(fileName, this.Width, this.Height);
            }
        }

        public string ShowUrl
        {

            get
            {
                if (this.Model == null)
                    return null;

                return this.Model.Url;
            }
            set
            {
                if (this.Model == null)
                    return;

                this.Model.Url = value;
            }
        }

        public bool Checked
        {
            get
            {
                return this.CheckBox.Checked;
            }
            set
            {
                if (this.Model == null)
                    return;

                this.CheckBox.Checked = value;
                this.Model.Checked = value;
            }
        }

        public PicturePanel()
        {

            this.CheckBox = new CheckBox();
            this.Controls.Add(this.CheckBox);
            this.CheckBox.Height = 20;
            this.CheckBox.Width = 15;
            this.CheckBox.Location = new Point(0, 8);

            this.PictureBoxPanel = new Panel();
            this.PictureBox = new PictureBox();

            this.CheckBox.Click += CheckBox_Click;

            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();

            this.Controls.Add(this.PictureBoxPanel);
            this.PictureBoxPanel.Controls.Add(this.PictureBox);

            this.PictureBoxPanel.Dock = DockStyle.Fill;
            this.PictureBox.Dock = DockStyle.Fill;

            PictureBox.Click += PictureBox_Click;
            PictureBox.MouseHover += PictureBox_MouseHover;

            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
        }

        public void SetCheckPoint(int x, int y)
        {
            this.CheckBox.Location = new Point(x, y);
        }

        private void PictureBox_MouseHover(object sender, EventArgs e)
        {
            this.OnPictureMouseHover?.Invoke(this);
        }

        private void CheckBox_Click(object sender, EventArgs e)
        {
            this.OnPanelClick?.Invoke(this);
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            this.Checked = !this.Checked;
            this.OnPanelClick?.Invoke(this);
        }
    }
}
