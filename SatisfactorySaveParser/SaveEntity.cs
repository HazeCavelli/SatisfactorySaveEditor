﻿using SatisfactorySaveParser.Fields;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser
{
    public class SaveEntity : SaveEntry
    {
        public int Int4 { get; set; }
        public byte[] Unknown5 { get; set; }
        public int Int6 { get; set; }
        public int Int7 { get; set; }

        public string DataStr1 { get; set; }
        public string DataStr2 { get; set; }
        public int DataInt3 { get; set; }
        public List<(string, string)> DataList4 { get; set; } = new List<(string, string)>();

        public SaveEntity(BinaryReader reader) : base(reader)
        {
            Int4 = reader.ReadInt32();
            Unknown5 = reader.ReadBytes(0x28);
            Int6 = reader.ReadInt32();
            Int7 = reader.ReadInt32();
        }

        public override void ParseData(int length, BinaryReader reader)
        {
            var newLen = length - 12;
            DataStr1 = reader.ReadLengthPrefixedString();
            if (DataStr1.Length > 0)
                newLen -= DataStr1.Length + 1;

            DataStr2 = reader.ReadLengthPrefixedString();
            if (DataStr2.Length > 0)
                newLen -= DataStr2.Length + 1;

            DataInt3 = reader.ReadInt32();
            for (int i = 0; i < DataInt3; i++)
            {
                var str1 = reader.ReadLengthPrefixedString();
                var str2 = reader.ReadLengthPrefixedString();
                DataList4.Add((str1, str2));
                newLen -= 10 + str1.Length + str2.Length;
            }

            base.ParseData(newLen, reader);
        }
    }
}
