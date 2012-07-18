using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SC2_External_Hack_II
{
    public partial class AdjustPanelPosition : Form
    {
        public AdjustPanelPosition(Configure conf, string Panels)
        {
            _cfg = conf;
            _sPnl = Panels;
            InitializeComponent();
        }

        private Configure _cfg = null;
        private string _sPnl = "";

        //DLL Import to catch the break
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vkey);

        //Timer Tick
        private void TmrRefreshTick(object sender, EventArgs e)
        {
            this.TopMost = true;
            Location = Cursor.Position;

            if (GetAsyncKeyState(Keys.Enter) == -32767 || GetAsyncKeyState(Keys.Enter) == 1)
            {
                tmrRefresh.Enabled = false;

                switch (_sPnl)
                {
                    case "Ressources":
                        _cfg._asEinstellungen.Location_Ressource = Location;
                        break;

                    case "Income":
                        _cfg._asEinstellungen.Location_Income = Location;
                        break;

                    case "Worker":
                        _cfg._asEinstellungen.Location_Worker = Location;
                        break;

                    case "States":
                        _cfg._asEinstellungen.Location_Information = Location;
                        break;

                    case "Maphack":
                        _cfg._asEinstellungen.Location_Maphack = Location;
                        break;

                    case "Units":
                        _cfg._asEinstellungen.Location_Production = Location;
                        break;

                    default:
                        _cfg._asEinstellungen.Location_APM = Location;
                        break;
                }

                _cfg.Show();
                this.Close();
            }
        }

        //Make the form unclickable
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        private void AdjustPanelPositionLoad(object sender, EventArgs e)
        {
            _cfg.Hide();
            tmrRefresh.Enabled = true;

            //Adjust the Width so the user has an idea about what he's changing
            switch (_sPnl)
            {
                case "Ressources":
                    Width = 583;
                    break;

                case "Income":
                    Width = 707;
                    break;

                case "Worker":
                    Width = 200;
                    break;

                case "States":
                    Width = 360;
                    break;

                case "Maphack":
                    Width = 262;
                    break;

                case "Units":
                    Width = 578;
                    break;

                case "Army":
                    Width = 583;
                    break;

                case "APM":
                    Width = 300;
                    break;

                default:
                    Width = 288;
                    break;
            }
        }

    }
}
