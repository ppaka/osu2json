using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OsuParsers;

namespace osu2json
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string getPath()
        {
            var dialog = openFileDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = getPath();
            textBox1.Text = path ?? textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = getPath();
            textBox2.Text = path ?? textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool text1_is_null = false, text2_is_null = false;
            Level level = new Level();
            if (textBox1.Text == "")
            {
                text1_is_null = true;
            }
            if (textBox2.Text == "")
            {
                text2_is_null = true;
            }

            if (text1_is_null && text2_is_null)
            {
                MessageBox.Show("선택한 파일이 없습니다", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!text1_is_null)
            {
                var info = new Info();
                var key4Lv = OsuParsers.Decoders.BeatmapDecoder.Decode(textBox1.Text);
                info.title = key4Lv.MetadataSection.Title;
                info.artist = key4Lv.MetadataSection.Artist;
                info.creator = key4Lv.MetadataSection.Creator;
                info.difficultyName = key4Lv.MetadataSection.Version;
                info.difficultyLevel = 1;
                info.audioPath = key4Lv.GeneralSection.AudioFilename ?? "";
                info.audioPreviewTime = key4Lv.GeneralSection.PreviewTime;
                info.videoPath = key4Lv.EventsSection.Video ?? "";
                info.backgroundImagePath = key4Lv.EventsSection.BackgroundImage ?? "";
                info.level4KFilePath = Path.GetFileName(textBox1.Text);
                if (!text2_is_null) info.level9KFilePath = Path.GetFileName(textBox2.Text);
                level.info = info;

                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(level, settings);
                var dialog = saveFileDialog1.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, jsonData);
                    MessageBox.Show("변환 성공", "작업 알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                var info = new Info();
                var key9Lv = OsuParsers.Decoders.BeatmapDecoder.Decode(textBox2.Text);
                info.title = key9Lv.MetadataSection.Title;
                info.artist = key9Lv.MetadataSection.Artist;
                info.creator = key9Lv.MetadataSection.Creator;
                info.difficultyName = key9Lv.MetadataSection.Version;
                info.difficultyLevel = 1;
                info.audioPath = key9Lv.GeneralSection.AudioFilename ?? "";
                info.audioPreviewTime = key9Lv.GeneralSection.PreviewTime;
                info.videoPath = key9Lv.EventsSection.Video ?? "";
                info.backgroundImagePath = key9Lv.EventsSection.BackgroundImage ?? "";
                info.level9KFilePath = Path.GetFileName(textBox2.Text);
                level.info = info;

                var settings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };
                var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(level, settings);
                var dialog = saveFileDialog1.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, jsonData);
                    MessageBox.Show("변환 성공", "작업 알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
