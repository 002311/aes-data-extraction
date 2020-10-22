using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ListViewColumnSorter : IComparer

{
    private int ColumnToSort;

    private SortOrder OrderOfSort;

    private CaseInsensitiveComparer ObjectCompare;

    public ListViewColumnSorter()
    {

        ColumnToSort = 0;

        OrderOfSort = SortOrder.None;

        ObjectCompare = new CaseInsensitiveComparer();

    }

    public int Compare(Object x, Object y)
    {

        int CompareResult;

        ListViewItem listViewX, listViewY;

        listViewX = (ListViewItem)x;
        listViewY = (ListViewItem)y;

        CompareResult = ObjectCompare.Compare(listViewX.SubItems[ColumnToSort].Text,
            listViewY.SubItems[ColumnToSort].Text);

        if (OrderOfSort == SortOrder.Descending)
            return CompareResult;

        else if (OrderOfSort == SortOrder.Ascending)
            return (-CompareResult);

        else
            return 0;
    }

 
    public int SortColumn
    {
       set{

            ColumnToSort = value;
        }

        get
        {

            return ColumnToSort;
        }
    }

    public SortOrder Order
    {

        set
        {
            OrderOfSort = value;

        }

        get
        {

            return OrderOfSort;
        }
    }
}
