using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SO_Projektna_Naloga_AHP
{
    public partial class Form1 : Form
    {
        int števecEna = 1;
        int števecDva = 1;
        int števecTri = 1;
        int števecŠtiri = 0;
        public static List<TreeNodeCollection> seznamStaršev = new List<TreeNodeCollection>();
        public static List<string> seznamNodov = new List<string>();
        public static List<TreeNode> seznamOtrok = new List<TreeNode>();
        public static List<TreeNode> seznamVseh = new List<TreeNode>();
        public static Dictionary<string, double> seznamUteži = new Dictionary<string, double>();
        public static List<double> seznamSamoUteži = new List<double>();
        public static Dictionary<string, List<double>> seznamKoristnosti = new Dictionary<string, List<double>>();
        public static Dictionary<string, int> seznamGlobin = new Dictionary<string, int>();
        public static bool PreveriVnos(double value)
        {
            if (value < 1 || value > 9)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void ShraniUteži(Dictionary<string, double> seznamUteži, DataGridView grid)
        {
            double temp = 0;
            for (int j = 0; j < grid.Columns.Count - 1; j++)
            {
                temp = Convert.ToDouble(grid.Rows[j].Cells[grid.Columns.Count - 1].Value);
                seznamUteži.Add(grid.Rows[j].HeaderCell.Value.ToString(), Convert.ToDouble(grid.Rows[j].Cells[grid.Columns.Count - 1].Value));
            }
        }
        public static void PrimerjavaPoParih(DataGridView grid)
        {
            double rezultat = 0;
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    if (grid.Rows[i].Cells[j].Value != null)
                    {
                        /*if (Convert.ToDouble(grid.Rows[i].Cells[j].Value) != 1)
                        {*/
                        rezultat = 1 / Convert.ToDouble(grid.Rows[i].Cells[j].Value);

                        if (rezultat < 1)
                        {
                            grid.Rows[j].Cells[i].Value = Math.Round(rezultat, 2);
                        }
                        else
                        {
                            grid.Rows[j].Cells[i].Value = (int)rezultat;
                        }
                    }
                }
            }
        }
        public static void NormaliziranaTabela(DataGridView grid, DataGridView gridDva)
        {
            double temp = 0;
            //var števec = 0;
            for (int i = 0; i < grid.Rows.Count - 1; i++)
            {
                gridDva.Columns.Add(grid.Columns[i].HeaderText, grid.Columns[i].HeaderText);
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    try
                    {
                        gridDva.Rows.Add();
                        temp = Convert.ToDouble(grid.Rows[j].Cells[i].Value) / Convert.ToDouble(grid.Rows[grid.Rows.Count - 1].Cells[i].Value);
                        gridDva.Rows[i].HeaderCell.Value = grid.Columns[i].HeaderText;
                        gridDva.Rows[j].Cells[i].Value = Math.Round(temp, 2);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                temp = 0;
            }
            for (int i = gridDva.Rows.Count - 1; i > -1; i--)
            {
                DataGridViewRow row = gridDva.Rows[i];
                if (!row.IsNewRow && row.Cells[0].Value == null)
                {
                    gridDva.Rows.RemoveAt(i);
                }
            }
        }
        public static List<double> SeštejStolpce(DataGridView grid)
        {
            grid.Rows.Add();
            List<double> skupaj = new List<double>();
            double seštevek = 0;
            for (int i = 0; i < grid.Rows.Count - 1; i++)
            {
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    seštevek += Convert.ToDouble(grid.Rows[j].Cells[i].Value);

                }
                grid.Rows[grid.Rows.Count - 1].Cells[i].Value = seštevek;
                grid.Rows[grid.Rows.Count - 1].Cells[i].Style.BackColor = Color.LightGreen;
                skupaj.Add(seštevek);
                seštevek = 0;
            }
            grid.Rows[grid.Rows.Count - 1].HeaderCell.Value = "Skupaj";
            return skupaj;
        }
        public static void IzračunajUteži(DataGridView grid)
        {
            grid.Columns.Add("Utež", "Utež");
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                double seštevek = 0;
                for (int j = 0; j < grid.Columns.Count - 1; j++)
                {
                    seštevek += Convert.ToDouble(grid.Rows[i].Cells[j].Value);
                }
                grid.Rows[i].Cells[grid.Columns.Count - 1].Value = Math.Round((seštevek / (grid.Columns.Count - 1)), 2); //Ni vedno 1
                grid.Rows[i].Cells[grid.Columns.Count - 1].Style.BackColor = Color.LightBlue;
            }
        }
        public static void IzračunajKoristnostZaAlternative(DataGridView grid)
        {
            grid.Columns.Add("Koristnost", "Koristnost");
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                double seštevek = 0;
                for (int j = 0; j < grid.Columns.Count - 1; j++)
                {
                    seštevek += Convert.ToDouble(grid.Rows[i].Cells[j].Value);
                }
                grid.Rows[i].Cells[grid.Columns.Count - 1].Value = Math.Round((seštevek / (grid.Columns.Count - 1)), 2); //Ni vedno 1
                grid.Rows[i].Cells[grid.Columns.Count - 1].Style.BackColor = Color.LightBlue;
            }
        }
        public static void PridobiSamoUtežiIzDictionarya(Dictionary<string, double> seznamUteži)
        {
            seznamSamoUteži.Clear();
            foreach (KeyValuePair<string, double> x in seznamUteži)
            {
                seznamSamoUteži.Add(x.Value);
            }
        }
        public static bool ImaOtroke(TreeNode node)
        {
            if (node.Nodes.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void PreberiDrevoZaStarše(TreeNodeCollection nodes, int globina)
        {
            globina++;
            foreach (TreeNode node in nodes)
            {
                if (ImaOtroke(node))
                {
                    seznamStaršev.Add(node.Nodes);
                    seznamGlobin.Add(node.Text, globina);
                    seznamNodov.Add(node.Text);
                    PreberiDrevoZaStarše(node.Nodes, globina);
                }
            }
        }
        public static void PreberiDrevoZaOtroke(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (!ImaOtroke(node))
                {
                    seznamOtrok.Add(node);
                    PreberiDrevoZaOtroke(node.Nodes);
                }
                else
                {
                    PreberiDrevoZaOtroke(node.Nodes);
                }
            }
        }
        public static void PreberiSeznam(ListView seznam, DataGridView grid)
        {
            var i = 0;
            foreach (ListViewItem x in seznam.Items)
            {
                grid.Columns.Add(x.Text, x.Text);
                grid.Rows.Add();
                grid.Rows[i].HeaderCell.Value = x.Text;
                grid.Rows[i].Cells[i].Value = 1;
                grid.Rows[i].Cells[i].ReadOnly = true;
                grid.Rows[i].Cells[i].Style.BackColor = Color.PapayaWhip;
                i++;
            }
            grid.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
        }
        public static void PreberiKoristnosti(DataGridView grid)
        {
            double temp = 0;
            List<double> celice = new List<double>();
            for (int j = 0; j < grid.Columns.Count - 1; j++)
            {
                temp = Convert.ToDouble(grid.Rows[j].Cells[grid.Columns.Count - 1].Value);
                celice.Add(temp);
            }
            seznamKoristnosti.Add(grid.TopLeftHeaderCell.Value.ToString(), celice);
        }
        //Testing purposes
        public static void PreberiCeloDrevo(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                seznamVseh.Add(node);
                PreberiCeloDrevo(node.Nodes);
            }
        }
        public Form1()
        {
            InitializeComponent();
            TreeView.Nodes.Add("Cilj");
            dataGridView2.AllowUserToOrderColumns = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView3.AllowUserToOrderColumns = false;
            dataGridView3.AllowUserToResizeColumns = false;
            dataGridView5.AllowUserToOrderColumns = false;
            dataGridView5.AllowUserToResizeColumns = false;
            dataGridView3.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView4.AllowUserToAddRows = false;
            dataGridView5.AllowUserToAddRows = false;
            dataGridView5.ReadOnly = true;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        private void button1_Click(object sender, EventArgs e) //Dodaj node
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                if (TreeView.SelectedNode != null)
                {
                    try
                    {
                        TreeNode node = TreeView.SelectedNode;
                        var newNode = new TreeNode();
                        newNode.Text = textBox1.Text;
                        node.Nodes.Add(newNode);
                        node.Expand();
                        textBox1.Clear();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    MessageBox.Show("Izberi node!");
                }
            }
            else
            {
                MessageBox.Show("Vpiši ime parametra!");
            }
        }

        private void button2_Click(object sender, EventArgs e) //Dodaj alternativo
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                try
                {
                    listView1.Items.Add(textBox2.Text);
                    textBox2.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                MessageBox.Show("Vpiši ime alternative!");
            }
        }

        private void button3_Click(object sender, EventArgs e) //Naprej
        {
            tabControl1.SelectTab(1);
        }

        private void button4_Click(object sender, EventArgs e) //Naprej #2
        {
            tabControl1.SelectTab(2);
        }

        private void NaložiTabelo(object sender, EventArgs e)
        {
            seznamStaršev.Clear();
            števecEna = 1;
            PreberiDrevoZaStarše(TreeView.Nodes, 0);
            var i = 0;
            foreach (TreeNode x in seznamStaršev[0])
            {
                dataGridView2.Columns.Add(x.Text, x.Text);
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].HeaderCell.Value = x.Text;
                dataGridView2.Rows[i].Cells[i].Value = 1;
                dataGridView2.Rows[i].Cells[i].ReadOnly = true;
                dataGridView2.Rows[i].Cells[i].Style.BackColor = Color.PapayaWhip;
                i++;
            }
            dataGridView2.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
        }

        private void button6_Click(object sender, EventArgs e) //Ponastavi za parametre
        {
            seznamSamoUteži.Clear();
            seznamGlobin.Clear();
            seznamNodov.Clear();
            seznamUteži.Clear();
            števecEna = 1;
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
            dataGridView3.Update();
            dataGridView3.Refresh();
            dataGridView2.Update();
            dataGridView2.Refresh();
        }

        private void button7_Click(object sender, EventArgs e) //Odstrani iz drevesa
        {
            if (TreeView.SelectedNode != null)
            {
                if (TreeView.SelectedNode.Text == "Cilj")
                {
                    MessageBox.Show("Korena ni možno odstraniti!");
                }
                else
                {
                    TreeNode node = TreeView.SelectedNode;
                    node.Remove();
                }
            }
            else
            {
                MessageBox.Show("Izberi parameter!");
            }
        }

        private void button8_Click(object sender, EventArgs e) //Izračun za parametre
        {
            PrimerjavaPoParih(dataGridView2);
            SeštejStolpce(dataGridView2);
            NormaliziranaTabela(dataGridView2, dataGridView3);
            IzračunajUteži(dataGridView3);
            ShraniUteži(seznamUteži, dataGridView3);
        }

        private void VnosVCelico(object sender, DataGridViewCellEventArgs e)
        {
            if (!PreveriVnos(Convert.ToDouble(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)))
            {
                MessageBox.Show("Vneseno število ne sme biti manjše od 1 ali večje od 9");
                dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.OrangeRed;
                dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                dataGridView2.Rows[e.ColumnIndex].Cells[e.RowIndex].Value = 0;
            }
            else
            {
                dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
        }

        private void button10_Click(object sender, EventArgs e) //Naslednje za parametre
        {
            if (!(števecEna >= seznamStaršev.Count))
            {
                dataGridView3.Columns.Clear();
                dataGridView3.Rows.Clear();
                dataGridView3.Update();
                dataGridView3.Refresh();
                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                dataGridView2.Update();
                dataGridView2.Refresh();
                var j = 0;
                for (int i = števecEna; i <= števecEna; i++)
                {
                    foreach (TreeNode x in seznamStaršev[i])
                    {
                        dataGridView2.Columns.Add(x.Text, x.Text);
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[j].HeaderCell.Value = x.Text;
                        dataGridView2.Rows[j].Cells[j].Value = 1;
                        dataGridView2.Rows[j].Cells[j].ReadOnly = true;
                        dataGridView2.Rows[j].Cells[j].Style.BackColor = Color.PapayaWhip;
                        j++;
                    }
                }
                števecEna++;
                dataGridView2.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
            }
            else
            {
                MessageBox.Show("Konec starš-parametrov!");
            }
        }

        private void button12_Click(object sender, EventArgs e) //Naloži tabelo za alternative
        {
            seznamOtrok.Clear();
            števecDva = 1;
            PreberiDrevoZaOtroke(TreeView.Nodes);
            var i = 0;
            dataGridView4.TopLeftHeaderCell.Value = seznamOtrok[0].Text;
            dataGridView5.TopLeftHeaderCell.Value = seznamOtrok[0].Text;
            foreach (ListViewItem x in listView1.Items)
            {
                dataGridView4.Columns.Add(x.Text, x.Text);
                dataGridView4.Rows.Add();
                dataGridView4.Rows[i].HeaderCell.Value = x.Text;
                dataGridView4.Rows[i].Cells[i].Value = 1;
                dataGridView4.Rows[i].Cells[i].ReadOnly = true;
                dataGridView4.Rows[i].Cells[i].Style.BackColor = Color.PapayaWhip;
                i++;
            }
            dataGridView4.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
        }

        private void button11_Click(object sender, EventArgs e) //Odstrani alternativo iz seznama
        {
            listView1.SelectedItems[0].Remove();
        }

        private void button13_Click(object sender, EventArgs e) //Izračun za alternative
        {
            PrimerjavaPoParih(dataGridView4);
            SeštejStolpce(dataGridView4);
            NormaliziranaTabela(dataGridView4, dataGridView5);
            IzračunajKoristnostZaAlternative(dataGridView5);
            PreberiKoristnosti(dataGridView5);
        }

        private void button14_Click(object sender, EventArgs e) //Ponastavi za alternative
        {
            števecDva = 1;
            števecTri = 1;
            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            dataGridView5.Rows.Clear();
            dataGridView5.Columns.Clear();
            dataGridView5.Update();
            dataGridView5.Refresh();
            dataGridView4.Update();
            dataGridView4.Refresh();
            seznamKoristnosti.Clear();
        }

        private void button15_Click(object sender, EventArgs e) //Naslednje za alternative
        {
            if (!(števecDva >= seznamOtrok.Count))
            {
                dataGridView4.Columns.Clear();
                dataGridView4.Rows.Clear();
                dataGridView4.Update();
                dataGridView4.Refresh();
                dataGridView5.Rows.Clear();
                dataGridView5.Columns.Clear();
                dataGridView5.Update();
                dataGridView5.Refresh();
                dataGridView4.TopLeftHeaderCell.Value = seznamOtrok[števecTri].Text;
                dataGridView5.TopLeftHeaderCell.Value = seznamOtrok[števecTri].Text;
                števecTri++;
                var j = 0;
                for (int i = števecDva; i <= števecDva; i++)
                {
                    foreach (ListViewItem x in listView1.Items)
                    {
                        dataGridView4.Columns.Add(x.Text, x.Text);
                        dataGridView4.Rows.Add();
                        dataGridView4.Rows[j].HeaderCell.Value = x.Text;
                        dataGridView4.Rows[j].Cells[j].Value = 1;
                        dataGridView4.Rows[j].Cells[j].ReadOnly = true;
                        dataGridView4.Rows[j].Cells[j].Style.BackColor = Color.PapayaWhip;
                        j++;
                    }
                }
                števecDva++;
                dataGridView4.Columns.Cast<DataGridViewColumn>().ToList().ForEach(x => x.SortMode = DataGridViewColumnSortMode.NotSortable);
            }
            else
            {
                MessageBox.Show("Konec otrok-parametrov!");

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, double> x in seznamUteži)
            {
                MessageBox.Show("Key: " + x.Key + " Value: " + x.Value);
            }
        }

        private void button16_Click(object sender, EventArgs e) //Kočni izračun
        {
            števecŠtiri = 0;
            seznamVseh.Clear();
            PreberiCeloDrevo(TreeView.Nodes);
            PridobiSamoUtežiIzDictionarya(seznamUteži);
            seznamVseh.RemoveAt(0); //Da odstrani korenski node

            dataGridView1.TopLeftHeaderCell.Value = "Parameter";
            dataGridView1.TopLeftHeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            foreach (ListViewItem x in listView1.Items)
            {
                dataGridView1.Columns.Add(x.Text, x.Text);
            }
            dataGridView1.Columns.Add("Uteži", "Uteži");
            dataGridView1.Columns[dataGridView1.ColumnCount - 1].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            foreach (TreeNode x in seznamVseh)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[števecŠtiri].HeaderCell.Value = x.Text;
                števecŠtiri++;
            }
            dataGridView1.Rows.Add();
            dataGridView1.Rows[dataGridView1.RowCount - 1].HeaderCell.Value = "Ocena";
            dataGridView1.Rows[dataGridView1.RowCount - 1].HeaderCell.Style.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);

            for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
            {
                foreach (var item in seznamUteži)
                {
                    if (dataGridView1.Rows[j].HeaderCell.Value.ToString() == item.Key)
                    {
                        dataGridView1.Rows[j].Cells[dataGridView1.Columns.Count - 1].Value = item.Value.ToString();
                    }
                }
            }

            foreach (KeyValuePair<string, List<double>> x in seznamKoristnosti)
            {
                var n = 0;
                for (int l = 0; l < dataGridView1.RowCount - 1; l++)
                {
                    var m = 0;
                    if (x.Key == dataGridView1.Rows[l].HeaderCell.Value.ToString())
                    {
                        foreach (double y in x.Value)
                        {
                            dataGridView1.Rows[n].Cells[m].Value = y.ToString();
                            m++;
                        }
                    }
                    n++;
                }
            }
            for (int i = 0, j = 1; i < dataGridView1.RowCount - 1; i++)
            {
                if (seznamNodov.Contains(dataGridView1.Rows[i].HeaderCell.Value.ToString()))
                {
                    for (int m = 0; m < dataGridView1.ColumnCount - 1; m++)
                    {
                        double temp = 0;
                        for (int p = 0; p < seznamStaršev[j].Count; p++)
                        {
                            temp += Convert.ToDouble(dataGridView1.Rows[i + p + 1].Cells[m].Value) * Convert.ToDouble(dataGridView1.Rows[i + p + 1].Cells[dataGridView1.ColumnCount - 1].Value);
                        }
                        dataGridView1.Rows[i].Cells[m].Value = Math.Round(temp, 2);
                        dataGridView1.Rows[i].Cells[m].Style.BackColor = Color.LightCoral;
                    }
                    j++;
                }
            }
            List<string> seznamNodovTemp = new List<string>();

            foreach (TreeNode node in TreeView.Nodes[0].Nodes)
            {
                seznamNodovTemp.Add(node.Text);
            }

            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                double temp = 0;
                for (int j = 0; j < dataGridView1.RowCount - 1; j++)
                {
                    if (seznamNodovTemp.Contains(dataGridView1.Rows[j].HeaderCell.Value.ToString()))
                    {
                        temp += Convert.ToDouble(dataGridView1.Rows[j].Cells[i].Value) * Convert.ToDouble(dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 1].Value);
                    }
                }
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[i].Value = Math.Round(temp, 2);
            }
            //Ocena
            double max = 0;

            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                if (Convert.ToDouble(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[i].Value) > max)
                {
                    max = Convert.ToDouble(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[i].Value);
                }
            }
            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                if (Convert.ToDouble(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[i].Value) == max)
                {
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[i].Style.BackColor = Color.LightGreen;
                    break;
                }
            }
        }
    }
}
