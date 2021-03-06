﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Speech
{

    public class AITalk3Enumerator : VoiceroidPlusEnumerator
    {

        public AITalk3Enumerator() : base()
        {
            Initialize();
        }

        private void Initialize()
        {
            EngineName = "AITalk3";
            List<Data> voiceData = new List<Data>();
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\AI\AITalk3";
            if (Directory.Exists(basePath))
            {
                string[] dirs = Directory.GetDirectories(basePath);
                foreach (var d in dirs)
                {
                    voiceData.AddRange(FindAITalk(d));
                }
            }
            _info = voiceData.ToArray();
        }

        private List<Data> FindAITalk(string path)
        {
            List<Data> data = new List<Data>();
            try
            {
                string[] dirs = Directory.GetDirectories(Path.Combine(path, "voice"));
                Array.Sort(dirs); // AITalkの話者の表示順序は英語フォルダ名の辞書式順序

                for (int i = 0; i < dirs.Length; i++)
                {
                    string xmlFile = Path.Combine(dirs[i], "dbconf.xml");
                    if (File.Exists(xmlFile))
                    {
                        Data d = new Data();
                        var xml = XElement.Load(Path.Combine(xmlFile));
                        d.Name = xml.Element("profile").Attribute("name").Value;
                        if (path.IndexOf("AITalkLite") > 0)
                        {
                            d.Name += "(LITE)";
                        }
                        d.Path = Directory.GetFiles(path,"*.exe")[0];
                        data.Add(d);
                    }
                }
            }
            catch
            {
                // 初期化に途中で失敗した場合はうまく処理できたところまで返す
            }

            return data;
        }
        public override ISpeechController GetControllerInstance(SpeechEngineInfo info)
        {
            return EngineName == info.EngineName ? new AITalk3Controller(info) : null;
        }


    }
}
