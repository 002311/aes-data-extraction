using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace displayInformation
{
    public partial class Form1 : Form
    {

        ListViewColumnSorter lvwColumnSorter;
        int lastSearchLength;


        public Form1()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            listView1.ListViewItemSorter = lvwColumnSorter;
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
        }



        private void loadForm()
        {
            listView1.Clear();

            string connectionString = @"Data Source=DESKTOP-9PQJ6V1\SQLEXPRESS;Initial Catalog=PRIDEMO;Integrated Security=True";

            SqlConnection connection = new SqlConnection(connectionString);


            SqlDataAdapter adapter = new SqlDataAdapter("Select Clientes.Cliente As client, Clientes.Nome As name from Clientes", connectionString);

            DataTable table = new DataTable();

            adapter.Fill(table);

            List<string> clientCodes = new List<string>();
            List<string> clientNames = new List<string>();
            listView1.View = View.Details;
            listView1.Columns.Add("Client Code", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("Client Name", 300, HorizontalAlignment.Left);

            foreach (DataRow row in table.Rows)
            {

                clientCodes.Add(row["client"].ToString());
                clientNames.Add(row["name"].ToString());

            }


            for (int i = 0; i < clientCodes.Count; i++)
            {

                listView1.Items.Add(new ListViewItem
                    (new String[] {clientCodes[i], clientNames[i]}));
            }


        }



        private void Form1_Load(object sender, EventArgs e)
        {
            loadForm();
            lastSearchLength = 0;
            
        }

        


        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            if (e.Column == lvwColumnSorter.SortColumn)
            {

                if (lvwColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {

                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                

                }
                else
                {

                    lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                    
                }
            } else
            {

                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            listView1.Sort();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) &&
                    textBox1.Text.Length > lastSearchLength)
            {
                String searchTxt = textBox1.Text;
                

            foreach (ListViewItem item in listView1.Items)
            {


                    if (item.Text.ToLower().Contains(searchTxt.ToLower()))
                    {
                        item.Selected = true;

                    }
                    else if (item.SubItems[1].Text.ToLower().Contains(searchTxt.ToLower())) {

                        item.Selected = true;

                    }
                    else
                    {
                        listView1.Items.Remove(item);

                    }
                }

                if (listView1.SelectedItems.Count == 1)
                {

                    listView1.Focus();
                } 

                lastSearchLength = textBox1.Text.Length;

            } else if (textBox1.Text.Length < lastSearchLength) {

                
                lastSearchLength = 0;

                loadForm();
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView3.Clear();
            ListView.SelectedListViewItemCollection selected
                = this.listView1.SelectedItems;
            string client = "";

            if (listView1.SelectedItems != null) { 
            foreach (ListViewItem item in selected)
            {
                    client = item.Text;
  
            }


                string connectionString = @"Data Source=DESKTOP-9PQJ6V1\SQLEXPRESS;Initial Catalog=PRIDEMO;Integrated Security=True";

                SqlConnection connection = new SqlConnection(connectionString);


                SqlDataAdapter adapter = new SqlDataAdapter("Select CondPag from Clientes where Cliente = '" + client + "'", connectionString);

                DataTable table = new DataTable();

                adapter.Fill(table);

                List<string> clientKey = new List<string>();

                foreach (DataRow row in table.Rows)
                {

                    clientKey.Add(row["CondPag"].ToString());
                    

                }
                string s = "";
                if (clientKey.Count > 0)
                {
                     s = clientKey[0];
                }
                SqlDataAdapter adapter2 = new SqlDataAdapter("Select Pendentes.ValorPendente, Pendentes.TipoDoc, Pendentes.Serie, Pendentes.NumDocInt, Pendentes.DataDoc," +
                    " Pendentes.DataVenc, Pendentes.ValorTotal, Pendentes.ValorPendente from Pendentes where CondPag = '" + s + "'" , connectionString);
                DataTable table2 = new DataTable();
                adapter2.Fill(table2);

                List<string> pendingValue = new List<string>();
                List<string> serie = new List<string>();
                List<string> number = new List<string>();
                List<string> document = new List<string>();
                List<string> date = new List<string>();
                List<string> dueDate = new List<string>();
                List<string> totalAmount = new List<string>();
                List<string> openAmount = new List<string>();
            
                foreach (DataRow row in table2.Rows)
                {

                    pendingValue.Add(row["ValorPendente"].ToString());
                    serie.Add(row["Serie"].ToString());
                    number.Add(row["NumDocInt"].ToString());
                    document.Add(row["TipoDoc"].ToString());
                    date.Add(row["DataDoc"].ToString());
                    dueDate.Add(row["DataVenc"].ToString());
                    totalAmount.Add(row["ValorTotal"].ToString());
                    openAmount.Add(row["ValorPendente"].ToString());
                    
                                   
                }

                decimal totalvalue = 0;

                foreach (String value in pendingValue)
                {

                    decimal temp = decimal.Parse(value, System.Globalization.NumberStyles.AllowDecimalPoint |
                        System.Globalization.NumberStyles.AllowLeadingSign);
                    totalvalue = totalvalue + temp;

                }

                String total = totalvalue.ToString();

                listView2.Clear();
                listView2.View = View.Details;
                listView2.Columns.Add("Outstanding Value:", 200, HorizontalAlignment.Left);
                listView2.Items.Add(total);

                listView3.View = View.Details;
                listView3.Columns.Add("Document", 70, HorizontalAlignment.Left);
                listView3.Columns.Add("Serie", 40, HorizontalAlignment.Left);
                listView3.Columns.Add("Number", 50, HorizontalAlignment.Left);
                listView3.Columns.Add("Date", 70, HorizontalAlignment.Left);
                listView3.Columns.Add("Due", 70, HorizontalAlignment.Left);
                listView3.Columns.Add("Total", 70, HorizontalAlignment.Left);
                listView3.Columns.Add("Pending", 70, HorizontalAlignment.Left);

                for (int i = 0; i < document.Count; i++)
                {

                    DateTime due = DateTime.Parse(dueDate[i]);
                    Boolean red = false;
                    if (DateTime.Compare(due, DateTime.Now) <= 0 )
                    {

                        red = true;
                    }
                    

                    if (date[i].Length <= 21)
                    {
                        date[i] = date[i].Substring(0, 9);
                        dueDate[i] = dueDate[i].Substring(0, 9);
                    }
                    else
                    {
                        date[i] = date[i].Substring(0, 10);
                        dueDate[i] = dueDate[i].Substring(0, 10);

                    }

                    ListViewItem temp = new ListViewItem(new string[] { document[i], serie[i], number[i], date[i], dueDate[i], totalAmount[i], pendingValue[i] });
                    if (red)
                    {
                        temp.ForeColor = Color.Red;
                    }
                    listView3.Items.Add(temp);


                }
                

                
            }
        }
    }
}
