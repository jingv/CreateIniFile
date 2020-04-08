﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Media.Effects;
using System.Windows.Xps.Serialization;
using System.Windows.Media.Animation;

namespace CreateIniFile
{
    class IniFileHandler
    {
        /// <summary>
        /// 默认引用的文件
        /// </summary>
        private static string filePath;

        #region 引用DLL文件
        /// <summary>
        /// 为INI文件中指定的节点取得字符串
        /// </summary>
        /// <param name="lpSectionName">欲在其中查找关键字的节点名称</param>
        /// <param name="lpKeyName">欲获取的项名</param>
        /// <param name="lpDefault">指定的项没有找到时返回的默认值</param>
        /// <param name="lpReturnedString">指定一个字串缓冲区，长度至少为nSize</param>
        /// <param name="nSize">指定装载到lpReturnedString缓冲区的最大字符数量</param>
        /// <param name="lpFileName">INI文件完整路径</param>
        /// <returns>复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符</returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpSectionName, string lpKeyName, string lpDefalut, StringBuilder lpReturnedString, int nSize, string lpFileName);

        [DllImport("kernel32")]
        private static extern uint GetPrivateProfileString(string lpSectionName, string koKeyName, string lpDefault, Byte[] retVal, int nSize, string lpFileName);

        /// <summary>
        /// 修改INI文件中内容
        /// </summary>
        /// <param name="lpApplicationName">欲在其中写入的节点名称</param>
        /// <param name="lpKeyName">欲设置的项名</param>
        /// <param name="lpString">要写入的新字符串</param>
        /// <param name="lpFileName">INI文件完整路径</param>
        /// <returns>非零表示成功，零表示失败</returns>
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpStirng, string lpFileName);

        #endregion

        /// <summary>
        /// 拷贝Default模板文件到新建默认文件YourIniFile（可操作）
        /// </summary>
        /// <returns>返回新文件路径</returns>
        public static string CopyFile()
        {
            string SourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Default.ini");
            if (!File.Exists(SourceFile))
            {
                MessageBox.Show("ERR: The Defalut File Does Not Exist!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            else
            {
                string NewFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DateTime.Now.ToFileTime()}.ini");
                File.Copy(SourceFile, NewFilePath);
                //将copy的副本文件路径赋值给默认问价路径，默认接下来的操作都是在此文件上进行的
                filePath = NewFilePath;
                return NewFilePath;
            }
        }

        /// <summary>
        /// 读取文件中所有的Section的值
        /// </summary>
        /// <param name="filePath">从filePath中读取所有的Section</param>
        /// <returns>以字符串数组的形式返回所有的seciton</returns>
        public static List<string> ReadSections(string filePath)
        {
            List<string> result = new List<string>();
            Byte[] buf = new byte[65536];
            uint len = GetPrivateProfileString(null, null, null, buf, 65536, filePath);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }
        public static List<string> ReadSections() => ReadSections(filePath);

        /// <summary>
        /// 读取指定section中所有的key的值
        /// </summary>
        /// <param name="section">指定的section</param>
        /// <param name="filePath">指定的文件</param>
        /// <returns>List<string>所有的key的值</returns>
        public static List<string> ReadKeys(string section, string filePath)
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileString(section, null, null, buf, 65536, filePath);
            int j = 0;
            for(int i = 0; i < len; i++)
                if(buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }
        public static List<string> ReadKeys(string section) => ReadKeys(section, filePath);

        /// <summary>
        /// 读取INI文件指定的key值
        /// </summary>
        /// <param name="section">节点名</param>
        /// <param name="key">键</param>
        /// <param name="def">未取到值时返回的默认值</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>读取的值</returns>
        public static string ReadValue(string section, string key, string def, string filePath)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, stringBuilder, 1024, filePath);
            return stringBuilder.ToString();
        }
        public static string ReadValue(string section, string key) => ReadValue(section, key, "", filePath);

        /// <summary>
        /// 写INI文件值
        /// </summary>
        /// <param name="section">欲在其中写入的节点名称</param>
        /// <param name="key">欲设置的项名</param>
        /// <param name="value">要写入的新字符串</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>非零表示成功，零表示失败</returns>
        public static int Write(string section, string key, string value, string filePath)
        {
            return WritePrivateProfileString(section, key, value, filePath);
        }
        public static int Write(string section, string key, string value) => Write(section, key, value, filePath);

        /// <summary>
        /// 删除节
        /// </summary>
        /// <param name="section">节点名</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>非零表示成功，零表示失败</returns>
        public static int DeleteSection(string section, string filePath)
        {
            return Write(section, null, null, filePath);
        }
        public static int DeleteSection(string section) => DeleteSection(section, filePath);

        /// <summary>
        /// 删除键的值
        /// </summary>
        /// <param name="section">节点名</param>
        /// <param name="key">键名</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>非零表示成功，零表示失败</returns>
        public static int DeleteKey(string section, string key, string filePath)
        {
            return Write(section, key, null, filePath);
        }
        public static int DeleteKey(string section, string key) => DeleteKey(section, key, filePath);
    }
}
