using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiredSolver;

namespace testForm
{
    public partial class Form1 : Form
    {

        WiredSolverInstance wireSolver = new WiredSolverInstance();
        Dictionary<Point, TestWire> yaaaay = new Dictionary<Point, TestWire>(); //List of items at point T
        WireCurrentTransfer selectedTranser = WireCurrentTransfer.NONE;
        CurrentType newType = CurrentType.OFF;
        private bool removeWire;

        public Form1()
        {
            InitializeComponent();
            generateButtons();
        }

        private void generateButtons()
        {
            TestWire leWire;
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0; y < 14; y++)
                {
                    leWire = new TestWire(x, y);
                    leWire.Click += new EventHandler(leWire_Click);
                    this.Controls.Add(leWire);
                    yaaaay.Add(leWire.LocationPoint, leWire);
                }
            }
        }


        internal void Remove(int x, int y)
        {
            List<Point> result = null;
            result = wireSolver.RemoveWire(x, y);
            foreach (Point t in result)
            {
                try
                {
                    yaaaay[t].updateWireState(wireSolver.getWireTransfer(t));
                }
                catch { }
            }
            StringBuilder sb = new StringBuilder();
            foreach (Point t in result)
            {
                sb.AppendLine(t.ToString());
            }
        }

        internal void AddOrUpdateWire(int x, int y, CurrentType type, WireCurrentTransfer transfer)
        {
            List<Point> result = null;
            result = wireSolver.AddOrUpdateWire(x, y, type, transfer);
            foreach (Point t in result)
            {
                try
                {
                    yaaaay[t].updateWireState(wireSolver.getWireTransfer(t));
                }
                catch { }
            }
            StringBuilder sb = new StringBuilder();
            foreach (Point t in result)
            {
                sb.AppendLine(t.ToString());
            }
        }

        void leWire_Click(object sender, EventArgs e)
        {
           TestWire origin = sender as TestWire;
           List<Point> result = null;
           if (removeWire)
           {
               result = wireSolver.RemoveWire(origin.LocationPoint.X, origin.LocationPoint.Y);
           }
           else
           {
               result = wireSolver.AddOrUpdateWire(origin.LocationPoint.X, origin.LocationPoint.Y, this.newType, this.selectedTranser);
           }
           foreach (Point t in result)
           {
               try
               {
                   yaaaay[t].updateWireState(wireSolver.getWireTransfer(t));
               }
               catch { }
           }
           StringBuilder sb = new StringBuilder();
           foreach (Point t in result)
           {
               sb.AppendLine( t.ToString());
           }
           //MessageBox.Show("changed items: " + result.Count + "\r\n" + sb.ToString());
        }


        
    }
}
