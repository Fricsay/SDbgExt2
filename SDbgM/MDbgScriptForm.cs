﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronRuby;
using Microsoft.Scripting.Hosting;

namespace SDbgM
{
    internal partial class MDbgScriptForm : Form
    {
        private MSDbgExt _ext;
        private ScriptEngine _host;
        private ScriptScope _scope;

        private class Environment 
        {
            private MDbgScriptForm _form;
            public Environment(MDbgScriptForm form)
            {
                _form = form;
            }

            public void Output(object text)
            {
                _form.txtResults.Text += text.ToString() + "\r\n";
            }
        }

        public MDbgScriptForm(MSDbgExt ext)
        {
            _host = Ruby.CreateEngine();
           
            _scope = _host.CreateScope();
            _scope.SetVariable("ext", ext);
            _scope.SetVariable("dbg", new Environment(this));

            _ext = ext;
            InitializeComponent();
        }

        private void AddText(string text)
        {
            txtResults.Text += text + "\r\n";
            txtResults.Select(txtResults.Text.Length - 1, 0);
            txtResults.ScrollToCaret();
        }
        
        private void btExecute_Click(object sender, EventArgs e)
        {
            string code = txtCmd.Text.Trim();
            ScriptSource source = _host.CreateScriptSourceFromString(code, Microsoft.Scripting.SourceCodeKind.AutoDetect);
            
            try
            {
                var ret = source.Execute(_scope);
                if (ret != null)
                {
                    AddText(ret.ToString());
                }
                txtCmd.Text = "";
            }
            catch (Exception ex)
            {
                AddText(ex.ToString());
            }
            
        }
    }
}