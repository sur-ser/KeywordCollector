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
    public class PicturePanelListBox : Control
    {
        public List<PicturePanel> Items { get; } = new List<PicturePanel>();
        public Func<PicturePanel, SnapshotModel, Task> OnItemClick { get; set; }
        public Action<PicturePanel> OnItemMouseHover { get; set; }

        public void CreateItems(int count)
        {
            var width = this.Parent.Width / 2;
            var height = this.Parent.Height / 2;

            for (var i = 0; i < count; i++)
            {
                var panel = new PicturePanel();
                panel.Index = i;
                panel.Size = new Size(width, height);
                this.Parent.Controls.Add(panel);

                if(i < count / 2)
                {
                    panel.Location = new Point(i * width, 0);
                }
                else
                {
                    panel.Location = new Point((i - count/ 2) * width, height);
                }

                if (i == 0)
                {
                    panel.SetCheckPoint(width - 20, height - 20);
                }
                else if(i == 1)
                {
                    panel.SetCheckPoint(8, height - 20);
                }
                else if(i == 2)
                {
                    panel.SetCheckPoint(width - 20, 8);
                }
                else if(i == 3)
                {
                    panel.SetCheckPoint(8, 8);
                }

                Items.Add(panel);

                //数据绑定
                panel.OnPanelClick += p => 
                {
                    p.Model.Checked = p.Checked;
                    OnItemClick?.Invoke(p, p.Model);
                };

                panel.OnPictureMouseHover += p =>
                {
                    this.OnItemMouseHover?.Invoke(p);
                };
            }
        }

        public void UpdateItems(List<SnapshotModel> models)
        {
            for(var i=0;i< models.Count; i++)
            {
                var item = Items[i];
                var model = models[i];
                item.Model = model;
                item.Checked = model.Checked;
                item.ImageFile = model.GetFullName();
                item.Show();
            }

            for(var i=models.Count;i< Items.Count; i++)
            {
                var item = Items[i];
                item.Model = null;
                item.Checked = false;
                item.ImageFile = null;
                item.Hide();
            }
        }
    }
}
