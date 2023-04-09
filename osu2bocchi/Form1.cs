using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OsuParsers.Beatmaps.Objects;

namespace osu2bocchi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string GetPath()
        {
            var dialog = openFileDialog1.ShowDialog();
            return dialog == DialogResult.OK ? openFileDialog1.FileName : null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = GetPath();
            textBox1.Text = path ?? textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var text1IsNull = false;
            var level = new Level();
            if (textBox1.Text == "")
            {
                text1IsNull = true;
            }

            if (text1IsNull)
            {
                MessageBox.Show("선택한 파일이 없습니다", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var beatmap = OsuParsers.Decoders.BeatmapDecoder.Decode(textBox1.Text);
            beatmap.HitObjects.ForEach(x =>
            {
                if (x.GetType() == typeof(Spinner))
                {
                    var noteData = new NoteData
                    {
                        startTime = x.StartTime,
                        y = 0.5f
                    };
                    level.notes.Add(noteData);
                }
                else
                {
                    var noteData = new NoteData
                    {
                        type = 0,
                        direction = 0
                    };

                    var lane = 2;
                    if (!checkBox1.Checked)
                    {
                        lane = (int)Math.Floor(x.Position.X * 5 / 512f);
                        if (lane < 0) lane = 0;
                        else if (lane > 4) lane = 4;

                        switch (lane)
                        {
                            case 2:
                                noteData.type = 0;
                                noteData.direction = 0;
                                break;
                            case 1:
                                noteData.type = 1;
                                noteData.direction = -1;
                                break;
                            case 3:
                                noteData.type = 1;
                                noteData.direction = 1;
                                break;
                            case 0:
                                noteData.type = 2;
                                noteData.direction = -1;
                                break;
                            case 4:
                                noteData.type = 2;
                                noteData.direction = 1;
                                break;
                        }
                    }

                    noteData.startTime = x.StartTime * 0.001f;
                    noteData.y = 1f - x.Position.Y / 384f;
                    level.notes.Add(noteData);

                    if (x.StartTime >= x.EndTime) return;
                    if (x.GetType() != typeof(Slider)) return;
                    var slider = (Slider)x;
                    var singleRepeatTime = (float)x.TotalTimeSpan.TotalMilliseconds / slider.Repeats * 0.001f;

                    var lastLane = 2;
                    if (!checkBox1.Checked) lastLane = (int)Math.Floor(slider.SliderPoints.First().X * 5 / 512f);
                    if (lastLane < 0) lastLane = 0;
                    else if (lastLane > 4) lastLane = 4;
                    var lastPoint = 1f - slider.SliderPoints.First().Y / 384f;

                    if (slider.Repeats != 0)
                    {
                        for (var i = 1; i < slider.Repeats; i++)
                        {
                            var noteData2 = new NoteData();
                            int lane2;
                            if (i % 2 == 0)
                            {
                                noteData2.y = noteData.y;
                                lane2 = lane;
                            }
                            else
                            {
                                noteData2.y = lastPoint;
                                lane2 = lastLane;
                            }
                            noteData2.startTime = noteData.startTime + singleRepeatTime * i;

                            switch (lane2)
                            {
                                case 2:
                                    noteData2.type = 0;
                                    noteData2.direction = 0;
                                    break;
                                case 1:
                                    noteData2.type = 1;
                                    noteData2.direction = -1;
                                    break;
                                case 3:
                                    noteData2.type = 1;
                                    noteData2.direction = 1;
                                    break;
                                case 0:
                                    noteData2.type = 2;
                                    noteData2.direction = -1;
                                    break;
                                case 4:
                                    noteData2.type = 2;
                                    noteData2.direction = 1;
                                    break;
                            }

                            level.notes.Add(noteData2);
                        }
                    }
                    
                    var noteData3 = new NoteData();
                    int lane3;

                    if (slider.Repeats != 0)
                    {
                        if (slider.Repeats % 2 == 0)
                        {
                            noteData3.y = noteData.y;
                            lane3 = lane;
                        }
                        else
                        {
                            noteData3.y = lastPoint;
                            lane3 = lastLane;
                        }
                    }
                    else
                    {
                        noteData3.y = lastPoint;
                        lane3 = lastLane;
                    }

                    switch (lane3)
                    {
                        case 2:
                            noteData3.type = 0;
                            noteData3.direction = 0;
                            break;
                        case 1:
                            noteData3.type = 1;
                            noteData3.direction = -1;
                            break;
                        case 3:
                            noteData3.type = 1;
                            noteData3.direction = 1;
                            break;
                        case 0:
                            noteData3.type = 2;
                            noteData3.direction = -1;
                            break;
                        case 4:
                            noteData3.type = 2;
                            noteData3.direction = 1;
                            break;
                    }

                    noteData3.startTime = x.EndTime * 0.001f;
                    level.notes.Add(noteData3);
                }
            });

            level.notes = level.notes.OrderBy(x => x.startTime).ToList();
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            };
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(level, settings);
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "JSON 텍스트 파일 (*.json)|*.json";
            var dialog = saveFileDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, jsonData);
                MessageBox.Show("변환 성공", "결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
