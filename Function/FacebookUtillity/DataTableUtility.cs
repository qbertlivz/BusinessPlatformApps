using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookUtillity
{
    public class DataTableUtility
    {
        public static DataTable GetCommentsDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Created Date", typeof(DateTime));
            table.Columns.Add("Message");
            table.Columns.Add("From Id");
            table.Columns.Add("From Name");
            table.Columns.Add("Post Id");
            table.Columns.Add("Page");
            table.Columns.Add("PageDisplayName");
            table.Columns.Add("PageId");
            return table;
        }

        public static DataTable GetReactionsDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Reaction Type");
            table.Columns.Add("Count", typeof(long));
            return table;
        }

        public static DataTable GetPostsDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Created Date", typeof(DateTime));
            table.Columns.Add("Message");
            table.Columns.Add("From Id");
            table.Columns.Add("From Name");
            table.Columns.Add("Media");
            table.Columns.Add("Page");
            table.Columns.Add("PageDisplayName");
            table.Columns.Add("PageId");
            table.Columns.Add("Total Comments", typeof(int));
            return table;
        }


        public static DataTable GetSentimentDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Sentiment", typeof(float));
            return table;
        }

        public static DataTable GetKeyPhraseDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("KeyPhrase");
            return table;
        }

        public static DataTable GetHashTagDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("HashTags");
            return table;
        }

        public static DataTable GetErrorDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("Error");
            table.Columns.Add("Posts");
            return table;
        }
    }
}
